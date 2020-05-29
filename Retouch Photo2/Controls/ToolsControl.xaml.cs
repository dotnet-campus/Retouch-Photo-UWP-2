﻿using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ToolsControl" />. 
    /// </summary>
    public sealed partial class ToolsControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel = App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        /// <summary> Left panel of Tool. </summary>
        UIElementCollection ToolLeft => this.StackPanel.Children;
        /// <summary> Left more button's flyout panel's children. </summary>
        UIElementCollection ToolLeftMore = null;


        //@Construct
        public ToolsControl()
        {
            this.InitializeComponent();

            //Tool
            foreach (ITool tool in this.TipViewModel.Tools)
            {
                this.ConstructTool(tool);
            }
            this.ToolFirst();
        }


        //Tool
        private void ConstructTool(ITool tool)
        {
            if (tool == null)
            {
                Rectangle rectangle = new Rectangle
                {
                    Style = this.SeparatorRectangle
                };

                if (this.ToolLeftMore == null)
                    this.ToolLeft.Add(rectangle);
                else
                    this.ToolLeftMore.Add(rectangle);

                return;
            }
            else if (tool.Type == ToolType.None)
            {
                return;
            }
            else if (tool.Type == ToolType.More)
            {
                if (tool.Button is ToolMoreButton moreButton)
                {
                    this.ToolLeft.Add(moreButton);
                    this.ToolLeftMore = moreButton.Children;
                }
                return;
            }
            else
            {
                if (tool.Button is FrameworkElement element)
                {
                    if (this.ToolLeftMore == null)
                        this.ToolLeft.Add(element);
                    else
                        this.ToolLeftMore.Add(element);

                    element.Tapped += (s, e) =>
                    {
                        //Change tools group value.
                        {
                            this.TipViewModel.Tool = tool;
                            this.TipViewModel.ToolGroupType(tool.Type);
                            this.SelectionViewModel.ToolType = tool.Type;

                            this.ViewModel.TipTextBegin(tool.Title);
                            this.ViewModel.Invalidate();//Invalidate
                        }
                    };
                }
            }
        }

        #region Tool


        /// <summary>
        /// Select the first Tool by default. 
        /// </summary>
        private void ToolFirst()
        {
            ITool tool = this.TipViewModel.Tools.FirstOrDefault();
            if (tool != null)
            {
                this.TipViewModel.Tool = tool;
                this.TipViewModel.ToolGroupType(tool.Type);
            }
        }


        #endregion

    }
}
