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
using Android.Content.PM;
using Java.IO;
using Android.Provider;
using Android.Graphics;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using CameraAppDemo;
using System.Net.Http;

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

            //button.Click += delegate {
            //    var imageIntent = new Intent();
            //    imageIntent.SetType("image/*");
            //    imageIntent.SetAction(Intent.ActionGetContent);
            //    StartActivityForResult(
            //        Intent.CreateChooser(imageIntent, "Select photo"), 0);
            //};

            if (IsThereAnAppToTakePictures()) {
                CreateDirectoryForPictures();
                var imageView =
                                    FindViewById<ImageView>(Resource.Id.myImageView);
                Button photoBtn = FindViewById<Button>(Resource.Id.photoButton);
                imageView = FindViewById<ImageView>(Resource.Id.myImageView);
                button.Click += TakeAPicture;
            }

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);
            var imageView =
                               FindViewById<ImageView>(Resource.Id.myImageView);
            if (resultCode == Result.Ok && requestCode == 0) {

                imageView.SetImageURI(data.Data);
                var exifInterface = new ExifInterface(data.Data.Path);
                string orientation = exifInterface.GetAttribute(ExifInterface.TagOrientation.ToString());
                text.Text = orientation.ToString();
            }

            if (requestCode == 1 && resultCode == Result.Ok) {

                // Make it available in the gallery

                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri contentUri = Uri.FromFile(App._file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                // Display in ImageView. We will resize the bitmap to fit the display.
                // Loading the full sized image will consume to much memory
                // and cause the application to crash.

                int height = Resources.DisplayMetrics.HeightPixels;
                int width = imageView.Height;
                App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
                if (App.bitmap != null) {
                    imageView.SetImageBitmap(App.bitmap);
                    App.bitmap = null;
                }

                // Dispose of the Java side bitmap.
                GC.Collect();
            }
        }

        private void TakeAPicture(object sender, EventArgs eventArgs) {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 1);
        }


        private void CreateDirectoryForPictures() {
            App._dir = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists()) {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures() {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
    }

    public static class App {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }
}