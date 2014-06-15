using System;
using System.Runtime.CompilerServices;

namespace Ultra.Web.Core.Common
{
    public class PathInfo
    {
        public string Directory { get; internal set; }

        public string Drive { get; internal set; }

        public string FileExtenName { get; internal set; }

        public string FileName { get; internal set; }

        public string FileWithOutExt { get; internal set; }
    }
}

