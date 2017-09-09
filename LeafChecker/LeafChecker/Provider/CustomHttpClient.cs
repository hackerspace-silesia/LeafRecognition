using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Provider {
    public class CustomHttpClient {

        public void UploadImage(string path) {
            //variable
            var url = "http://155.158.2.55:8080/";
            var file = path;

            try {
                //read file into upfilebytes array
                var upfilebytes = File.ReadAllBytes(file);
                //create new HttpClient and MultipartFormDataContent and add our file, and StudentId
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();

                string base64 = Convert.ToBase64String(upfilebytes);
                StringContent baseContent = new StringContent(base64);

                var imageContent = new StreamContent(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(imageContent, "file", "image.jpg");

                //content.Add(baseContent, "file");
                client.GetAsync("http://155.158.2.55:8080/");
                //upload MultipartFormDataContent content async and store response in response var
                var response =
                    client.PostAsync(url, content);

                ////debug
                //Debug.WriteLine(responsestr);

            }
            catch (Exception e) {
                //debug
                //Debug.WriteLine("Exception Caught: " + e.ToString());

                return;
            }
        }


    }
}
