using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Models;

namespace WPF
{
    public class MaterialObject
    {
        public int Id { get; set; }
        public string? Vendor { get; set; }
        public string? Pattern { get; set; }
        public string? Color { get; set; }
        public string? ProductNum { get; set; }
        public double VertRepeat { get; set; }
        public double HorzRepeat { get; set; }
        public string? Railroaded { get; set; }
        public string? Backing { get; set; }
        public int Durability { get; set; }
        public double Width { get; set; }
        public double Weight { get; set; }
        public double RollWidth { get; set; }
        public double UsableWidth { get; set; }
        public double StretchAcrossWidth { get; set; }
        public double StretchDownLength { get; set; }
        public double RepeatAcrossWidth { get; set; }
        public double SalvageToMatchOffset { get; set; }
        public double RepeatDownLength { get; set; }
        public double EndToMatchOffset { get; set; }
        public virtual ICollection<ApprovedMatchObject>? ApprovedMatches { get;  set; } = new ObservableCollection<ApprovedMatchObject>();
        public virtual ICollection<ImageObject>? Images { get;  set; } = new ObservableCollection<ImageObject>();
        public virtual ICollection<NoteObject>? Notes { get;  set; } = new ObservableCollection<NoteObject>();
    }
}
