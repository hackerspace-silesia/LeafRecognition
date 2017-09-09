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
using Newtonsoft.Json;

namespace LeafChecker {

    public class Leaf {
        public string Name { get; set; }
        public string percent { get; set; }
    }

    [Activity(Label = "LeafRecognition", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class LeafRecognition : Activity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LeafRecognition);
            string json = Intent.GetStringExtra("json");

            var leaf = JsonConvert.DeserializeObject<Leaf>(json);
            TextView text = FindViewById<TextView>(Resource.Id.name);
            text.Text = leaf.Name;
            TextView percentText = FindViewById<TextView>(Resource.Id.percent);
            percentText.Text = leaf.percent;
        }
    }
}