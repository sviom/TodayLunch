using LunchLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TodayLunchUWP.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace TodayLunchUWP.Views
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class LunchList : Page
    {

        public LunchViewModel ViewMdoel { get; set; }

        public LunchList()
        {
            this.InitializeComponent();
            ViewMdoel = new LunchViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navigatedItem = e.Parameter as LunchPageNavigate;
            LunchListLoad(navigatedItem.Owner, navigatedItem.AddressId);
        }

        public void LunchListLoad(Owner owner, string addressId = null)
        {
            Guid ownerGuid = owner.Id;
            var getOwnerResult = Owner.Instance.Get<Owner>(x => x.Id.Equals(ownerGuid));

            Guid addressGuid = GetAddressGuid(addressId);
            if (addressGuid == Guid.Empty)
            {
                //return RedirectToAction("UpsertAddress", "Lunch");
            }

            var addressList = Address.Instance.GetAll<Address>(x => x.OwnerId.Equals(ownerGuid));
            foreach (var item in addressList)
            {
                if (item.Id == addressGuid)
                {
                    item.IsForeground = true;
                }
            }
            ViewMdoel.Addresses = new ObservableCollection<Address>(addressList);
            //ViewBag.AddressList = addressList;

            if (getOwnerResult != null && addressGuid != Guid.Empty)
            {
                var placeList = ModelAction.Instance.GetAll<Place>(x => x.OwnerId.Equals(getOwnerResult.Id) && x.AddressId.Equals(addressGuid));
                //ViewBag.Owner = getOwnerResult;
                //return View(placeList);
                ViewMdoel.Places = new ObservableCollection<Place>(placeList);
            }
            //return View();
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
    }
}
