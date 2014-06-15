using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using Ultra.Web.Core.Common;
using Ultra.Web.Core.Interface;

namespace Ultra.Web.Core.Configuration
{
    public class OptionConfig : IOptionConfig
    {
        private EnOptionConfigType _OptionConfigType;
        public static readonly string NodeItem = "item";
        public static readonly string NodeItemName = "name";
        public static readonly string NodeItemValue = "value";
        public static readonly string RootNode = "RootConfig";

        public OptionConfig(string cfgFile)
        {
            this._OptionConfigType = EnOptionConfigType.Optional;
            this._optConfigFile = cfgFile;
        }

        public OptionConfig(EnOptionConfigType _en = 0)
        {
            this._OptionConfigType = _en;
        }

        protected internal XDocument CreateOptionConfigFile()
        {
            XDocument document = null;
            string filePath = string.Empty;
            switch (this.OptionConfigType)
            {
                case EnOptionConfigType.Web:
                {
                    string str2 = HttpContext.Current.Server.MapPath(OptionConfigSitePrefix);
                    if (!str2.FileExists())
                    {
                        Directory.CreateDirectory(str2);
                    }
                    filePath = OptionConfigWebFileName;
                    break;
                }
                case EnOptionConfigType.App:
                    filePath = OptionConfigAppFileName;
                    break;

                case EnOptionConfigType.Optional:
                    filePath = this._optConfigFile;
                    break;
            }
            if (!filePath.FileExists())
            {
                document = new XDocument(new object[] { new XElement(RootNode) });
                document.Save(filePath);
                return document;
            }
            return XDocument.Load(filePath);
        }

        public IList<T> Each<T>(Func<XElement, T> func)
        {
            if (func == null)
            {
                return null;
            }
            IEnumerable<XElement> source = this.CreateOptionConfigFile().Element(RootNode).Elements(NodeItem);
            if (source == null)
            {
                return null;
            }
            List<T> lst = new List<T>(source.Count<XElement>());
            source.ToList<XElement>().ForEach(delegate (XElement k) {
                lst.Add(func(k));
            });
            return lst;
        }

        public List<T> EachDef<T>()
        {
            return this.Each<T>(ki => this.GetCData<T>(ki.Attribute(NodeItemName).Value, null)).ToList<T>();
        }

        public virtual XElement FindItem(string keyname, XDocument doc)
        {
            XDocument document = (doc == null) ? this.CreateOptionConfigFile() : doc;
            IEnumerable<XElement> source = document.Element(RootNode).Elements(NodeItem);
            if (source != null)
            {
                source = from k in source
                    where k.HasAttributes && (k.Attribute(NodeItemName).Value == keyname)
                    select k;
                if ((source != null) && (source.Count<XElement>() >= 1))
                {
                    return source.First<XElement>();
                }
            }
            return null;
        }

        public T Get<T>(string keyName)
        {
            XDocument doc = this.CreateOptionConfigFile();
            XElement element = this.FindItem(keyName, doc);
            if (element == null)
            {
                return default(T);
            }
            string str = element.Attribute(NodeItemValue).Value;
            if (string.IsNullOrEmpty(str))
            {
                return default(T);
            }
            return (T) Convert.ChangeType(str, typeof(T));
        }

        public T GetCData<T>(string keyName, Func<string, T> fnc)
        {
            XDocument doc = this.CreateOptionConfigFile();
            XElement element = this.FindItem(keyName, doc);
            if (element == null)
            {
                return default(T);
            }
            string jsonstr = element.Value;
            if (fnc == null)
            {
                return ObjectHelper.DeSerialize<T>(jsonstr);
            }
            return fnc(jsonstr);
        }

        public IOptionConfig Remove(List<string> keyName)
        {
            XDocument xdoc = this.CreateOptionConfigFile();
            keyName.ForEach(delegate (string k) {
                if (!string.IsNullOrEmpty(k))
                {
                    XElement element = this.FindItem(k, xdoc);
                    if (element != null)
                    {
                        element.Remove();
                    }
                }
            });
            xdoc.Save(this.OptionConfigFile);
            return this;
        }

        public IOptionConfig Remove(string keyName)
        {
            if (!string.IsNullOrEmpty(keyName))
            {
                XDocument doc = this.CreateOptionConfigFile();
                XElement element = this.FindItem(keyName, doc);
                if (element == null)
                {
                    return this;
                }
                element.Remove();
                doc.Save(this.OptionConfigFile);
            }
            return this;
        }

        public IOptionConfig Set<T>(string keyName, T value)
        {
            XDocument doc = this.CreateOptionConfigFile();
            XElement element = this.FindItem(keyName, doc);
            if (element == null)
            {
                doc.Element(RootNode).Add(new XElement(NodeItem, new object[] { new XAttribute(NodeItemName, keyName), new XAttribute(NodeItemValue, value.ToString()) }));
            }
            else
            {
                element.Attribute(NodeItemValue).Value = value.ToString();
            }
            doc.Save(this.OptionConfigFile);
            return this;
        }

        public IOptionConfig SetCData<T>(string keyName, T value)
        {
            XDocument doc = this.CreateOptionConfigFile();
            XElement element = this.FindItem(keyName, doc);
            XCData data = new XCData(ObjectHelper.SerializeJson(value));
            if (element == null)
            {
                doc.Element(RootNode).Add(new XElement(NodeItem, new object[] { new XAttribute(NodeItemName, keyName), data }));
            }
            else
            {
                ((XCData) element.FirstNode).Value = data.Value;
            }
            doc.Save(this.OptionConfigFile);
            return this;
        }

        protected string _optConfigFile { get; set; }

        public static string OptionConfigAppFileName
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), OptionConfigFileName);
            }
        }

        public string OptionConfigFile
        {
            get
            {
                switch (this.OptionConfigType)
                {
                    case EnOptionConfigType.Web:
                        return OptionConfigWebFileName;

                    case EnOptionConfigType.App:
                        return OptionConfigAppFileName;

                    case EnOptionConfigType.Optional:
                        return this._optConfigFile;
                }
                return OptionConfigWebFileName;
            }
        }

        public static string OptionConfigFileName
        {
            get
            {
                return "Ultra.Web.Core.Configuration.Global.xml";
            }
        }

        public static string OptionConfigSitePrefix
        {
            get
            {
                return "~/OptionConfig";
            }
        }

        public EnOptionConfigType OptionConfigType
        {
            get
            {
                return this._OptionConfigType;
            }
            set
            {
                this._OptionConfigType = value;
            }
        }

        public static string OptionConfigWebFileName
        {
            get
            {
                return HttpContext.Current.Server.MapPath(OptionConfigSitePrefix + "/" + OptionConfigFileName);
            }
        }
    }
}

