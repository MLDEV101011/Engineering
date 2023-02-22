using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Data;
using WPF.Models;

namespace WPF
{
    /// <summary>
    /// Interaction logic for EditDataPage.xaml
    /// </summary>
    public partial class EditDataPage : Page
    {
        DataContext context = new();

        MaterialObject? _materialObject = new();
        List<ImageObject> _images = new();
        List<ApprovedMatchObject> _approvedMatches = new();
        List<NoteObject> _notes = new();


        public EditDataPage()
        {
            InitializeComponent();
        }

        public EditDataPage(MaterialObject material)
        {

            _materialObject = context.Materials.Include(x => x.Images).Include(x => x.Notes).Include(x => x.ApprovedMatches).FirstOrDefault(v => v.Id.Equals(material.Id)) as MaterialObject;


            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleText.Text = "Edit Details for " + _materialObject?.Vendor;
            VendorTextBox.Text = _materialObject?.Vendor;
            PatternTextBox.Text = _materialObject?.Pattern;
            ColorTextBox.Text = _materialObject?.Color;
            ProdNumTextBox.Text = _materialObject?.ProductNum;
            VertRepeatTextBox.Text = _materialObject?.VertRepeat.ToString();
            HorzRepeatTextBox.Text = _materialObject?.HorzRepeat.ToString();
            RailroadedComboBox.Text = _materialObject?.Railroaded;
            DurabilityTextBox.Text = _materialObject?.Durability.ToString();
            WidthTextBox.Text = _materialObject?.Width.ToString();
            WeightTextBox.Text = _materialObject?.Weight.ToString();
            RollWidthTextBox.Text = _materialObject?.RollWidth.ToString();
            UsableWidthTextBox.Text = _materialObject?.UsableWidth.ToString();
            StretchAcrossTextBox.Text = _materialObject?.StretchAcrossWidth.ToString();
            StretchDownTextBox.Text = _materialObject?.StretchDownLength.ToString();
            RepeatAcrossTextBox.Text = _materialObject?.RepeatAcrossWidth.ToString();
            RepeatDownTextBox.Text = _materialObject?.RepeatDownLength.ToString();
            SavlageToMatchTextBox.Text = _materialObject?.SalvageToMatchOffset.ToString();
            EndOfRollTextBox.Text = _materialObject?.EndToMatchOffset.ToString();
            GetCheckBoxValues();
            GetNote();
            GetImages();
        }

        void GetImages()
        {
            if (_materialObject?.Images?.Count > 0)
            {
                foreach (var image in _materialObject.Images)
                {
                    if (image.Name == @"(MATCH POINT DETAIL VIEW)")
                    {
                        MatchPointInputTextBox.Text = image.FilePath;
                    }
                    if (image.Name == @"(V2 MATCH POINT DETAIL VIEW)")
                    {
                        V2TextBox.Text = image.FilePath;
                    }
                    if (image.Name == @"(H2 MATCH POINT DETAIL VIEW)")
                    {
                        H2TextBox.Text = image.FilePath;
                    }
                    if (image.Name == @"(V2H2 MATCH POINT DETAIL VIEW)")
                    {
                        V2H2TextBox.Text = image.FilePath;
                    }
                    if (image.Name == @"(FULL REPEAT/MATCH POINT DETAIL VIEW)")
                    {
                        FullRepeatTextBox.Text = image.FilePath;
                    }
                }
            }
        }

        void GetNote()
        {
            if (_materialObject?.Notes?.Count > 0)
            {
                foreach (var note in _materialObject?.Notes)
                {
                    NotesTextBox.Text = note.Content;
                }
            }
        }

        void GetNewCheckBoxVals()
        {
            List<string> preSortNew = new();
            List<string> preSortOld = new();

            _approvedMatches.Clear();

            foreach (var m in _materialObject?.ApprovedMatches.Distinct())
            {
                preSortOld.Add(m.MatchType);
            }

            var oldMatches = preSortOld?.OrderBy(i => i).Distinct();
            var newMatches = preSortNew?.OrderBy(i => i).Distinct();

            if(PlainCheck.IsChecked == true)
            {
                preSortNew.Add(PlainCheck.Content.ToString());
            }
            if(RandomCheck.IsChecked == true)
            {
                preSortNew.Add(RandomCheck.Content.ToString());
            }
            if(VertCheck.IsChecked == true)
            {
                preSortNew.Add(VertCheck.Content.ToString());
            }
            if(HorzCheck.IsChecked == true)
            {
                preSortNew.Add(HorzCheck.Content.ToString());
            }
            if(CenterCheck.IsChecked == true)
            {
                preSortNew.Add(CenterCheck.Content.ToString());
            }
            if(FlowCheck.IsChecked == true)
            {
                preSortNew.Add(FlowCheck.Content.ToString());
            }
            if(oldMatches != newMatches)
            {
                foreach (var m in newMatches)
                {
                    ApprovedMatchObject newMatch = new()
                    {
                        MatchType = m,
                        IsChecked = true
                    };
                    _approvedMatches.Add(newMatch);
                }
                _materialObject.ApprovedMatches = _approvedMatches;
                context.Materials.Update(_materialObject);
                context.SaveChanges();
            }


        }

