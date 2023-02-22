using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF.Data;
using WPF.Models;


namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataContext context = new();

        List<MaterialObject> materials = new();
        List<NoteObject> notes = new();
        List<ApprovedMatchObject> approvedMatches = new();
        List<ImageObject> images = new();
        List<string> matches = new();
        List<ImageObject> newImages = new();

        public DataGrid DataGrid { get; set; }

        string searchTerm = "";

        List<MaterialObject> SearchResults = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = context;
            materials = context.Materials.Include(
                x => x.ApprovedMatches).Include(
                x => x.Images).Include
                (x => x.Notes).ToList();
            DataGrid = MaterialDataGrid;
            MaterialDataGrid.ItemsSource = materials;
        }

        private void GetCheckBoxValues()
        {
            matches.Clear();
            if (PlainCheck.IsChecked == true)
            {
                matches.Add(PlainCheck.Content.ToString());
            }
            if (RandomCheck.IsChecked == true)
            {
                matches.Add(RandomCheck.Content.ToString());
            }
            if (VertCheck.IsChecked == true)
            {
                matches.Add(VertCheck.Content.ToString());
            }
            if (HorzCheck.IsChecked == true)
            {
                matches.Add(HorzCheck.Content.ToString());
            }
            if (CenterCheck.IsChecked == true)
            {
                matches.Add(CenterCheck.Content.ToString());
            }
            if (FlowCheck.IsChecked == true)
            {
                matches.Add(FlowCheck.Content.ToString());
            }
        }


        private void SearchEntry_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchEntry.Text = string.Empty;
            SearchEntry.Foreground = new SolidColorBrush(Colors.Black);

        }

        private void SearchEntry_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchEntry.Text = "Search";
            SearchEntry.Foreground = new SolidColorBrush(Colors.Gray);

        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTerm = SearchEntry.Text;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Search Word Needed.", "Error");
            }
            else
            {
                SearchResults.Clear();
                SearchResults.AddRange(context.Materials.ToList().Where(x =>
                    x.Vendor.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Pattern.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Color.Contains(searchTerm.ToLower()) ||
                    x.ProductNum.ToLower().Equals(searchTerm.ToLower())));
                MaterialDataGrid.ItemsSource = null;
                MaterialDataGrid.ItemsSource = SearchResults;
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchEntry.Text = "Search";
            SearchEntry.Foreground = new SolidColorBrush(Colors.Gray);
            SearchResults.Clear();
            MaterialDataGrid.ItemsSource = null;
            MaterialDataGrid.ItemsSource = context.Materials.ToList();
        }
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {            
            // get selected object
            var selectedMaterialObj = MaterialDataGrid.SelectedItem as MaterialObject;

            //selectedMaterialObj.Images = images.Where(x => x.MaterialId == selectedMaterialObj.Id).ToList();
            //selectedMaterialObj.Notes = notes.Where(x => x.MaterialId == selectedMaterialObj.Id).ToList();
            //selectedMaterialObj.ApprovedMatches = approvedMatches.Where(x => x.MaterialId == selectedMaterialObj.Id).ToList();
            DetailsWindow detailsWindow = new(selectedMaterialObj);
            detailsWindow.Show();
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            materials = context.Materials.ToList();
            approvedMatches = context.ApprovedMatches.ToList();
            notes = context.Notes.ToList();
            images = context.Images.ToList();
            //MaterialDataGrid.ItemsSource = materials; 
        }        

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearEntries();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            
            var material = new MaterialObject()
            {
                Vendor = VendorEntry.Text,
                Pattern = PatternEntry.Text,
                Color = ColorEntry.Text,
                ProductNum = ProdNumEntry.Text,
                VertRepeat = double.Parse(VertRepeatEntry.Text),
                HorzRepeat = double.Parse(HorzRepeatEntry.Text),
                Railroaded = RailroadedComboBox.Text,
                Durability = int.Parse(DurabilityEntry.Text),
                Backing = BackingComboBox.Text,
                Width = double.Parse(WidthEntry.Text),
                Weight = double.Parse(WeightEntry.Text),
                RollWidth = double.Parse(RollWidthEntry.Text),
                UsableWidth = double.Parse(UsableWidthEntry.Text),
                StretchAcrossWidth = double.Parse(StretchAcrossEntry.Text),
                StretchDownLength = double.Parse(StretchDownEntry.Text),
                RepeatAcrossWidth = double.Parse(RepeatAcrossEntry.Text),
                RepeatDownLength = double.Parse(RepeatDownEntry.Text),
                EndToMatchOffset = double.Parse(EndToMatchEntry.Text),
                SalvageToMatchOffset = double.Parse(SalvageToMatchEntry.Text),
                Images = newImages
            };

            
            GetCheckBoxValues();
            foreach (var x in matches)
            {
                ApprovedMatchObject am = new()
                {
                    MatchType = x,
                    IsChecked = true
                };
                material?.ApprovedMatches?.Add(am);
            }

            context.Materials.Add(material);
            context.SaveChanges();

            ClearEntries();
            MaterialDataGrid.ItemsSource = null;
            MaterialDataGrid.ItemsSource = context.Materials.ToList();
        }



        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
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
                newImages.Add(img);

                FileInputTextBox.Text = openFileDialog.FileName;
            }
        }

        private void ClearEntries()
        {
            VendorEntry.Text = string.Empty;
            PatternEntry.Text = string.Empty;
            ColorEntry.Text = string.Empty;
            ProdNumEntry.Text = string.Empty;
            VertRepeatEntry.Text = string.Empty;
            HorzRepeatEntry.Text = string.Empty;
            RailroadedComboBox.Text = string.Empty;
            BackingComboBox.Text = string.Empty;
            DurabilityEntry.Text = string.Empty;
            WidthEntry.Text = string.Empty;
            WeightEntry.Text = string.Empty;
            RollWidthEntry.Text = string.Empty;
            UsableWidthEntry.Text = string.Empty;
            StretchAcrossEntry.Text = string.Empty;
            StretchDownEntry.Text = string.Empty;
            RepeatAcrossEntry.Text = string.Empty;
            RepeatDownEntry.Text = string.Empty;
            SalvageToMatchEntry.Text = string.Empty;
            FileInputTextBox.Text = string.Empty;
            EndToMatchEntry.Text = string.Empty;    
            V2TextBox.Text = string.Empty;
            H2TextBox.Text = string.Empty;
            V2H2TextBox.Text = string.Empty;
            FullRepeatTextBox.Text = string.Empty;
            NotesEntry.Text = string.Empty;
            ApprovedMatchingCombobox.Text = string.Empty;

            notes.Clear();
            approvedMatches.Clear();
            newImages.Clear();

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
                newImages.Add(img);

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
                newImages.Add(img);

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
                newImages.Add(img);

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
                newImages.Add(img);

                FullRepeatTextBox.Text = openFileDialog.FileName;
            }
        }

        private void SearchEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Enter)
            {
                if(SearchEntry.Text.Length > 0)
                {
                    Search();
                }
                else
                {
                    MessageBox.Show("Search field is empty.", "Error");
                }
            }
        }
    }
}
