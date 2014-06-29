using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders.Hosts
{
    class LinkCollector
    {
        protected string baseUrl = null;

        public LinkCollector(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        virtual public string getBaseUrl()
        {
            return this.baseUrl;
        }

        public virtual string getName()
        {
            return null;
        }

        public virtual System.Drawing.Image getIcon()
        {
            return null;
        }

        // This is the default video host configuration, shouldn't really change from this unless its needed

        public virtual List<VideoProviders.BaseProvider> getEmbeddedList()
        {
            string pageData = App.Core.getUrl(this.baseUrl);

            if (pageData == null) return null;

            List<BaseProvider> r = new List<BaseProvider>();

            System.Text.RegularExpressions.MatchCollection m =
                System.Text.RegularExpressions.Regex.Matches(pageData, "src=\"(.+?)\"");

            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].Success)
                {
                    VideoProviders.BaseProvider provider = App.Core.getProvider(m[i].Groups[1].Value);

                    if (provider != null)
                    {
                        r.Add(provider);
                    }
                }
            }

            m = System.Text.RegularExpressions.Regex.Matches(pageData, "src='(.+?)'");

            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].Success)
                {
                    VideoProviders.BaseProvider provider = App.Core.getProvider(m[i].Groups[1].Value);

                    if (provider != null)
                    {
                        r.Add(provider);
                    }
                }
            }


            return r;
        }
    }
}
