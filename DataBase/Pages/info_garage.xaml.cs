using DataBase.Model;
using DataBase.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DataBase.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class info_garage : Page
    {
        private ObservableCollection<Factor> factors = new ObservableCollection<Factor>();
        private ObservableCollection<Car> cars = new ObservableCollection<Car>();
        private ObservableCollection<Garbage> garbages = new ObservableCollection<Garbage>();
        private ObservableCollection<Base> bases = new ObservableCollection<Base>();
        private ObservableCollection<Factor_trade_data> factor_Trade_Datas = new ObservableCollection<Factor_trade_data>();
        public info_garage()
        {
            this.InitializeComponent();
            SqlHelper.GetAllFactor(factors, "factor");
            SqlHelper.GetAllCar(cars, "car");
            SqlHelper.GetAllGarbage(garbages, "garbage");
            SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "factor_trade_data");
            foreach (Garbage temp_g in garbages)
            {
                Base new_one = new Base();
                new_one.garage_id = temp_g.gid;
                foreach(Car temp_c in cars)
                {
                    if(temp_g.cid == temp_c.cid)
                    {
                        new_one.car_brand = temp_c.cbrand;
                        new_one.fid = temp_c.fid;
                        break;
                    }
                }
                foreach(Factor temp_f in factors)
                {
                    if(new_one.fid == temp_f.fid)
                    {
                        new_one.factory_name = temp_f.fname;
                        new_one.factory_address = temp_f.faddress;
                        break;
                    }
                }
                foreach(Factor_trade_data temp_ft in factor_Trade_Datas)
                {
                    if((new_one.fid == temp_ft.fid) && (temp_g.cid == temp_ft.cid))
                    {
                        new_one.garage_tnum = temp_ft.ftnum;
                        new_one.car_tprice = temp_ft.ftprice;
                    }
                }
                bases.Add(new_one);
            }
            Debug.Write(bases);
        }
    }
}
