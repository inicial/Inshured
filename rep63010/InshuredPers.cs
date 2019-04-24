using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rep6050
{
    class InshuredPers
    {
        public string Name;
        //public string secondName;
        public DateTime birhtDay;
        public decimal sumIns;
        public decimal sumMed;
        public decimal sumMedRb;
        public decimal deductible;
        public decimal sumVal;
        public decimal sumRb;
        public int tu_key;

        public InshuredPers(string fname/*,string sname*/,DateTime birday,int tukey)
        {
            Name = fname;
            tu_key = tukey;
           // secondName = sname;
            birhtDay = birday;
            sumIns = 0;
            deductible = 0;
            sumRb = 0;
        }
      

    }
}
