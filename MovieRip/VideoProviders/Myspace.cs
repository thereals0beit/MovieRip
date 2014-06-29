using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Myspace : BaseProvider
    {
        public Myspace(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "myspace.com/(.+?)/videos/(.+?)/(.*)");

            if (m.Success)
            {
                if (m.Groups[3].Value.Contains('#'))
                {
                    string[] spl = m.Groups[3].Value.Split('#');

                    if (spl.Length > 0)
                    {
                        return spl[0];
                    }
                }
                else
                {
                    return m.Groups[3].Value;
                }
            }

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "myspace.com/video/vid/(.*)");

            if (m.Success)
            {
                if (m.Groups[1].Value.Contains("?"))
                {
                    string[] spl = m.Groups[1].Value.Split('?');

                    if (spl.Length > 0)
                    {
                        return spl[0];
                    }
                }
                else
                {
                    return m.Groups[1].Value;
                }
            }

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "myspace.com/services/media/embed.aspx/m=(.*)");

            if (m.Success)
            {
                if (m.Groups[1].Value.Contains(","))
                {
                    string[] spl = m.Groups[1].Value.Split('?');

                    if (spl.Length > 0)
                    {
                        return spl[0];
                    }
                }
                else
                {
                    return m.Groups[1].Value;
                }
            }

            return null;
        }

        private System.Xml.XmlDocument getRssXml()
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string xmlContent = App.Core.getUrl("http://mediaservices.myspace.com/services/rss.ashx?type=video&videoID=" + id);

            if (xmlContent == null) return null;

            if (xmlContent.Contains("Neither user ID nor item ID is specified for RSS feed.")) return null;

            System.Xml.XmlDocument r = new System.Xml.XmlDocument();

            r.LoadXml(xmlContent);

            return r;
        }

        override public string getName()
        {
            return "Myspace";
        }

        override public string getVideoUrl(string quality = "")
        {
            System.Xml.XmlDocument xml = getRssXml();

            if (xml == null) return null;

            System.Xml.XmlNodeList src = xml.GetElementsByTagName("media:content");

            if (src == null || src.Count == 0) return null;

            return src[0].Attributes.GetNamedItem("url").Value;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.myspace;
        }

        override public string getVideoTitle()
        {
            System.Xml.XmlDocument xml = getRssXml();

            if (xml == null) return null;

            System.Xml.XmlNodeList src = xml.GetElementsByTagName("media:title");

            if (src == null || src.Count == 0) return null;

            return src[0].InnerText;
        }
    }
}

// http://www.myspace.com/440881282/videos/kaze-no-stigma-episode-1/53072688
// http://www.myspace.com/video/vid/53072688?pm_cmp=vid_OEV_M_C#ivp-comments
// http://mediaservices.myspace.com/services/media/embed.aspx/m=53072688,t=1,mt=video
// http://mediaservices.myspace.com/services/rss.ashx?type=video&videoID=53072688