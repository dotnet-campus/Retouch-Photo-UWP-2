﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingPage" />. 
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TitleTextBlock.Text = resource.GetString("/$SettingPage/Title");

            this.ThemeTextBlock.Text = resource.GetString("/$SettingPage/Theme");
            this.LightRadioButton.Content = resource.GetString("/$SettingPage/Theme_Light");
            this.DarkRadioButton.Content = resource.GetString("/$SettingPage/Theme_Dark");
            this.DefaultRadioButton.Content = resource.GetString("/$SettingPage/Theme_UseSystem");

            this.DeviceLayoutTextBlock.Text = resource.GetString("/$SettingPage/DeviceLayout");
            this.PhoneButton.Content = resource.GetString("/$SettingPage/DeviceLayout_Phone");
            this.PadButton.Content = resource.GetString("/$SettingPage/DeviceLayout_Pad");
            this.PCButton.Content = resource.GetString("/$SettingPage/DeviceLayout_PC");
            this.AdaptiveButton.Content = resource.GetString("/$SettingPage/DeviceLayout_Adaptive");

            this.AdaptiveTextBlock.Text = resource.GetString("/$SettingPage/DeviceLayout_AdaptiveWidth");
            this.AdaptiveResetButton.Content = resource.GetString("/$SettingPage/DeviceLayout_ResetAdaptiveWidth");

            this.KeyTextBlock.Text = resource.GetString("/$SettingPage/Key");
            this.IsCenterToggleButton.Content = resource.GetString("/$SettingPage/Key_IsCenter");
            this.IsRatioToggleButton.Content = resource.GetString("/$SettingPage/Key_IsRatio");
            this.IsSquareToggleButton.Content = resource.GetString("/$SettingPage/Key_IsSquare");
            this.IsStepFrequencyToggleButton.Content = resource.GetString("/$SettingPage/Key_IsStepFrequency");
            this.FullScreenToggleButton.Content = resource.GetString("/$SettingPage/Key_FullScreen");

            this.MenuTypeTextBlock.Text = resource.GetString("/$SettingPage/MenuType");
            this.MenuTypeTipTextBlock.Text = resource.GetString("/$SettingPage/MenuTypeTip");

            this.LocalTextBlock.Text = resource.GetString("/$SettingPage/Local");
            this.LocalOpenTextBlock.Text = resource.GetString("/$SettingPage/Local_Open");
        }



        //Theme
        private void ConstructTheme()
        {
            ElementTheme theme = this.SettingViewModel.Setting.Theme;
            this.LightRadioButton.IsChecked = (theme == ElementTheme.Light);
            this.DarkRadioButton.IsChecked = (theme == ElementTheme.Dark);
            this.DefaultRadioButton.IsChecked = (theme == ElementTheme.Default);
            
            this.LightRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Light);
            this.DarkRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Dark);
            this.DefaultRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Default);
        }



        //DeviceLayout
        private void ConstructDeviceLayout()
        {
            DeviceLayout deviceLayout = this.SettingViewModel.Setting.DeviceLayout;
            this.ConstructDeviceLayoutType(deviceLayout.FallBackType, deviceLayout.IsAdaptive);
            this.ConstructDeviceLayoutAdaptive(deviceLayout.PhoneMaxWidth, deviceLayout.PadMaxWidth);
        }


        private void ConstructDeviceLayoutType(DeviceLayoutType type, bool isAdaptive)
        {
            this.DeviceLayoutType = type;
            this.IsAdaptive = isAdaptive;
            this.PhoneButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Phone);
            this.PadButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Pad);
            this.PCButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.PC);
            this.AdaptiveButton.IsChecked = (isAdaptive);
                       
            this.PhoneButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.Phone, false);
            this.PadButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.Pad, false);
            this.PCButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.PC, false);
            this.AdaptiveButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.PC, true);
        }


        private void ConstructDeviceLayoutAdaptive(int phone, int pad)
        {
            this.AdaptiveGrid.PhoneWidth = phone;
            this.AdaptiveGrid.PadWidth = pad;
            this.AdaptiveGrid.SetWidth();


            this.AdaptiveGrid.ScrollModeChanged += (s, mode) =>
            {
                this.ScrollViewer.HorizontalScrollMode = mode;
                this.ScrollViewer.VerticalScrollMode = mode;
            };
            this.AdaptiveGrid.PhoneWidthChanged += async (s, value) =>
            {
                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PhoneMaxWidth = value;
                await this.Write();
            };
            this.AdaptiveGrid.PadWidthChanged += async (s, value) =>
            {
                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PadMaxWidth = value;
                await this.Write();
            };


            this.AdaptiveResetButton.Click += async (s, e) =>
            {
                DeviceLayout default2 = DeviceLayout.Default;
                int phone2 = default2.PhoneMaxWidth;
                int pad2 = default2.PadMaxWidth;

                this.AdaptiveGrid.PhoneWidth = phone2;
                this.AdaptiveGrid.PadWidth = pad2;
                this.AdaptiveGrid.SetWidth();

                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PhoneMaxWidth = phone2;
                this.SettingViewModel.Setting.DeviceLayout.PadMaxWidth = pad2;
                await this.Write();//Write
            };
        }



        //MenuType
        private void ConstructMenuType()
        {
            bool isParity = false;
            IList<MenuType> menuTypes = this.SettingViewModel.Setting.MenuTypes;

            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                bool isVisible = menuTypes.Any(m => m == menu.Type);

                CheckBox checkBox = new CheckBox
                {
                    Content = menu.Expander.Title,
                    IsChecked = isVisible,
                };
                checkBox.Checked += async (s, e) => await this.AddMenu(menu.Type);
                checkBox.Unchecked += async (s, e) => await this.RemoveMenu(menu.Type);

                isParity = !isParity;
                this.MenusStackPanel.Children.Add(new Border
                {
                    Child = checkBox,
                    Style = this.MenuBorderStyle,
                });
            }
        }



    }
}