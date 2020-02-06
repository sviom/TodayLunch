using LunchLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LunchLibrary.GooglePlatform
{
    public class PlaceAPI
    {
        public static HttpClient httpClient = new HttpClient();

        public const string GEOCODING_BASE_URL =
            "https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}";

        private const string NEARBY_BASE_URL =
            "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&radius=500&type=restaurant&language=ko&key={2}"; //&keyword=cruise

        public static string API_KEY { get; set; }

        public static async Task<GeometryModel> GetAddressGeometry(string address)
        {
            try
            {
                if (string.IsNullOrEmpty(address))
                    return null;

                if (string.IsNullOrEmpty(API_KEY))
                    return null;

                var url = string.Format(GEOCODING_BASE_URL, address, API_KEY);
                var httpResponse = await httpClient.GetAsync(url);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var contentString = await httpResponse.Content.ReadAsStringAsync();
                    var geometry = JsonConvert.DeserializeObject<GeometryModel>(contentString);

                    return geometry;
                }
            }
            catch (Exception ex)
            {
                //Log.Report(ex);
            }
            return null;
        }

        public static async Task<GooglePlace> GetNearbyPlacesAsync(double? lat, double? lng)
        {
            try
            {
                if (lat == null || lng == null)
                    return null;

                if (string.IsNullOrEmpty(API_KEY))
                    return null;

                var url = string.Format(NEARBY_BASE_URL, lat, lng, API_KEY);
                var httpResponse = await httpClient.GetAsync(url);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var contentString = await httpResponse.Content.ReadAsStringAsync();
                    var googlePlaces = JsonConvert.DeserializeObject<GooglePlace>(contentString);

                    return googlePlaces;
                }
            }
            catch (Exception ex)
            {
                //Log.Report(ex);
            }
            return null;
        }
    }
}