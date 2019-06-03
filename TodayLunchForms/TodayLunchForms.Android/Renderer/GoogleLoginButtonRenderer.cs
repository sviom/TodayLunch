using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TodayLunchForms.CustomControl;
using TodayLunchForms.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Gms.Common.Apis.GoogleApiClient;

[assembly: ExportRenderer(typeof(GoogleLoginButton), typeof(GoogleLoginButtonRenderer))]
namespace TodayLunchForms.Droid.Renderer
{
    public class GoogleLoginButtonRenderer : ButtonRenderer
    {
        Context ThisContext;
        GoogleLoginButton GoogleLoginButton;
        public static GoogleApiClient GoogleApiClient { get; set; }

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
                if (GoogleApiClient == null)
                    GoogleApiClient = CreateGoogleApiClient(ThisContext);
                GoogleLoginButton.Clicked += LoginButton_Clicked;
            }
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(GoogleApiClient);
            ((FormsAppCompatActivity)ThisContext).StartActivityForResult(signInIntent, 9001);
        }

        public static GoogleApiClient CreateGoogleApiClient(Context ctx)
        {
            string clientId = "744732431693-8pr7b1fv5d5s9ig1ogu0reh91n7o3vuq.apps.googleusercontent.com";
            string lunchKeyClientId = "744732431693-07us5nv5h8ok4i8rjbek8np43nhqt8nt.apps.googleusercontent.com";
            string webId = "744732431693-73ravg1rf2ismdu3n6lej6grvk5lcibi.apps.googleusercontent.com";

            var googleSignInOption = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .RequestProfile()
                .RequestIdToken(webId)
                .RequestServerAuthCode(webId)
                .Build();
            var apiClient = new GoogleApiClient.Builder(ctx)
                .EnableAutoManage((FormsAppCompatActivity)ctx, new OnConnectionFailedListener())
                .AddApi(Auth.GOOGLE_SIGN_IN_API, googleSignInOption)
                .AddScope(new Scope(Scopes.Email))
                .Build();
            return apiClient;
        }

        private class OnConnectionFailedListener : Java.Lang.Object, IOnConnectionFailedListener
        {
            public void OnConnectionFailed(ConnectionResult result)
            {
                //Log.Debug("error", "error");
            }
        }
    }
}