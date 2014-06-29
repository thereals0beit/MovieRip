using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace MovieRip.App
{
    class Core
    {
        public static string getUrl(string url, string cookieJar = null)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                
                System.Net.WebRequest wr = System.Net.WebRequest.Create(url);
                
                ((System.Net.HttpWebRequest)wr).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.63 Safari/535.7";
                
                if (cookieJar != null) { wr.Headers["Cookie"] = cookieJar; }
                
                System.Net.HttpWebResponse ws = (System.Net.HttpWebResponse) wr.GetResponse();
                
                System.IO.Stream s = ws.GetResponseStream();
                
                System.IO.StreamReader sr = new System.IO.StreamReader(s, System.Text.Encoding.ASCII);
                
                string r = sr.ReadToEnd();
                
                sr.Close();

                s.Close();
                
                ws.Close();

                return r;
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        public static string getUrlDestination(string url)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            System.Net.WebRequest wr = System.Net.WebRequest.Create(url);
            System.Net.HttpWebResponse ws = (System.Net.HttpWebResponse)wr.GetResponse();
            ws.Close();
            return ws.ResponseUri.ToString();
        }

        public static string getFilenameFromWebPath(string url)
        {
            Int32 i = url.LastIndexOf("/");
            if (i <= 0) return null;
            return url.Substring(i + 1);
        }

        public static string getInternalString(string raw, string before, string after)
        {
            Int32 idxOfRaw = raw.IndexOf(before);

            if (idxOfRaw == -1) return null;

            idxOfRaw += before.Length;

            string examineString = raw.Substring(idxOfRaw);

            Int32 idxOfNew = examineString.IndexOf(after);

            if (idxOfNew == -1) return null;

            return examineString.Substring(0, idxOfNew);
        }

        public static VideoProviders.BaseProvider getProvider(string url)
        {
            if (VideoProviders.Dailymotion.getId(url) != null)
            {
                return new VideoProviders.Dailymotion(url);
            }
            else if (VideoProviders.Gametrailers.getId(url) != null)
            {
                return new VideoProviders.Gametrailers(url);
            }
            else if (VideoProviders.BlipTV.getId(url) != null)
            {
                return new VideoProviders.BlipTV(url);
            }
            else if (VideoProviders.Vimeo.getId(url) != null)
            {
                return new VideoProviders.Vimeo(url);
            }
            else if (VideoProviders.VideoBB.getId(url) != null)
            {
                return new VideoProviders.VideoBB(url);
            }
            else if (VideoProviders.Youtube.getId(url) != null)
            {
                return new VideoProviders.Youtube(url);
            }
            else if (VideoProviders.Facebook.getId(url) != null)
            {
                return new VideoProviders.Facebook(url);
            }
            else if (VideoProviders.AUEngine.getId(url) != null)
            {
                return new VideoProviders.AUEngine(url);
            }
            else if (VideoProviders._4shared.getId(url) != null)
            {
                return new VideoProviders._4shared(url);
            }
            else if (VideoProviders.Myspace.getId(url) != null)
            {
                return new VideoProviders.Myspace(url);
            }
            else if (VideoProviders.VideoWeed.getId(url) != null)
            {
                return new VideoProviders.VideoWeed(url);
            }
            else if (VideoProviders.NovaMov.getId(url) != null)
            {
                return new VideoProviders.NovaMov(url);
            }
            else if (VideoProviders.Veoh.getId(url) != null)
            {
                return new VideoProviders.Veoh(url);
            }
            else if (VideoProviders.OnePieceOfBleach.getId(url) != null)
            {
                return new VideoProviders.OnePieceOfBleach(url);
            }

            return null;
        }

        public static VideoProviders.Hosts.LinkCollector getLinkCollector(string url)
        {
            if (VideoProviders.Hosts.OnePieceOfBleach.isValidLink(url))
            {
                return new VideoProviders.Hosts.OnePieceOfBleach(url);
            }
            else if (VideoProviders.Hosts.GoodAnime.isValidLink(url))
            {
                return new VideoProviders.Hosts.GoodAnime(url);
            }
            else if (VideoProviders.Hosts.WatchAnimeOn.isValidLink(url))
            {
                return new VideoProviders.Hosts.WatchAnimeOn(url);
            }
            else if (VideoProviders.Hosts.AnimeAvenue.isValidLink(url))
            {
                return new VideoProviders.Hosts.AnimeAvenue(url);
            }
            else if (VideoProviders.Hosts.AnimeUltima.isValidLink(url))
            {
                return new VideoProviders.Hosts.AnimeUltima(url);
            }

            return null;
        }
    }
}