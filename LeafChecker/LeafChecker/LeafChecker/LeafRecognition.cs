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
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace LeafChecker {

    public class Leaf {
        public string Name { get; set; }
        public string percent { get; set; }
    }

    [Activity(Label = "LeafRecognition", Theme= "@style/Theme.Custom")]
    public class LeafRecognition : Activity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LeafRecognition);
            string json = Intent.GetStringExtra("json");

            var leaf = JsonConvert.DeserializeObject<Leaf>(json);
            TextView text = FindViewById<TextView>(Resource.Id.percent);
            text.Text = string.Format("To na {0}% {1}", leaf.percent, leaf.Name);
            ImageView image = FindViewById<ImageView>(Resource.Id.leafImg);

            var drawableImage = Resources.GetDrawable(Resources.GetIdentifier(leaf.Name, "drawable", PackageName));
            var bitmap = (drawableImage as BitmapDrawable).Bitmap;
            if (bitmap != null) {
                image.SetImageBitmap(bitmap);
            }
        }
    }
}