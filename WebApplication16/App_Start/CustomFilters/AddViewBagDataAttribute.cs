using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebApplication16.Models;

namespace WebApplication16.App_Start.CustomFilters
{
    public class AddViewBagDataAttribute : ActionFilterAttribute
    {
        private readonly BookingEntities _dbContext;

        public AddViewBagDataAttribute(BookingEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var data = _dbContext.Users.Find(filterContext.HttpContext.User.Identity.GetUserId());
            filterContext.Controller.ViewBag.LoggedInUser = data;

            base.OnActionExecuting(filterContext);
        }

    }
}