        void GetCheckBoxValues()
        {
            var matches = new List<string>();
            foreach (var match in _materialObject?.ApprovedMatches.Distinct())
            {
                
                if (match.MatchType == PlainCheck.Content.ToString())
                {
                    PlainCheck.IsChecked = true;
                }
                if (match.MatchType == RandomCheck.Content.ToString())
                {
                    RandomCheck.IsChecked = true;
                }
                if (match.MatchType == VertCheck.Content.ToString())
                {
                    VertCheck.IsChecked = true;
                }
                if (match.MatchType == HorzCheck.Content.ToString())
                {
                    HorzCheck.IsChecked = true;
                }
                if (match.MatchType == CenterCheck.Content.ToString())
                {
                    CenterCheck.IsChecked = true;
                }
                if (match.MatchType == FlowCheck.Content.ToString())
                {
                    FlowCheck.IsChecked = true;
                }
            }
            ApprovedMatchingCombobox.Text = string.Join(", ", matches);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete " + _materialObject?.Vendor + "?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                context.Materials.Remove(_materialObject);
                context.SaveChanges();

                var win = Application.Current.MainWindow;
                (win as MainWindow).DataGrid.ItemsSource = null;
                (win as MainWindow).DataGrid.ItemsSource = context.Materials.ToList();

                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                window?.Close();
            }
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            _materialObject.Vendor = VendorTextBox.Text;
            _materialObject.Pattern = PatternTextBox.Text;
            _materialObject.Color = ColorTextBox.Text;
            _materialObject.ProductNum = ProdNumTextBox.Text;
            ValidateAllInputs();
            GetNewCheckBoxVals();
            GetFileInputValues();
            GetNewNote();


            context.SaveChanges();



            if (MessageBox.Show("Successfully Updated " + _materialObject.Vendor + ".", "Confirm", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                var win = Application.Current.MainWindow;
                (win as MainWindow).DataGrid.ItemsSource = null;
                (win as MainWindow).DataGrid.ItemsSource = context.Materials.ToList();

                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                window?.Close();
            }
        }

        void GetNewNote()
        {
            _notes.Clear();
            List<string> old = new();
            string oldNote = "";
            foreach (var item in _materialObject.Notes)
            {
                old.Add(item.Content);
            }
            if (old.Count > 0)
            {
                oldNote = old[0];
            }
            if (oldNote != NotesTextBox.Text)
            {
                NoteObject newNote = new()
                {
                    Content = NotesTextBox.Text,
                    CreatedDate = DateTime.Now.ToShortDateString()
                };
                _notes.Add(newNote);
                _materialObject.Notes = _notes;
                context.SaveChanges();
            }


        }

        void ValidateAllInputs()
        {
            ValidateInputForDouble(VertRepeatTextBox);
            ValidateInputForDouble(HorzRepeatTextBox);
            ValidateInputForInteger(DurabilityTextBox);
            ValidateInputForDouble(WidthTextBox);
            ValidateInputForDouble(WeightTextBox);
            ValidateInputForDouble(RollWidthTextBox);
            ValidateInputForDouble(UsableWidthTextBox);
            ValidateInputForDouble(StretchAcrossTextBox);
            ValidateInputForDouble(StretchAcrossTextBox);
            ValidateInputForDouble(RepeatAcrossTextBox);
            ValidateInputForDouble(SavlageToMatchTextBox);
            ValidateInputForDouble(RepeatDownTextBox);
            ValidateInputForDouble(EndOfRollTextBox);
        }

        

