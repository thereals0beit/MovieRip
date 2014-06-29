using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Veoh : BaseProvider
    {
        public Veoh(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m =
                System.Text.RegularExpressions.Regex.Match(baseUrl, "veoh.com/watch/(.*)");

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

            return null;
        }

        override public string getName()
        {
            return "Veoh";
        }

        override public string getVideoUrl(string quality = "")
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            // TODO: Fix this rofl

            string dest = App.Core.getUrlDestination("http://www.animeultima.tv/getveoh2.php?id=" + id);

            if (dest == null) return null;

            return dest;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.veoh_com;
        }

        override public string getVideoTitle()
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string data = App.Core.getUrl("http://veoh.com/watch/" + id);

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(data, "<meta name=\"og:title\" content=\"(.+?)\"");

            if (m.Success)
            {
                return m.Groups[1].Value;
            }

            return null;
        }
    }
}