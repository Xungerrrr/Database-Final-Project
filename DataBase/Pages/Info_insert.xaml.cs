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
    public sealed partial class Info_insert : Page
    {
        ArrayList smallbases = new ArrayList();
        private ObservableCollection<Base> bases = new ObservableCollection<Base>();
        public int nowIndex;
        public int smallIndex;
        public Info_insert()
        {
            this.InitializeComponent();
            smallIndex = 0;
            nowIndex = 0;
            bases.Add(new Base { icon = Symbol.Add, type = "基本信息", id = 0 });
            bases.Add(new Base { icon = Symbol.Add, type = "交易", id = 1 });

            
            ObservableCollection<Base> temp = new ObservableCollection<Base>();
            ObservableCollection<Base> temp2 = new ObservableCollection<Base>();
            temp.Add(new Base { icon = Symbol.Add, type = "汽车", id = 0 });
            temp.Add(new Base { icon = Symbol.Add, type = "用户", id = 1 });
            temp.Add(new Base { icon = Symbol.Add, type = "车库", id = 2 });
            temp2.Add(new Base { icon = Symbol.Add, type = "购入", id = 0 });
            temp2.Add(new Base { icon = Symbol.Add, type = "卖出", id = 1 });
            smallbases.Add(temp);
            smallbases.Add(temp2);
            SmallTags.ItemsSource = smallbases[nowIndex];

            help();
            Car_info.Visibility = Visibility.Visible;

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
            if(nowIndex == 0)
            {
                if(smallIndex == 0)
                {
                    Car_info.Visibility = Visibility.Visible;
                }
                else if(smallIndex == 1)
                {
                    Custom_info.Visibility = Visibility.Visible;
                }
                else
                {
                    Garbage_info.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (smallIndex == 0)
                {
                    in_info.Visibility = Visibility.Visible;
                }
                else
                {
                    out_info.Visibility = Visibility.Visible;
                }
            }
        }

        private void BigTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            Base tag = (Base)e.ClickedItem;
            nowIndex = tag.id;
            Debug.Write(nowIndex);
            SmallTags.ItemsSource = smallbases[nowIndex];
            smallIndex = 0;
        }

        private void SmallTags_ItemClick(object sender, ItemClickEventArgs e)
        {
            Base tag = (Base)e.ClickedItem;
            smallIndex = tag.id;
            help();
            show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cleanhelp();
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

                    SqlHelper.AddFactor(factor, "factor");
                    SqlHelper.AddCar(car, "car");
                    Cleanhelp();
                    var dialog = new ContentDialog()
                    {
                        Content = "创建成功",
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
                    SqlHelper.AddCustomer(customer, "customer");
                    Cleanhelp();
                    var dialog = new ContentDialog()
                    {
                        Content = "创建成功",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
                else
                {
                    ObservableCollection<Car> cars = new ObservableCollection<Car>();
                    SqlHelper.GetAllCar(cars, "car");
                    bool judge = false;
                    foreach(Car temp in cars)
                    {
                        if(temp.cid == cid2.Text)
                        {
                            judge = true;
                            break;
                        }
                    }
                    if(judge == true)
                    {
                        Garbage garbage = new Garbage();
                        garbage.gid = qid.Text;
                        garbage.ctnum = String2int(ctnum.Text);
                        garbage.cid = cid2.Text;
                        SqlHelper.AddGarbage(garbage, "garbage");
                        Cleanhelp();
                        var dialog = new ContentDialog()
                        {
                            Content = "创建成功",
                            PrimaryButtonText = "确定",
                            FullSizeDesired = false,
                        };
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        var dialog = new ContentDialog()
                        {
                            Title = "提示",
                            Content = "车的编号不存在",
                            PrimaryButtonText = "确定",
                            FullSizeDesired = false,
                        };
                        await dialog.ShowAsync();
                    }
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
                    bool judge = false;
                    bool judge2 = false;

                    foreach (Car temp in cars)
                    {
                        if (temp.cid == cid3.Text)
                        {
                            judge = true;
                            break;
                        }
                    }

                    foreach (Factor temp in factors)
                    {
                        if (temp.fid == fid2.Text)
                        {
                            judge2 = true;
                            break;
                        }
                    }

                    if(judge && judge2)
                    {
                        Factor_trade_data factor_Trade_Data = new Factor_trade_data { ftid = ftid.Text, ftprice = String2int(ftprice.Text), cid = cid3.Text, fid = fid2.Text, ftnum = String2int(ftnum.Text) };
                        SqlHelper.AddFactor_trade_data(factor_Trade_Data, "Factor_trade_data");
                        Cleanhelp();
                        var dialog = new ContentDialog()
                        {
                            Content = "创建成功",
                            PrimaryButtonText = "确定",
                            FullSizeDesired = false,
                        };
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        var dialog = new ContentDialog()
                        {
                            Title = "提示",
                            Content = "车的编号或者车商的编号不存在",
                            PrimaryButtonText = "确定",
                            FullSizeDesired = false,
                        };
                        await dialog.ShowAsync();
                    }
                }
                else
                {
                    ObservableCollection<Car> cars = new ObservableCollection<Car>();
                    SqlHelper.GetAllCar(cars, "car");
                    ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
                    SqlHelper.GetAllCustomer(customers, "Customer");
                    bool judge = false;
                    bool judge2 = false;

                    foreach (Car temp in cars)
                    {
                        if (temp.cid == cid4.Text)
                        {
                            judge = true;
                            break;
                        }
                    }

                    foreach (Customer temp in customers)
                    {
                        if (temp.cuid == cuid2.Text)
                        {
                            judge2 = true;
                            break;
                        }
                    }

                    if (judge && judge2)
                    {

                        Customer_trade_data customer_Trade_Data = new Customer_trade_data { ctid = ctid.Text, ctprice = String2int(ctprice.Text), cid = cid4.Text, cuid = cuid2.Text, ctnum = String2int(ctnum2.Text) };
                        SqlHelper.AddCustomer_trade_data(customer_Trade_Data, "customer_trade_data");
                        Cleanhelp();
                        var dialog = new ContentDialog()
                        {
                            Content = "创建成功",
                            PrimaryButtonText = "确定",
                            FullSizeDesired = false,
                        };
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        var dialog = new ContentDialog()
                        {
                            Title = "提示",
                            Content = "车的编号或者客户编号不存在",
                            PrimaryButtonText = "确定",
                            FullSizeDesired = false,
                        };
                        await dialog.ShowAsync();
                    }
                }
            }
        }
    }
}
