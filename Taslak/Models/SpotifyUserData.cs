using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class SpotifyUserData
    {
        public string country { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public Image[] images { get; set; }
        public string product { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        
    }
}