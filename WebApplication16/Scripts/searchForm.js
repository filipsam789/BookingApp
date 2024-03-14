document.getElementById('livesearchtags').addEventListener('keyup', function (e) {
    //Run LiveSearch on ever key up 
    LiveSearch()
});

function LiveSearch() {
    //Get the input value
    let value = document.getElementById('livesearchtags').value

    $.ajax({
        type: 'POST',
        url: '/Listings/LiveTagSearch',
        data: { search: value },
        dataType: 'html',
        success: function (data) {
            $('#result').html(data);
        }
    });

    $('#result').on('click', '.search-tag', function () {
        // Get the text content of the clicked element
        var selectedDestination = $(this).text().trim();

        // Set the value of the input field to the selected destination
        $('#livesearchtags').val(selectedDestination);

        // Clear the result container
        $('#result').empty();
    });
}