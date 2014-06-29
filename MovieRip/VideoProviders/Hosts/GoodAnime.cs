using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders.Hosts
{
    class GoodAnime : LinkCollector
    {
        public GoodAnime(string baseUrl)
            : base(baseUrl)
        {
        }

        public static Boolean isValidLink(string url)
        {
            return url.Contains("goodanime.net");
        }

        public override string getName()
        {
            return "GoodAnime";
        }

        public override System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.goodanime;
        }
    }
}