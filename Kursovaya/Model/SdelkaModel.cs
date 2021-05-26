using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Model
{
    class SdelkaModel
    {
        public int id { get; set; }

        public DateTime data { get; set; }

        public int count { get; set; }

        public decimal sum { get; set; }

        public string name { get; set; }
        public string fio { get; set; }
    }
}
