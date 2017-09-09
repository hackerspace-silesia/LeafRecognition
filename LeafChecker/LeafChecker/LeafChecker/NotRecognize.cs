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

namespace LeafChecker {
    [Activity(Label = "NotRecognize", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class NotRecognize : Activity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NotRecognizeView);
            Button findAnother = FindViewById<Button>(Resource.Id.findAnother);
        }
    }
}