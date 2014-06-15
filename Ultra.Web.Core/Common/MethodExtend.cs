using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ultra.Web.Core.Class;
using Ultra.Web.Core.Interface;

namespace Ultra.Web.Core.Common
{
    public static class MethodExtend
    {
        private static Regex RgxAllChineseChar = new Regex(@"([\u4e00-\u9fa5]*)?");
        private static Regex RgxIP = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

        public static NameValueCollection AsQueryString(this string querystr)
        {
            HttpRequest request = new HttpRequest(string.Empty, "http://www.g.cn", querystr);
            return request.QueryString;
        }

        public static string Calc(this string exp)
        {
            return UltraDynamic.Default.Calc(exp);
        }

        public static bool CanConnToDBServer(this string src)
        {
            bool flag2;
            if (string.IsNullOrEmpty(src))
            {
                return false;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(src))
                {
                    connection.Open();
                    bool flag = connection.State == ConnectionState.Open;
                    if (flag)
                    {
                        connection.Close();
                    }
                    flag2 = flag;
                }
            }
            catch (Exception)
            {
                flag2 = false;
            }
            return flag2;
        }

        public static bool CanOpen(this string filePath)
        {
            try
            {
                File.OpenWrite(filePath).Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool CanOpen(this string filePath, out string errmsg)
        {
            try
            {
                errmsg = string.Empty;
                File.OpenWrite(filePath).Close();
            }
            catch (Exception exception)
            {
                errmsg = exception.Message;
                return false;
            }
            return true;
        }

        public static T ChangeType<T>(this string str)
        {
            return (T) Convert.ChangeType(str, typeof(T));
        }

        public static void CombinImage(this string srcFile, string combinFile, string destFile, Point pt, EnImageCombinOrd encmb, bool buseadjust)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(srcFile);
            int width = image.Width;
            int height = image.Height;
            System.Drawing.Image image2 = System.Drawing.Image.FromFile(combinFile);
            if (encmb == EnImageCombinOrd.Normal)
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    if (buseadjust)
                    {
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.High;
                    }
                    graphics.DrawImage(image2, pt);
                    image.Save(destFile);
                    image2.Dispose();
                    image.Dispose();
                    return;
                }
            }
            if (encmb == EnImageCombinOrd.Reverse)
            {
                Bitmap bitmap = new Bitmap(width, height);
                using (Graphics graphics2 = Graphics.FromImage(bitmap))
                {
                    if (buseadjust)
                    {
                        graphics2.SmoothingMode = SmoothingMode.HighQuality;
                        graphics2.CompositingQuality = CompositingQuality.HighQuality;
                        graphics2.InterpolationMode = InterpolationMode.High;
                    }
                    graphics2.DrawImage(image2, pt);
                    graphics2.DrawImage(image, 0, 0);
                    bitmap.Save(destFile);
                    image2.Dispose();
                    image.Dispose();
                }
            }
        }

        public static System.Drawing.Image CopyImage(this System.Drawing.Image img)
        {
            Bitmap bitmap = new Bitmap(img, img.Width, img.Height);
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, img.RawFormat);
                byte[] buffer = new byte[stream.Length];
                stream.Position = 0L;
                stream.Read(buffer, 0, buffer.Length);
                return System.Drawing.Image.FromStream(stream);
            }
        }

        public static int DateDiff(this DateTime d, EnDatePart enDatePrt, DateTime d2)
        {
            return DateDiff(enDatePrt, d2, d);
        }

        public static int DateDiff(EnDatePart enDatePrt, DateTime de1, DateTime de2)
        {
            TimeSpan ts = new TimeSpan(de1.Ticks);
            TimeSpan span3 = new TimeSpan(de2.Ticks).Subtract(ts);
            switch (enDatePrt)
            {
                case EnDatePart.NONE:
                    return 0;

                case EnDatePart.DAY:
                    return (int) span3.TotalDays;

                case EnDatePart.HOUR:
                    return (int) span3.TotalHours;

                case EnDatePart.MINUTE:
                    return (int) span3.TotalMinutes;

                case EnDatePart.SECOND:
                    return (int) span3.TotalSeconds;
            }
            return 0;
        }

        public static bool EqualIgnorCase(this string src, string cmp)
        {
            return (string.Compare(src, cmp, true) == 0);
        }

        public static bool FileExists(this string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            return File.Exists(filePath);
        }

        public static string FileHash(this string path)
        {
            return ByteStringUtil.ByteArrayToHexStr(HashDigest.FileDigest(path, DigestType.MD5));
        }

        public static string FilePathToMapPath(this Page pg, string fileFullPath)
        {
            string oldValue = pg.Server.MapPath(pg.Request.ApplicationPath);
            return fileFullPath.Replace(oldValue, string.Empty);
        }

        private static string FilterStringReplace(string src)
        {
            return src.Replace("[", "[[ ").Replace("]", " ]]").Replace("*", "[*]").Replace("%", "[%]").Replace("[[ ", "[[]").Replace(" ]]", "[]]").Replace("\"", "\"\"");
        }

        public static string FromBase64(this string src)
        {
            byte[] bytes = Convert.FromBase64String(src);
            return Encoding.Default.GetString(bytes);
        }

        public static void GenerateThumbNail(this string sourcefile, string destinationfile, int width, int height)
        {
            int num4;
            Bitmap bitmap;
            System.Drawing.Image image = System.Drawing.Image.FromFile(sourcefile);
            int srcWidth = image.Width;
            int srcHeight = image.Height;
            int num3 = width;
            if (srcHeight > srcWidth)
            {
                num4 = height;
                bitmap = new Bitmap(num3, num4);
            }
            else
            {
                num4 = height;
                num3 = width;
                bitmap = new Bitmap(num3, num4);
            }
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.High;
            Rectangle destRect = new Rectangle(0, 0, num3, num4);
            graphics.DrawImage(image, destRect, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
            bitmap.Save(destinationfile);
            bitmap.Dispose();
            image.Dispose();
        }

        public static System.Drawing.Image GenLiteImage(System.Drawing.Image img, int width, int height)
        {
            Bitmap bitmap = new Bitmap(img, width, height);
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, img.RawFormat);
                byte[] buffer = new byte[stream.Length];
                stream.Position = 0L;
                stream.Read(buffer, 0, buffer.Length);
                return System.Drawing.Image.FromStream(stream);
            }
        }

        public static System.Drawing.Image GenLiteImage(byte[] imgData, int width, int height)
        {
            System.Drawing.Image image2;
            using (MemoryStream stream = new MemoryStream(imgData))
            {
                System.Drawing.Image original = System.Drawing.Image.FromStream(stream);
                Bitmap bitmap = new Bitmap(original, width, height);
                using (MemoryStream stream2 = new MemoryStream())
                {
                    bitmap.Save(stream2, original.RawFormat);
                    byte[] buffer = new byte[stream2.Length];
                    stream2.Position = 0L;
                    stream2.Read(buffer, 0, buffer.Length);
                    image2 = System.Drawing.Image.FromStream(stream2);
                }
            }
            return image2;
        }

        public static List<Type> GetAllClassOfClass(this Assembly asm, Type tbase)
        {
            return ObjectHelper.GetAllClassOfClass(tbase, asm);
        }

        public static List<Type> GetAllClassOfClass(this string str, Type tbase)
        {
            return ObjectHelper.GetAllClassOfClass(tbase, str.LoadAsMemAsm());
        }

        public static List<Type> GetAllClassOfInterface(this Assembly asm, string interfaceName = "")
        {
            return ObjectHelper.GetAllClassOfInterface(asm, interfaceName);
        }

        public static string GetAsyncConnString(this string connstr)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connstr) {
                AsynchronousProcessing = true
            };
            return builder.ConnectionString;
        }

        public static string GetBetween(this string str, string strStart, string strEnd)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            int index = str.IndexOf(strStart);
            if (index < 0)
            {
                return string.Empty;
            }
            int num2 = str.IndexOf(strEnd);
            if (num2 < 0)
            {
                return string.Empty;
            }
            int length = num2 - (index + strStart.Length);
            if (length < 1)
            {
                return string.Empty;
            }
            return str.Substring(index + strStart.Length, length);
        }

        public static byte[] GetBytes(this System.Drawing.Image img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, img.RawFormat);
                return stream.ToArray();
            }
        }

        public static string GetChinaDate(this DateTime d)
        {
            return ChineseDate.GetDate(d);
        }

        public static string GetClientIP(this Page pg)
        {
            return pg.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static DataTable GetDataTable<T>(this IList<T> lstT, params string[] propertyArgs)
        {
            Func<PropertyInfo, string> func2 = null;
            if (lstT == null)
            {
                return null;
            }
            DataTable table = ObjectDataMaper<T>.DataTableSechma(propertyArgs);
            foreach (T local in lstT)
            {
                if (local != null)
                {
                    DataRow row = table.NewRow();
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    if ((propertyArgs == null) || (propertyArgs.Length <= 0))
                    {
                    }
                    propertyArgs = (func2 != null) ? propertyArgs : properties.Select<PropertyInfo, string>(((Func<PropertyInfo, string>) (func2 = im => im.Name))).ToArray<string>();
                    string[] strArray = propertyArgs;
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        Func<PropertyInfo, bool> predicate = null;
                        string pty = strArray[i];
                        if (predicate == null)
                        {
                            predicate = arg => arg.Name == pty;
                        }
                        List<PropertyInfo> source = properties.Where<PropertyInfo>(predicate).ToList<PropertyInfo>();
                        if ((source == null) || (source.Count < 1))
                        {
                            throw new Exception("找不到包含: " + pty + " 的属性");
                        }
                        PropertyInfo info = source.First<PropertyInfo>();
                        if (null != info)
                        {
                            Type propertyType = info.PropertyType;
                            object obj2 = info.GetValue(local, null);
                            row[pty] = (null == info) ? DBNull.Value : ((obj2 == null) ? DBNull.Value : obj2);
                        }
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public static DataTable GetDataTableEx<T>(this IList<T> lstT, string[] propOrFields)
        {
            List<PropertyInfo> list;
            List<FieldInfo> list2;
            if (lstT == null)
            {
                return null;
            }
            DataTable table = ObjectDataMaper<T>.DataTableSechmaEx(propOrFields, out list, out list2);
            foreach (T local in lstT)
            {
                if (local == null)
                {
                    continue;
                }
                DataRow row = table.NewRow();
                if ((list != null) && (list.Count > 0))
                {
                    foreach (PropertyInfo info in list)
                    {
                        object obj2 = info.GetValue(local, null);
                        row[info.Name] = (null == info) ? DBNull.Value : ((obj2 == null) ? DBNull.Value : obj2);
                    }
                }
                if ((list2 != null) && (list2.Count > 0))
                {
                    foreach (FieldInfo info2 in list2)
                    {
                        object obj3 = info2.GetValue(local);
                        row[info2.Name] = (null == info2) ? DBNull.Value : ((obj3 == null) ? DBNull.Value : obj3);
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static string GetFieldDesc(this FieldInfo fi)
        {
            if (null == fi)
            {
                return string.Empty;
            }
            object[] customAttributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if ((customAttributes == null) || (customAttributes.Count<object>() < 1))
            {
                return string.Empty;
            }
            DescriptionAttribute attribute = customAttributes.First<object>() as DescriptionAttribute;
            if (attribute == null)
            {
                return string.Empty;
            }
            return attribute.Description;
        }

        public static string GetFileExtName(this byte[] fileData)
        {
            return GetFileSuffix(fileData);
        }

        private static string GetFileSuffix(byte[] fileData)
        {
            if ((fileData != null) && (fileData.Length >= 10))
            {
                if (((fileData[0] == 0x47) && (fileData[1] == 0x49)) && (fileData[2] == 70))
                {
                    return "GIF";
                }
                if (((fileData[1] == 80) && (fileData[2] == 0x4e)) && (fileData[3] == 0x47))
                {
                    return "PNG";
                }
                if (((fileData[6] == 0x4a) && (fileData[7] == 70)) && ((fileData[8] == 0x49) && (fileData[9] == 70)))
                {
                    return "JPG";
                }
                if ((fileData[0] == 0x42) && (fileData[1] == 0x4d))
                {
                    return "BMP";
                }
            }
            return null;
        }

        public static string GetGuidStr(this string str)
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        public static string GetHost(this HttpRequest req)
        {
            return req.Url.Authority;
        }

        public static string GetHost(this Page pg)
        {
            return pg.Request.GetHost();
        }

        public static InterlockBoolean GetLockBool(this bool b)
        {
            return InterlockBoolean.Create(b);
        }

        public static string GetMimeType(this string filename)
        {
            return GetMimeTypeString(filename);
        }

        public static string GetMimeType(this byte[] fileData)
        {
            return GetMimeTypeString(fileData);
        }

        private static string GetMimeTypeString(string fileName)
        {
            fileName = fileName.ToLower();
            if (fileName.EndsWith(".bmp", StringComparison.CurrentCulture))
            {
                return "image/bmp";
            }
            if (fileName.EndsWith(".gif", StringComparison.CurrentCulture))
            {
                return "image/gif";
            }
            if (fileName.EndsWith(".jpg", StringComparison.CurrentCulture) || fileName.EndsWith(".jpeg", StringComparison.CurrentCulture))
            {
                return "image/jpeg";
            }
            if (fileName.EndsWith(".png", StringComparison.CurrentCulture))
            {
                return "image/png";
            }
            return "application/octet-stream";
        }

        private static string GetMimeTypeString(byte[] fileData)
        {
            switch (GetFileSuffix(fileData))
            {
                case "JPG":
                    return "image/jpeg";

                case "GIF":
                    return "image/gif";

                case "PNG":
                    return "image/png";

                case "BMP":
                    return "image/bmp";
            }
            return "application/octet-stream";
        }

        public static T GetObject<T>(this HttpCookie cke)
        {
            if (cke == null)
            {
                return default(T);
            }
            T local = new UltraCookie<T>().DeSeriesObject<T>(cke.Value);
            return local;
        }

        public static PathInfo GetPathInfo(this string filePath)
        {
            return new PathInfo { Directory = Path.GetDirectoryName(filePath), FileName = Path.GetFileName(filePath), FileExtenName = Path.GetExtension(filePath), FileWithOutExt = Path.GetFileNameWithoutExtension(filePath), Drive = Directory.GetDirectoryRoot(filePath) };
        }

        public static string GetPinyin(this string str)
        {
            string str2 = string.Empty;
            foreach (char ch in str)
            {
                try
                {
                    ChineseChar ch2 = new ChineseChar(ch);
                    string str3 = ch2.Pinyins[0].ToString();
                    str2 = str2 + str3.Substring(0, str3.Length - 1);
                }
                catch
                {
                    str2 = str2 + ch.ToString();
                }
            }
            return str2;
        }

        public static string GetPropertyDesc(this PropertyInfo pi)
        {
            return ObjectHelper.GetPropertyDesc(pi);
        }

        public static string GetShortPinyin(this string str)
        {
            string str2 = string.Empty;
            foreach (char ch in str)
            {
                try
                {
                    ChineseChar ch2 = new ChineseChar(ch);
                    string str3 = ch2.Pinyins[0].ToString();
                    str2 = str2 + str3.Substring(0, 1);
                }
                catch
                {
                    str2 = str2 + ch.ToString();
                }
            }
            return str2;
        }

        public static string Hash(this string str)
        {
            return ByteStringUtil.ByteArrayToHexStr(HashDigest.StringDigest(str));
        }

        public static byte[] HexStrToBytes(this string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return null;
            }
            return ByteStringUtil.ByteArrayFromHexStr(src);
        }

        public static string HtmlDecode(this string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static string HtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static bool IgnoreCaseEqual(this string str, string str2)
        {
            return (string.Compare(str, str2, true) == 0);
        }

        public static Size ImageSize(this string srcFile)
        {
            if (string.IsNullOrEmpty(srcFile) || !File.Exists(srcFile))
            {
                return new Size(0, 0);
            }
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(srcFile))
            {
                return new Size(image.Width, image.Height);
            }
        }

        public static bool IsAllChineseChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            Match match = RgxAllChineseChar.Match(str);
            if ((match == null) || (match.Groups.Count < 2))
            {
                return false;
            }
            for (int i = 1; i < match.Groups.Count; i++)
            {
                if (match.Groups[i].Length < 1)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDatePartEqual(this DateTime deOri, DateTime de2)
        {
            return (((deOri.Year == de2.Year) && (deOri.Month == de2.Month)) && (deOri.Day == de2.Month));
        }

        public static bool IsEven(this int i)
        {
            return ((i & 1) == 0);
        }

        public static bool IsHtmlContainImg(this string htmlstr)
        {
            Regex regex = new Regex("(<img.*?src=(.*?)>){1,}");
            return regex.Match(htmlstr).Success;
        }

        public static bool IsIPAddress(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return RgxIP.IsMatch(str);
        }

        public static bool IsMobileNum(this string str)
        {
            return ValidMobileNumber(str);
        }

        public static bool IsRequired(this FieldInfo fi)
        {
            return ObjectHelper.IsRequired(fi);
        }

        public static bool IsRequired(this PropertyInfo pi)
        {
            return ObjectHelper.IsRequired(pi);
        }

        public static bool IsTimePartEqual(this DateTime deOri, DateTime de2)
        {
            return (((deOri.Hour == de2.Hour) && (deOri.Minute == de2.Minute)) && (deOri.Second == de2.Second));
        }

        private static string Left(this string src, int length)
        {
            if (string.IsNullOrEmpty(src))
            {
                return src;
            }
            if (src.Length <= length)
            {
                return src;
            }
            return src.Substring(0, length);
        }

        public static string Left(this string src, int length, string apend )
        {
            if (string.IsNullOrEmpty(apend))
            {
                return src.Left(length);
            }
            if (src.Length > apend.Length)
            {
                return string.Format("{0}{1}", src.Left(length, apend));
            }
            return src;
        }

        public static string Left(this string str, string strSrc, int length)
        {
            if (string.IsNullOrEmpty(strSrc))
            {
                return strSrc;
            }
            if (strSrc.Length <= length)
            {
                return strSrc;
            }
            return strSrc.Substring(0, length);
        }

        public static Assembly LoadAsMemAsm(this string str)
        {
            return ObjectHelper.LoadAsm(str);
        }

        public static string NewGuidStr(this Guid gid)
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        public static Dictionary<string, string> ParseCmdArgs(this string[] cmdArgs)
        {
            if ((cmdArgs == null) || (cmdArgs.Length < 1))
            {
                return null;
            }
            Dictionary<string, string> dictionary = new Dictionary<string, string>(cmdArgs.Length);
            Regex regex = new Regex("(.+?):(.+)");
            foreach (string str in cmdArgs)
            {
                string[] strArray = regex.Split(str);
                if (((strArray != null) && (strArray.Length >= 3)) && !dictionary.ContainsKey(strArray[1]))
                {
                    dictionary.Add(strArray[1], strArray[2]);
                }
            }
            return dictionary;
        }

        public static int Random(this int maxValue, RNGCryptoServiceProvider rng)
        {
            rng = rng ?? new RNGCryptoServiceProvider();
            decimal num = 9223372036854775807M;
            byte[] data = new byte[8];
            rng.GetBytes(data);
            return (int) ((Math.Abs(BitConverter.ToInt64(data, 0)) / num) * maxValue);
        }

        public static bool RandomBool(this bool b)
        {
            int maxValue = 10;
            return maxValue.Random(null).IsEven();
        }

        public static string RandomString(this string keySet, int stringlength)
        {
            if (string.IsNullOrEmpty(keySet))
            {
                keySet = "abcdefghijkmnpqrstuvwxyz23456789";
            }
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            int maxValue = (keySet.Length > keySet.Length) ? keySet.Length : stringlength;
            StringBuilder builder = new StringBuilder(keySet.Length);
            for (int i = 0; i < stringlength; i++)
            {
                builder.Append(keySet[maxValue.Random(rng)]);
            }
            return builder.ToString();
        }

        public static string RawUrl(this Page pg)
        {
            return pg.RequestUrl();
        }

        public static string ReadFirstLine(this string filePath)
        {
            return filePath.ReadFirstLine(Encoding.Default);
        }

        public static string ReadFirstLine(this string filePath, Encoding enc)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return string.Empty;
            }
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }
            using (StreamReader reader = new StreamReader(filePath, enc))
            {
                return reader.ReadLine();
            }
        }

        public static void Redirect(this Page pg, string url)
        {
            pg.Response.Redirect(url);
        }

        public static string ReplaceBetween(this string str, string strStart, string strEnd, string rep = "")
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            int index = str.IndexOf(strStart);
            if (index < 0)
            {
                return string.Empty;
            }
            int startIndex = str.IndexOf(strEnd);
            if (startIndex < 0)
            {
                return string.Empty;
            }
            int num3 = startIndex - (index + strStart.Length);
            if (num3 < 1)
            {
                return string.Empty;
            }
            return (str.Left((index + strStart.Length)) + rep + str.Substring(startIndex, str.Length - startIndex));
        }

        public static string RequestUrl(this Page pg)
        {
            return pg.Request.RawUrl;
        }

        public static void SaveImage(this FileUpload fup, ref UploadImageThumnail thumbnailArg, bool bGenLite = false)
        {
            if (fup.HasFile)
            {
                fup.SaveAs(thumbnailArg.FileSaveAsPath);
                if (bGenLite)
                {
                    string str = !string.IsNullOrEmpty(thumbnailArg.FileThumnailPath) ? thumbnailArg.FileThumnailPath : null;
                    if (string.IsNullOrEmpty(str))
                    {
                        PathInfo pathInfo = thumbnailArg.FileSaveAsPath.GetPathInfo();
                        str = Path.Combine(pathInfo.Directory, string.Format("{0}_{1}_{2}{3}", new object[] { pathInfo.FileWithOutExt, thumbnailArg.ThumWidth, thumbnailArg.ThumHeight, pathInfo.FileExtenName }));
                    }
                    thumbnailArg.FileSaveAsPath.GenerateThumbNail(str, thumbnailArg.ThumWidth, thumbnailArg.ThumHeight);
                    thumbnailArg.FileThumnailPath = str;
                }
            }
        }

        public static List<DateTime> Split(this DateTime begin, DateTime end, int days)
        {
            if (days <= 0)
            {
                days = 1;
            }
            DateTime deCmp = begin;
            end = end.Swap(ref deCmp);
            begin = deCmp;
            List<DateTime> list = new List<DateTime>();
            if (end.DateDiff(EnDatePart.DAY, begin) > days)
            {
                list.Add(begin);
                while (end.CompareTo(begin = begin.AddDays((double) days)) > 0)
                {
                    list.Add(begin);
                }
                list.Add(end);
                return list;
            }
            return new List<DateTime> { begin, end };
        }

        public static List<DateTimeRange> SplitDateRange(this DateTime begin, DateTime end, int days)
        {
            List<DateTime> list = begin.Split(end, days);
            if ((list == null) || (list.Count < 1))
            {
                return null;
            }
            if (list.Count == 2)
            {
                List<DateTimeRange> list3 = new List<DateTimeRange>();
                DateTimeRange item = new DateTimeRange {
                    Begin = list[0],
                    End = list[1]
                };
                list3.Add(item);
                return list3;
            }
            Stack<DateTime> stack = new Stack<DateTime>(2);
            List<DateTimeRange> list2 = new List<DateTimeRange>(list.Count);
            foreach (DateTime time2 in list)
            {
                stack.Push(time2);
                if (stack.Count > 1)
                {
                    DateTime time;
                    DateTimeRange range = new DateTimeRange {
                        End = time = stack.Pop(),
                        Begin = stack.Pop()
                    };
                    stack.Clear();
                    stack.Push(time);
                    list2.Add(range);
                }
            }
            return list2;
        }

        public static string SqlKeyCharFilter(this string str, string strFilter)
        {
            if (string.IsNullOrEmpty(strFilter))
            {
                return strFilter;
            }
            return FilterStringReplace(strFilter);
        }

        public static DateTime Swap(this DateTime deOri, ref DateTime deCmp)
        {
            DateTime time = (deOri.CompareTo((DateTime) deCmp) > 0) ? deOri : deCmp;
            DateTime time2 = (deOri.CompareTo((DateTime) deCmp) < 0) ? deOri : deCmp;
            deCmp = time2;
            return time;
        }

        public static DateTime SyncYearMonthDay(this DateTime deOri, DateTime de2)
        {
            return UpdateYearMonthDay(deOri, de2);
        }

        public static void ThumbnailImage(this string sourcefile, string destinationfile, int width, int height)
        {
            sourcefile.GenerateThumbNail(destinationfile, width, height);
        }

        public static System.Drawing.Image Thumnail(this System.Drawing.Image img)
        {
            return img.Thumnail(0x61, 0x2d);
        }

        public static System.Drawing.Image Thumnail(this System.Drawing.Image img, int width, int height)
        {
            return GenLiteImage(img, width, height);
        }

        public static System.Drawing.Image Thumnail(this string srcImgPath, int width, int height)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(srcImgPath));
            System.Drawing.Image image2 = img.Thumnail(width, height);
            img.Dispose();
            return image2;
        }

        public static string ToBase64(this string src)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(src));
        }

        public static byte[] ToBytes(this string src)
        {
            return src.ToBytes(Encoding.Default);
        }

        public static byte[] ToBytes(this string src, Encoding coding)
        {
            src = string.IsNullOrEmpty(src) ? string.Empty : src;
            return coding.GetBytes(src);
        }

        public static decimal? ToDecimal(this string str)
        {
            decimal num;
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (!decimal.TryParse(str, out num))
            {
                return null;
            }
            return new decimal?(num);
        }

        public static decimal? ToDecimal(this string str, decimal defaultvalue)
        {
            decimal? nullable = str.ToDecimal();
            return new decimal?(!nullable.HasValue ? defaultvalue : nullable.Value);
        }

        public static string ToDefaultStr(this DateTime de)
        {
            return de.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDefaultStr(this DateTime? de)
        {
            if (!de.HasValue)
            {
                return string.Empty;
            }
            return de.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToEncodeStr(this string src, Encoding coding)
        {
            if (string.IsNullOrEmpty(src))
            {
                return src;
            }
            byte[] bytes = src.ToBytes(coding);
            return coding.GetString(bytes);
        }

        public static List<T> ToEntity<T>(this DataTable dt) where T: new()
        {
            if ((dt != null) && (dt.Rows.Count >= 1))
            {
                return ObjectHelper.Create<T>(dt);
            }
            return null;
        }

        public static int ToInt(this long lng)
        {
            return (int) lng;
        }

        public static int? ToInt(this string str)
        {
            int num;
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (!int.TryParse(str, out num))
            {
                return null;
            }
            return new int?(num);
        }

        public static int ToInt(this string str, int defaultvalue)
        {
            int? nullable = str.ToInt();
            if (nullable.HasValue)
            {
                return nullable.Value;
            }
            return defaultvalue;
        }

        public static long? ToLong(this string str)
        {
            long num;
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (!long.TryParse(str, out num))
            {
                return null;
            }
            return new long?(num);
        }

        public static long? ToLong(this string str, long defaultvalue)
        {
            long num;
            if (string.IsNullOrEmpty(str))
            {
                return new long?(defaultvalue);
            }
            if (!long.TryParse(str, out num))
            {
                return new long?(defaultvalue);
            }
            return new long?(num);
        }

        public static System.Drawing.Image ToThumnailImage(this byte[] byt)
        {
            return GenLiteImage(byt, 0x61, 0x2d);
        }

        public static System.Drawing.Image ToThumnailImage(this byte[] byt, int width, int heigth)
        {
            return GenLiteImage(byt, width, heigth);
        }

        public static List<string> TrimColumnName(this DataTable dt)
        {
            if (((dt == null) || (dt.Columns == null)) || (dt.Columns.Count < 1))
            {
                return null;
            }
            List<string> list = new List<string>(dt.Columns.Count);
            foreach (DataColumn column in dt.Columns)
            {
                column.ColumnName = column.ColumnName.Trim();
                list.Add(column.ColumnName);
            }
            return list;
        }

        public static DateTime UpdateYearMonthDay(DateTime deOri, DateTime de)
        {
            DateTime time = deOri;
            return DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:{5}", new object[] { de.Year, de.Month, de.Day, time.Hour, time.Minute, time.Second }));
        }

        public static string UrlDecode(this string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        public static string UrlDecode(this string url, Encoding enc)
        {
            return HttpUtility.UrlDecode(url, enc);
        }

        public static string UrlEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        public static string UrlEncode(this string url, Encoding enc)
        {
            return HttpUtility.UrlEncode(url, enc);
        }

        public static bool ValidMobileNumber(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
            {
                return false;
            }
            Match match = Regex.Match(phoneNum, @"\d{11,12}");
            return (((match != null) && match.Success) && (match.Value == phoneNum));
        }

        public static void WriteToServer<T>(this SqlBulkCopy blk, IList<T> lstT)
        {
            PropertyInfo[] properties = typeof(IBaseEntity).GetProperties();
            List<string> pis = (from i in properties select i.Name).ToList<string>();
            if (!pis.Contains("Timestamp"))
            {
                pis.Add("Timestamp");
            }
            blk.WriteToServer<T>(lstT, delegate (DataTable t1) {
                pis.ForEach(delegate (string i) {
                    if (t1.Columns.Contains(i))
                    {
                        t1.Columns.Remove(i);
                    }
                });
                return t1;
            });
        }

        public static void WriteToServer<T>(this SqlBulkCopy blk, T ent, params string[] propertyArgs)
        {
            blk.WriteToServer<T>(new List<T> { ent }, propertyArgs);
        }

        public static void WriteToServer<T>(this SqlBulkCopy blk, IList<T> lstT, params string[] propertyArgs)
        {
            if ((lstT != null) && (lstT.Count >= 1))
            {
                DataTable dataTable = lstT.GetDataTable<T>(propertyArgs);
                if ((dataTable != null) && (dataTable.Rows.Count >= 1))
                {
                    blk.ColumnMappings.Clear();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        blk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    blk.WriteToServer(dataTable);
                }
            }
        }

        public static void WriteToServer<T>(this SqlBulkCopy blk, IList<T> lstT, Func<DataTable, DataTable> fnc )
        {
            if ((lstT != null) && (lstT.Count >= 1))
            {
                DataTable dataTable = lstT.GetDataTable<T>(new string[0]);
                if ((dataTable != null) && (dataTable.Rows.Count >= 1))
                {
                    blk.ColumnMappings.Clear();
                    if (fnc != null)
                    {
                        dataTable = fnc(dataTable);
                    }
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        blk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    blk.WriteToServer(dataTable);
                }
            }
        }

        public static void WriteToServer(this DataTable dt, string connstr, string tbName, params string[] tbFields)
        {
            if (((!string.IsNullOrEmpty(tbName) && !string.IsNullOrEmpty(connstr)) && ((dt != null) && (dt.Rows.Count >= 1))) && ((dt.Columns != null) && (dt.Columns.Count >= 1)))
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(connstr))
                {
                    copy.DestinationTableName = tbName;
                    if ((tbFields != null) && (tbFields.Length > 0))
                    {
                        foreach (string str in tbFields)
                        {
                            copy.ColumnMappings.Add(str, str);
                        }
                    }
                    else
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            copy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                    }
                    copy.WriteToServer(dt);
                }
            }
        }

        public static void WriteToServerAdv<T>(this SqlBulkCopy blk, IList<T> lstT, Func<DataTable, DataTable> fnc)
        {
            if ((lstT != null) && (lstT.Count >= 1))
            {
                DataTable dataTable = lstT.GetDataTable<T>(new string[0]);
                if ((dataTable != null) && (dataTable.Rows.Count >= 1))
                {
                    blk.ColumnMappings.Clear();
                    if (fnc != null)
                    {
                        dataTable = fnc(dataTable);
                    }
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        blk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    blk.WriteToServer(dataTable);
                }
            }
        }

        public static void WriteToServerAdv<T>(this SqlBulkCopy blk, IList<T> lstT, string[] excludes) {
            List<string> pis = new List<string>(15) { "Timestamp" };
            if ((excludes != null) && (excludes.Length > 0)) {
                pis.AddRange(excludes);
            }
            blk.WriteToServer<T>(lstT, (dt)=> {
                pis.ForEach(col=> {
                    if (dt.Columns.Contains(col)) {
                        dt.Columns.Remove(col);
                    }
                });
                return dt;
            });
        }

        public static void WriteToServerEx<T>(this SqlBulkCopy blk, IList<T> lstT, string[] propOrField)
        {
            if ((lstT != null) && (lstT.Count >= 1))
            {
                DataTable dataTableEx = lstT.GetDataTableEx<T>(propOrField);
                if ((dataTableEx != null) && (dataTableEx.Rows.Count >= 1))
                {
                    blk.ColumnMappings.Clear();
                    foreach (DataColumn column in dataTableEx.Columns)
                    {
                        blk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    blk.WriteToServer(dataTableEx);
                }
            }
        }

        public static void WriteToServerEx<T>(this SqlBulkCopy blk, IList<T> lstT, IEnumerable<PropertyMap> fieldMap, Func<DataTable, DataTable> defaultVluSet )
        {
            if (((lstT != null) && (lstT.Count >= 1)) && ((fieldMap != null) && (fieldMap.Count<PropertyMap>() >= 1)))
            {
                DataTable dataTableEx = lstT.GetDataTableEx<T>((from i in fieldMap select i.PropOrFieldName).ToArray<string>());
                Dictionary<string, PropertyMap> dictionary = fieldMap.ToDictionary<PropertyMap, string>(i => i.PropOrFieldName);
                foreach (DataColumn column in dataTableEx.Columns)
                {
                    if (dictionary.ContainsKey(column.ColumnName))
                    {
                        column.ColumnName = dictionary[column.ColumnName].TableFieldName;
                    }
                }
                if (defaultVluSet != null)
                {
                    dataTableEx = defaultVluSet(dataTableEx);
                }
                blk.ColumnMappings.Clear();
                foreach (DataColumn column2 in dataTableEx.Columns)
                {
                    blk.ColumnMappings.Add(column2.ColumnName, column2.ColumnName);
                }
                blk.WriteToServer(dataTableEx);
            }
        }
    }
}

