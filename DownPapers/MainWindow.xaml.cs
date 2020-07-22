using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;

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

        PaperGet paperGet = new PaperGet();
        List<Paper> source = Paper.InitPapers();

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
    }
}
