using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace DownPapers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DGPapers.ItemsSource = source;
            inputDirectory.Text = paperGet.SaveUrl = System.IO.Path.GetFullPath("."+"/");
        }

        readonly PaperGet paperGet = new PaperGet();
        readonly List<Paper> source = Paper.InitPapers();

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var doi = inputDoi.Text;
            var paper = new Paper() { Doi = doi, Status = Paper.PaperStatus.Wait };
            source.Add(paper);
            DGPapers.Items.Refresh();

            string url = await paperGet.GetPageAsync(doi,paper);
            if (url != null)
            {
                paper.Status = Paper.PaperStatus.Download;
                DGPapers.Items.Refresh();
            }

            try
            {
                await paperGet.DownPaper(url, paper);
                DGPapers.Items.Refresh();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                paper.Status = Paper.PaperStatus.Failed;
                DGPapers.Items.Refresh();
            }
            

        }

        

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;


            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;
                
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                paperGet.SaveUrl = inputDirectory.Text = path + @"\";
            }
        }

        private void Grid_GotFocus(object sender, EventArgs e)
        {
            var doi = Clipboard.GetText();

            if (!string.IsNullOrEmpty(doi))
            {
                var match = Regex.Match(doi, @"^https?://doi\.org/[0-9.]+/[0-9.]+");
                if (match.Success)
                {
                    inputDoi.Text = doi;
                    inputDoi.Focus();
                    inputDoi.SelectionLength = inputDoi.Text.Length;
                }
            }
        }

        private void DGPapers_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;
            Point aP = e.GetPosition(datagrid);
            IInputElement obj = datagrid.InputHitTest(aP);
            DependencyObject target = obj as DependencyObject;


            while (target != null)
            {
                if (target is DataGridRow)
                {
                    break;
                }
                target = VisualTreeHelper.GetParent(target);
            }

            var paper = (Paper)(target as DataGridRow)?.Item;
            if (paper == null || paper.Status != Paper.PaperStatus.Finish) return;

            Trace.WriteLine(paper.Path);
            Process.Start("explorer", paper.Path.Substring(0,paper.Path.LastIndexOf(@"\")));
        }
    }
}
