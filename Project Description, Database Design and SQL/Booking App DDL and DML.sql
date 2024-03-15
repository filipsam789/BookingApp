--DDL

create table booking_user(
	id_user varchar(200) primary key,
	name varchar(50),
	surname varchar(50),
	username varchar(30) not null,
	password varchar(150) not null,
	email varchar(100) not null
);

create table booking_traveller(
	id_user varchar(200) primary key,
	phone_number varchar(20),
	constraint fk_booking_traveller_is_booking_user foreign key (id_user)
	references booking_user(id_user)
);

create table booking(
	id_booking serial primary key,
	discount integer not null default 0,
	check_in_date date not null,
	check_out_date date not null,
	number_of_guests integer not null,
	id_user varchar(200) not null,
	id_listing integer not null,
	constraint fk_booking_traveller_makes_booking foreign key (id_user)
	references booking_traveller(id_user),
	constraint fk_reserves_listing foreign key (id_listing)
	references listing(id_listing) on delete cascade,
	constraint check_dates check(check_out_date>check_in_date)
);


create table listing(
	id_listing serial primary key,
	name varchar(100) not null,
	price integer not null,
	rating numeric(3,2),
	main_image varchar(200) not null,
	id_user varchar(200) not null,
	constraint fk_booking_owner_posts_listing foreign key (id_user)
	references booking_owner(id_user)
);

create table room(
	id_room serial primary key,
	capacity_of_guests integer not null,
	id_listing integer,
	id_property integer not null,
	constraint fk_listing_consists_of_room foreign key (id_listing)
	references listing(id_listing),
	constraint fk_room_is_in_property foreign key (id_property)
	references property(id_property) on delete cascade
);


create table property(
	id_property serial primary key,
	name varchar(100) not null,
	star_rating integer,
	id_destination integer not null,
	id_user varchar(200),
	constraint fk_booking_owner_owns_property foreign key (id_user)
	references booking_owner(id_user),
	constraint fk_property_is_at_destination foreign key (id_destination)
	references destination (id_destination) on delete cascade
);



create table destination(
	id_destination serial primary key,
	city_name varchar(50) not null,
	country_name varchar(100) not null
);

create table booking_owner(
	id_user varchar(200) primary key,
	constraint fk_booking_owner_is_booking_user foreign key(id_user)
	references booking_user(id_user)
);

create table booking_administrator(
	id_user varchar(200) primary key,
	constraint fk_booking_administrator_is_booking_user foreign key(id_user)
	references booking_user(id_user)
);

create table amenity(
	id_room integer,
	amenity varchar(100),
	constraint fk_room_has_amenity foreign key (id_room)
	references room(id_room) on delete cascade,
	constraint pk_amenity primary key(id_room, amenity)
);

create table phone_number(
	id_property integer,
	phone_number varchar(100),
	constraint fk_property_has_phone_number foreign key(id_property)
	references property(id_property) on delete cascade,
	constraint pk_phone_number primary key(id_property, phone_number)
);

create table facilities(
	id_property integer,
	facility varchar(100),
	constraint fk_property_has_facility foreign key(id_property)
	references property(id_property) on delete cascade,
	constraint pk_facility primary key(id_property, facility)
);

create table image(
	id_listing integer,
	image varchar(200),
	constraint fk_listing_has_image foreign key(id_listing)
	references listing(id_listing) on delete cascade,
	constraint pk_image primary key(id_listing, image)
);


--VIEWS


--list of listings
create view listing_list_info as 
select l.id_listing, l.name, l.main_image, avg(b.rating::float)::float as average_rating, 
p.star_rating,
	(select min(price)
	from room r1
	where r1.id_listing=l.id_listing) as listing_price,
	d.city_name, d.country_name,
	(
        select count(*)
        from room r2
        where l.id_listing=r2.id_listing
    ) as room_count, 
    r.capacity_of_guests, r.num_single_beds, r.num_double_beds,
     b.check_in_date, b.check_out_date
from listing l
left join booking b on l.id_listing=b.id_listing 
join room r on l.id_listing = r.id_listing 
join property p on r.id_property = p.id_property 
join destination d on p.id_destination = d.id_destination 
group by l.id_listing, l.name, l.main_image, p.star_rating, d.city_name, d.country_name,
r.num_single_beds, r.num_double_beds, r.capacity_of_guests, b.check_in_date, b.check_out_date
order by listing_price

--room info
create view room_info as 
select room.id_room, room.id_listing, room.id_property , room_type, capacity_of_guests, price, num_single_beds, num_double_beds, amenity.amenity 
from room
left join amenity on room.id_room = amenity.id_room 
group by room.id_room, room.id_listing , amenity.amenity 
order by capacity_of_guests asc

