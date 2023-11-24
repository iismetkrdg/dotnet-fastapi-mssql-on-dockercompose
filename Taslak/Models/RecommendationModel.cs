using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class RecommendationModel
    {
        public string Id { get; set; }
        public string[] PlaylistIds { get; set; }
    }
}