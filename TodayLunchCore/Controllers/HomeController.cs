using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using LunchLibrary.Models;
using System.Text;

namespace TodayLunchCore.Controllers
{
    public class HomeController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index(string ownerName)
        {
            if (ownerName == null)
                return View();

            // 아이디 생성 한 뒤에 들어오는 거면 아이디를 자동으로 텍스트 칸에 입력하게 해준다.
            var ownerInfo = new Owner()
            {
                Name = ownerName
            };
            return View(ownerInfo);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 사용자 아이디 중복 검사
        /// 이중을 한 것은 콘솔 네트워크에 노출하지 않기 위해
        /// </summary>
        /// <param name="ownerId">사용자가 입력한 아이디</param>
        /// <returns></returns>
        [HttpPost]
        public bool CheckDuplicateUser([FromBody]string ownerName)
        {
            return _CheckDuplicateUserAsync(ownerName);
        }

        /// <summary>
        /// 사용자 로그인 시 중복검사 후 페이지 이동
        /// </summary>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckUser([FromBody]string ownerName)
        {
            bool checkResult = _CheckDuplicateUserAsync(ownerName);
            var owner = LunchLibrary.SqlLauncher.GetByName<Owner>(ownerName);
            if (!checkResult && owner != null)
                return RedirectToAction("LunchList", "Lunch", new { id = owner.Name });
            else
                return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 사용자 아이디 중복 검사 내부 로직
        /// </summary>
        /// <param name="_ownerId">사용자가 입력한 아이디</param>
        /// <returns></returns>
        private bool _CheckDuplicateUserAsync(string _ownerName)
        {
            int count = LunchLibrary.SqlLauncher.Count<Owner>(x => x.Name.Equals(_ownerName));
            if (count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// 사용자 생성
        /// </summary>
        /// <param name="ownerId">사용자가 입력한 아이디</param>
        /// <returns>생성된 사용자 고유번호</returns>
        public IActionResult PostUser(string ownerName)
        {
            var owner = _PostUserAsync(ownerName);
            if (owner != null)
                return RedirectToAction("Index", "Home", new { ownerName = owner.Name });
            else
                return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 사용자 생성 실제 로직
        /// </summary>
        /// <param name="name">사용자가 입력한 아이디</param>
        /// <returns>생성된 사용자 고유번호</returns>
        private Owner _PostUserAsync(string name)
        {
            var owner = LunchLibrary.SqlLauncher.Insert(new Owner() { Name = name });
            return owner;
        }
    }
}
