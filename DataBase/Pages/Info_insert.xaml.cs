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
            smallbases.Add(temp);
            SmallTags.ItemsSource = smallbases[nowIndex];

            help();
            Car_info.Visibility = Visibility.Visible;

        }

        private void help()
        {
            Car_info.Visibility = Visibility.Collapsed;
            Custom_info.Visibility = Visibility.Collapsed;
            Garage_info.Visibility = Visibility.Collapsed;
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
                    Garage_info.Visibility = Visibility.Visible;
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

        private bool String2int(string s,ref int num)
        {
            try
            {   
                num = int.Parse(s);
                Debug.Write(s + "\neee" + num);
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
                (Garage_info.Visibility == Visibility.Visible &&
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
                    int x = 0;
                    if (!String2int(cprice.Text, ref x))
                    {
                        showNumberErrorDialogAsync();
                        return;
                    }
                    car.cprice = x;
                    car.fid = fid.Text;
                    Factory factory = new Factory();
                    factory.fid = fid.Text;
                    factory.faddress = faddress.Text;
                    factory.fname = fname.Text;

                    String feedback = "";
                    SqlHelper.AddFactory(factory, "factory");
                    String message2 = SqlHelper.AddCar(car, "car");

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
            }
        }
    }
}
