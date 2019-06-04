﻿using Retouch_Photo2.Layers;
using Retouch_Photo2.TestApp.Tools;
using Retouch_Photo2.TestApp.Tools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {

        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.TestApp.Tools.Tool" />. </summary>
        public Tool Tool
        {
            get => this.tool;
            set
            {
                //The current tool becomes the active tool.
                Tool oldTool = this.tool;
                oldTool.ToolOnNavigatedFrom();

                //The current page does not become an active page.
                Tool newTool = value;
                newTool.ToolOnNavigatedTo();

                this.tool = value;
                this.OnPropertyChanged(nameof(this.Tool));//Notify 
            }
        }
        private Tool tool = new NoneTool();


        /// <summary> <see cref="Tool"/>'s ViewTool. </summary>
        public ViewTool ViewTool { private set; get; } = new ViewTool();
        /// <summary> <see cref="Tool"/>'s RectangleTool. </summary>
        public RectangleTool RectangleTool { private set; get; } = new RectangleTool();

    }
}