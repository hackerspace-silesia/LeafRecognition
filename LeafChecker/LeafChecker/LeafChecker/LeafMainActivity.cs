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
using Android.Media;

namespace LeafChecker {
    [Activity(Label = "LeafMainActivity")]
    public class LeafMainActivity : Activity {
        TextView text;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LeafMainLayout);
            int counts = Intent.GetIntExtra("clicks", 0);
            var button = FindViewById(Resource.Id.MyButton);
            text = FindViewById<TextView>(Resource.Id.testView);
            text.Text = counts.ToString();

            button.Click += delegate {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok) {
                var imageView =
                    FindViewById<ImageView>(Resource.Id.myImageView);
                imageView.SetImageURI(data.Data);
                var exifInterface = new ExifInterface(data.Data.Path);
                string orientation = exifInterface.GetAttribute(ExifInterface.TagOrientation.ToString());
                text.Text = orientation.ToString();
            }
        }
    }
}