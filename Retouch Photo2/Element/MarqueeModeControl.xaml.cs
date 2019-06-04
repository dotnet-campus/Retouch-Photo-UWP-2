﻿using Retouch_Photo2.Library;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Element
{
    public sealed partial class MarqueeModeControl : UserControl
    {
        //Delegate
        public delegate void ModeChangedHandler(MarqueeMode mode);
        public event ModeChangedHandler ModeChanged = null;

        private MarqueeMode _Mode
        {
            set
            {
                this.SegmenteColor(this.NoneSegmented, (value == MarqueeMode.None));
                this.SegmenteColor(this.SquareSegmented, (value == MarqueeMode.Square));
                this.SegmenteColor(this.CenterSegmented, (value == MarqueeMode.Center));
                this.SegmenteColor(this.SquareAndCenterSegmented, (value == MarqueeMode.SquareAndCenter));

                this.ModeChanged?.Invoke(value);//Delegate
            }
        }

        #region DependencyProperty


        public MarqueeMode Mode
        {
            get { return (MarqueeMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeMode), typeof(MarqueeModeControl), new PropertyMetadata(MarqueeMode.None, (sender, e) =>
        {
            MarqueeModeControl con = (MarqueeModeControl)sender;

            if (e.NewValue is MarqueeMode value)
            {
                con._Mode = value;
            }
        }));



        #endregion

        public MarqueeModeControl()
        {            
            this.InitializeComponent();

            this.NoneSegmented.Tapped += (sender, e) => this.Mode = MarqueeMode.None;
            this.SquareSegmented.Tapped += (sender, e) => this.Mode = MarqueeMode.Square;
            this.CenterSegmented.Tapped += (sender, e) => this.Mode = MarqueeMode.Center;
            this.SquareAndCenterSegmented.Tapped += (sender, e) => this._Mode = MarqueeMode.SquareAndCenter;
        }

        private void SegmenteColor(ContentPresenter control, bool IsChecked)
        {
            control.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            control.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
        }
    }
}