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
using Provider;

namespace LeafChecker {
    [Activity(Label = "LeafMainActivity", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class LeafMainActivity : Activity {
        TextView text;
        Button getPhotoBtn;
        Button createPhotoBtn;
        ImageView imageView;
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LeafMainLayout);

            getPhotoBtn = FindViewById<Button>(Resource.Id.getPhotoBtn);
            createPhotoBtn = FindViewById<Button>(Resource.Id.madePhotoBtn);
            imageView = FindViewById<ImageView>(Resource.Id.photoView);

            this.getPhotoBtn.Click += delegate {
                GetPhoto();
            };
            InitTakingPhoto();
        }

        private void InitTakingPhoto() {
            if (IsThereAnAppToTakePictures()) {
                CreateDirectoryForPictures();
                createPhotoBtn.Click += TakeAPicture;
            }

        }

        private void GetPhoto() {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok && requestCode == 0) {
                ShowChososenPhoto(data);
            }
            if (requestCode == 1 && resultCode == Result.Ok) {
                ShowAndSendPhoto();
            }
        }

        private void ShowChososenPhoto(Intent data) {
            imageView.SetImageURI(data.Data);
            CustomHttpClient client = new CustomHttpClient();
            string json = client.UploadImage(data.Data.Path);
            Intent intent = new Intent(this, typeof(LeafRecognition));
            intent.PutExtra("json", json);
            StartActivity(intent);
        }

        private void ShowAndSendPhoto() {
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null) {
                imageView.SetImageBitmap(App.bitmap);
                CustomHttpClient client = new CustomHttpClient();
                string json = client.UploadImage(App._file.Path);
                Intent intent = new Intent(this, typeof(LeafRecognition));
                intent.PutExtra("json", json);
                StartActivity(intent);
                App.bitmap = null;
            }
            GC.Collect();
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