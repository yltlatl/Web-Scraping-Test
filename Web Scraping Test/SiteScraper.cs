using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace Web_Scraping_Test
{
    public class SiteScraper
    {
        #region Constructors

        //default constructor
        public SiteScraper()
        {
            //initialize properties
            LinkTree = new List<LinkNode>();
        }

        public SiteScraper(string url)
            //take care of any initializing
            : this()
        {
            //start the link tree
            try
            {
                LinkTree.Add(new LinkNode(url));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            //get the links from the current page
            HtmlDocument page = GetPage(url);
            IEnumerable<string> pageLinks = ExtractLinks(page, url);
            //add these links to the LinkTree
            foreach (string link in pageLinks)
            {
                try
                {
                    LinkTree.Add(new LinkNode(link, url));
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        private List<LinkNode> LinkTree { get; set; }

        private string CurrentLink { get; set; }

        #endregion

        #region Methods

        public List<string> GetLinkList(string parentLink)
        {
            //select the LinkNodes from LinkTree with parentLink as their ParentLink
            IEnumerable<string> branch = from link in LinkTree
                where link.ParentLink == parentLink
                select link.Link;

            //convert IEnumerable<string> to List<string>
            List<string> newBranch = branch.ToList();

            return newBranch;
        }

        private HtmlDocument GetPage(string url)
        {
            var page = new HtmlDocument();
            var htmlWeb = new HtmlWeb();
            try
            {
                page = htmlWeb.Load(url);
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }

            return page;
        }

        private IEnumerable<string> ExtractLinks(HtmlDocument page, string baseUrl)
        {
            //initialize links
            var links = new List<string>();

            //get the link nodes in the page
            HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//a");
            if (nodes == null) return links;

            //extract the URLs from the link nodes
            foreach (HtmlNode node in nodes)
            {
                string link = node.GetAttributeValue("href", string.Empty);

                //ignore links on same page, empty links, and duplicate links
                if ((link.Contains("#") || string.IsNullOrEmpty(link) || links.Contains<string>(link)))
                {
                    continue;
                }

                //for relative links, prepend the base URL and make sure we still have a valid link
                if (link.StartsWith("/"))
                {
                    link = baseUrl + link;
                    link = LinkNode.ValidateLink(link);
                    //if the link returned is empty, alert the user
                    if (string.IsNullOrEmpty(link)) Console.WriteLine("Error: invalid relative link extracted.");
                }

                links.Add(link);
            }

            return links;
        }

        #endregion
    }
}