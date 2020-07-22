using System.Collections.Generic;

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
            return papers;
        }
    
    }
}
