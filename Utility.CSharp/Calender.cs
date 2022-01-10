using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
   public class Calender
    {
        public DateTime Date { get; set; }
        public string DayName { get; set; }
        public int Month { get; set; }

        IEnumerable<Calender> myCalenders { get; set; }
    }
}
