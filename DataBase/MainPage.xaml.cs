using DataBase.Model;
using DataBase.Service;
using DataBase.ViewModels;
using System;
using System.Collections.Generic;
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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace DataBase
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MenuItemManager MenuItemManager = new MenuItemManager();

        public MainPage()
        {
            this.InitializeComponent();
            MenuItemManager.GetMenuItem();
            mainListView.ItemsSource = MenuItemManager.menuItems;
            SqlHelper.PrepareConnection();
            myFrame.Navigate(MenuItemManager.menuItems[0].content);
        }

        private void hanburgButton_Click(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }

        private void TextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
            //myFrame.Navigate();
        }

        private void mainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (mainSplitView.IsPaneOpen != false)
            {
                mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
            }

            MenuItem temp = (MenuItem)e.ClickedItem;
            myFrame.Navigate(temp.content);
        }
    }
}
