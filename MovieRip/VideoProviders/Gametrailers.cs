using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Gametrailers : BaseProvider
    {
        public Gametrailers(string baseUrl)
            : base(baseUrl)
        {
            //
        }

        public static string getId(string baseUrl)
        {
            string gtId = null;

            if (baseUrl.Contains("gametrailers.com/video/"))
            {
                gtId = baseUrl.Substring(baseUrl.LastIndexOf("/") + 1);
            }
            else if (baseUrl.Contains("gametrailers.com/remote_wrap.php?mid="))
            {
                gtId = baseUrl.Substring(baseUrl.LastIndexOf("=") + 1);
            }
            else if (baseUrl.Contains("gametrailers.com/player/"))
            {
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "gametrailers.com/player/(.+?).html");

                if (m.Success == false) return null;

                return m.Groups[1].Value;
            }
            else if (baseUrl.Contains("gametrailers.com/user-movie/"))
            {
                gtId = baseUrl.Substring(baseUrl.LastIndexOf("/") + 1);
            }
            else if (baseUrl.Contains("gametrailers.com/remote_wrap.php?umid="))
            {
                gtId = baseUrl.Substring(baseUrl.LastIndexOf("=") + 1);
            }

            return gtId;
        }

        private Boolean isUm()
        {
            if (baseUrl.Contains("gametrailers.com/user-movie/") || baseUrl.Contains("gametrailers.com/remote_wrap.php?umid="))
            {
                return true;
            }

            return false;
        }

        private System.Xml.XmlDocument getXml(string page)
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string info = App.Core.getUrl("http://www.gametrailers.com/neo/?page=" + page + "&movieId=" + id + "&um=" + ((isUm()) ? "1" : "0"));

            if (info == null) return null;

            System.Xml.XmlDocument r = new System.Xml.XmlDocument();

            r.LoadXml(info);

            return r;
        }

        override public string getName()
        {
            return "GameTrailers";
        }

        override public string getVideoUrl(string quality = "")
        {
            System.Xml.XmlDocument xml = getXml("xml.mediaplayer.Mediagen");

            if (xml == null) return null;

            System.Xml.XmlNodeList src = xml.GetElementsByTagName("src");

            if (src == null || src.Count == 0) return null;

            return src[0].InnerText;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.gametrailers_com;
        }

        override public string getVideoTitle()
        {
            System.Xml.XmlDocument xml = getXml("xml.mediaplayer.Mrss");

            if (xml == null) return null;

            System.Xml.XmlNodeList item = xml.GetElementsByTagName("item");

            if (item == null || item.Count == 0) return null;

            return item[0].ChildNodes[0].InnerText;
        }
    }
}