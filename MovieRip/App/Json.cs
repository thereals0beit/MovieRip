using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.App
{
    class Json
    {
        public static Newtonsoft.Json.Linq.JObject decode(string json)
        {
            return (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        }
    }
}