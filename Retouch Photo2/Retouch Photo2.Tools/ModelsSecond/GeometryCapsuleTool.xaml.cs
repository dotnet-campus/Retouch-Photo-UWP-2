﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryCapsuleTool.
    /// </summary>
    public sealed partial class GeometryCapsuleTool : Page, ITool
    { 
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public GeometryCapsuleTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryCapsuleTool.
    /// </summary>
    public partial class GeometryCapsuleTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/ToolsSecond/GeometryCapsule");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryCapsule;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryCapsuleIcon();
        readonly ComboBoxButton _button = new ComboBoxButton(new GeometryCapsuleIcon());

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCapsuleLayer
            {
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManager()
            };
        }


        public void Starting(Vector2 point) => this.TipViewModel.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isSingleStarted);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);


    }
}