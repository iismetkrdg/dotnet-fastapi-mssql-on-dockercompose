using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class ToSaveTracksIds
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RecommendationId { get; set; }
        public string PlaylistId { get; set; }
        public string[] TrackIds { get; set; }
        public DateTime At_Created { get; set; }

    }
}