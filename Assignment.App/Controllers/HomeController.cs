using Assignment.Identity;
using Assignment.Identity.ViewModels;
using Assignment.Identity.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Assignment.App.Controllers
{
    public class HomeController :Controller
    {
        private readonly ICRUDServices _services;

        public HomeController(ICRUDServices services)
        {
            _services = services;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Setting()
        {
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("draw", Request.Form["draw"].FirstOrDefault());
            obj.Add("sortColumn", Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault());
            obj.Add("sortColumnDirection", Request.Form["order[0][dir]"].FirstOrDefault());
            obj.Add("searchValue", Request.Form["search[value]"].FirstOrDefault());
            obj.Add("skip", Request.Form["start"].FirstOrDefault() ?? "0");
            obj.Add("take", Request.Form["length"].FirstOrDefault() ?? "0");
            var data = _services.getSetting(obj);
            return new JsonResult(data);
        }
        public IActionResult AddSetting(SettingViewModel payload)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return new JsonResult("Please fill all required fields");
            }
            payload.Created = DateTime.Now;

            var status = _services.AddSetting(payload);

            if (status == "success")
            {
                return new JsonResult(new { msg = "Added Successfully", item = payload });
            }
            Response.StatusCode = 400;
            return new JsonResult("Something went wrong");
        }
        public IActionResult UpdateSetting(SettingViewModel payload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = 400;
                    return new JsonResult("Please fill all required fields");
                }

                var status = _services.UpdateSetting(payload);

                if (status == "success")
                {
                    return new JsonResult(new { msg = "Updated Successfully", item = payload });
                }
                Response.StatusCode = 400;
                return new JsonResult("Something went wrong");
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return new JsonResult(e.Message);
            }
        }
        public IActionResult DeleteSetting(Int64 id)
        {
            var status = _services.DeleteSetting(id);
            if (status == "Deleted")
            {
                return new JsonResult("Deleted successfully");
            }
            Response.StatusCode = 400;
            return new JsonResult("Something went wrong");
        }
    }
}
