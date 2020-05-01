﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TextArtisticTool.
    /// </summary>
    public partial class TextArtisticTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public TextArtisticTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

    }

    /// <summary>
    /// <see cref="ITool"/>'s TextArtisticTool.
    /// </summary>
    public partial class TextArtisticTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/TextArtistic");
        }


        //@Content
        public ToolType Type => ToolType.TextArtistic;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new TextArtisticIcon();
        readonly ToolButton _button = new ToolButton(new TextArtisticIcon());
        
        private ILayer CreateLayer(Transformer transformer)
        {
            return new TextArtisticLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformer),
                StyleManager = new StyleManager
                {
                    FillBrush = new ColorBrush
                    {
                        Color = Colors.Black,
                    }
                }
            };
        }


        public void Starting(Vector2 point) => this.TipViewModel.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isSingleStarted);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}