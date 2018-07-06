using DataBase.Model;
using DataBase.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
//asdfsadfasdfasdf
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DataBase.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Base_Information : Page
    {
        public int num = 0;
        private ObservableCollection<Base> bases = new ObservableCollection<Base>();
        public int nowIndex;
        private ObservableCollection<Factor> factors = new ObservableCollection<Factor>();
        private ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
        private ObservableCollection<Car> cars = new ObservableCollection<Car>();
        private ObservableCollection<Customer_trade_data> customer_Trade_Datas = new ObservableCollection<Customer_trade_data>();
        private ObservableCollection<Factor_trade_data> factor_Trade_Datas = new ObservableCollection<Factor_trade_data>();
        private ObservableCollection<Garbage> garbages = new ObservableCollection<Garbage>();


        public Base_Information()
        {
            this.InitializeComponent();
            SqlHelper.GetAllFactor(factors, "factor");
            SqlHelper.GetAllCustomer(customers, "customer");
            SqlHelper.GetAllCar(cars, "car");
            SqlHelper.GetAllCustomer_trade_data(customer_Trade_Datas, "Customer_trade_data");
            SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "factor_trade_data");
            SqlHelper.GetAllGarbage(garbages, "garbage");

            bases.Add(new Base { icon = Symbol.Add, type = "产商", id = 0 });
            bases.Add(new Base { icon = Symbol.Add, type = "用户", id = 1 });
            bases.Add(new Base { icon = Symbol.Add, type = "汽车", id = 2 });
            bases.Add(new Base { icon = Symbol.Add, type = "车库", id = 5 });
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            switch (nowIndex)
            {
                case 0:
                    {
                        SqlHelper.DeleteFactor("Factor", (Factor)s.DataContext);
                        SqlHelper.GetAllFactor(factors, "Factor");
                        break;
                    }
                case 1:
                    {
                        SqlHelper.DeleteCustomer("customer", (Customer)s.DataContext);
                        SqlHelper.GetAllCustomer(customers, "customer");
                        break;
                    }
                case 2:
                    {
                        SqlHelper.DeleteCar("car", (Car)s.DataContext);
                        SqlHelper.GetAllCar(cars, "car");
                        break;
                    }
                case 3:
                    {
                        SqlHelper.DeleteCustomer_trade_data("Customer_trade_data", (Customer_trade_data)s.DataContext);
                        SqlHelper.GetAllCustomer_trade_data(customer_Trade_Datas, "Customer_trade_data");
                        break;
                    }
                case 4:
                    {
                        SqlHelper.DeleteFactor_trade_data("factor_trade_data", (Factor_trade_data)s.DataContext);
                        SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "factor_trade_data");
                        break;
                    }
                case 5:
                    {
                        SqlHelper.DeleteGarbage("garbage", (Garbage)s.DataContext);
                        SqlHelper.GetAllGarbage(garbages, "garbage");
                        break;
                    }
            }
        }   

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            info_update.type = nowIndex;
            //info_update.id = 
            switch (nowIndex)
            {
                case 0:
                    {
                        info_update.id = ((Factor)s.DataContext).fid;
                        break;
                    }
                case 1:
                    {
                        info_update.id = ((Customer)s.DataContext).cuid;
                        break;
                    }
                case 2:
                    {
                        info_update.id = ((Car)s.DataContext).cid;
                        break;
                    }
                case 3:
                    {
                        info_update.id = ((Customer_trade_data)s.DataContext).ctid;
                        break;
                    }
                case 4:
                    {
                        info_update.id = ((Factor_trade_data)s.DataContext).ftid;
                        break;
                    }
                case 5:
                    {
                        info_update.id = ((Garbage)s.DataContext).gid;
                        break;
                    }
            }


            this.Frame.Navigate(typeof(info_update));
        }

        private void FactorsList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }



        private void BigTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            Base tag = (Base)e.ClickedItem;
            nowIndex = tag.id;

            switch (nowIndex)
            {
                case 0:
                {
                        FactorsList.Visibility = Visibility.Visible;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;
                        Customer_trade_datalist.Visibility = Visibility.Collapsed;
                        Factor_trade_datalist.Visibility = Visibility.Collapsed;
                        Garbagelist.Visibility = Visibility.Collapsed;


                        FactorsGrid.Visibility = Visibility.Visible;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;
                        Customer_trade_dataGrid.Visibility = Visibility.Collapsed;
                        Factor_trade_dataGrid.Visibility = Visibility.Collapsed;
                        GarbageGrid.Visibility = Visibility.Collapsed;
                        break;
                }
                case 1:
                {
                        FactorsList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Visible;
                        Carlist.Visibility = Visibility.Collapsed;
                        Customer_trade_datalist.Visibility = Visibility.Collapsed;
                        Factor_trade_datalist.Visibility = Visibility.Collapsed;
                        Garbagelist.Visibility = Visibility.Collapsed;


                        FactorsGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Visible;
                        CarGrid.Visibility = Visibility.Collapsed;
                        Customer_trade_dataGrid.Visibility = Visibility.Collapsed;
                        Factor_trade_dataGrid.Visibility = Visibility.Collapsed;
                        GarbageGrid.Visibility = Visibility.Collapsed;
                        break;
                }
                case 2:
                {
                        FactorsList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Visible;
                        Customer_trade_datalist.Visibility = Visibility.Collapsed;
                        Factor_trade_datalist.Visibility = Visibility.Collapsed;
                        Garbagelist.Visibility = Visibility.Collapsed;

                        FactorsGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Visible;
                        Customer_trade_dataGrid.Visibility = Visibility.Collapsed;
                        Factor_trade_dataGrid.Visibility = Visibility.Collapsed;
                        GarbageGrid.Visibility = Visibility.Collapsed;
                        break;
                }
                case 3:
                {
                        FactorsList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;
                        Customer_trade_datalist.Visibility = Visibility.Visible;
                        Factor_trade_datalist.Visibility = Visibility.Collapsed;
                        Garbagelist.Visibility = Visibility.Collapsed;

                        FactorsGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;
                        Customer_trade_dataGrid.Visibility = Visibility.Visible;
                        Factor_trade_dataGrid.Visibility = Visibility.Collapsed;
                        GarbageGrid.Visibility = Visibility.Collapsed;
                        break;
                }
                case 4:
                {
                        FactorsList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;
                        Customer_trade_datalist.Visibility = Visibility.Collapsed;
                        Factor_trade_datalist.Visibility = Visibility.Visible;
                        Garbagelist.Visibility = Visibility.Collapsed;

                        FactorsGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;
                        Customer_trade_dataGrid.Visibility = Visibility.Collapsed;
                        Factor_trade_dataGrid.Visibility = Visibility.Visible;
                        GarbageGrid.Visibility = Visibility.Collapsed;
                        break;
                }
                case 5:
                {
                        FactorsList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;
                        Customer_trade_datalist.Visibility = Visibility.Collapsed;
                        Factor_trade_datalist.Visibility = Visibility.Collapsed;
                        Garbagelist.Visibility = Visibility.Visible;


                        FactorsGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;
                        Customer_trade_dataGrid.Visibility = Visibility.Collapsed;
                        Factor_trade_dataGrid.Visibility = Visibility.Collapsed;
                        GarbageGrid.Visibility = Visibility.Visible;
                        break;
                }
            }
        }
    }
}
