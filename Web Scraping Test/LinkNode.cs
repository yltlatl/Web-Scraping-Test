using System;
using System.Text.RegularExpressions;

namespace Web_Scraping_Test
{
    internal class LinkNode
    {
        #region Constructors

        public LinkNode(string link, string parentLink = null)
        {
            //validate links
            string validLink;
            try
            {
                validLink = ValidateLink(link);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }

            string validParentLink;

            //can't just pass parentLink because of argument checking in ValidateLink
            if (!string.IsNullOrEmpty(parentLink))
            {
                try
                {
                    validParentLink = ValidateLink(parentLink);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException(e.Message);
                }
            }
            else
            {
                validParentLink = parentLink;
            }

            //populate the object properties
            Link = validLink;
            ParentLink = validParentLink;
        }

        #endregion

        #region Properties

        public string ParentLink { get; private set; } //set to null if no parent
        public string Link { get; private set; }
        public bool Follow { get; set; }
        public bool Search { get; set; }

        #endregion

        #region Methods

        public static string ValidateLink(string link)
        {
            //check if the url is empty
            if (string.IsNullOrEmpty(link)) throw new ArgumentException("Null or empty link.");

            //add http:// if it is missing
            if (!Regex.IsMatch(link, "^http://|^https://", RegexOptions.IgnoreCase)) link = "http://" + link;

            //check if the url is valid by instantiating a new Uri
            try
            {
                var Uri = new Uri(link);
            }
            catch (UriFormatException)
            {
                throw new ArgumentException("Invalid URL.");
            }

            return link;
        }

        #endregion
    }
}