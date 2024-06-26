﻿using System.Web;
using System.Web.Mvc;

namespace WebApplication16
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AddViewBagDataAttribute(new Models.BookingEntities()));
            filters.Add(new HandleErrorAttribute());
        }
    }
}
