using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Facebook : BaseProvider
    {
        public Facebook(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string url)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(url, @"facebook.com/video/(.+?).php\?v=(.*)");

            if (m.Success) return m.Groups[2].Value;

            m = System.Text.RegularExpressions.Regex.Match(url, "facebook.com/v/(.+?)_(.+?).mp4");

            if (m.Success) return m.Groups[1].Value;

            m = System.Text.RegularExpressions.Regex.Match(url, "facebook.com/v/(.*)");

            if (m.Success) return m.Groups[1].Value;

            m = System.Text.RegularExpressions.Regex.Match(url, "video.(.+?).(?:facebook|fbcdn).(?:com|net)/(.*)/(.*)/(.*)/(.*)_(.*).mp4");

            if (m.Success) return m.Groups[5].Value;

            return null;
        }

        override public string getName()
        {
            return "Facebook";
        }

        override public string getVideoUrl(string quality = "")
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string data = App.Core.getUrl("http://www.facebook.com/video/external_video.php?v=" + id);

            if (data == null) return null;

            data = System.Web.HttpUtility.UrlDecode(data);
            data = data.Substring(6);

            Newtonsoft.Json.Linq.JObject jsonData = App.Json.decode(data);

            if (jsonData == null) return null;

            if (jsonData["status"].ToString() != "ok") return null;

            return jsonData["content"]["video_src"].ToString();
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.facebook;
        }

        override public string getVideoTitle()
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string data = App.Core.getUrl("http://www.facebook.com/video/external_video.php?v=" + id);

            if (data == null) return null;

            data = System.Web.HttpUtility.UrlDecode(data);
            data = data.Substring(6);

            Newtonsoft.Json.Linq.JObject jsonData = App.Json.decode(data);

            if (jsonData == null) return null;

            if (jsonData["status"].ToString() != "ok") return null;

            return jsonData["content"]["video_title"].ToString();
        }
    }
}