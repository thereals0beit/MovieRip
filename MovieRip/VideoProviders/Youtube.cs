using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Youtube : BaseProvider
    {
        public Youtube(string baseUrl)
            : base(baseUrl)
        {
            //
        }

        public static string getId(string baseUrl) // Disabled until a fix is released or some shit
        {
            return null;

            /*
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, @"youtube.com/watch\?v=(.*)");

            if (m.Success)
            {
                if (m.Groups[1].Value.Contains("&"))
                {
                    return m.Groups[1].Value.Split('&')[0];
                }

                return m.Groups[1].Value;
            }

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "youtube.com/v/(.*)");

            if (m.Success) return m.Groups[1].Value;

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "youtu.be/(.*)");

            if (m.Success) return m.Groups[1].Value;

            return null;
            */
        }

        private System.Collections.Specialized.NameValueCollection getVideoInfo()
        {
            string urlData = App.Core.getUrl(this.baseUrl);

            if (urlData == null) return null;

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(urlData, "flashvars=\"(.+?)\"");

            if (m.Success == false) return null;

            string flashVars = m.Groups[1].Value;

            flashVars = System.Web.HttpUtility.HtmlDecode(flashVars);

            return System.Web.HttpUtility.ParseQueryString(flashVars);
        }

        override public string getName()
        {
            return "Youtube";
        }

        override public string getVideoUrl(string quality = "")
        {
            System.Collections.Specialized.NameValueCollection videoInfo = getVideoInfo();

            if (videoInfo == null) return null;

            string videoId = videoInfo.Get("video_id");
            string t = videoInfo.Get("t");
            string fmtDetails = videoInfo.Get("fmt_list");

            App.Forms.MessageBox(videoId + " - " + t + " - " + fmtDetails, "Datas", App.Forms.eIconType.eError);

            if (fmtDetails == null) return null;

            string[] fmtSplit = fmtDetails.Split(',');

            List<string> r = new List<string>();

            for (int i = 0; i < fmtSplit.Length; i++)
            {
                string[] fmtData = fmtSplit[i].Split('/');

                if (fmtData[1] == quality)
                {
                    return "http://www.youtube.com/get_video?video_id=" + videoId + "&t=" + t + "&fmt=" + fmtData[0];
                }
            }

            return null;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.youtube16;
        }

        override public string getVideoTitle()
        {
            return null;
        }

        override public List<string> getQualityOptions()
        {
            System.Collections.Specialized.NameValueCollection videoInfo = getVideoInfo();

            if (videoInfo == null) return null;

            string fmtDetails = videoInfo.Get("fmt_list");

            if (fmtDetails == null) return null;

            string[] fmtSplit = fmtDetails.Split(',');

            List<string> r = new List<string>();

            for (int i = 0; i < fmtSplit.Length; i++)
            {
                string[] fmtData = fmtSplit[i].Split('/');

                r.Add(fmtData[1]);
            }

            return r;
        }
    }
}