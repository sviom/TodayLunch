using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using LunchLibrary.Models;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodayLunchCore.Controllers
{
    public class LunchController : Controller
    {
        private static Owner _owner = new Owner();

        public IActionResult LunchList(Owner owner = null)
        {
            if (owner.Id != Guid.Empty && owner != null)
            {
                _owner = owner;
                var placeList = LunchLibrary.SqlLauncher.GetAll<Place>(x => x.OwnerId.Equals(owner.Id));
                return View(placeList);
            }
            else if (_owner != null)
            {
                var placeList = LunchLibrary.SqlLauncher.GetAll<Place>(x => x.OwnerId.Equals(_owner.Id));
                return View(placeList);
            }
            return View();
        }


        public IActionResult CreatePlace()
        {
            if (_owner != null)
            {
                ViewBag.Owner = _owner;
                var placeList = LunchLibrary.SqlLauncher.GetAll<Place>(x => x.OwnerId.Equals(_owner.Id));
                return View(placeList);
            }
            else
            {
                return View();
            }
        }     

        /// <summary>
        /// 장소 생성
        /// </summary>
        /// <param name="jsonPlaceList">장소 목록 JSON 문자열</param>
        /// <returns>장소 생성이 정상적으로 이루어지면 장소 목록으로 이동</returns>
        public bool PostPlace([FromBody]List<Place> jsonPlaceList)
        {
            int createdPlaceCount = _PostPlaceAsync(jsonPlaceList);
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
        private int _PostPlaceAsync(List<Place> _placeList)
        {
            int insertCount = 0;
            int updateCount = 0;

            List<Place> forRemove = new List<Place>();
            foreach (var item in _placeList)
            {
                if (item.Id != Guid.Empty)
                {
                    item.UsingCount++;
                    item.UpdatedTime = DateTime.Now;
                    LunchLibrary.SqlLauncher.Update(item);
                    forRemove.Add(item);
                    updateCount++;

                    //item.Insert();
                }
            }

            foreach (var item in forRemove)
            {
                _placeList.Remove(item);
            }


            insertCount = LunchLibrary.SqlLauncher.InsertList(_placeList).Count;
            return insertCount;
        }

        /// <summary>
        /// 장소 삭제 Wrapper
        /// </summary>
        /// <param name="placeInfo"></param>
        /// <returns></returns>
        public bool DeletePlace([FromBody]Place placeInfo)
        {
            return _DeletePlace(placeInfo);
        }

        /// <summary>
        /// 장소 삭제 API 호출
        /// </summary>
        /// <param name="_placeInfo"></param>
        /// <returns></returns>
        private bool _DeletePlace(Place _placeInfo)
        {
            bool _deleteResult = false;
            _deleteResult = LunchLibrary.SqlLauncher.Delete(_placeInfo);
            return _deleteResult;
        }
    }
}
