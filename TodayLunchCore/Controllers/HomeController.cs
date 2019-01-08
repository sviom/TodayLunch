using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using LunchCommon.Models;
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
            // 아이디 생성 한 뒤에 들어오는 거면 아이디를 자동으로 텍스트 칸에 입력하게 해준다.
            Owner ownerInfo = new Owner()
            {
                OwnerName = ownerName
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
        public async Task<bool> CheckDuplicateUser([FromBody]string ownerName)
        {
            return await _CheckDuplicateUserAsync(ownerName);
        }

        /// <summary>
        /// 사용자 로그인 시 중복검사 후 페이지 이동
        /// </summary>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CheckUser(string ownerId, string ownerName)
        {
            Common comm = new Common();
            bool checkResult = await _CheckDuplicateUserAsync(ownerName);
            Owner userInfo = await comm._GetUserInfoAsync(ownerName);
            if (!checkResult && userInfo != null)
            {
                return RedirectToAction("LunchList", "Lunch", new { id = userInfo.OwnerName });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 사용자 아이디 중복 검사 내부 로직
        /// </summary>
        /// <param name="_ownerId">사용자가 입력한 아이디</param>
        /// <returns></returns>
        private async Task<bool> _CheckDuplicateUserAsync(string _ownerName)
        {
            using (HttpClient client = new HttpClient())
            {
                var response
                    = await client.PostAsync
                    (
                        LunchCommon.PreDefined.ServiceApiUrl + "SelectAPI/GetCheckUserDuplicate",
                        new StringContent(JsonConvert.SerializeObject(_ownerName), Encoding.UTF8, "application/json")
                    );

                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var model = JsonConvert.DeserializeObject<Owner>(content);
                    //return View(model.results);
                    if (int.TryParse(content, out int value))
                    {
                        if (value > 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }

                // an error occurred => here you could log the content returned by the remote server
                //return Content("An error occurred: " + content);
                return true;
            }
        }

        /// <summary>
        /// 사용자 생성
        /// </summary>
        /// <param name="ownerId">사용자가 입력한 아이디</param>
        /// <returns>생성된 사용자 고유번호</returns>
        public async Task<IActionResult> PostUser(string ownerName)
        {
            int userId = await _PostUserAsync(ownerName);
            if (userId > 0)
            {
                return RedirectToAction("Index", "Home", new { ownerName = ownerName });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// 사용자 생성 실제 로직
        /// </summary>
        /// <param name="_ownerId">사용자가 입력한 아이디</param>
        /// <returns>생성된 사용자 고유번호</returns>
        private async Task<int> _PostUserAsync(string _ownerId)
        {
            int _userId = -1;
            using (HttpClient client = new HttpClient())
            {
                var response
                    = await client.PostAsync
                    (
                        LunchCommon.PreDefined.ServiceApiUrl + "InsertAPI/CreateUser",
                        
                        new StringContent(JsonConvert.SerializeObject(_ownerId), Encoding.UTF8, "application/json")
                    );

                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var model = JsonConvert.DeserializeObject<Owner>(content);
                    //return View(model.results);
                    int value;
                    if (int.TryParse(content, out value))
                    {
                        if (value > 0)
                        {
                            _userId = value;
                        }
                    }
                }

                // an error occurred => here you could log the content returned by the remote server
                //return Content("An error occurred: " + content);
            }
            return _userId;
        }

    }
}
