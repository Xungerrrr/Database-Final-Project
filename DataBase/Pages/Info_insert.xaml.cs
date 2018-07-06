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

            
            ObservableCollection<Base> temp = new ObservableCollection<Base>();
            ObservableCollection<Base> temp2 = new ObservableCollection<Base>();
            temp.Add(new Base { icon = Symbol.Add, type = "汽车", id = 0 });
            temp.Add(new Base { icon = Symbol.Add, type = "用户", id = 1 });
            temp.Add(new Base { icon = Symbol.Add, type = "车库", id = 2 });
            smallbases.Add(temp);
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

        private bool String2int(string s, int num)
        {
            try
            {
                num = int.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async void showNumberErrorDialogAsync()
        {
            var errorDialog = new ContentDialog()
            {
                Content = "请输入纯数字",
                PrimaryButtonText = "确定",
                FullSizeDesired = false,
            };
            await errorDialog.ShowAsync();
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
            if (
                (Car_info.Visibility == Visibility.Visible && 
                (fid.Text == "" || fname.Text == "" || faddress.Text == "" ||
                cid.Text == "" || cbrand.Text == "" || cprice.Text == "")) ||
                (Custom_info.Visibility == Visibility.Visible && 
                (cuid.Text == "" || cuname.Text == "" || cuaddress.Text == "" )) ||
                (Garbage_info.Visibility == Visibility.Visible &&
                (qid.Text == "" || ctnum.Text == "" || cid2.Text == "" )) ||
                (in_info.Visibility == Visibility.Visible && 
                (ftid.Text == "" || ftprice.Text == "" || ftnum.Text == "" ||
                 fid2.Text == "" || cid3.Text == "" )) ||
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

            if (nowIndex == 0)
            {
                if (smallIndex == 0)
                {
                    Car car = new Car();
                    car.cid = cid.Text;
                    car.cbrand = cbrand.Text;
                    
                    if (!String2int(cprice.Text, car.cprice))
                    {
                        showNumberErrorDialogAsync();
                        return;
                    }
                    car.fid = fid.Text;

                    Factor factor = new Factor();
                    factor.fid = fid.Text;
                    factor.faddress = faddress.Text;
                    factor.fname = fname.Text;

                    String feedback = "";
                    String message1 = SqlHelper.AddFactor(factor, "factor");
                    String message2 = SqlHelper.AddCar(car, "car");

                    if (message1 != "not an error")
                    {
                        feedback += message1 + "\n";
                    }
                    if (message2 != "not an error")
                    {
                        feedback += message2 + "\n";
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
                else if (smallIndex == 1)
                {
                    Customer customer = new Customer();
                    customer.cuid = cuid.Text;
                    customer.cuname = cuname.Text;
                    customer.cuaddress = cuaddress.Text;
                    String feedback = SqlHelper.AddCustomer(customer, "customer");
                    Cleanhelp();
                    var dialog = new ContentDialog()
                    {
                        Content = feedback == "not an error" ? "创建成功" : feedback,
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
                else
                {
                    Garbage garbage = new Garbage();
                    garbage.gid = qid.Text;

                    if (!String2int(ctnum.Text, garbage.ctnum))
                    {
                        showNumberErrorDialogAsync();
                        return;
                    }

                    garbage.cid = cid2.Text;
                    String feedback = SqlHelper.AddGarbage(garbage, "garbage");
                    Cleanhelp();
                    var dialog = new ContentDialog()
                    {
                        Content = feedback == "not an error" ? "创建成功" : feedback,
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
                    

                    

                    Factor_trade_data factor_Trade_Data = new Factor_trade_data { ftid = ftid.Text, cid = cid3.Text, fid = fid2.Text};
                    if (!String2int(ftprice.Text, factor_Trade_Data.ftprice))
                    {
                        showNumberErrorDialogAsync();
                        return;
                    }
                    if (!String2int(ftnum.Text, factor_Trade_Data.ftnum))
                    {
                        showNumberErrorDialogAsync();
                        return;
                    }

                    String feedback = SqlHelper.AddFactor_trade_data(factor_Trade_Data, "Factor_trade_data");
                    Cleanhelp();
                    var dialog = new ContentDialog()
                    {
                        Content = feedback == "not an error" ? "创建成功" : feedback,
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
                else
                { 
                    Customer_trade_data customer_Trade_Data = new Customer_trade_data { ctid = ctid.Text, cid = cid4.Text, cuid = cuid2.Text};
                    if (!String2int(ctprice.Text, customer_Trade_Data.ctprice))
                    {
                        showNumberErrorDialogAsync();
                        return;
                    }
                    if (!String2int(ctnum2.Text, customer_Trade_Data.ctnum))
                    {
                        showNumberErrorDialogAsync();
                        return;
                    }
                    String feedback = SqlHelper.AddCustomer_trade_data(customer_Trade_Data, "customer_trade_data");
                    Cleanhelp();
                    var dialog = new ContentDialog()
                    {
                        Content = feedback == "not an error" ? "创建成功" : feedback,
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
            }
        }
    }
}
