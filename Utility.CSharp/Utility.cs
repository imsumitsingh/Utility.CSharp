
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using CSharp;



namespace CSharp
{
    public static class Utility
    {
        public static string ToProper(this string text)
        {
            // string newText = string.Join(" ", (text.Split(' ').Select(e => e.Substring(0, 1).ToUpper() + e.Substring(1).ToLower()).ToArray()));
            string newText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
            return newText;
        }
        public static int GetRandomNumber(int digit = 0)
        {
            Random r = new Random();
            if (digit == 0)
            {
                return r.Next();
            }
            int min = (int)Math.Pow(10, digit - 1);
            int max = (int)Math.Pow(10, digit) - 1;

            return r.Next(min, max);
        }
        public static int GetRandomNumber(int min,int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }
        private static Random rand = new Random();

        public static string Shuffle(this String str)
        {
            var sortedList = new SortedList<int, char>();
            foreach (var c in str)
                sortedList.Add(rand.Next(), c);
            return new string(sortedList.Values.ToArray());
        }
        public static string CharAt(this String text, int index)
        {

            return text[index].ToString();
        }
        public static string ToUpper(this String text, int length)
        {

            return text.Substring(0, length).ToUpper() + text.Substring(length, text.Length - length);
        }
        public static string GetRandomString(int length, bool SpecialCharecter = true, bool Numbers = true, bool Upper = true, bool Lower = true)
        {

            string strMix = "";
            string strUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strLower = "abcdefghijklmnopqrstuvwxyz";
            string strDigit = "1234567890";
            string strSpecialChar = "!@#$%^&(){}?./*-+";
            if (Upper)
            {
                strMix += strUpper;
            }
            if (Upper)
            {
                strMix += strLower;
            }
            if (SpecialCharecter)
            {
                strMix += strSpecialChar;
            }
            if (Numbers)
            {
                strMix += strDigit;
            }

            StringBuilder stringBuilder = new StringBuilder();
            strMix = strMix.Shuffle();

            for (int i = 0; i < length; i++)
            {

                var v = strMix[rand.Next(0, strMix.Length - 1)];
                stringBuilder.Append(v.ToString());

            }

            return stringBuilder.ToString();
        }


        public static List<string> GetFinancialYearList(int StartYear, int? EndYear = null)
        {
            List<string> FYear = new List<string>();
            if (EndYear is null)
            {
                EndYear = DateTime.Now.Year;
            }
            var minYear = new DateTime(StartYear, 01, 01).AddMonths(-3).Year;
            var maxYear = new DateTime(EndYear.Value, 01, 01).AddMonths(9).Year;
            for (int i = minYear; i < maxYear; i++)
            {
                FYear.Add(i.ToString() + "-" + (i + 1).ToString());
            }
            FYear.Reverse();
            return FYear;
        }
        private static String[] units = { "Zero", "One", "Two", "Three",
    "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
    "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
    "Seventeen", "Eighteen", "Nineteen" };
        private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
    "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public static String ToWords(this double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    var str = Conver(amount_int) + " Only.";
                    var arr = str.Split(' ');
                    var f = arr.Where(e => e != "").ToArray();
                    return string.Join(" ", f);

                }
                else
                {
                    var str = Conver(amount_int) + " Point " + Conver(amount_dec) + " Only.";
                    var arr = str.Split(' ');
                    var f = arr.Where(e => e != "").ToArray();
                    return string.Join(" ", f);
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }

        public static String Conver(Int64 i)
        {
            if (i < 20)
            {
                return units[i];
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + Conver(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + Conver(i % 100) : "");
            }
            if (i < 100000)
            {
                return Conver(i / 1000) + " Thousand "
                + ((i % 1000 > 0) ? " " + Conver(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return Conver(i / 100000) + " Lakh "
                        + ((i % 100000 > 0) ? " " + Conver(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return Conver(i / 10000000) + " Crore "
                        + ((i % 10000000 > 0) ? " " + Conver(i % 10000000) : "");
            }
            return Conver(i / 1000000000) + " Arab "
                    + ((i % 1000000000 > 0) ? " " + Conver(i % 1000000000) : "");
        }
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in Props)
            {
                dt.Columns.Add(property.Name);
            }

            foreach (T item in list)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetList<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetList<T>(DataRow dr)
        {

            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in typeof(T).GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }


        public static double FindDistance(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }



        static Age FindAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            var age = new Age
            {
                Days = Days,
                Months = Months,
                Years = Years
            };

            return age;
        }
        public static string Trim(this string text, int length = 0)
        {
            string output = "";
            if (length == 0 || text.Length <= length)
            {
                return text;
            }
            else if (length < 0)
            {

                output = text.Substring(length - (length * 2));

            }
            else
            {
                output = text.Substring(0, text.Length - length);
            }
            return output;
        }

        public static string Left(this string text, int length)
        {
            if (length <= 0 || text.Length <= length)
            {
                return text;
            }
            else
            {
                return text.Substring(0, length);
            }
        }
        public static string Right(this string text, int length)
        {
            if (length <= 0 || text.Length <= length)
            {
                return text;
            }
            else
            {
                return text.Substring(text.Length - length, length);
            }
        }
        public static bool IsMobile(this string text)
        {
            Regex reg = new Regex(@"^[0-9]{10}$"); ;
            return reg.IsMatch(text);
        }
        public static bool IsEmail(this string text)
        {
            Regex reg = new Regex(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}");
            return reg.IsMatch(text);
        }
        public static string Reverse(this string text)
        {
            string output = "";
            for (int i = text.Length - 1; i >= 0; i--)
            {
                output += text.Substring(i, 1);
            }

            

            return output;


        }
        public static string ToCurrency(this double amount)
        {

            return amount.ToString("C");


        }
        public static string Encrypt(this string str)
        {
            string EncrptKey = "sumitkumarsingh1994";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(this string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "sumitkumarsingh1994";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        public static string CreatorName { get; } = "Sumit";

        public static string ToyyyyMMdd(this DateTime dt) => dt.ToString("yyyy-MM-dd");
        public static DateTime EMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        public static string ToddMMMyyyy(this DateTime dt) => dt.ToString("dd-MMM-yyyy");


        public static string ToJSON<T>(this T obj)
        {


            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize<T>(obj, opt);
            return strJson;
     

        }
        public static string ToJSON<T>(this List<T> obj)
        {


            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string json = JsonSerializer.Serialize<List<T>>(obj, opt);
            return json;


        }
        public static string ToJSON(this DataTable dataTable)
        {
            string JSONString = "";
            JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }
        public static DataTable ToDataTable(this string JSONString)
        {
           DataTable dt = null;
            dt =(DataTable) Newtonsoft.Json.JsonConvert.DeserializeObject(JSONString,typeof(DataTable));
            return dt;
        }
        public static List<T> ToLists<T>(this string JSONString)
        {

            List<T> list = new List<T>();
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            list = JsonSerializer.Deserialize<List<T>>(JSONString, opt);
            return list;


        }
        public static T ToList<T>(this string JSONString)
        {

            T list = Activator.CreateInstance<T>();
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            list = JsonSerializer.Deserialize<T>(JSONString, opt);
            return list;


        }
        public static dynamic ToAnnonimousList<dynamic>(this string JSONString)
        {

            dynamic list = Activator.CreateInstance<dynamic>();
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            list = JsonSerializer.Deserialize<dynamic>(JSONString, opt);
            return list;


        }
        public static List<Property> Properties<T>(this T type)
        {
            List<Property> list = new List<Property>();
            var prop = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in prop)
            {
                list.Add(new Property() { Name = p.Name, Type = p.PropertyType.Name });
            }

            return list;
        }


        public static DataTable CreateDataTable(params string[] ColumnNames)
        {
            DataTable dt = new DataTable();
            foreach (var str in ColumnNames)
            {
                dt.Columns.Add(str, typeof(object));
            }
            return dt;
        }
        public static DataTable Sort(this DataTable dt,SortType type, params string[] ColumnName)
        {
            DataView dv = new DataView(dt);
            dv.Sort = String.Join(",",ColumnName) + " " + type;
            return dv.ToTable();
        }
        public static DataTable Filter(this DataTable dt, string Filter)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = Filter;
            return dv.ToTable();
        }
        public static object Sum(this DataTable dt, string ColumnName,string Filter=null)
        {
            string query = Aggregate.SUM + " (" + ColumnName + ")";
           object obj= dt.Compute(query, Filter);
            return obj;
        }
        public static object Min(this DataTable dt, string ColumnName, string Filter = null)
        {
            string query = Aggregate.MIN + " (" + ColumnName + ")";
            object obj = dt.Compute(query, Filter);
            return obj;
        }
        public static object Max(this DataTable dt, string ColumnName, string Filter = null)
        {
            string query = Aggregate.MAX + " (" + ColumnName + ")";
            object obj = dt.Compute(query, Filter);
            return obj;
        }
        public static object Count(this DataTable dt, string ColumnName, string Filter = null)
        {
            string query = Aggregate.COUNT + " (" + ColumnName + ")";
            object obj = dt.Compute(query, Filter);
            return obj;
        }
       
        public static object Avg(this DataTable dt, string ColumnName, string Filter = null)
        {
            string query = Aggregate.AVG + " (" + ColumnName + ")";
            object obj = dt.Compute(query, Filter);
            return obj;
        }
       
        public static DataTable Remove(this DataTable dt, int ColumnIndex, object ColumnValue)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row[ColumnIndex] == ColumnValue)
                {
                    dt.Rows.Remove(row);
                }
            }
            return dt;
        }
        public static DataTable Remove(this DataTable dt, int RowIndex)
        {
            dt.Rows.Remove(dt.Rows[RowIndex]);
            return dt;
        }
        public static object GetCellText(this DataTable dt, int RowIndex, int ColumnIndex)
        {
           return dt.Rows[RowIndex][ColumnIndex];
        }
        public static DataTable Fetch(this DataTable dt, int PageNo, int PageLength)
        {
            int startRecord = (PageLength * PageNo) - PageLength;
            return dt.AsEnumerable().Skip(startRecord).Take(PageLength).CopyToDataTable();
        }
        public static void SetCellText(this DataTable dt, int RowIndex, int ColumnIndex,object CellText)
        {
           dt.Rows[RowIndex][ColumnIndex]=CellText;
        }

        public static DataSet GetCalender(int year)
        {
            DataSet ds = new DataSet();

            for (int i = 1; i <= 12; i++)
            {
                var MonthCalender = new DataTable(new DateTime(year, i, 01).ToString("MMMM-yy"));
                var lastdate = new DateTime(year, i, 01).EMonth().Day;
                MonthCalender.Columns.Add("Date");
                MonthCalender.Columns.Add("DayName");


                for (int j = 1; j <= lastdate; j++)
                {
                    MonthCalender.Rows.Add(new DateTime(year, i, j), new DateTime(year, i, j).ToString("dddd"));
                }
                ds.Merge(MonthCalender);
            }

            return ds;
        }
        public static List<Calender> GetCalenderOf(int year)
        {
            List<Calender> myCalenders = new List<Calender>();

            for (int i = 1; i <= 12; i++)
            {
                
                var lastdate = new DateTime(year, i, 01).EMonth().Day;
                for (int j = 1; j <= lastdate; j++)
                {
                    myCalenders.Add(new Calender() { Date = new DateTime(year, i, j), DayName = new DateTime(year, i, j).ToString("dddd"), Month = i });
                }
               
            }

            return myCalenders;
        }
        public static DataTable GetCalenderOf(int year, int month)
        {

            var MonthCalender = new DataTable(new DateTime(year, month, 01).ToString("MMMM-yy"));
            var lastdate = new DateTime(year, month, 01).EMonth().Day;
            MonthCalender.Columns.Add("Date");
            MonthCalender.Columns.Add("DayName");
            for (int j = 1; j <= lastdate; j++)
            {
                MonthCalender.Rows.Add(new DateTime(year, month, j).ToString("yyyy-MM-dd"), new DateTime(year, month, j).ToString("dddd"));
            }
            return MonthCalender;
        }
       
        public static DateTime[] GetDatesOfMonth(int year, int month)
        {

            DateTime[] dt = new DateTime[new DateTime(year, month, 01).EMonth().Day + 1];
            var lastdate = new DateTime(year, month, 01).EMonth().Day;
            for (int j = 1; j <= lastdate; j++)
            {
                dt[j] = new DateTime(year, month, j);
            }
            return dt;
        }
    }




    public enum SortType
    {
        ASC,DESC
    }
    public enum Aggregate
    {
        MIN,MAX,AVG,SUM,COUNT
    }

    public enum Month
    {
        January = 1, February, March, April, May, June, July, August, September, October, November, December
    }

}