--selected listing info
create view listing_info as
select distinct listing.id_listing as idListing, property.id_property as idProperty, listing.name as listingName,
room.id_room as idRoom, listing.main_image as mainImage, image.image, listing.description, 
avg(booking.rating::float) as averageRating, property.star_rating as starRating, property.address, room.room_type as roomType, 
room.capacity_of_guests as capacityOfGuests, room.price, room.amenity, room.num_single_beds as numberOfSingleBeds, room.num_double_beds as numberOfDoubleBeds
from listing
join room_info as room on listing.id_listing = room.id_listing 
join property on room.id_property = property.id_property 
left join amenity as am on room.id_room = am.id_room 
left join image on listing.id_listing = image.id_listing
full join booking on booking.id_listing = listing.id_listing 
group by listing.id_listing, room.id_room, listing.description, property.star_rating, property.address,
image.image, am.amenity, room.room_type, room.capacity_of_guests,
room.price, room.amenity, room.num_single_beds, room.num_double_beds, property.id_property
order by listing.id_listing

--bookings per user
create view booking_info as
select booking.id_user as idUser, booking.id_booking as idBooking, booking.check_in_date as checkInDate,
booking.check_out_date as checkOutDate, booking.number_of_guests as numberOfGuests, booking.id_listing as idListing, room.room_type as roomType,  
(select price
from room 
where room.id_room = booking.id_room )*(100-booking.discount)/100 as totalPrice,
booking.rating, phone_number.phone_number as phoneNumber, property.id_property as idProperty, booking.id_room as idRoom
from booking
join booking_traveller on booking_traveller.id_user = booking.id_user 
join room on room.id_room = booking.id_room
join property on room.id_property = property.id_property 
left join phone_number on phone_number.id_property = property.id_property 
group by booking.id_booking, booking.id_user, booking.check_in_date, phone_number.phone_number, 
booking.check_out_date, booking.number_of_guests, booking.rating, booking.discount, booking.id_listing,
room.room_type, property.id_property, booking.id_room  


--DML

  
alter table booking 
add constraint check_guests check(number_of_guests>0)

alter table listing 
add constraint check_price check(price>0)

alter table booking_user 
add constraint check_password check(length(password)>8)

alter table listing
add column description varchar(1000),
drop column price,
drop column rating

alter table booking 
add column rating numeric(4,2) check(rating>=0 and rating<=10)

alter table room 
add column num_single_beds integer,
add column num_double_beds integer,
add column price integer,
add column room_type varchar(100)

alter table property 

update room 
set num_single_beds=2, num_double_beds=0, price=2000, room_type='2 Single Bed Room'
where id_room = 1

update room 
set num_single_beds=0, num_double_beds=2, price=4000, room_type='Deluxe Room'
where id_room = 2;

update room 
set num_single_beds=0, num_double_beds=1, price=1800, room_type='Double Bed Room'
where id_room = 3

update listing 
set description = 'navistina evtino'
where id_listing = 1;

update listing 
set description = 'na ezero'
where id_listing = 2;

update listing 
set description = 'navistina preskapo'
where id_listing = 3;

alter table listing 
alter column description set not null;

alter table room 
alter column num_single_beds set not null,
alter column num_double_beds set not null,
alter column price set not null,
alter column room_type set not null;

alter table room
add column room_number integer

alter table room
alter column room_number set not null

select * from room r 

update room 
set room_number=1
where room.id_room = 1

update room 
set room_number=1
where room.id_room = 2;

update room 
set room_number=2
where room.id_room = 3

select * from booking

update booking 
set rating=4
where id_booking = 1

update booking 
set rating=5
where id_booking = 2

alter table booking 
alter column rating set data type integer using rating::integer 

select * from property p 

update property 
set address='Sekspirova bb,, Skopje 1000'
where property.id_property = 1

update property 
set address='Kej Makedonija 55, Ohrid 6000'
where property.id_property = 2

update property 
set address='Main Street, Sani 630 77, Greece'
where property.id_property = 3

alter table property 
add column address varchar(200)

alter table property 
alter column address set not null

drop table listing  

drop table
booking_reserves_room 

alter table booking 
alter column id_room set not null

update booking 
set id_room = 1
where id_booking =2

select * from image i 
select * from destination d 
select * from listing l 
select * from room
select * from property p 
select * from amenity a 
select * from booking b
select * from booking_user bu 
select * from booking_traveller bt 
select * from booking_administrator ba  
select * from booking_owner bo 
select * from phone_number pn  


