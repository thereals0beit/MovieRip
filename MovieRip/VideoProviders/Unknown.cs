using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class Unknown : BaseProvider
    {
        public Unknown(string baseUrl)
            : base(baseUrl)
        {
        }

        override public string getBaseUrl()
        {
            return this.baseUrl;
        }

        override public string getName()
        {
            return "Unknown Provider";
        }

        override public string getVideoUrl(string quality = "")
        {
            return this.baseUrl;
        }

        override public System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.yellow_light_large;
        }

        override public string getVideoTitle()
        {
            return "Unknown Video";
        }

        override public List<string> getQualityOptions()
        {
            return null;
        }
    }
}

// http://bl.rutube.ru/b038d70fb0ce6c7b21506c0ce91572d5.f4m?max-age=0&schema=rtmp