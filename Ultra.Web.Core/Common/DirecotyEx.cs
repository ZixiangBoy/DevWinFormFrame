using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ultra.Web.Core.Common
{
    public static class DirecotyEx
    {
        public static List<string> EnumerateFiles(string path, string[] schpartens, SearchOption schopt)
        {
            if (((schpartens == null) || (schpartens.Length < 1)) || (string.IsNullOrEmpty(path) || !Directory.Exists(path)))
            {
                return null;
            }
            List<string> list = new List<string>(100);
            foreach (string str in schpartens)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    List<string> collection = Directory.EnumerateFiles(path, str, schopt).ToList<string>();
                    if ((collection != null) && (collection.Count >= 1))
                    {
                        list.AddRange(collection);
                    }
                }
            }
            return list;
        }
    }
}

