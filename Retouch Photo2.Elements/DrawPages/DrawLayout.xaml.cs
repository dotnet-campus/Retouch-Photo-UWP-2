﻿using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// <see cref = "DrawPage" />'s layout. 
    /// </summary>
    public sealed partial class DrawLayout : UserControl
    {
        //@Content
        #region Content

        /// <summary> BackButton. </summary>
        public Button BackButton => this._BackButton;
        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterChild { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        /// <summary> RightBorder's Child. </summary>
        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> LeftBorder's Child. </summary>
        public UIElementCollection LeftPaneChildren => this.LeftStackPanel.Children;

        /// <summary> IconLeftContentControl's Content. </summary>
        public object LeftIcon { get => this.IconLeftContentControl.Content; set => this.IconLeftContentControl.Content = value; }
        /// <summary> IconRightContentControl's Content. </summary>
        public object RightIcon { get => this.IconRightContentControl.Content; set => this.IconRightContentControl.Content = value; }

        /// <summary> TouchbarBorder's Child. </summary>
        public UIElement Touchbar { get => this.TouchbarBorder.Child; set => this.TouchbarBorder.Child = value; }
        /// <summary> Gets or sets FootScrollViewer's content. </summary>
        public Page FootPage
        {
            get => this.footPage;
            set
            {
                //If you choose a different tool, PhoneState will hided.
                Page oldPage = this.footPage;

                if (value != oldPage)
                {
                    if (this._vsPhoneType != PhoneLayoutType.Hided)
                    {
                        this._vsPhoneType = PhoneLayoutType.Hided;
                        this.VisualState = this.VisualState;//State
                    }
                }

                this.FootScrollViewer.Content = value;
                this.footPage=value;
            }
        }
        private Page footPage;
        
        #endregion 


        #region HeadLeft & HeadRight


        /// <summary> HeadLeftBorder's Child. </summary>
        public UIElement HeadLeftPane { get => this.HeadLeftBorder.Child; set => this.HeadLeftBorder.Child = value; }


        /// <summary> HeadRightStackPane's Children. </summary>
        public UIElementCollection HeadRightChildren => this.HeadRightStackPane.Children;

        StackPanel HeadRightStackPane = new StackPanel { Orientation = Orientation.Horizontal };

        bool _IsHeadRightExpand;

        bool isHeadRightExpand;
        bool IsHeadRightExpand
        {
            get => this.isHeadRightExpand;
            set
            {
                if (this.isHeadRightExpand == value) return;

                if (value)
                {
                    this.HeadRightScrollViewer.Content = null;
                    this.HeadRightExpandBorder.Child = this.HeadRightStackPane;
                }
                else
                {
                    this.HeadRightExpandBorder.Child = null;
                    this.HeadRightScrollViewer.Content = this.HeadRightStackPane;
                }
                this.isHeadRightExpand = value;
            }
        }


        #endregion


        #region DependencyProperty


        /// <summary> Backgroud's Color. </summary>
        public ElementTheme Theme
        {
            set
            {
                switch (value)
                {
                    case ElementTheme.Light:
                        this.LightStoryboard.Begin();//Storyboard
                        break;
                    case ElementTheme.Dark:
                        this.DarkStoryboard.Begin();//Storyboard
                        break;
                }
            }
        }


        /// <summary> Sets or Gets the page layout is full screen. </summary>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DrawLayout.IsFullScreen" /> dependency property. </summary>
        public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(nameof(IsFullScreen), typeof(bool), typeof(DrawLayout), new PropertyMetadata(false, (sender, e) =>
        {
            DrawLayout con = (DrawLayout)sender;

            if (e.NewValue is bool value)
            {
                con._vsIsFullScreen = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@VisualState
        bool _vsIsFullScreen;
        double _vsScreenWidth;
        PhoneLayoutType _vsPhoneType= PhoneLayoutType.Hided;

        public DeviceLayoutType VisualStateDeviceType = DeviceLayoutType.Adaptive;
        public double VisualStatePhoneMaxWidth = 600.0;
        public double VisualStatePadMaxWidth = 900.0;

        public VisualState VisualState
        {
            get
            {
                this._IsHeadRightExpand = true;

                if (this._vsIsFullScreen) return this.FullScreen;

                switch (this.VisualStateDeviceType)
                {
                    case DeviceLayoutType.PC: return this.PC;
                    case DeviceLayoutType.Pad: return this.Pad;
                    case DeviceLayoutType.Phone:  break;
                    case DeviceLayoutType.Adaptive:
                        {
                            double width = this._vsScreenWidth;
                            if (width > this.VisualStatePadMaxWidth) return this.PC;
                            if (width > this.VisualStatePhoneMaxWidth) return this.Pad;
                        }
                        break;
                }

                this._IsHeadRightExpand = false;
                switch (this._vsPhoneType)
                {
                    case PhoneLayoutType.Hided: return this.Phone;
                    case PhoneLayoutType.ShowLeft: return this.PhoneShowLeft;
                    case PhoneLayoutType.ShowRight: return this.PhoneShowRight;
                }

                return this.Normal;
            }
            set
            {
                this.IsHeadRightExpand = this._IsHeadRightExpand;
                VisualStateManager.GoToState(this, value.Name, false);
            }
        }


        //@Construct
        public DrawLayout()
        {
            this.InitializeComponent();

            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this._vsScreenWidth = e.NewSize.Width;
                this.VisualState = this.VisualState;//State
            };

            //HeadRight
            this.HeadRightToggleButton.Checked += (s, e) => this.HeadRightScrollViewer.Visibility = Visibility.Visible;
            this.HeadRightToggleButton.Unchecked += (s, e) => this.HeadRightScrollViewer.Visibility = Visibility.Collapsed;

            //DismissOverlay
            this.IconDismissOverlay.Tapped += (s, e) =>
            {
                this._vsPhoneType = PhoneLayoutType.Hided;
                this.VisualState = this.VisualState;//State
            };

            //IconLeft
            this.IconLeftGrid.Tapped += (s, e) =>
            {
                this._vsPhoneType = PhoneLayoutType.ShowLeft;
                this.VisualState = this.VisualState;//State
            };
            this.IconLeftGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this._vsPhoneType = PhoneLayoutType.ShowLeft;
                    this.VisualState = this.VisualState;//State
                }
            };
            //IconRight
            this.IconRightGrid.Tapped += (s, e) =>
            {
                this._vsPhoneType = PhoneLayoutType.ShowRight;
                this.VisualState = this.VisualState;//State
            };
            this.IconRightGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this._vsPhoneType = PhoneLayoutType.ShowRight;
                    this.VisualState = this.VisualState;//State
                }
            };
        }
    }     
}