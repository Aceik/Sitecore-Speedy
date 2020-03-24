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
        private const string REMOTE_ERROR = "Tiny PNG Remote API caused an error";
        
        private const string TIMY_CONNETION_ERROR = "Could not download Image from Tiny PNG";
        private const string LOCATON_RESPONSE = "Location";

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

            var request = CreateUploadRequest(currentItem, client);

            client.Timeout = 300000;

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            try
            {
                var response2 = client.Execute<ImageUpload>(request);
                var content = response2.Content;

                response2.Data.Location = response2.Headers.ToList()
                .Find(x => x.Name == LOCATON_RESPONSE)
                .Value.ToString();
                                                                                                         
                return response2.Data;
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error(REMOTE_ERROR, ex);
                RecordError(currentItem, ex.Message);
            }
            return null;
        }

        private RestRequest CreateUploadRequest(Item currentItem, RestClient client)
        {
            var request = new RestRequest(Method.POST);
            request.Parameters.Clear();
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");

            if (currentItem != null)
            {
                var _mediaItem = new MediaItem(currentItem);

                if (_mediaItem != null)
                {
                    var imageBytes = ReadMediaStream(_mediaItem);

                    request.AddHeader("content-length", imageBytes.Length.ToString());
                    request.AddHeader("accept-encoding", "gzip, deflate");
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddHeader("Content-Disposition",
                        string.Format("file; filename=\"{0}\"; documentid={1}; fileExtension=\"{2}\"",
                        _mediaItem.Name, 1234, _mediaItem.Extension));
                    request.AddParameter("application/x-www-form-urlencoded", imageBytes, ParameterType.RequestBody);

                }
            }
            return request;
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

            try
            {
                byte [] responseData = client.DownloadData(request);
                string sizeBefore = currentItem.InnerItem.Fields["Size"].Value;

                UpdateImageFile(currentItem, responseData);

                string sizeAfter = currentItem.InnerItem.Fields["Size"].Value;

                UpdateImageInformation(currentItem, sizeBefore, sizeAfter);
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error(TIMY_CONNETION_ERROR, ex);
                RecordError(currentItem, ex.Message);
            }
            return "API ISSUE";
        }

        private void RecordError(MediaItem currentItem, string message)
        {
            currentItem.InnerItem.Editing.BeginEdit();
            currentItem.InnerItem.Fields[ImageCompressionSettings.GetInformationField()].Value = message;
            currentItem.InnerItem.Editing.EndEdit();
        }

        private void UpdateImageFile(MediaItem currentItem, byte[] responseData)
        {
            currentItem.BeginEdit();
            Media media = MediaManager.GetMedia(currentItem);
            Stream stream = new MemoryStream(responseData);
            media.SetStream(stream, currentItem.Extension);
            currentItem.EndEdit();
        }

        private void UpdateImageInformation(MediaItem currentItem, string sizeBefore, string sizeAfter)
        {
            currentItem.InnerItem.Editing.BeginEdit();
            string sizeBeforeStr = $"Size before {SizeSuffix(Int64.Parse(sizeBefore))}";
            string sizeAfterStr = $"Size after {SizeSuffix(Int64.Parse(sizeAfter))}";
            currentItem.InnerItem.Fields[ImageCompressionSettings.GetInformationField()].Value = $"{ImageCompressionConstants.Messages.OPTIMISED_BY} \n\r {sizeBeforeStr} \n\r {sizeAfterStr}";
            currentItem.InnerItem.Editing.EndEdit();
        }

        static double ConvertBytesToMegabytes(string bytes)
        {
            return (Int32.Parse(bytes) / 1024f) / 1024f;
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
    }
}