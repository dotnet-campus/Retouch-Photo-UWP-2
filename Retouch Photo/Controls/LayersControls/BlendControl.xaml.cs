﻿using Retouch_Photo.Blends;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Controls.LayersControls
{
    public sealed partial class BlendControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        #region DependencyProperty

        public int BlendIndex
        {
            get { return (int)GetValue(BlendIndexProperty); }
            set { SetValue(BlendIndexProperty, value); }
        }
        public static readonly DependencyProperty BlendIndexProperty = DependencyProperty.Register(nameof(BlendIndex), typeof(Layer), typeof(BlendControl), new PropertyMetadata(0, (sender, e) =>
        {
            BlendControl con = (BlendControl)sender;

            if (e.NewValue is int value)
            {
                if (value < 0) return;
                if (value >= con.ComboBox.Items.Count) return;

                if (con.ComboBox.SelectedIndex == value) return;

                con.ComboBox.SelectedIndex = value;
            }
        }));

        #endregion


        //Delegate
        public delegate void IndexChangedHandler(int index);
        public event IndexChangedHandler IndexChanged = null;


        public BlendControl()
        {
            this.InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.ComboBox.ItemsSource = Blend.BlendList;

            if (this.ComboBox.SelectedIndex < 0) this.ComboBox.SelectedIndex = 0;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.ComboBox.SelectedIndex;
            if (this.BlendIndex == index) return;
            this.BlendIndex = index;
            this.IndexChanged?.Invoke(index);
        }

    }
}