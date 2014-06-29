using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Vimeo : BaseProvider
    {
        public Vimeo(string baseUrl)
            : base(baseUrl)
        {
            //
        }

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "vimeo.com/(.+?)");

            if (m.Success) return m.Groups[1].Value;

            return null;
        }

        private Newtonsoft.Json.Linq.JObject getVideoConfig()
        {
            string info = App.Core.getUrl(this.baseUrl);

            if (info == null) return null;

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(info, "{config:{(.*)}};");

            if (m.Success)
            {
                return App.Json.decode( "{config:{" + m.Groups[1].Value + "}}" );
            }

            return null;
        }

        private System.Xml.XmlDocument getUserStreamData(string userId, string streamId)
        {
            string rawData = App.Core.getUrl("http://vimeo.com/hubnut/load/" + userId + "?stream=" + streamId);
            if (rawData == null) return null;
            System.Xml.XmlDocument x = new System.Xml.XmlDocument();
            x.LoadXml(rawData);
            return x;
        }

        override public string getName()
        {
            return "Vimeo";
        }

        override public string getVideoUrl(string quality = "")
        {
            Newtonsoft.Json.Linq.JObject videoConfig = getVideoConfig();

            if (videoConfig == null) return null;

            string videoId = videoConfig["config"]["video"]["id"].ToString();
            string ownerId = videoConfig["config"]["video"]["owner"]["url"].ToString();

            ownerId = ownerId.Substring(ownerId.LastIndexOf("/") + 1);

            System.Xml.XmlDocument x = getUserStreamData(ownerId, "uploads");

            if (x == null) return null;

            System.Xml.XmlNodeList n = x.GetElementsByTagName("video");

            for (int i = 0; i < n.Count; i++)
            {
                if (n[i].ChildNodes[3].InnerText.Equals(videoId))
                {
                    return n[i].ChildNodes[7].InnerText;
                }
            }

            return null;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.vimeo;
        }

        override public string getVideoTitle()
        {
            Newtonsoft.Json.Linq.JObject videoConfig = getVideoConfig();

            if (videoConfig == null) return null;

            return videoConfig["config"]["video"]["title"].ToString();
        }
    }
}