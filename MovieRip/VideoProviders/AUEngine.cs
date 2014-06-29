using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class AUEngine : BaseProvider
    {
        public AUEngine(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "auengine.com/embed.php" + @"\?" + "file=(.*)");

            if (m.Success)
            {
                if (m.Groups[1].Value.Contains("&"))
                {
                    string[] spl = m.Groups[1].Value.Split('&');

                    return spl[0];
                }

                return m.Groups[1].Value;
            }

            return null;
        }

        override public string getName()
        {
            return "AUEngine";
        }

        override public string getVideoUrl(string quality = "")
        {
            string data = App.Core.getUrl(baseUrl);

            if (data == null) return null;

            System.Text.RegularExpressions.MatchCollection m = System.Text.RegularExpressions.Regex.Matches(data, "url:\x20\'(.+?)\'");

            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].Success == false) continue;

                if (m[i].Groups[1].Value.Contains("auengine.com%2Fvideos%2F"))
                {
                    return System.Web.HttpUtility.UrlDecode(m[i].Groups[1].Value);
                }
            }

            return null;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.auengine;
        }

        override public string getVideoTitle()
        {
            string data = App.Core.getUrl(this.baseUrl);

            if (data == null) return null;

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(data, "<title>(.*)</title>");

            if (m.Success)
            {
                return m.Groups[1].Value;
            }

            return null;
        }
    }
}

// http://auengine.com/embed.php?file=U7nDWKug&amp;w=480&amp;h=370
// wmode: 'transparent'},  
// );