using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LunchLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodayLunchCore3.Models;

namespace TodayLunchCore3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string ownerName)
        {
            HttpContext.Session.Clear();
            if (ownerName == null)
                return View(new Owner());
            // 아이디 생성 한 뒤에 들어오는 거면 아이디를 자동으로 텍스트 칸에 입력하게 해준다.
            var ownerInfo = new Owner() { Name = ownerName };
            return View(ownerInfo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
