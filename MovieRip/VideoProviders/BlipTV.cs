using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class BlipTV : BaseProvider
    {
        private System.Xml.XmlDocument xmlCache = null;

        public BlipTV(string baseUrl)
            : base(baseUrl)
        {
            //
        }

        public static string getId(string baseUrl)
        {
            if (baseUrl.Contains("blip.tv") == false)
            {
                return null;
            }

            int i = baseUrl.LastIndexOf('-');

            if (i == -1) return null;

            i++;

            string idTest = baseUrl.Substring(i);

            if (int.TryParse(idTest, out i) == false) return null;

            return idTest;
        }

        private Boolean cacheXmlFeed()
        {
            if (this.raw != null && this.raw != string.Empty) return true;

            string id = getId(this.baseUrl);

            if (id == null) return false;

            string data = App.Core.getUrl("http://blip.tv/rss/" + id);

            if (data == null || data.Contains("The item you requested was not found")) return false;

            this.raw = data;
            this.xmlCache = new System.Xml.XmlDocument();
            this.xmlCache.LoadXml(this.raw);

            if (this.xmlCache.HasChildNodes == false)
            {
                return false;
            }

            return true;
        }

        override public string getName()
        {
            return "BlipTV";
        }

        override public string getVideoUrl(string quality = "")
        {
            string useQuality = "Source";

            if (quality != "") useQuality = quality;

            if (cacheXmlFeed())
            {
                System.Xml.XmlNodeList xn = this.xmlCache.GetElementsByTagName("media:content");

                for (int i = 0; i < xn.Count; i++)
                {
                    if (xn[i].Attributes.GetNamedItem("blip:role").InnerText.Equals(useQuality))
                    {
                        return xn[i].Attributes.GetNamedItem("url").InnerText;
                    }
                }
            }

            return null;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.bliptv;
        }

        override public string getVideoTitle()
        {
            if (cacheXmlFeed() == false) return null;

            return xmlCache.GetElementsByTagName("blip:show")[0].InnerText + " - " + xmlCache.GetElementsByTagName("title")[1].InnerText;
        }

        override public List<string> getQualityOptions()
        {
            if (cacheXmlFeed() == false) return null;

            System.Xml.XmlNodeList xn = this.xmlCache.GetElementsByTagName("media:content");

            List<string> r = new List<string>();

            for (int i = 0; i < xn.Count; i++)
            {
                r.Add(xn[i].Attributes.GetNamedItem("blip:role").InnerText);
            }

            return r;
        }
    }
}
