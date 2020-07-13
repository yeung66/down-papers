using System;
using System.Collections.Generic;
using System.Text;

namespace DownPapers
{
    public class Paper
    {
        public string Name { get; set; }
        public string Doi { get; set; }
        public string Path { get; set; }

        public PaperStatus Status { get; set; }

        public enum PaperStatus
        {
            Wait,Download,Finish,Failed
        }

        public static List<Paper> InitPapers()
        {
            List<Paper> papers = new List<Paper>();

            //papers.Add(new Paper()
            //{
            //    Name = "Compiler validation via equivalence modulo inputs",
            //    Doi = @"https://doi.org/10.1145/2666356.2594334",
            //    Path = @"C:\codes\c#\DownPapers\DownPapers\asdasg",
            //    //Status = "Waiting..."
            //});

            //papers.Add(new Paper()
            //{
            //    Name = "CUTE: a concolic unit testing engine for C",
            //    Doi = @"https://doi.org/10.1145/2666356.2594334",
            //    Path = @"C:\codes\c#\DownPapers\DownPapers\ssss",
            //    //Status = //"Downloading..."
            //});

            //papers.Add(new Paper()
            //{
            //    Name = "CUTE: a concolic unit testing engine for C",
            //    Doi = @"https://doi.org/10.1145/2666356.2594334",
            //    Path = @"C:\codes\c#\DownPapers\DownPapers\ssss",
            //    //Status = "Finished!!!"
            //});

            return papers;
        }
    
    }
}
