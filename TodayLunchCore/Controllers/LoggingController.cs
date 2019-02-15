using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LunchLibrary;
using LunchLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodayLunchCore.Controllers
{
    public class LoggingController : Controller
    {
        public IActionResult Index()
        {
            var logList = SqlLauncher.GetAll<Log>();
            return View(logList);
        }
    }
}