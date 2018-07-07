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
        private ObservableCollection<Factory> factorys = new ObservableCollection<Factory>();
        private ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
        private ObservableCollection<Car> cars = new ObservableCollection<Car>();
        private ObservableCollection<Customer_trade_data> customer_Trade_Datas = new ObservableCollection<Customer_trade_data>();
        private ObservableCollection<Factory_trade_data> factory_Trade_Datas = new ObservableCollection<Factory_trade_data>();
        private ObservableCollection<Garage> garages = new ObservableCollection<Garage>();


        public Base_Information()
        {
            this.InitializeComponent();
            SqlHelper.GetAllFactory(factorys, "factory");
            SqlHelper.GetAllCustomer(customers, "customer");
            SqlHelper.GetAllCar(cars, "car");
            SqlHelper.GetAllCustomer_trade_data(customer_Trade_Datas, "Customer_trade_data");
            SqlHelper.GetAllFactory_trade_data(factory_Trade_Datas, "factory_trade_data");
            SqlHelper.GetAllGarage(garages, "garage");

            bases.Add(new Base { icon = Symbol.Add, type = "厂商", id = 0 });
            bases.Add(new Base { icon = Symbol.Add, type = "客户", id = 1 });
            bases.Add(new Base { icon = Symbol.Add, type = "车辆", id = 2 });
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            switch (nowIndex)
            {
                case 0:
                    {
                        SqlHelper.DeleteFactory("Factory", (Factory)s.DataContext);
                        SqlHelper.GetAllFactory(factorys, "Factory");
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
                        SqlHelper.DeleteFactory_trade_data("factory_trade_data", (Factory_trade_data)s.DataContext);
                        SqlHelper.GetAllFactory_trade_data(factory_Trade_Datas, "factory_trade_data");
                        break;
                    }
                case 5:
                    {
                        SqlHelper.DeleteGarage("garage", (Garage)s.DataContext);
                        SqlHelper.GetAllGarage(garages, "garage");
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
                        info_update.id = ((Factory)s.DataContext).fid;
                        info_update.id2 = "";
                        break;
                    }
                case 1:
                    {
                        info_update.id = ((Customer)s.DataContext).cuid;
                        break;
                    }
                case 2:
                    {
                        info_update.id = ((Car)s.DataContext).fid;
                        info_update.id2 = ((Car)s.DataContext).cid;
                        break;
                    }
                case 3:
                    {
                        info_update.id = ((Customer_trade_data)s.DataContext).ctid;
                        break;
                    }
                case 4:
                    {
                        info_update.id = ((Factory_trade_data)s.DataContext).ftid;
                        break;
                    }
                case 5:
                    {
                        info_update.id = ((Garage)s.DataContext).gid;
                        break;
                    }
            }


            this.Frame.Navigate(typeof(info_update));
        }

        private void FactorysList_ItemClick(object sender, ItemClickEventArgs e)
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
                        FactorysList.Visibility = Visibility.Visible;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;



                        FactorysGrid.Visibility = Visibility.Visible;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;

                        break;
                }
                case 1:
                {
                        FactorysList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Visible;
                        Carlist.Visibility = Visibility.Collapsed;



                        FactorysGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Visible;
                        CarGrid.Visibility = Visibility.Collapsed;

                        break;
                }
                case 2:
                {
                        FactorysList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Visible;


                        FactorysGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Visible;

                        break;
                }
                case 3:
                {
                        FactorysList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;


                        FactorysGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;

                        break;
                }
                case 4:
                {
                        FactorysList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;


                        FactorysGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;

                        break;
                }
                case 5:
                {
                        FactorysList.Visibility = Visibility.Collapsed;
                        CustomeList.Visibility = Visibility.Collapsed;
                        Carlist.Visibility = Visibility.Collapsed;



                        FactorysGrid.Visibility = Visibility.Collapsed;
                        CustomeGrid.Visibility = Visibility.Collapsed;
                        CarGrid.Visibility = Visibility.Collapsed;

                        break;
                }
            }
        }
    }
}
