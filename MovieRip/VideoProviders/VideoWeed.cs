using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class VideoWeed : BaseProvider
    {
        public VideoWeed(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "embed.videoweed.(.?es|com)/embed.php(.*)v=(.+?)&??");

            if (m.Success) return m.Groups[2].Value;

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "videoweed.(.?es|com)/file/(.*)");

            if (m.Success) return m.Groups[2].Value;

            return null;
        }

        private System.Collections.Specialized.NameValueCollection getPlayerApi(string id, string key)
        {
            string queryValue = App.Core.getUrl("http://www.videoweed.es/api/player.api.php?key=" + key + "&file=" + id);

            if (queryValue == null) return null;

            return System.Web.HttpUtility.ParseQueryString(queryValue);
        }

        override public string getName()
        {
            return "VideoWeed";
        }

        override public string getVideoUrl(string quality = "")
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string baseContent = App.Core.getUrl(this.baseUrl);

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

            string baseContent = App.Core.getUrl(this.baseUrl);

            if (baseContent == null) return null;

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseContent, "flashvars.filekey=\"(.+?)\";");

            if (m.Success == false) return null;

            string fileKey = m.Groups[1].Value;

            System.Collections.Specialized.NameValueCollection api = getPlayerApi(id, fileKey);

            if (api == null) return null;

            return System.Web.HttpUtility.UrlDecode(api.Get("title"));
        }

    }
}