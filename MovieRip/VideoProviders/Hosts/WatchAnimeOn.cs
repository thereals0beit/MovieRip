using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders.Hosts
{
    class WatchAnimeOn : LinkCollector
    {
        public WatchAnimeOn(string baseUrl)
            : base(baseUrl)
        {
        }

        public static Boolean isValidLink(string url)
        {
            return url.Contains("watchanimeon.com");
        }

        public override string getName()
        {
            return "WatchAnimeOn";
        }

        public override System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.watchanimeon;
        }

        public override List<VideoProviders.BaseProvider> getEmbeddedList()
        {
            string pageData = App.Core.getUrl(this.baseUrl);

            if (pageData == null) return null;

            List<BaseProvider> r = new List<BaseProvider>();

            string internalEncoded = App.Core.getInternalString(pageData, "document.write(unescape(\"%3c%65%6d%62%65%64", "\"));");

            string decodedUrl = System.Web.HttpUtility.UrlDecode(internalEncoded);

            System.Text.RegularExpressions.Match internalSrc =
                System.Text.RegularExpressions.Regex.Match(decodedUrl, "src=\"(.*)\"");

            if (internalSrc.Success)
            {
                if (internalSrc.Groups[1].Value.Substring(0, 4).Equals("http"))
                {
                    VideoProviders.BaseProvider provider = App.Core.getProvider(decodedUrl);

                    if (provider != null)
                    {
                        r.Add(provider);
                    }
                }
            }

            System.Text.RegularExpressions.MatchCollection m = 
                System.Text.RegularExpressions.Regex.Matches(pageData, "<a href=\"(.+?)\" rel=\"nofollow\">");

            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].Success)
                {
                    if (m[i].Groups[1].Value.Contains("watchanimeon")) continue; // Filter official links
                    if (m[i].Groups[1].Value.Substring(0, 4).Equals("http") == false) continue;
                    if (m[i].Groups[1].Value.Contains("megaupload")) continue;

                    VideoProviders.BaseProvider provider = App.Core.getProvider(m[i].Groups[1].Value);

                    if (provider != null)
                    {
                        r.Add(provider);
                    }
                }
            }

            return r;
        }
    }
}
