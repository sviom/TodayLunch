using System;
using System.Collections.Generic;
using System.Text;

namespace LunchLibrary.GooglePlatform
{
    public class GooglePlace
    {
        public List<PlacesResult> results { get; set; }
        public string status { get; set; }
        public string[] html_attributions { get; set; }
    }

    public class PlacesResult
    {
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string place_id { get; set; }
        public PlusCode plus_code { get; set; }
        public int price_level { get; set; }
        public double rating { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public double user_ratings_total { get; set; }
        public string vicinity { get; set; }
        public string[] types { get; set; }
        public List<Photo> photos { get; set; }
        public OpeningHours opening_hours { get; set; }

        public class Geometry
        {
            public Location location { get; set; }
            public string location_type { get; set; }
            public ViewPort viewport { get; set; }

            public class ViewPort
            {
                public Location northeast { get; set; }
                public Location southwest { get; set; }
            }
            public class Location
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }

        }

        public class PlusCode
        {
            public string compound_code { get; set; }
            public string global_code { get; set; }
        }

        public class OpeningHours
        {
            public bool open_now { get; set; }
        }

        public class Photo
        {
            public int height { get; set; }
            public int width { get; set; }
            public string[] html_attributions { get; set; }
            public string photo_reference { get; set; }
        }
    }
}
