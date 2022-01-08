using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
   public class MyCalender
    {
        public DateTime Date { get; set; }
        public string DayName { get; set; }
        public int Month { get; set; }

        IEnumerable<MyCalender> myCalenders { get; set; }
    }
}
