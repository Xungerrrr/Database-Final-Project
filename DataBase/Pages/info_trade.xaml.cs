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
    public sealed partial class info_trade : Page
    {
        ArrayList smallbases = new ArrayList();
        private ObservableCollection<Base> bases = new ObservableCollection<Base>();
        private ObservableCollection<Customer_trade_data> customer_Trade_Datas = new ObservableCollection<Customer_trade_data>();
        private ObservableCollection<Factor_trade_data> factor_Trade_Datas = new ObservableCollection<Factor_trade_data>();
        public int nowIndex;
        public int smallIndex;
        public info_trade()
        {
            this.InitializeComponent();
            smallIndex = 0;
            nowIndex = 1;
            bases.Add(new Base { icon = Symbol.Add, type = "交易", id = 1 });
            bases.Add(new Base { icon = Symbol.Add, type = "记录", id = 0 });


            ObservableCollection<Base> temp = new ObservableCollection<Base>();
            ObservableCollection<Base> temp2 = new ObservableCollection<Base>();
            temp.Add(new Base { icon = Symbol.Add, type = "购入记录", id = 0 });
            temp.Add(new Base { icon = Symbol.Add, type = "卖出记录", id = 1 });
            temp2.Add(new Base { icon = Symbol.Add, type = "购入", id = 0 });
            temp2.Add(new Base { icon = Symbol.Add, type = "卖出", id = 1 });
            smallbases.Add(temp);
            smallbases.Add(temp2);
            SmallTags.ItemsSource = smallbases[nowIndex];

            help();
            in_info.Visibility = Visibility.Visible;
            SqlHelper.GetAllCustomer_trade_data(customer_Trade_Datas, "Customer_trade_data");
            SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "factor_trade_data");
        }

        private void help()
        {
            Car_info.Visibility = Visibility.Collapsed;
            Car_info2.Visibility = Visibility.Collapsed;
            Custom_info.Visibility = Visibility.Collapsed;
            Custom_info2.Visibility = Visibility.Collapsed;
            Garbage_info.Visibility = Visibility.Collapsed;
            in_info.Visibility = Visibility.Collapsed;
            out_info.Visibility = Visibility.Collapsed;
            Total_profit.Visibility = Visibility.Collapsed;
        }

        private void show()
        {
            if (nowIndex == 0)
            {
                if (smallIndex == 0)
                {
                    Car_info.Visibility = Visibility.Visible;
                    Car_info2.Visibility = Visibility.Visible;
                }
                else if (smallIndex == 1)
                {
                    Total_profit.Text = "总收益：" + SqlHelper.GetTotal_profit("Customer_trade_data").ToString();
                    Custom_info.Visibility = Visibility.Visible;
                    Custom_info2.Visibility = Visibility.Visible;
                    Total_profit.Visibility = Visibility.Visible;
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
            help();
            show();
            if(nowIndex == 0)
            {
                menu.Visibility = Visibility.Collapsed;
            }
            else
            {
                menu.Visibility = Visibility.Visible;
            }
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

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            switch (smallIndex)
            {
                case 1:
                    {
                        SqlHelper.DeleteCustomer_trade_data("Customer_trade_data", (Customer_trade_data)s.DataContext);
                        SqlHelper.GetAllCustomer_trade_data(customer_Trade_Datas, "Customer_trade_data");
                        break;
                    }
                case 0:
                    {
                        SqlHelper.DeleteFactor_trade_data("factor_trade_data", (Factor_trade_data)s.DataContext);
                        SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "factor_trade_data");
                        break;
                    }
            }
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            info_update.type = nowIndex;
            
        }


        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (
                (Garbage_info.Visibility == Visibility.Visible &&
                (qid.Text == "" || ctnum.Text == "" || cid2.Text == "")) ||
                (in_info.Visibility == Visibility.Visible &&
                (ftid.Text == "" || ftprice.Text == "" || ftnum.Text == "" ||
                 fid2.Text == "" || cid3.Text == "")) ||
                (out_info.Visibility == Visibility.Visible &&
                (ctid.Text == "" || ctprice.Text == "" || ctnum2.Text == "" ||
                 cuid2.Text == "" || cid4.Text == ""))
                )
            {
                var dialog = new ContentDialog()
                {
                    Content = "请不要留空",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
                return;
            }
            if (smallIndex == 0)
            {
                Factor_trade_data factor_Trade_Data = new Factor_trade_data { ftid = ftid.Text, ftprice = String2int(ftprice.Text), cid = cid3.Text, fid = fid2.Text, ftnum = String2int(ftnum.Text) };
                String message1 = SqlHelper.AddFactor_trade_data(factor_Trade_Data, "Factor_trade_data");
                String message2 = SqlHelper.GetAllCustomer_trade_data(customer_Trade_Datas, "Customer_trade_data");
                String message3 = SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "factor_trade_data");
                String feedback = "";
                if (message1 != "not an error")
                {
                    feedback += message1 + "\n";
                }
                if (message2 != "not an error")
                {
                    feedback += message2 + "\n";
                }
                if (message3 != "not an error")
                {
                    feedback += message3 + "\n";
                }
                Cleanhelp();
                    var dialog = new ContentDialog()
                    {
                        Content = feedback == "" ? "创建成功" : feedback,
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
                ObservableCollection<Factor_trade_data> factor_Trade_Datas = new ObservableCollection<Factor_trade_data>();
                SqlHelper.GetAllCustomer(customers, "Customer");
                SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "Factor_Trade_Data");
                bool judge = false;
                bool judge2 = false;
                int price = 0;
                int count = 0;
                foreach (Factor_trade_data temp in factor_Trade_Datas)
                {
                    if (temp.cid == cid4.Text)
                    {
                        judge = true;
                        price += (temp.ftnum*temp.ftprice);
                        count += temp.ftnum;
                    }
                }
                if(count != 0)
                {
                    price = price / count;
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
                    int temp_ctnum = 0;
                    ObservableCollection<Garbage> temp_g = new ObservableCollection<Garbage>();
                    SqlHelper.GetAllGarbage(temp_g, "garbage");
                    foreach (Garbage temp in temp_g)
                    {
                        if (temp.cid == cid4.Text)
                        {
                            temp_ctnum = temp.ctnum;
                        }
                    }
                    if(String2int(ctnum2.Text) > temp_ctnum)
                    {
                        var dialog2 = new ContentDialog()
                        {
                            Content = "车库剩余不足",
                            PrimaryButtonText = "确定",
                            FullSizeDesired = false,
                        };
                        await dialog2.ShowAsync();
                        return;
                    }



                    Debug.Write(price);
                    price = (String2int(ctprice.Text) - price) * String2int(ctnum2.Text);
                    Customer_trade_data customer_Trade_Data = new Customer_trade_data { ctid = ctid.Text, ctprice = String2int(ctprice.Text), cid = cid4.Text, cuid = cuid2.Text, ctnum = String2int(ctnum2.Text), cprofit = price  };
                    SqlHelper.AddCustomer_trade_data(customer_Trade_Data, "customer_trade_data");
                    SqlHelper.GetAllCustomer_trade_data(customer_Trade_Datas, "Customer_trade_data");
                    SqlHelper.GetAllFactor_trade_data(factor_Trade_Datas, "factor_trade_data");
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