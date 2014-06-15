using System;

namespace Ultra.Web.Core.Common
{
    public static class ChineseDate
    {
        private static string[] DayName = new string[] { 
            "*", "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", 
            "十六", "十七", "十八", "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十"
         };
        private static string[] DiZhi = new string[] { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
        private static int[] LunarData = new int[] { 
            0xa4b, 0x5164b, 0x6a5, 0x6d4, 0x415b5, 0x2b6, 0x957, 0x2092f, 0x497, 0x60c96, 0xd4a, 0xea5, 0x50da9, 0x5ad, 0x2b6, 0x3126e, 
            0x92e, 0x7192d, 0xc95, 0xd4a, 0x61b4a, 0xb55, 0x56a, 0x4155b, 0x25d, 0x92d, 0x2192b, 0xa95, 0x71695, 0x6ca, 0xb55, 0x50ab5, 
            0x4da, 0xa5b, 0x30a57, 0x52b, 0x8152a, 0xe95, 0x6aa, 0x615aa, 0xab5, 0x4b6, 0x414ae, 0xa57, 0x526, 0x31d26, 0xd95, 0x70b55, 
            0x56a, 0x96d, 0x5095d, 0x4ad, 0xa4d, 0x41a4d, 0xd25, 0x81aa5, 0xb54, 0xb6a, 0x612da, 0x95b, 0x49b, 0x41497, 0xa4b, 0xa164b, 
            0x6a5, 0x6d4, 0x615b4, 0xab6, 0x957, 0x5092f, 0x497, 0x64b, 0x30d4a, 0xea5, 0x80d65, 0x5ac, 0xab6, 0x5126d, 0x92e, 0xc96, 
            0x41a95, 0xd4a, 0xda5, 0x20b55, 0x56a, 0x7155b, 0x25d, 0x92d, 0x5192b, 0xa95, 0xb4a, 0x416aa, 0xad5, 0x90ab5, 0x4ba, 0xa5b, 
            0x60a57, 0x52b, 0xa93, 0x40e95
         };
        private static int[] MonthAdd = new int[] { 0, 0x1f, 0x3b, 90, 120, 0x97, 0xb5, 0xd4, 0xf3, 0x111, 0x130, 0x14e };
        private static string[] MonthName = new string[] { "*", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };
        private static string[] ShengXiao = new string[] { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };
        private static string[] TianGan = new string[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        public static string GetDate(DateTime dt)
        {
            return GetLunarCalendar(dt);
        }

        public static string GetLunarCalendar(DateTime dtDay)
        {
            int year;
            int month;
            int day;
            var y = dtDay.Year.ToString();
            var m = dtDay.Month.ToString();
            var d = dtDay.Day.ToString();
            try
            {
                year = int.Parse(y);
                month = int.Parse(m);
                day = int.Parse(d);
            }
            catch
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                day = DateTime.Now.Day;
            }
            string str4 = string.Empty;
            int num4 = (((((year - 0x781) * 0x16d) + ((year - 0x781) / 4)) + day) + MonthAdd[month - 1]) - 0x26;
            if (((year % 4) == 0) && (month > 2))
            {
                num4++;
            }
            int num5 = 0;
            int index = 0;
            int num6 = 0;
            int num8 = 0;
            while (num5 != 1)
            {
                if (LunarData[index] < 0xfff)
                {
                    num6 = 11;
                }
                else
                {
                    num6 = 12;
                }
                num8 = num6;
                while (num8 >= 0)
                {
                    int num9 = LunarData[index];
                    for (int i = 1; i < (num8 + 1); i++)
                    {
                        num9 /= 2;
                    }
                    num9 = num9 % 2;
                    if (num4 <= (0x1d + num9))
                    {
                        num5 = 1;
                        break;
                    }
                    num4 = (num4 - 0x1d) - num9;
                    num8--;
                }
                if (num5 == 1)
                {
                    break;
                }
                index++;
            }
            year = 0x781 + index;
            month = (num6 - num8) + 1;
            day = num4;
            if (num6 == 12)
            {
                if (month == ((LunarData[index] / 0x10000) + 1))
                {
                    month = 1 - month;
                }
                else if (month > ((LunarData[index] / 0x10000) + 1))
                {
                    month--;
                }
            }
            str4 = (((year + "年") + ShengXiao[((year - 4) % 60) % 12].ToString() + "年 ") + TianGan[((year - 4) % 60) % 10].ToString()) + DiZhi[((year - 4) % 60) % 12].ToString() + " ";
            if (month < 1)
            {
                str4 = str4 + "闰" + MonthName[-1 * month].ToString() + "月";
            }
            else
            {
                str4 = str4 + MonthName[month].ToString() + "月";
            }
            return (str4 + DayName[day].ToString() + "日");
        }
    }
}

