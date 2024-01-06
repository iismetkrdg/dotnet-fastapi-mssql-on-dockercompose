using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class RecommendationModel
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string[] PlaylistIds { get; set; }
    }
}