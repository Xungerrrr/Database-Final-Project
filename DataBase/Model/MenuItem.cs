using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DataBase.Model
{
    class MenuItem
    {
        public int id { get; set; }
        public Symbol Icon { get; set; }
        public string Text { get; set; }
        public Type content { get; set; }
    }
}
