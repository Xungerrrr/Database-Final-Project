using DataBase.Model;
using DataBase.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DataBase.ViewModels
{
    class MenuItemManager
    {
        public ObservableCollection<MenuItem> menuItems { set; get; } = new ObservableCollection<MenuItem>();

        public void GetMenuItem()
        {
            menuItems.Add(new MenuItem() { id = 0, Icon = Symbol.PreviewLink, Text = "基本信息" , content= typeof(Base_Information) });
            //添加管理页
            menuItems.Add(new MenuItem() { id = 1, Icon = Symbol.Edit, Text = "信息录入" , content = typeof(Info_insert) });
            menuItems.Add(new MenuItem() { id = 1, Icon = Symbol.Switch, Text = "交易记录", content = typeof(info_trade) });
            menuItems.Add(new MenuItem() { id = 1, Icon = Symbol.Library, Text = "车库管理", content = typeof(info_garage) });
        }
    }
}
