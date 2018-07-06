using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DataBase.Model
{
    class Base
    {
        public int id { get; set; }
        public Symbol icon { get; set; }
        public string type { get; set; }

        public string fid { get; set; }

        public string garage_id { get; set; }
        public int garage_tnum { get; set; }
        public int car_tprice { get; set; }
        public string factory_name { get; set; }
        public string factory_address { get; set; }
        public string car_brand { get; set; }

    }
}
