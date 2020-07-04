﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
    internal enum GeometryStarMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Points. </summary>
        Points,

        /// <summary> Inner-radius. </summary>
        InnerRadius,
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryStarTool.
    /// </summary>
    public partial class GeometryStarTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar  
        private GeometryStarMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case GeometryStarMode.None:
                        this.PointsTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarPicker = null;
                        this.TipViewModel.TouchbarSlider = null;
                        break;
                    case GeometryStarMode.Points:
                        this.PointsTouchbarButton.IsSelected = true;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarPicker = this.PointsTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.PointsTouchbarSlider;
                        break;
                    case GeometryStarMode.InnerRadius:
                        this.PointsTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarPicker = this.InnerRadiusTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.InnerRadiusTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private double PointsValueConverter(float points) => points;

        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double InnerRadiusValueConverter(float innerRadius) => innerRadius * 100d;


        //@Construct
        /// <summary>
        /// Initializes a GeometryStarTool. 
        /// </summary>
        public GeometryStarTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructPoints1();
            this.ConstructPoints2();
            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = GeometryStarMode.None;
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryStarTool.
    /// </summary>
    public sealed partial class GeometryStarTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryStar");
            this._button.Style = this.IconSelectedButtonStyle;

            this.PointsTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryStar_Points");
            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryStar_InnerRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryStar;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryStarIcon();
        readonly Button _button = new Button { Tag = new GeometryStarIcon()};

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryStarLayer(customDevice)
            {
                Points = this.SelectionViewModel.GeometryStarPoints,
                InnerRadius = this.SelectionViewModel.GeometryStarInnerRadius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryStarTool.
    /// </summary>
    public sealed partial class GeometryStarTool : Page, ITool
    {

        //Points
        private void ConstructPoints1()
        {
            //Button
            this.PointsTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryStarMode.Points;
                else
                    this.TouchBarMode = GeometryStarMode.None;
            };

            this.PointsTouchbarPicker.Minimum = 3;
            this.PointsTouchbarPicker.Maximum = 36;
            this.PointsTouchbarPicker.ValueChange += (sender, value) =>
            {
                int points = (int)value;

                this.MethodViewModel.TLayerChanged<int, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryStarPoints = points,
                    set: (tLayer) => tLayer.Points = points,

                    historyTitle: "Set star layer points",
                    getHistory: (tLayer) => tLayer.Points = points,
                    setHistory: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

        private void ConstructPoints2()
        { 
            this.PointsTouchbarSlider.Minimum = 3.0d;
            this.PointsTouchbarSlider.Maximum = 36.0d;
            this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) =>  this.MethodViewModel.TLayerChangeStarted<GeometryStarLayer>
            (
                layerType: LayerType.GeometryStar,
                cache: (tLayer) => tLayer.CachePoints()
            );
            this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) =>this.MethodViewModel.TLayerChangeDelta<GeometryStarLayer>
            (
                layerType: LayerType.GeometryStar,
                set: (tLayer) => tLayer.Points = (int)value
            );
            this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryStarPoints = points,
                    set: (tLayer) => tLayer.Points = points,

                    historyTitle: "Set star layer points",
                    getHistory: (tLayer) => tLayer.StartingPoints,
                    setHistory: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }


        //InnerRadius
        private void ConstructInnerRadius1()
        {
            //Button
            this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryStarMode.InnerRadius;
                else
                    this.TouchBarMode = GeometryStarMode.None;
            };

            this.InnerRadiusTouchbarPicker.Unit = "%";
            this.InnerRadiusTouchbarPicker.Minimum = 0;
            this.InnerRadiusTouchbarPicker.Maximum = 100;
            this.InnerRadiusTouchbarPicker.ValueChange += (sender, value) =>
            {
                float innerRadius = (float)value / 100.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryStarInnerRadius = innerRadius,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    historyTitle: "Set star layer inner radius",
                    getHistory: (tLayer) => tLayer.InnerRadius,
                    setHistory: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }

        private void ConstructInnerRadius2()
        {
            this.InnerRadiusTouchbarSlider.Minimum = 0.0d;
            this.InnerRadiusTouchbarSlider.Maximum = 1.0d;
            this.InnerRadiusTouchbarSlider.ValueChangeStarted += (sender, value) =>  this.MethodViewModel.TLayerChangeStarted<GeometryStarLayer>
            (
                layerType: LayerType.GeometryStar,
                cache: (tLayer) => tLayer.CacheInnerRadius()
            );
            this.InnerRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>this.MethodViewModel.TLayerChangeDelta<GeometryStarLayer>
            (
                layerType: LayerType.GeometryStar,
                set: (tLayer) => tLayer.InnerRadius = (float)value
            );
            this.InnerRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryStarInnerRadius = innerRadius,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    historyTitle: "Set star layer inner radius",
                    getHistory: (tLayer) => tLayer.StartingInnerRadius,
                    setHistory: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }

    }
}