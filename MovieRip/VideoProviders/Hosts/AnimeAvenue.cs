using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders.Hosts
{
    class AnimeAvenue : LinkCollector
    {
        public AnimeAvenue(string baseUrl)
            : base(baseUrl)
        {
        }

        public static Boolean isValidLink(string url)
        {
            return url.Contains("animeavenue.net");
        }

        public override string getName()
        {
            return "AnimeAvenue";
        }

        public override System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.animeavenue;
        }
    }
}