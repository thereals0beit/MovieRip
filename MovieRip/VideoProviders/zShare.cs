using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class zShare : BaseProvider
    {
        public zShare(string baseUrl)
            : base(baseUrl)
        {
        }

        override public string getName()
        {
            return "zShare";
        }

        // zshare.net/video/61815157ebe12ec6/
        // http://www.zshare.net/videoplayer/player.php?SID=dl048&FID=61815157&FN=The.Middleman.S01E01.La.Sanzione.Dell.Episodio.Pilota.iTALiAN.PDTV.XviD-SiD.avi.flv&iframewidth=642&iframeheight=419&width=640&height=385&H=61815157ebe12ec6&ISL=1

        override public string getVideoUrl(string quality = "")
        {
            return this.baseUrl;
        }

/*
 *         public override string getVideoUrls(string url)
        {
            CookieContainer cc = new CookieContainer();
            string data = SiteUtilBase.GetWebData(HttpUtility.HtmlDecode(url), cc);
            CookieCollection ccol = cc.GetCookies(new Uri("http://tmp.zshare.net"));
            if (data.Contains(@"name=""flashvars"" value="""))
            {
                data = GetSubString(data, @"name=""flashvars"" value=""", "&player");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string[] tmp = data.Split('&');
                foreach (string s in tmp)
                {
                    string[] t = s.Split('=');
                    dic[t[0]] = t[1];

                }
                string hash = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dic["filename"] + "tr35MaX25P7aY3R", "MD5").ToLower();
                string turl = @"http://" + dic["serverid"] + ".zshare.net/stream/" + dic["hash"] + '/' + dic["fileid"] + '/' +
                    dic["datetime"] + '/' + dic["filename"] + '/' + hash + '/' + dic["hnic"];
                foreach (Cookie cook in ccol)
                    CookieHelper.SetIECookie(String.Format("http://{0}.zshare.net", dic["serverid"]), cook);

                videoType = VideoType.flv;
                return turl;
            }
            return "";
        }*/

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.zshare;
        }

        override public string getVideoTitle()
        {
            return "Unknown Video";
        }
    }
}
