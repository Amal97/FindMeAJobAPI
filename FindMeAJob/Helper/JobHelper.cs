using FindMeAJob.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindMeAJob.Helper
{
    public class JobHelper
    {
        public static void testProgram()
        {
            Console.WriteLine(GetJobInfo("Software","auckland","indeed"));
            Console.ReadLine();
        }

        public static int jobLength(String searchJob, String location)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.seek.co.nz/" + searchJob + "-jobs/in-" + location);
            var title = doc.DocumentNode.SelectNodes("//a[@data-automation='jobTitle']").ToList();
            var JobURL = doc.DocumentNode.SelectNodes("//a[@class='_2iNL7wI']").ToList();

            return title.Count;
        }

        public static List<Jobs> getFromIndeed(String searchJob, String location)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            String indeedURL = "https://nz.indeed.com/" + searchJob + "-jobs-in-" + location;
            HtmlAgilityPack.HtmlDocument doc = web.Load(indeedURL);

            var jobTitle = doc.DocumentNode.SelectNodes("//div[@class='title']").ToList();
            var description = doc.DocumentNode.SelectNodes("//div[@class='summary']").ToList();
            var company = doc.DocumentNode.SelectNodes("//span[@class='company']").ToList();


            String pattern = "href=\"(.*?).\"";

            List<Jobs> jobs = new List<Jobs>();
            for (int i = 0; i < description.Count; i++)
            {
                Match match = Regex.Match(jobTitle[i].InnerHtml, pattern);
                if (match.ToString() != "")
                {
                    String title = jobTitle[i].InnerText;
                    title = title.Replace("\n", "").Trim();
                    String jobLink = "https://nz.indeed.com/" + match;
                    jobLink = CleanIndeedLink(jobLink);
                    String companyName;
                    try
                    {
                        companyName = company[i].InnerText;
                        companyName = companyName.Replace("\n", "").Trim();
                    }
                    catch
                    {
                        companyName = " ";
                    }

                    String desc = description[i].InnerText;
                    desc = desc.Replace("\n", "");
                    desc = desc.Trim();

                    Jobs job = new Jobs
                    {
                        JobTitle = title,
                        WebUrl = jobLink,
                        CompanyName = companyName,
                        Location = location,
                        JobDescription = desc,
                        ImageUrl = "blank",
                        Applied = false,
                    };
                    jobs.Add(job);
                }
            }
            return jobs;
        }

        public static List<Jobs> getFromSeek(String searchJob, String location)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.seek.co.nz/" + searchJob + "-jobs/in-" + location);

            var jobTitle = doc.DocumentNode.SelectNodes("//a[@data-automation='jobTitle']").ToList();
            var description = doc.DocumentNode.SelectNodes("//div[@data-search-sol-meta]").ToList();
            var JobURL = doc.DocumentNode.SelectNodes("//a[@class='_2iNL7wI']").ToList();
            var company = doc.DocumentNode.SelectNodes("//span[@class='_3FrNV7v _3PZrylH E6m4BZb']").ToList();

            String pattern = "href=.+ c";

            List<Jobs> jobs = new List<Jobs>();
            for (int i = 0; i < description.Count; i++)
            {
                Match match = Regex.Match(JobURL[i].OuterHtml, pattern);
                if (match.ToString() != "")
                {

                    String jobLink = "https://www.seek.co.nz" + match;
                    jobLink = CleanSeekLink(jobLink);

                    String JobTitleTEST = jobTitle[i].InnerHtml.ToString();
                    String WebUrlTEST = jobLink;
                    String CompanyNameTEST = description[i].FirstChild.ChildNodes[4].InnerText;
                    String LocationTEST = location;
                    String JobDescriptionTEST = description[i].FirstChild.ChildNodes[7].InnerText;
                        
                    Jobs job = new Jobs
                    {
                        JobTitle = JobTitleTEST,
                        WebUrl = jobLink,
                        CompanyName = CompanyNameTEST,
                        Location = location,
                        JobDescription = JobDescriptionTEST,
                        ImageUrl = "blank",
                        Applied = false,
                    };
                    jobs.Add(job);
                }
            }
            return jobs;
        }

        public static List<Jobs> GetJobInfo(String searchJob, String location, String from)
        {
            List<Jobs> jobs = new List<Jobs>();
            if (from == "seek")
            {
                jobs = getFromSeek(searchJob, location);
            }
            else if(from == "indeed")
            {
                jobs = getFromIndeed(searchJob, location);
            }
            return jobs;
        }

        private static String CleanIndeedLink(String webURL)
        {
            webURL = webURL.Replace("href=\"", "");
            webURL = webURL.Replace("\"", "");
            return webURL;
        }


        private static String CleanSeekLink(String webURL)
        {
            webURL = webURL.Replace("href=\"","");
            webURL = webURL.Replace("\" c", "");
            return webURL;
        }
    }
}
