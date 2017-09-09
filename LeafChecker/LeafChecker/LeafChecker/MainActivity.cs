using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.Net.Http;

namespace LeafChecker {
    [Activity(Label = "LeafChecker", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class MainActivity : Activity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.Main);
            SetFonts();
            ChangeIntentButton();
        }

        private void ChangeIntentButton() {
            Button button = FindViewById<Button>(Resource.Id.getIn);
            button.Click += delegate {
                Intent intent = new Intent(this, typeof(LeafMainActivity));
                StartActivity(intent);
            };
        }

        private void SetFonts() {
            var textView = FindViewById<TextView>(Resource.Id.appName);
            Typeface tf = Typeface.CreateFromAsset(Assets, "futura.TTF");
            textView.SetTypeface(tf, TypefaceStyle.Normal);

            var textDescription = FindViewById<TextView>(Resource.Id.appDescription);
            Typeface tfDescription = Typeface.CreateFromAsset(Assets, "LATO-BOLD.TTF");
            textDescription.SetTypeface(tfDescription, TypefaceStyle.Normal);
        }
    }
}

