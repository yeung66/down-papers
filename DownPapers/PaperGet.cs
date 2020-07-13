using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace DownPapers
{
    public class PaperGet
    {
        readonly string resourceUrl = @"https://sci-hub.st/";
        public string SaveUrl { get; set; } = @"./";

        readonly HttpClient client = new HttpClient();

        public async Task<String> GetPageAsync(string doi, Paper paper)
        {
            string url = resourceUrl + doi;

            var resp = await client.GetAsync(url);
            //var paper = new Paper() { Doi = doi,Status=Paper.PaperStatus.Wait };
            
            if (resp.IsSuccessStatusCode)
            {
                var html = await resp.Content.ReadAsStringAsync();
                var xmldoc = new HtmlDocument();
                xmldoc.LoadHtml(html);
                paper.Name = xmldoc.DocumentNode.SelectSingleNode("//head/title").InnerText.Split("|")?[1].Trim();
                string pdfUrl = xmldoc.DocumentNode.SelectSingleNode(@"//div[@id='article']/iframe").GetAttributeValue("src","");

                return pdfUrl;
                //await DownPaper(pdfUrl,paper);

            }

            return null;
            
        }

        public async Task DownPaper(string url,Paper paper)
        {
            var resp = await client.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                using var stream = await resp.Content.ReadAsStreamAsync();
                using var f = File.Create(SaveUrl+paper.Name+".pdf");
                {
                    byte[] temp = new byte[1024];
                    int c;
                    while ((c = stream.Read(temp,0,temp.Length)) > 0)
                    {
                        f.Write(temp, 0, c);
                    }
                    
                }

                paper.Path = Path.GetFullPath(SaveUrl + paper.Name + ".pdf");
                paper.Status = Paper.PaperStatus.Finish;
            }
        }


    }

    
}
