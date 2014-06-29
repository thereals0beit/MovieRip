using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MovieRip.App
{
    public class Config
    {
        [XmlRoot("jwplayer")]
        public class jwplayer
        {
            [XmlElement("skin")]
            public string skin = string.Empty;
            
            [XmlElement("startMovie")]
            public string startMovie = string.Empty;
        };

        public class Manager
        {
            public static jwplayer jwplayer = null;

            public static void programStart(string startupPath)
            {
                Manager.jwplayer = Config.load<jwplayer>(startupPath + "\\config\\jwplayer.xml");
            }
        };

        public static T load<T>(string path)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));

            System.IO.TextReader r = new System.IO.StreamReader(path);

            T returnValue = (T)x.Deserialize(r);

            r.Close();

            return returnValue;
        }

        public static void save<T>(T saveObj, string path)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));

            System.IO.TextWriter w = new System.IO.StreamWriter(path);

            x.Serialize(w, saveObj);

            w.Close();
        }
    }
}