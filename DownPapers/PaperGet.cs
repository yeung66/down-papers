using HtmlAgilityPack;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DownPapers
{
    public class PaperGet
    {
        readonly string resourceUrl = @"https://sci-hub.st/";
        public string SaveUrl { get; set; } = @".\";

        readonly HttpClient client = new HttpClient();

        public async Task<String> GetPageAsync(string doi, Paper paper)
        {
            string url = resourceUrl + doi;

            var resp = await client.GetAsync(url);

            if (resp.IsSuccessStatusCode)
            {
                var html = await resp.Content.ReadAsStringAsync();
                var xmldoc = new HtmlDocument();
                xmldoc.LoadHtml(html);
                paper.Name = xmldoc.DocumentNode.SelectSingleNode("//head/title").InnerText.Split("|")?[1].Trim();
                string pdfUrl = xmldoc.DocumentNode.SelectSingleNode(@"//div[@id='article']/iframe").GetAttributeValue("src", "");

                if (pdfUrl.StartsWith("//")) pdfUrl = "https:" + pdfUrl;

                return pdfUrl;

            }

            return null;

        }

        public async Task DownPaper(string url, Paper paper)
        {
            var resp = await client.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                string filename = paper.Name;

                if (!IsValidFileName(paper.Name))
                {
                    DateTime dateTime = DateTime.Now.ToLocalTime();
                    filename = "temp-" + dateTime.ToString("yyyyMMddhhmmss");
                    paper.Name = string.IsNullOrEmpty(paper.Name) ? filename : paper.Name;
                }
                using var stream = await resp.Content.ReadAsStreamAsync();
                using var f = File.Create(SaveUrl + filename + ".pdf");
                {
                    byte[] temp = new byte[1024];
                    int c;
                    while ((c = stream.Read(temp, 0, temp.Length)) > 0)
                    {
                        f.Write(temp, 0, c);
                    }

                }

                paper.Path = Path.GetFullPath(SaveUrl + filename + ".pdf");
                paper.Status = Paper.PaperStatus.Finish;
            }
        }

        private bool IsValidFileName(string filename)
        {
            return !(string.IsNullOrEmpty(filename) || Regex.IsMatch(filename, "[\\/:*?\"<>|]"));
        }


    }


}
