using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WPF.Models
{
    public class ApprovedMatchObject
    {
        public int Id { get; set; }
        public string? MatchType { get; set; }
        public bool IsChecked { get; set; }

        public int MaterialId { get; set; }
        public virtual MaterialObject? Material { get; set; }

    }
}
