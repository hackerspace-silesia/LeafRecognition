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
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();

                var imageContent = new StreamContent(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(imageContent, "file", "image.jpg");

                client.GetAsync("http://155.158.2.55:8080/");
                var response =
                    client.PostAsync(url, content);
                string responseBody = ContentToString(response.Result.Content);
                Console.WriteLine("RESPONSE: " + responseBody);

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public string ContentToString(HttpContent httpContent) {
            var readAsStringAsync = httpContent.ReadAsStringAsync();
            return readAsStringAsync.Result;
        }


    }
}
