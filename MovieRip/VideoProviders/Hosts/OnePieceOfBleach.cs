using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.VideoProviders.Hosts
{
    class OnePieceOfBleach : LinkCollector
    {
        public OnePieceOfBleach(string baseUrl)
            : base(baseUrl)
        {
        }

        public static Boolean isValidLink(string url)
        {
            return url.Contains("onepieceofbleach.com");
        }

        public override string getName()
        {
            return "OnePieceOfBleach";
        }

        public override System.Drawing.Image getIcon()
        {
            return MovieRip.Properties.Resources.onepieceofbleach;
        }

        public override List<BaseProvider> getEmbeddedList()
        {
            string pageData = App.Core.getUrl(this.baseUrl);

            if (pageData == null) return null;

            List<BaseProvider> r = new List<BaseProvider>();

            System.Text.RegularExpressions.MatchCollection m = 
                System.Text.RegularExpressions.Regex.Matches(pageData, "<iframe\n(.*)></iframe>");

            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].Success)
                {
                    // Exception to the rule here
                    if (m[i].Groups[1].Value.Contains("book.php?config"))
                    {
                        System.Text.RegularExpressions.Match fb =
                            System.Text.RegularExpressions.Regex.Match(m[i].Groups[1].Value, "config=\"(.+?)\"");

                        if (fb.Success)
                        {
                            VideoProviders.BaseProvider p = App.Core.getProvider(fb.Groups[1].Value);

                            if (p != null) r.Add(p);
                        }
                    }
                    else
                    {
                        System.Text.RegularExpressions.Match fb =
                            System.Text.RegularExpressions.Regex.Match(m[i].Groups[1].Value, "src=\"(.*)\"");

                        if (fb.Success)
                        {
                            VideoProviders.BaseProvider p = App.Core.getProvider(fb.Groups[1].Value);

                            if (p != null) r.Add(p);
                        }
                    }
                }
            }

            m = System.Text.RegularExpressions.Regex.Matches(pageData, "<embed\n(.*)>");

            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].Success == false) continue;

                System.Text.RegularExpressions.Match fb =
                    System.Text.RegularExpressions.Regex.Match(m[i].Groups[1].Value, "src=\"(.+?)\"");

                if (fb.Success == false) continue;

                if (fb.Groups[1].Value.Contains("onepieceofbleach.com/player"))
                {
                    System.Text.RegularExpressions.Match op =
                        System.Text.RegularExpressions.Regex.Match(m[i].Groups[1].Value, "flashvars=\"file=(.+?)\"");

                    if (op.Success)
                    {
                        VideoProviders.BaseProvider opp = App.Core.getProvider(op.Groups[1].Value);

                        if (opp != null) r.Add(opp);
                    }

                    op = System.Text.RegularExpressions.Regex.Match(m[i].Groups[1].Value, "flashvars=\"config=(.+?)\"");

                    if (op.Success)
                    {
                        VideoProviders.BaseProvider opp = App.Core.getProvider(op.Groups[1].Value);

                        if (opp != null) r.Add(opp);
                    }

                    continue;
                }

                VideoProviders.BaseProvider p = App.Core.getProvider(fb.Groups[1].Value);

                if (p != null) r.Add(p);
            }

            return r;
        }
    }
}