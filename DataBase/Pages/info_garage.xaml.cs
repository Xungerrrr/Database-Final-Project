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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DataBase.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class info_garage : Page
    {
        private ObservableCollection<Garbage> garbages = new ObservableCollection<Garbage>();
        public info_garage()
        {
            this.InitializeComponent();
            SqlHelper.GetAllGarbage(garbages, "garbage");
        }

        private void ItemDeletion(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            SqlHelper.DeleteGarbage("garbage", (Garbage)s.DataContext);
            SqlHelper.GetAllGarbage(garbages, "garbage");
        }

        private void ItemModification(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            info_update.type = 5;//type of garage is 5
            info_update.id = ((Garbage)s.DataContext).gid;
            this.Frame.Navigate(typeof(info_update));
        }
    }
}
