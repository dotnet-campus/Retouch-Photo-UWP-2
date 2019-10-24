﻿using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "KeyboardViewModel.IsSquare" /> and <see cref = "KeyboardViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreCreateButton : UserControl
    {
        //@Static
        public static Flyout Flyout;

        //@Construct
        public MoreCreateButton()
        {
            this.InitializeComponent();

            this.Button.Tapped += (s, e) =>
            {
                if (MoreCreateButton.Flyout == null) return;

                if (this.Parent is FrameworkElement placementTarget)
                {
                    MoreCreateButton.Flyout.ShowAt(placementTarget);
                }
                else
                {
                    MoreCreateButton.Flyout.ShowAt(this);
                }
            };
        }
    }
}