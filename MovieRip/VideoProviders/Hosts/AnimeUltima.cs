using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders.Hosts
{
    class AnimeUltima : LinkCollector
    {
        public AnimeUltima(string baseUrl)
            : base(baseUrl)
        {
        }

        public static Boolean isValidLink(string url)
        {
            return url.Contains("animeultima.tv");
        }

        public override string getName()
        {
            return "AnimeUltima";
        }

        public override System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.animeultima;
        }
    }
}