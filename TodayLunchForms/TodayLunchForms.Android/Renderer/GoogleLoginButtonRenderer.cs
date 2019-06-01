using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TodayLunchForms.CustomControl;
using TodayLunchForms.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GoogleLoginButton), typeof(GoogleLoginButtonRenderer))]
namespace TodayLunchForms.Droid.Renderer
{
    public class GoogleLoginButtonRenderer : ButtonRenderer
    {
        Context ThisContext;
        GoogleLoginButton GoogleLoginButton;

        public GoogleLoginButtonRenderer(Context context) : base(context)
        {
            ThisContext = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                GoogleLoginButton.Clicked -= LoginButton_Clicked;
            }
            if (e.NewElement != null)
            {
                GoogleLoginButton = e.NewElement as GoogleLoginButton;
                GoogleLoginButton.Clicked += LoginButton_Clicked;
            }
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}