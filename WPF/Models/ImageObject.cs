using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WPF.Models
{
    public class ImageObject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FilePath { get; set; }

        public int MaterialId { get; set; }
        public MaterialObject? Material { get; set; }

    }
}
