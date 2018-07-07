using DataBase.Model;
using DataBase.Service;
using System;
using System.Collections;
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
    public sealed partial class info_update : Page
    {
        ArrayList smallbases = new ArrayList();
        private ObservableCollection<Base> bases = new ObservableCollection<Base>();
        public int nowIndex;
        public int smallIndex;
        public static int type { get; set; } = 0;
        public static string id { get; set; }
        public static string id2 { get; set; }
        public info_update()
        {
            this.InitializeComponent();
            switch (type)
            {
                case 0:
                    {
                        nowIndex = 0;
                        smallIndex = 0;
                        break;
                    }
                case 1:
                    {
                        nowIndex = 0;
                        smallIndex = 1;
                        break;
                    }
                case 2:
                    {
                        nowIndex = 0;
                        smallIndex = 0;
                        break;
                    }
                case 3:
                    {
                        nowIndex = 1;
                        smallIndex = 0;
                        break;
                    }
                case 4:
                    {
                        nowIndex = 1;
                        smallIndex = 1;
                        break;
                    }
                case 5:
                    {
                        nowIndex = 0;
                        smallIndex = 2;
                        break;
                    }
            }
            
            

            help();
            show();

            


        }

        private void help()
        {
            Car_info.Visibility = Visibility.Collapsed;
            Custom_info.Visibility = Visibility.Collapsed;
            Garbage_info.Visibility = Visibility.Collapsed;
            in_info.Visibility = Visibility.Collapsed;
            out_info.Visibility = Visibility.Collapsed;
        }

        private void show()
        {
            if (nowIndex == 0)
            {
                if (smallIndex == 0)
                {
                    Car_info.Visibility = Visibility.Visible;
                    ObservableCollection<Car> cars = new ObservableCollection<Car>();
                    ObservableCollection<Factor> factors = new ObservableCollection<Factor>();
                    SqlHelper.GetAllCar(cars, "car", id2);

                    if (id2.Equals(""))
                    {
                        cid.Visibility = Visibility.Collapsed;
                        cbrand.Visibility = Visibility.Collapsed;
                        cprice.Visibility = Visibility.Collapsed;
                        a1.Visibility = Visibility.Collapsed;
                        a2.Visibility = Visibility.Collapsed;
                        a3.Visibility = Visibility.Collapsed;
                    }


                    SqlHelper.GetAllFactor(factors, "factor", id);
                    fid.Text = factors[0].fid;
                    fname.Text = factors[0].fname;
                    faddress.Text = factors[0].faddress;
                    cid.Text = cars[0].cid;
                    cbrand.Text = cars[0].cbrand;
                    cprice.Text = (cars[0].cprice).ToString();

                }
                else if (smallIndex == 1)
                {
                    Custom_info.Visibility = Visibility.Visible;
                    ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
                    SqlHelper.GetAllCustomer(customers, "customer", id);
                    cuid.Text = customers[0].cuid;
                    cuname.Text = customers[0].cuname;
                    cuaddress.Text = customers[0].cuaddress;
                }
                else
                {
                    Garbage_info.Visibility = Visibility.Visible;
                    ObservableCollection<Garbage> garbages = new ObservableCollection<Garbage>();
                    SqlHelper.GetAllGarbage(garbages, "garbage", id);
                    qid.Text = garbages[0].gid;
                    ctnum.Text = (garbages[0].ctnum).ToString();
                    cid2.Text = garbages[0].cid;
                }
            }
            else
            {
                if (smallIndex == 0)
                {
                    in_info.Visibility = Visibility.Visible;
                    ObservableCollection<Factor_trade_data> factor_Trade_Datas = new ObservableCollection<Factor_trade_data>();
                    ftid.Text = factor_Trade_Datas[0].ftid;
                    ftprice.Text = (factor_Trade_Datas[0].ftprice).ToString();
                    ftnum.Text = (factor_Trade_Datas[0].ftnum).ToString();
                    fid2.Text = (factor_Trade_Datas[0].fid);
                    cid3.Text = (factor_Trade_Datas[0].cid);
                }
                else
                {
                    out_info.Visibility = Visibility.Visible;
                    ObservableCollection<Customer_trade_data> customer_Trade_Datas = new ObservableCollection<Customer_trade_data>();
                    ctid.Text = customer_Trade_Datas[0].ctid;
                    ctprice.Text = customer_Trade_Datas[0].cuid;
                    ctprice.Text = (customer_Trade_Datas[0].ctprice).ToString();
                    ctnum2.Text = (customer_Trade_Datas[0].ctnum).ToString();
                    cuid2.Text = (customer_Trade_Datas[0].cuid);
                    cid4.Text = (customer_Trade_Datas[0].cid);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Base_Information));
        }

        private int String2int(string s)
        {
            int a1 = int.Parse(s);
            int a2;
            int.TryParse(s, out a2);
            return Convert.ToInt32(s);
        }

        private void Cleanhelp()
        {
            fid.Text = "";
            fname.Text = "";
            faddress.Text = "";
            cid.Text = "";
            cbrand.Text = "";
            cprice.Text = "";
            cuid.Text = "";
            cbrand.Text = "";
            cuid.Text = "";
            cuname.Text = "";
            cuaddress.Text = "";
            qid.Text = "";
            ctnum.Text = "";
            cid2.Text = "";
            ftid.Text = "";
            ftprice.Text = "";
            ftnum.Text = "";
            fid2.Text = "";
            cid3.Text = "";
            ctid.Text = "";
            ctprice.Text = "";
            ctnum2.Text = "";
            cuid2.Text = "";
            cid4.Text = "";
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (nowIndex == 0)
            {
                if (smallIndex == 0)
                {
                    Car car = new Car();
                    car.cid = cid.Text;
                    car.cbrand = cbrand.Text;
                    car.cprice = String2int(cprice.Text);
                    car.fid = fid.Text;

                    Factor factor = new Factor();
                    factor.fid = fid.Text;
                    factor.faddress = faddress.Text;
                    factor.fname = fname.Text;

                    SqlHelper.UpdateFactor("factor", factor);
                    SqlHelper.UpdateCar("car", car);
                    var dialog = new ContentDialog()
                    {
                        Content = "更新成功",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
                else if (smallIndex == 1)
                {
                    Customer customer = new Customer();
                    customer.cuid = cuid.Text;
                    customer.cuname = cuname.Text;
                    customer.cuaddress = cuaddress.Text;
                    SqlHelper.UpdateCustomer("customer" ,customer);
                    var dialog = new ContentDialog()
                    {
                        Content = "更新成功",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
                else
                {
                    ObservableCollection<Car> cars = new ObservableCollection<Car>();
                    SqlHelper.GetAllCar(cars, "car");
                    Garbage garbage = new Garbage();
                    garbage.gid = qid.Text;
                    garbage.ctnum = String2int(ctnum.Text);
                    garbage.cid = cid2.Text;
                    SqlHelper.UpdateGarbage("garbage",garbage);
                    var dialog = new ContentDialog()
                    {
                        Content = "更新成功",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
            }
            else
            {
                if (smallIndex == 0)
                {
                    ObservableCollection<Car> cars = new ObservableCollection<Car>();
                    SqlHelper.GetAllCar(cars, "car");
                    ObservableCollection<Factor> factors = new ObservableCollection<Factor>();
                    SqlHelper.GetAllFactor(factors, "factor");
                    Factor_trade_data factor_Trade_Data = new Factor_trade_data { ftid = ftid.Text, ftprice = String2int(ftprice.Text), cid = cid3.Text, fid = fid2.Text, ftnum = String2int(ftnum.Text) };
                    SqlHelper.UpdateFactor_trade_data( "Factor_trade_data", factor_Trade_Data);
                    var dialog = new ContentDialog()
                    {
                        Content = "更新成功",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
                else
                {
                    ObservableCollection<Car> cars = new ObservableCollection<Car>();
                    SqlHelper.GetAllCar(cars, "car");
                    ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
                    SqlHelper.GetAllCustomer(customers, "Customer");
                    Customer_trade_data customer_Trade_Data = new Customer_trade_data { ctid = ctid.Text, ctprice = String2int(ctprice.Text), cid = cid4.Text, cuid = cuid2.Text, ctnum = String2int(ctnum.Text) };
                    SqlHelper.UpdateCustomer_trade_data("customer_trade_data", customer_Trade_Data);
                    var dialog = new ContentDialog()
                    {
                        Content = "更新成功",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
            }
            this.Frame.Navigate(typeof(Base_Information));
        }
    }
}
