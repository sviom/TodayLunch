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
        public IActionResult CheckUser(string ownerName, string password)
        {
            (Owner ownerVal, bool isOnwer) = Login(ownerName, password);
            if (isOnwer)
                return RedirectToAction("LunchList", "Lunch", ownerVal);
            else
                return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 로그인 체크
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public (Owner ownerVal, bool isOnwer) Login(string name, string password)
        {
            var hashedPw = LunchLibrary.UtilityLauncher.EncryptSHA256(password);
            var _owner = new Owner
            {
                Name = name,
                Password = hashedPw
            };

            var isOwner = LunchLibrary.SqlLauncher.Get(_owner, x => x.Name.Equals(name) && x.Password.Equals(hashedPw));
            var returnTuple = (ownerVal: isOwner, isOnwer: true);
            if (isOwner != null)
                return returnTuple;
            else
            {
                returnTuple.isOnwer = false;
                return returnTuple;
            }
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
        public IActionResult PostUser(string name, string password)
        {
            var owner = _PostUserAsync(name, password);
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
        private Owner _PostUserAsync(string name, string password)
        {
            // 사용자 입력 암호 암호화작업
            var hashedPw = LunchLibrary.UtilityLauncher.EncryptSHA256(password);

            var owner = LunchLibrary.SqlLauncher.Insert(new Owner() { Name = name, Password = hashedPw });
            return owner;
        }
    }
}
