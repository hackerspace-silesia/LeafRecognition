﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace LeafChecker {
    [Activity(Label = "LeafChecker", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        int count = 5;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate {
                Intent intent = new Intent(this, typeof(LeafMainActivity));
                intent.PutExtra("clicks", count);
                StartActivity(intent);
            };
        }
    }
}

