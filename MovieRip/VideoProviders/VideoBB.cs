using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class VideoBB : BaseProvider
    {
        public VideoBB(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string url)
        {
            /*
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(url, "videobb.com/video/(.*)");

            if (m.Success) return m.Groups[1].Value;

            m = System.Text.RegularExpressions.Regex.Match(url, @"videobb.com/watch_video.php\?v=(.*)");

            if (m.Success) return m.Groups[1].Value;

            return null;
            */

            return null; // Disabled until i figure out how to do the proper stuffs with the url
        }

        private Newtonsoft.Json.Linq.JObject getPlayerControlSettings()
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            this.raw = App.Core.getUrl("http://videobb.com/player_control/settings.php?v=" + id);

            if (this.raw == null) return null;

            Newtonsoft.Json.Linq.JObject r = App.Json.decode(this.raw);

            if (r == null) return null;

            string getDisplayText = null;

            try
            {
                getDisplayText = r["settings"]["messages"]["display"]["text"].ToString();
            }
            catch (Exception) { }

            if (getDisplayText == "The page or video you are looking for cannot be found.")
            {
                this.raw = null;

                return null;
            }

            return r;
        }

        override public string getName()
        {
            return "VideoBB";
        }

        override public string getVideoUrl(string quality = "")
        {
            Newtonsoft.Json.Linq.JObject settings = getPlayerControlSettings();

            if (settings == null) return null;

            Newtonsoft.Json.Linq.JToken res = settings["settings"]["res"];

            List<string> q = new List<string>();

            foreach (Newtonsoft.Json.Linq.JObject child in res.Children())
            {
                if (child["l"].ToString() == quality)
                {
                    string videoUrl = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(child["u"].ToString()));

                    App.Forms.MessageBox(videoUrl, "Url", App.Forms.eIconType.eError);

                    return videoUrl;
                }
            }

            return null;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.videobb;
        }

        override public string getVideoTitle()
        {
            Newtonsoft.Json.Linq.JObject settings = getPlayerControlSettings();

            if (settings == null) return null;

            return settings["settings"]["video_details"]["video"]["title"].ToString();
        }

        override public List<string> getQualityOptions()
        {
            Newtonsoft.Json.Linq.JObject settings = getPlayerControlSettings();

            if (settings == null) return null;

            Newtonsoft.Json.Linq.JToken res = settings["settings"]["res"];

            List<string> q = new List<string>();

            foreach (Newtonsoft.Json.Linq.JObject child in res.Children())
            {
                q.Add(child["l"].ToString());
            }

            return q;
        }
    }
}

//http://www.videobb.com/video/ufLEKBmnvROx
//http://www.videobb.com/watch_video.php?v=ufLEKBmnvROx

//http://videobb.com/player_control/settings.php?v=ufLEKBmnvROx