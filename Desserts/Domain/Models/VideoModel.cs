using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Models
{
    public class VideoModel
    {
        public Stream stream { get; set; }
        public string FileName { get; set; }
        public string videoData { get; set; }
        public string videoUrl { get; set; }
    }
}