        void GetFileInputValues()
        {
            _images.Clear();
            List<string> og = new();
            foreach (var item in _materialObject.Images)
            {
                og.Add(item.FilePath);
            }
            List<string> newVals = new();
            if (!string.IsNullOrEmpty(MatchPointInputTextBox.Text))
            {
                newVals.Add(MatchPointInputTextBox.Text);
            }
            if (!string.IsNullOrEmpty(H2TextBox.Text))
            {
                newVals.Add(H2TextBox.Text);
            }
            if (!string.IsNullOrEmpty(V2TextBox.Text))
            {
                newVals.Add(V2TextBox.Text);
            }
            if (!string.IsNullOrEmpty(V2H2TextBox.Text))
            {
                newVals.Add(V2H2TextBox.Text);
            }
            if (!string.IsNullOrEmpty(FullRepeatTextBox.Text))
            {
                newVals.Add(FullRepeatTextBox.Text);
            }
            var a1 = og.OrderBy(i => i);
            var a2 = newVals.OrderBy(i => i);
            if (!a1.SequenceEqual(a2))
            {                
                if (!string.IsNullOrEmpty(MatchPointInputTextBox.Text))
                {
                    ImageObject newImg = new()
                    {
                        Name = @"(MATCH POINT DETAIL VIEW)",
                        FilePath = MatchPointInputTextBox.Text,
                    };
                    _images.Add(newImg);
                }
                if (!string.IsNullOrEmpty(H2TextBox.Text))
                {
                    ImageObject newImg = new()
                    {
                        Name = @"(V2 MATCH POINT DETAIL VIEW)",
                        FilePath = H2TextBox.Text,
                    };
                    _images.Add(newImg);
                }
                if (!string.IsNullOrEmpty(V2TextBox.Text))
                {
                    ImageObject newImg = new()
                    {
                        Name = @"(H2 MATCH POINT DETAIL VIEW)",
                        FilePath = V2TextBox.Text,
                    };
                    _images.Add(newImg);
                }
                if (!string.IsNullOrEmpty(V2H2TextBox.Text))
                {
                    ImageObject newImg = new()
                    {
                        Name = @"(V2H2 MATCH POINT DETAIL VIEW)",
                        FilePath = V2H2TextBox.Text
                    };
                    _images.Add(newImg);
                }
                if (!string.IsNullOrEmpty(FullRepeatTextBox.Text))
                {
                    ImageObject newImg = new()
                    {
                        Name = @"(FULL REPEAT/MATCH POINT DETAIL VIEW)",
                        FilePath = FullRepeatTextBox.Text,
                    };
                    _images.Add(newImg);
                }
                _materialObject.Images = _images;
                context.SaveChanges();
            }
        }

        static void ValidateInputForDouble(TextBox input)
        {
            bool result = double.TryParse(input.Text, out double i);
            if (result)
            {
                input.Text = input.Text;
            }
            else
            {
                MessageBox.Show("Input for " + input.Name + " must be a number.", "Error");
                return;
            }
        }

        static void ValidateInputForInteger(TextBox input)
        {
            bool result = int.TryParse(input.Text, out int i);
            if (result)
            {
                input.Text = input.Text;
            }
            else
            {
                bool isInt = double.TryParse(input.Text, out double d);
                if (isInt)
                {
                    MessageBox.Show("Input for " + input.Name + " must be a whole number.");
                    return;
                }
                else
                {
                    MessageBox.Show("Input for " + input.Name + " must be a whole number.");
                }
            }
        }



        private void MatchPointFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false
            };
            bool? result = openFileDialog.ShowDialog();

            ImageObject img = new();

            if (result == true)
            {
                img.Name = @"(MATCH POINT DETAIL VIEW)";
                img.FilePath = openFileDialog.FileName;
                _images.Add(img);

                MatchPointInputTextBox.Text = openFileDialog.FileName;
            }
        }

        private void V2FileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false
            };
            bool? result = openFileDialog.ShowDialog();

            ImageObject img = new();

            if (result == true)
            {
                img.Name = @"(V2 MATCH POINT DETAIL VIEW)";
                img.FilePath = openFileDialog.FileName;
                _images.Add(img);

                V2TextBox.Text = openFileDialog.FileName;
            }
        }



        private void H2FileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false
            };
            bool? result = openFileDialog.ShowDialog();

            ImageObject img = new();

            if (result == true)
            {
                img.Name = @"(H2 MATCH POINT DETAIL VIEW)";
                img.FilePath = openFileDialog.FileName;
                _images.Add(img);

                H2TextBox.Text = openFileDialog.FileName;
            }
        }

        private void V2H2FileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false
            };
            bool? result = openFileDialog.ShowDialog();

            ImageObject img = new();

            if (result == true)
            {
                img.Name = @"(V2H2 MATCH POINT DETAIL VIEW)";
                img.FilePath = openFileDialog.FileName;
                _images.Add(img);

                V2H2TextBox.Text = openFileDialog.FileName;
            }
        }

        private void FullRepeatFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false
            };
            bool? result = openFileDialog.ShowDialog();

            ImageObject img = new();

            if (result == true)
            {
                img.Name = @"(FULL REPEAT/MATCH POINT DETAIL VIEW)";
                img.FilePath = openFileDialog.FileName;
                _images.Add(img);

                FullRepeatTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
