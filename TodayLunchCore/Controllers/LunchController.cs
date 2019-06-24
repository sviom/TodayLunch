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
        public IActionResult LunchList(string id, string addressId = null)
        {

            Guid ownerGuid = LunchLibrary.UtilityLauncher.ConvertBase64ToGuid(id);
            var getOwnerResult = ModelAction.Instance.Get<Owner>(x => x.Id.Equals(ownerGuid));

            Guid addressGuid = GetAddressGuid(addressId);
            if (addressGuid == Guid.Empty)
                return RedirectToAction("UpsertAddress", "Lunch");

            var addressList = new Address().GetAll<Address>(x => x.OwnerId.Equals(ownerGuid));
            foreach (var item in addressList)
            {
                if (item.Id == addressGuid)
                {
                    item.IsForeground = true;
                }
            }
            ViewBag.AddressList = addressList;

            if (getOwnerResult != null  && addressGuid != Guid.Empty)
            {
                var placeList = Owner.Instance.GetAll<Place>(x => x.OwnerId.Equals(getOwnerResult.Id) && x.AddressId.Equals(addressGuid));
                ViewBag.Owner = getOwnerResult;
                return View(placeList);
            }
            else if (getOwnerResult != null && addressGuid != Guid.Empty)
            {
                var placeList = Owner.Instance.GetAll<Place>(x => x.OwnerId.Equals(getOwnerResult.Id) && x.AddressId.Equals(addressGuid));
                ViewBag.Owner = getOwnerResult;

                return View(placeList);
            }
            return View();
        }

        public IActionResult CreatePlace(string addressId)
        {
            if (globalOwner != null)
            {
                Guid addressGuid = GetAddressGuid(addressId);
                if (addressGuid == Guid.Empty)
                    return RedirectToAction("UpsertAddress", "Lunch");

                var address = new Address();
                var addressGetResult = new Address().Get(ref address, x => x.OwnerId.Equals(globalOwner.Id) && x.Id.Equals(addressGuid));

                ViewBag.Address = address;
                ViewBag.Owner = globalOwner;
                var placeList = globalOwner.GetAll<Place>(x => x.OwnerId.Equals(globalOwner.Id) && x.AddressId.Equals(addressGuid));
                return View(placeList);
            }
            else
            {
                return View();
            }
        }

        private Guid GetAddressGuid(string addressId)
        {
            Guid addressGuid = Guid.Empty;
            if (!string.IsNullOrEmpty(addressId))
            {
                addressGuid = LunchLibrary.UtilityLauncher.ConvertBase64ToGuid(addressId);
            }
            else
            {
                var address = new Address();
                var addressGetResult = new Address().Get(ref address, x => x.OwnerId.Equals(globalOwner.Id) && x.IsDefault == true);
                if (address != null)
                {
                    addressGuid = address.Id;
                }
            }

            return addressGuid;
        }

        public IActionResult UpsertAddress(string addressId = null)
        {
            if (globalOwner != null)
            {
                ViewBag.Owner = globalOwner;
                var addressList = new Address().GetAll<Address>(x => x.OwnerId.Equals(globalOwner.Id));
                return View(addressList);
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
            var (insertCount, updateCount) = _PostPlaceAsync(jsonPlaceList);
            if (insertCount > 0 || updateCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 장소 생성 실제 로직
        /// </summary>
        /// <param name="_placeList">장소 JSON 문자열</param>
        /// <returns>생성된 장소 갯수</returns>
        private (int insertCount, int updateCount) _PostPlaceAsync(List<Place> _placeList)
        {
            int insertCount = 0;
            int updateCount = 0;

            foreach (var item in _placeList)
            {
                if (item.Id != Guid.Empty)
                {
                    if (new Place().Update(item))
                        updateCount++;
                }
                else
                {
                    if (new Place().Insert(item))
                        insertCount++;
                }
            }

            return (insertCount, updateCount);
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
            _deleteResult = new Place().Delete(_placeInfo);
            return _deleteResult;
        }

        [HttpPost]
        public string GetRandom([FromBody]string addressId)
        {
            Guid addressGuid = GetAddressGuid(addressId);

            var placeList = Owner.Instance.GetAll<Place>(x => x.OwnerId.Equals(globalOwner.Id) && x.AddressId.Equals(addressGuid));
            var randomPick = LunchLibrary.UtilityLauncher.RandomPick(placeList);
            ModelAction.Instance.Update(randomPick);
            return JsonConvert.SerializeObject(randomPick);
        }

        public async Task<bool> PostAddress([FromBody]List<Address> addresses)
        {
            var (insertCount, updateCount) = await _PostAddress(addresses);
            if (insertCount > 0 || updateCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 주소 생성 실제 로직
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns>생성/수정된 주소 갯수</returns>
        private async Task<(int insertCount, int updateCount)> _PostAddress(List<Address> addresses)
        {
            int insertCount = 0;
            int updateCount = 0;

            foreach (var item in addresses)
            {
                if (item.Id != Guid.Empty)
                {
                    var upsertResult = ModelAction.Instance.Update(item);
                    if (upsertResult != null)
                        updateCount++;
                }
                else
                {
                    var upsertResult = ModelAction.Instance.Insert(item);
                    if (upsertResult != null)
                        insertCount++;
                }
            }

            return (insertCount, updateCount);
        }
    }
}
