using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders
{
    class BaseProvider
    {
        protected string baseUrl = null;
        protected string raw = null;

        public BaseProvider(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        virtual public string getBaseUrl()
        {
            return this.baseUrl;
        }

        virtual public string getName()
        {
            return null;
        }

        virtual public string getVideoUrl(string quality = "")
        {
            return null;
        }

        virtual public System.Drawing.Image getIcon()
        {
            return null;
        }

        virtual public string getVideoTitle()
        {
            return null;
        }

        virtual public List<string> getQualityOptions()
        {
            return null;
        }
    }
}
