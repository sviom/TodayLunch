using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using LunchCommon.Models;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodayLunchCore.Controllers
{
    public class LunchController : Controller
    {
        Common comm = new Common();
        static Owner userInfo = new Owner();

        public async Task<IActionResult> LunchList(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                userInfo = await comm._GetUserInfoAsync(id);
                string placeList = await _GetPlaceListAsync(userInfo);
                if (!placeList.Equals("실패"))
                {
                    return View(JsonConvert.DeserializeObject<List<Place>>(placeList));
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult CreatePlace()
        {
            return View(userInfo);
        }

        /// <summary>
        /// 해당 사용자가 갖고 있는 장소 전부 목록으로 가져오기
        /// </summary>
        /// <param name="ownerInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetPlaceList([FromBody]Owner ownerInfo)
        {
            //return await _GetPlaceListAsync(JsonConvert.DeserializeObject<Owner>(ownerInfo));
            return await _GetPlaceListAsync(ownerInfo);
        }

        /// <summary>
        /// 해당 사용자가 갖고있는 장소 전부 목록으로 가져오기
        /// </summary>
        /// <param name="_ownerInfo"></param>
        /// <returns></returns>
        private async Task<string> _GetPlaceListAsync(Owner _ownerInfo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response
                        = await client.PostAsync
                        (
                            "http://todaylunchapi.azurewebsites.net/api/SelectAPI/GetPlaceList",
                            //"http://localhost:7011/api/SelectAPI/GetPlaceList",
                            new StringContent(JsonConvert.SerializeObject(_ownerInfo), Encoding.UTF8, "application/json")
                        );

                    string content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        //var model = JsonConvert.DeserializeObject<Owner>(content);
                        //return View(model.results);
                        return content;
                    }

                    // an error occurred => here you could log the content returned by the remote server
                    //return Content("An error occurred: " + content);
                    return "실패";
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 장소 생성
        /// </summary>
        /// <param name="jsonPlaceList">장소 목록 JSON 문자열</param>
        /// <returns>장소 생성이 정상적으로 이루어지면 장소 목록으로 이동</returns>
        public async Task<bool> PostPlace([FromBody]List<Place> jsonPlaceList)
        {
            int createdPlaceCount = await _PostPlaceAsync(jsonPlaceList);
            if (createdPlaceCount >= 0)
            {
                //return RedirectToAction("LunchList", "Lunch", new { ownerName = userInfo.OwnerName });
                return true;
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return false;
            }

        }

        /// <summary>
        /// 장소 생성 실제 로직
        /// </summary>
        /// <param name="_placeList">장소 JSON 문자열</param>
        /// <returns>생성된 장소 갯수</returns>
        private async Task<int> _PostPlaceAsync(List<Place> _placeList)
        {
            int _createdPlaceCount = -1;
            using (HttpClient client = new HttpClient())
            {
                var response
                    = await client.PostAsync
                    (
                        "http://todaylunchapi.azurewebsites.net/api/InsertAPI/CreatePlace",
                        //"http://localhost:7011/api/InsertAPI/CreatePlace",
                        new StringContent(JsonConvert.SerializeObject(_placeList), Encoding.UTF8, "application/json")
                    );

                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var model = JsonConvert.DeserializeObject<Owner>(content);
                    //return View(model.results);
                    if (int.TryParse(content, out int value))
                    {
                        if (value >= 0)
                        {
                            _createdPlaceCount = value;
                        }
                    }
                }

                // an error occurred => here you could log the content returned by the remote server
                //return Content("An error occurred: " + content);
            }
            return _createdPlaceCount;
        }

        /// <summary>
        /// 장소 삭제 Wrapper
        /// </summary>
        /// <param name="placeInfo"></param>
        /// <returns></returns>
        public async Task<bool> DeletePlace([FromBody]Place placeInfo)
        {
            return await _DeletePlaceAsync(placeInfo);
        }

        /// <summary>
        /// 장소 삭제 API 호출
        /// </summary>
        /// <param name="_placeInfo"></param>
        /// <returns></returns>
        private async Task<bool> _DeletePlaceAsync(Place _placeInfo)
        {
            bool _deleteResult = false;
            using (HttpClient client = new HttpClient())
            {
                var response
                    = await client.PostAsync
                    (
                        //"http://todaylunchapi.azurewebsites.net/api/DeleteAPI/DeletePlace",
                        "http://localhost:7011/api/DeleteAPI/DeletePlace",
                        new StringContent(JsonConvert.SerializeObject(_placeInfo), Encoding.UTF8, "application/json")
                    );

                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var model = JsonConvert.DeserializeObject<Owner>(content);
                    //return View(model.results);

                    if (Convert.ToBoolean(content))
                    {
                        _deleteResult = true;
                    }
                    
                }

                // an error occurred => here you could log the content returned by the remote server
                //return Content("An error occurred: " + content);
            }
            return _deleteResult;
        }
    }
}
