using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WPF.Models
{
    public class NoteObject
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? CreatedDate { get; set; }
        public int MaterialId { get; set; }
        public virtual MaterialObject? Material { get; set; }

    }
}
