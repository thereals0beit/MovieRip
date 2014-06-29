using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Dailymotion : BaseProvider
    {
        public Dailymotion(string baseUrl)
            : base(baseUrl)
        {
            //
        }

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "dailymotion.com/video/(.+?)_");

            if (m.Success) return m.Groups[1].Value;

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "dailymotion.com/swf/(.*)");

            if (m.Success)
            {
                if (m.Groups[1].Value.Contains("&"))
                {
                    string[] spl = m.Groups[1].Value.Split('&');

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

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "dailymotion.com/embed/video/(.*)");

            if (m.Success) return m.Groups[1].Value;

            return null;
        }

        override public string getName()
        {
            return "Dailymotion";
        }

        override public string getVideoUrl(string quality = "")
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string information = App.Core.getUrl("http://www.dailymotion.com/json/video/" + id + "?fields=" + quality);

            if (information == null) return null;

            Newtonsoft.Json.Linq.JObject jsonObj = App.Json.decode(information);

            if (jsonObj == null) return null;

            return jsonObj[quality].ToString();
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.dailymotion1;
        }

        override public string getVideoTitle()
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string information = App.Core.getUrl("http://www.dailymotion.com/json/video/" + id + "?fields=title");

            if (information == null) return null;

            Newtonsoft.Json.Linq.JObject jsonObj = App.Json.decode(information);

            if (jsonObj == null) return null;

            return jsonObj["title"].ToString();
        }

        override public List<string> getQualityOptions()
        {
            string id = getId(this.baseUrl);

            if (id == null) return null;

            string information = App.Core.getUrl("http://www.dailymotion.com/json/video/" + id + "?fields=stream_h264_sd_url,stream_h264_url,stream_h264_hd_url");

            if (information == null) return null;

            List<string> returnVariable = new List<string>();

            Newtonsoft.Json.Linq.JObject jsonObj = App.Json.decode(information);

            if (jsonObj == null) return returnVariable;

            if (jsonObj["stream_h264_url"].Type.ToString().Equals("Null") == false)
            {
                returnVariable.Add("stream_h264_url");
            }

            if (jsonObj["stream_h264_sd_url"].Type.ToString().Equals("Null") == false)
            {
                returnVariable.Add("stream_h264_sd_url");
            }

            if (jsonObj["stream_h264_hd_url"].Type.ToString().Equals("Null") == false)
            {
                returnVariable.Add("stream_h264_hd_url");
            }

            return returnVariable;
        }
    }
}
