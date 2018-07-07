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
        private ObservableCollection<Factory> factorys = new ObservableCollection<Factory>();
        private ObservableCollection<Car> cars = new ObservableCollection<Car>();
        private ObservableCollection<Garage> garages = new ObservableCollection<Garage>();
        private ObservableCollection<Base> bases = new ObservableCollection<Base>();
        private ObservableCollection<Factory_trade_data> factory_Trade_Datas = new ObservableCollection<Factory_trade_data>();
        public info_garage()
        {
            this.InitializeComponent();
            SqlHelper.GetAllFactory(factorys, "factory");
            SqlHelper.GetAllCar(cars, "car");
            SqlHelper.GetAllGarage(garages, "garage");
            SqlHelper.GetAllFactory_trade_data(factory_Trade_Datas, "factory_trade_data");
            foreach (Garage temp_g in garages)
            {
                Base new_one = new Base();
                new_one.garage_id = temp_g.gid;
                new_one.garage_tnum = temp_g.ctnum;
                foreach(Car temp_c in cars)
                {
                    if(temp_g.cid == temp_c.cid)
                    {
                        new_one.car_brand = temp_c.cbrand;
                        new_one.fid = temp_c.fid;
                        break;
                    }
                }
                foreach(Factory temp_f in factorys)
                {
                    if(new_one.fid == temp_f.fid)
                    {
                        new_one.factory_name = temp_f.fname;
                        new_one.factory_address = temp_f.faddress;
                        break;
                    }
                }
                foreach(Factory_trade_data temp_ft in factory_Trade_Datas)
                {
                    if((new_one.fid == temp_ft.fid) && (temp_g.cid == temp_ft.cid))
                    {
                        new_one.car_tprice = temp_ft.ftprice;
                    }
                }
                bases.Add(new_one);
            }
            Debug.Write(bases);
        }
    }
}
