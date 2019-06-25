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
            var getOwnerResult = Owner.Instance.Get<Owner>(x => x.Id.Equals(ownerGuid));

            Guid addressGuid = GetAddressGuid(addressId);
            if (addressGuid == Guid.Empty)
                return RedirectToAction("UpsertAddress", "Lunch");

            var addressList = Address.Instance.GetAll<Address>(x => x.OwnerId.Equals(ownerGuid));
            foreach (var item in addressList)
            {
                if (item.Id == addressGuid)
                {
                    item.IsForeground = true;
                }
            }
            ViewBag.AddressList = addressList;

            if (getOwnerResult != null && addressGuid != Guid.Empty)
            {
                var placeList = ModelAction.Instance.GetAll<Place>(x => x.OwnerId.Equals(getOwnerResult.Id) && x.AddressId.Equals(addressGuid));
                ViewBag.Owner = getOwnerResult;
                return View(placeList);
            }
            return View();
        }

        public IActionResult CreatePlace(string addressId)
        {
            if (Owner.OwnerInstance != null)
            {
                Guid addressGuid = GetAddressGuid(addressId);
                if (addressGuid == Guid.Empty)
                    return RedirectToAction("UpsertAddress", "Lunch");

                var addressGetResult = Address.Instance.Get<Address>(x => x.OwnerId.Equals(Owner.OwnerInstance.Id) && x.Id.Equals(addressGuid));

                ViewBag.Address = addressGetResult;
                ViewBag.Owner = Owner.OwnerInstance;
                var placeList = Place.Instance.GetAll<Place>(x => x.OwnerId.Equals(Owner.OwnerInstance.Id) && x.AddressId.Equals(addressGuid));
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
                var addressGetResult = ModelAction.Instance.Get<Address>(x => x.OwnerId.Equals(Owner.OwnerInstance.Id) && x.IsDefault == true);
                if (addressGetResult != null)
                {
                    addressGuid = addressGetResult.Id;
                }
            }

            return addressGuid;
        }

        public IActionResult UpsertAddress(string addressId = null)
        {
            if (Owner.OwnerInstance != null)
            {
                ViewBag.Owner = Owner.OwnerInstance;
                var addressList = ModelAction.Instance.GetAll<Address>(x => x.OwnerId.Equals(Owner.OwnerInstance.Id));
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


            int insertCount = 0;
            int updateCount = 0;

            foreach (var item in jsonPlaceList)
            {
                if (item.Id != Guid.Empty)
                {
                    if (ModelAction.Instance.Update(item) != null)
                        updateCount++;
                }
                else
                {
                    if (ModelAction.Instance.Insert(item) != null)
                        insertCount++;
                }
            }

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
        /// 장소 삭제 Wrapper
        /// </summary>
        /// <param name="placeInfo"></param>
        /// <returns></returns>
        public bool DeletePlace([FromBody]Place placeInfo)
        {
            bool _deleteResult = ModelAction.Instance.Delete(placeInfo);
            return _deleteResult;
        }

        [HttpPost]
        public string GetRandom([FromBody]string addressId)
        {
            Guid addressGuid = GetAddressGuid(addressId);

            var placeList = ModelAction.Instance.GetAll<Place>(x => x.OwnerId.Equals(Owner.OwnerInstance.Id) && x.AddressId.Equals(addressGuid));
            var randomPick = LunchLibrary.UtilityLauncher.RandomPick(placeList);
            ModelAction.Instance.Update(randomPick);
            return JsonConvert.SerializeObject(randomPick);
        }

        public bool PostAddress([FromBody]List<Address> addresses)
        {
            int insertCount = 0;
            int updateCount = 0;

            foreach (var item in addresses)
            {
                if (item.Id != Guid.Empty)
                {
                    var updateResult = ModelAction.Instance.Update(item);
                    if (updateResult != null)
                        updateCount++;
                }
                else
                {
                    var insertResult = ModelAction.Instance.Insert(item);
                    if (insertResult != null)
                        insertCount++;
                }
            }

            if (insertCount > 0 || updateCount > 0)
                return true;
            else
                return false;
        }
    }
}
