using LunchLibrary.Models;
using System;
using System.Collections.Generic;
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

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace TodayLunchUWP
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            string sss = Environment.CurrentDirectory;
            string fff = Directory.GetCurrentDirectory();
            string ggg = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var name = UserId.Text;
            var password = UserPassword.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                return;
            }

            var hashedPw = LunchLibrary.UtilityLauncher.EncryptSHA256(password);
            var owner = ModelAction.Instance.Get<Owner>(x => x.Name.Equals(name) && x.Password.Equals(hashedPw));
            if (owner != null)
            {                
                Owner.OwnerInstance = owner;

                LunchPageNavigate naviagateObject = new LunchPageNavigate
                {
                    Owner = owner,
                    AddressId = string.Empty
                };
                Frame.Navigate(typeof(Views.LunchList), naviagateObject);
            }
        }
    }
}
