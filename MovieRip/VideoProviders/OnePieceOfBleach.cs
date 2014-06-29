using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class OnePieceOfBleach : BaseProvider
    {
        public OnePieceOfBleach(string baseUrl)
            : base(baseUrl)
        {
        }

        public static string getId(string baseUrl)
        {
            if (baseUrl.Contains("onepieceofbleach.com/sapo.php"))
            {
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(baseUrl, "id=(.+?)&");

                if (m.Success) return m.Groups[1].Value;
            }
            else if (baseUrl.Contains("onepieceofbleach.com/xml.php"))
            {
                return "1"; // No ID required for any purpose really lol
            }
            else if (baseUrl.Contains("onepieceofbleach.com/v.php"))
            {
                return "1";
            }

            return null;
        }

        override public string getName()
        {
            return "OnePieceOfBleach Player";
        }

        override public string getVideoUrl(string quality = "")
        {
            if (baseUrl.Contains("onepieceofbleach.com/sapo.php"))
            {
                return App.Core.getUrlDestination(this.baseUrl);
            }
            else if (baseUrl.Contains("onepieceofbleach.com/xml.php"))
            {
                return this.baseUrl;
            }
            else if (baseUrl.Contains("onepieceofbleach.com/v.php"))
            {
                return this.baseUrl;
            }

            return null;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.onepieceofbleach;
        }

        override public string getVideoTitle()
        {
            return "OnePieceOfBleach Player";
        }
    }
}
