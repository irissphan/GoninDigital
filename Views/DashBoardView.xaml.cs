﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GoninDigital.Models;
using GoninDigital.Views.DashBoardPages;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using System.Linq;
using Frame = System.Windows.Controls.Frame;
using Page = System.Windows.Controls.Page;

namespace GoninDigital.Views
{
    struct SearchItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
    public partial class DashBoard : UserControl
    {
        private static Frame rootFrame;
        public static Frame RootFrame
        {
            get => rootFrame;
        }

        Dictionary<string, Page> pages;

        public DashBoard()
        {
            InitializeComponent();
            rootFrame = contentFrame;
            pages = new Dictionary<string, Page>();
        }

        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(SettingPage));
                return;
            }
            var selectedItem = (ModernWpf.Controls.NavigationViewItem)args.SelectedItem;
            if (selectedItem != null)
            {
                string selectedItemTag = (string)selectedItem.Tag;
                string pageName = "GoninDigital.Views.DashBoardPages." + selectedItemTag;
                Page togo;
                if (!pages.TryGetValue(pageName, out togo))
                {
                    Type pageType = typeof(HomePage).Assembly.GetType(pageName);
                    togo = (Page)Activator.CreateInstance(pageType);
                    pages.Add(pageName, togo);
                }
                contentFrame.Navigate(togo);
            }
            else
            {
                contentFrame.Navigate(typeof(HomePage));
            }
        }

        private void navigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            contentFrame.GoBack();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (contentFrame.CanGoBack)
                navigationView.IsBackEnabled = true;
            else
                navigationView.IsBackEnabled = false;

            var desType = contentFrame.SourcePageType;
            if (desType == typeof(HomePage))
            {
                homeItem.IsSelected = true;
            }
            else if (desType == typeof(CartPage))
            {
                cartItem.IsSelected = true;
            }
            else if (desType == typeof(SettingPage))
            {
                ((NavigationViewItem)navigationView.SettingsItem).IsSelected = true;
            }
            else if (desType == typeof(HotDealPage))
            {
                hotDealItem.IsSelected = true;
            }
            else if (desType == typeof(MyShopPage))
            {
                myShopItem.IsSelected = true;
            }
            
        }

        private void NavigationViewItem_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var content = sender.Text;
                using (var context = new GoninDigitalDBContext())
                {
                    var result = context.Products.Where(product => product.Name.Contains(content))
                        .Select(product => new SearchItem{ Name=product.Name, Description=product.Description, Image=product.Image })
                        .ToList();
                    var result2 = context.Vendors.Where(vendor => vendor.Name.Contains(content))
                        .Select(vendor => new SearchItem { Name = vendor.Name, Description = vendor.Description, Image = vendor.Avatar })
                        .ToList();


                    var combined = result2.Concat(result);
                    if (combined.Any())
                        sender.ItemsSource = combined;
                    else
                        sender.ItemsSource = new List<SearchItem>() 
                        { new SearchItem { Name = "No Results Found", Image = null, Description = null } };
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                MessageBox.Show((string)args.ChosenSuggestion);
            }
            else
            {
                // Use args.QueryText to determine what to do.
                MessageBox.Show((string)args.QueryText);
            }
        }
    }
}
