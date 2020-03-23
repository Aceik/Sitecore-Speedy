//  References: https://github.com/TomTyack/CriticalCSSAPI
//  Postman Examples: https://www.getpostman.com/collections/f2df62c43496395dbdd8


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Sitecore.Data.Items;
using Sitecore.Foundation.ImageCompression.Model;
using Sitecore.Foundation.ImageCompression.Services;
using Sitecore.Foundation.ImageCompression.Settings;
using Sitecore.IO;
using Sitecore.Resources.Media;

namespace Sitecore.Foundation.ImageCompression.Services
{
    /// <summary>
    /// Call 
    /// </summary>
    public class TinyPngApiGateway : IImageCompressionService
    {
        private const string Message = "Tiny PNG Remote API caused an error";

        public string CompressImage(Item currentItem)
        {
            var uploadedImage = SendToTinyForCompression(currentItem);
            DownloadImage(currentItem, uploadedImage);
            return uploadedImage.Location;
        }

        public ImageUpload SendToTinyForCompression(Item currentItem)
        {
            var client = new RestClient(ImageCompressionSettings.GetApiEndpoint());
            client.Authenticator = new HttpBasicAuthenticator("Api", ImageCompressionSettings.GetApiEndpointKey());

            var request = new RestRequest(Method.POST);
            //request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            //request.AddHeader("Content-Type", "application/json");

            if (currentItem != null)
            {
               var _mediaItem = new MediaItem(currentItem);

                if(_mediaItem != null)
                {
                    var imageBytes = ReadMediaStream(_mediaItem);

                    //request.AddHeader("Content-Type", "application/json");
                    // request.AddFileBytes(_mediaItem.Name, imageBytes, _mediaItem.Name, _mediaItem.MimeType);

                    //request.AddHeader("Content-Type", "multipart/form-data");

                    //client.AddHandler("application/octet-stream", new RestSharp.Deserializers.JsonDeserializer());

                    //string image = System.Convert.ToBase64String(imageBytes);

                    //Dictionary<string, object> dict = new Dictionary<string, object>();
                    //dict.Add("FILE_EXT", _mediaItem.Extension);
                    //dict.Add("FILE_MIME_TYPE", _mediaItem.MimeType);

                    //dict.Add("IMAGE_DATA", image);
                    //byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict));
                    //request.AddParameter("application/octet-stream", data, ParameterType.RequestBody);
                    //request.AddParameter(_mediaItem.MimeType, image, ParameterType.RequestBody);

                    //request.AddHeader("Content-Type", "application/pdf");
                    request.AddHeader("content-length", imageBytes.Length.ToString());
                    request.AddHeader("accept-encoding", "gzip, deflate");
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddHeader("Content-Disposition",
                        string.Format("file; filename=\"{0}\"; documentid={1}; fileExtension=\"{2}\"",
                        _mediaItem.Name, 1234, _mediaItem.Extension));
                    request.AddParameter("application/x-www-form-urlencoded", imageBytes, ParameterType.RequestBody);

                }
            }

            client.Timeout = 300000;

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            try
            {
                var response2 = client.Execute<ImageUpload>(request);
                var content = response2.Content;

                response2.Data.Location = response2.Headers.ToList()
                .Find(x => x.Name == "Location")
                .Value.ToString();

                //response2.Data.Location = (string) response2.Headers[response2.Headers.IndexOf()].Value;
                return response2.Data;
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error(Message, ex);
            }
            return null;
        }

        public byte[] ReadMediaStream(MediaItem _currentMedia)
        {
            Stream stream = _currentMedia.GetMediaStream();
            long fileSize = stream.Length;
            byte[] buffer = new byte[(int)fileSize];
            stream.Read(buffer, 0, (int)stream.Length);
            stream.Close();

            return buffer;
        }

        public string DownloadImage(MediaItem currentItem, ImageUpload img)
        {
            var client = new RestClient(img.Location);
            client.Authenticator = new HttpBasicAuthenticator("Api", ImageCompressionSettings.GetApiEndpointKey());

            var request = new RestRequest(Method.GET);
        
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
                                                     
            client.Timeout = 300000;

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            try
            {
                byte [] responseData = client.DownloadData(request);

                currentItem.BeginEdit();
                Media media = MediaManager.GetMedia(currentItem);
                Stream stream = new MemoryStream(responseData);
                media.SetStream(stream, currentItem.Extension);
                currentItem.EndEdit();

                currentItem.InnerItem.Editing.BeginEdit();
                //currentItem.InnerItem.Fields["Size"].Value = responseData.Length.ToString();
                currentItem.InnerItem.Fields["Keywords"].Value = "Optimised by TinyPng";
                currentItem.InnerItem.Editing.EndEdit();
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error("Could not download Image from Tiny PNG", ex);
            }
            return "API ISSUE";
        }
    }
}