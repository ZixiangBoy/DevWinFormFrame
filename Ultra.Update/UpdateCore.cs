using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Ultra.Update {
    public class UpdateCore {

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="filename">要解压的文件</param>
        /// <param name="targetDirectoryName">解压完成文件保存的目录</param>
        public void UnZip(string filename, string targetDirectoryName) {
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(filename))) {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null) {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0) {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty) {
                        using (FileStream streamWriter = File.Create(Path.Combine(targetDirectoryName, theEntry.Name))) {
                            var data = new byte[2048];
                            while (true) {
                                var size = s.Read(data, 0, data.Length);
                                if (size > 0) {
                                    streamWriter.Write(data, 0, size);
                                } else {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="URL">网络文件的地址</param>
        /// <param name="filename">保存到本地的文件名</param>
        public void DownFile(string URL, string filename) {
            HttpWebRequest httpreq = (HttpWebRequest)HttpWebRequest.Create(URL);
            HttpWebResponse httpresp = (HttpWebResponse)httpreq.GetResponse();
            var totalLength = httpresp.ContentLength;

            using (var readStream = httpresp.GetResponseStream()) {
                using (var writeStream = File.Create(filename)) {
                    byte[] buffer = new byte[2048];
                    while (true) {
                        var size = readStream.Read(buffer, 0, buffer.Length);
                        if (size > 0) {
                            writeStream.Write(buffer, 0, size);
                        } else {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 备份文件到指定的目录
        /// </summary>
        /// <param name="frompath">要备份的目录</param>
        /// <param name="topath">目标目录</param>
        public void BackupTo(string frompath, string topath) {
            var files = Directory.GetFiles(frompath);
            for (int i = 0; i < files.Length; i++) {
                var fileto = Path.Combine(topath, files[i].Substring(files[i].LastIndexOf("\\") + 1));
                using (var streamRead = File.OpenRead(files[i])) {
                    using (var streamWriter = File.Create(fileto)) {
                        byte[] data = new byte[2048];
                        while (true) {
                            var size = streamRead.Read(data, 0, data.Length);
                            if (size > 0) {
                                streamWriter.Write(data, 0, size);
                            } else {
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 反序列化xml文件
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="filename">xml文件</param>
        /// <param name="t">目标类型的对象</param>
        /// <returns></returns>
        public T XmlDeserialize<T>(string filename,Type t) {
            using (var readStream = File.OpenRead(filename)) {
                XmlSerializer xmlser = new XmlSerializer(t);
                return (T)xmlser.Deserialize(readStream);
            }
        }

        public T XmlDeserializeWeb<T>(string url, Type t) {
            var httpreq = (HttpWebRequest)WebRequest.Create(url);
            var httpresp = httpreq.GetResponse();
            using (var readStream = httpresp.GetResponseStream()) {
                XmlSerializer xmlser = new XmlSerializer(t);
                return (T)xmlser.Deserialize(readStream);
            }
        }

        /// <summary>
        /// 序列化对象为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <param name="t"></param>
        /// <param name="et"></param>
        public void XmlSerialize<T>(string filename, Type t,T et) {
            using (var writerStream = File.Create(filename)) {
                XmlSerializer xmlser = new XmlSerializer(t);
                xmlser.Serialize(writerStream, et);
            }
        } 
    }
}
