using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class _4shared : BaseProvider
    {
        public _4shared(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string baseUrl)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "4shared.com/embed/(.*)/(.*)");

            if (m.Success)
            {
                return m.Groups[2].Value; // The first number does not seem significant..
            }

            m = System.Text.RegularExpressions.Regex.Match(baseUrl, "4shared.com/video/(.*)/(.*).html");

            if (m.Success)
            {
                return m.Groups[1].Value;
            }

            return null;
        }

        private Boolean isEmbeddedLink()
        {
            return baseUrl.Contains("/embed/");
        }

        override public string getName()
        {
            return "4shared";
        }

        override public string getVideoUrl(string quality = "")
        {
            if (isEmbeddedLink())
            {
                string urlDestination = App.Core.getUrlDestination(baseUrl);

                if (urlDestination == null) return null;

                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(urlDestination, "file=(.+?)&image");

                if (m.Success)
                {
                    return m.Groups[1].Value;
                }
            }
            else
            {
                string data = App.Core.getUrl(baseUrl);

                if (data == null) return null;

                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(data, "&file=(.*)\" />");

                if (m.Success)
                {
                    return m.Groups[1].Value;
                }
            }

            return null;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources._4shared;
        }

        override public string getVideoTitle()
        {
            if (isEmbeddedLink())
            {
                string urlDestination = App.Core.getUrlDestination(baseUrl);

                if (urlDestination == null) return null;

                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(urlDestination, "&image=(.*)/img/(.*)/(.*).flv");

                if (m.Success)
                {
                    return m.Groups[3].Value;
                }
            }
            else
            {
                string data = App.Core.getUrl(baseUrl);

                if (data == null) return null;

                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(data, "<meta property=\"og:title\" content=\"(.*)\" />");

                if (m.Success)
                {
                    return m.Groups[1].Value;
                }
            }

            return null;
        }
    }
}

//http://www.4shared.com/embed/1000705322/f2df3ca6
//http://static.4shared.com/flash/player/5.6/player.swf?file=http://dc429.4shared.com/img/1000705322/f2df3ca6/dlink__2Fdownload_2FqN8s3zkp_3Ftsid_3D20111230-102611-c84337bc/preview.flv&image=http://dc429.4shared.com/img/qN8s3zkp/Bleach_-_351.flv&logo.link=http://www.4shared.com/video/qN8s3zkp/Bleach_-_351.html&logo.hide=false&logo.file=http://dc429.4shared.com/images/logo.png&logo.position=top-left&plugins=sharing-2&sharing-2.link=http://www.4shared.com/video/qN8s3zkp/Bleach_-_351.html&sharing-2.code=%3Cembed%20src%3D%22http://www.4shared.com/embed/1000705322/f2df3ca6%22%20width%3D%22420%22%20height%3D%22320%22%20allowfullscreen%3D%22true%22%20allowscriptaccess%3D%22always%22%20%2F%3E&sign=7e6aa45fadcc845cb76369cac10ac770
//http://dc429.4shared.com/img/1000705322/f2df3ca6/dlink__2Fdownload_2FqN8s3zkp_3Ftsid_3D20111230-102611-c84337bc/preview.flv