using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class NovaMov : BaseProvider
    {
        public NovaMov(string baseUrl)
            : base(baseUrl)
        {
        }

        // http://embed.novamov.com/embed.php?width=718&#038;height=480&#038;v=jr5iimxz8p8ns
        // http://www.novamov.com/api/player.api.php?key=99%2E133%2E150%2E60%2D61fc92a892cc5ce8eeaaab0acc115daa&user=undefined&codes=undefined&pass=undefined&file=

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "embed.novamov.com/embed.php(.*)v=(.*)&??");

            if (m.Success) return m.Groups[2].Value;

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, @"novamov.com/file/(.*)");

            if (m.Success) return m.Groups[2].Value;

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, @"novamov.com/player.php\?v=(.*)");

            return null;
        }

        private System.Collections.Specialized.NameValueCollection getPlayerApi(string id, string key)
        {
            string queryValue = App.Core.getUrl("http://www.novamov.com/api/player.api.php?key=" + key + "&file=" + id);

            if (queryValue == null) return null;

            return System.Web.HttpUtility.ParseQueryString(queryValue);
        }

        override public string getName()
        {
            return "NovaMov";
        }

        override public string getVideoUrl(string quality = "")
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string baseContent = App.Core.getUrl("http://novamov.com/player.php?v=" + id);

            if (baseContent == null) return null;

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseContent, "flashvars.filekey=\"(.+?)\";");

            if (m.Success == false) return null;

            string fileKey = m.Groups[1].Value;

            System.Collections.Specialized.NameValueCollection api = getPlayerApi(id, fileKey);

            if (api == null) return null;

            return api.Get("url");
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.videoweed;
        }

        override public string getVideoTitle()
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string baseContent = App.Core.getUrl("http://novamov.com/player.php?v=" + id);

            if (baseContent == null) return null;

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseContent, "flashvars.filekey=\"(.+?)\";");

            if (m.Success == false) return null;

            string fileKey = m.Groups[1].Value;

            System.Collections.Specialized.NameValueCollection api = getPlayerApi(id, fileKey);

            if (api == null) return null;

            string finalTitle = System.Web.HttpUtility.UrlDecode(api.Get("title"));

            if (finalTitle.Contains("&asdasdas"))
            {
                finalTitle = finalTitle.Substring(0, finalTitle.IndexOf("&asdasdas"));
            }

            return finalTitle;
        }

    }
}