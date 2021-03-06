﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
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
    /// <see cref="ITool"/>'s ImageTool.
    /// </summary>
    public partial class ImageTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        Layerage MezzanineLayerage = null;


        //@Content
        public ToolType Type => ToolType.Image;
        public FrameworkElement Icon { get; } = new ImageIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new ImageIcon()
        };
        public FrameworkElement Page => this.ImagePage;
        ImagePage ImagePage = new ImagePage();


        //@Construct
        /// <summary>
        /// Initializes a ImageTool. 
        /// </summary>
        public ImageTool()
        {
            this.ConstructStrings();
        }


        private float _sizeWidth;
        private float _sizeHeight;

        private Transformer CreateTransformer(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer canvasTransformer = Transformer.CreateWithAspectRatio(startingPoint, point, sizeWidth, sizeHeight);
            return canvasTransformer * inverseMatrix;
        }

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Photocopier photocopier = this.SelectionViewModel.Photocopier;
            if (photocopier.FolderRelativeId == null)
            {
                this.ImagePage.TipSelect();
                return;
            }

            Photo photo = Photo.FindFirstPhoto(photocopier);
            if (photo == null)
            {
                this.ImagePage.TipSelect();
                return;
            }

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Add layer", this.ViewModel.LayerageCollection);
            this.ViewModel.HistoryPush(history);

            //Transformer
            this._sizeWidth = photo.Width;
            this._sizeHeight = photo.Height;
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, photo.Width, photo.Height);

            //Mezzanine         
            ImageLayer imageLayer = new ImageLayer(this.ViewModel.CanvasDevice)
            {
                Photocopier = photocopier,
                IsSelected = true,
                Transform = new Transform(transformerSource, transformerDestination),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
            Layerage imageLayerage = imageLayer.ToLayerage();
            LayerBase.Instances.Add(imageLayer);


            this.MezzanineLayerage = imageLayerage;
            LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, this.MezzanineLayerage);

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //ILayer
            if (this.MezzanineLayerage == null) return;
            ILayer mezzanineLayer = this.MezzanineLayerage.Self;

            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this._sizeWidth, this._sizeHeight);
            mezzanineLayer.Transform.Transformer = transformerDestination;

            //Refactoring
            mezzanineLayer.IsRefactoringRender = true;
            mezzanineLayer.IsRefactoringIconRender = true;
            this.MezzanineLayerage.RefactoringParentsRender();
            this.MezzanineLayerage.RefactoringParentsIconRender();


            //Selection
            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            if (this.MezzanineLayerage == null) return;

            if (isOutNodeDistance)
            {
                if (this.MezzanineLayerage == null) return;
                ILayer mezzanineLayer = this.MezzanineLayerage.Self;


                Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this._sizeWidth, this._sizeHeight);

                this.SelectionViewModel.Transformer = transformerDestination;//Selection
                mezzanineLayer.Transform.Transformer = transformerDestination;

                //Refactoring
                mezzanineLayer.IsRefactoringRender = true;
                mezzanineLayer.IsRefactoringIconRender = true;
                this.MezzanineLayerage.RefactoringParentsRender();
                this.MezzanineLayerage.RefactoringParentsIconRender();


                foreach (Layerage layerage in this.ViewModel.LayerageCollection.RootLayerages)
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = false;
                }

                mezzanineLayer.IsSelected = true;
                this.MezzanineLayerage = null;
            }
            else LayerageCollection.RemoveMezzanine(this.ViewModel.LayerageCollection, this.MezzanineLayerage);//Mezzanine

            //         this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection

            LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;
                    drawingSession.DrawLayerBound(layer2, matrix, this.ViewModel.AccentColor);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                    }
                    break;
            }
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Image");
        }
        
    }


    /// <summary>
    /// Page of <see cref="ImageTool"/>.
    /// </summary>
    internal partial class ImagePage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        /// <summary> Tip. </summary>
        public void TipSelect() => this.EaseStoryboard.Begin();//Storyboard
        

        //@Construct
        /// <summary>
        /// Initializes a ImagePage. 
        /// </summary>
        public ImagePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSelect();
            this.ConstructReplace();
            this.ClearButton.Click += (s, e) => this.SelectionViewModel.Photocopier = new Photocopier();//Photocopier
        }

    }

    /// <summary>
    /// Page of <see cref="ImageTool"/>.
    /// </summary>
    internal partial class ImagePage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.SelectTextBlock.Text = resource.GetString("/Tools/Image_Select");
            this.ReplaceTextBlock.Text = resource.GetString("/Tools/Image_Replace");
            this.ClearTextBlock.Text = resource.GetString("/Tools/Image_Clear");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }

        private void ConstructSelect()
        {
            this.SelectButton.Click += (s, e) => Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.SelectImage);//Delegate
            Retouch_Photo2.PhotosPage.SelectImageCallBack += (photo) =>
            {
                if (photo == null) return;
                Photocopier photocopier = photo.ToPhotocopier();
                this.SelectionViewModel.Photocopier = photocopier;
            };
        }

        private void ConstructReplace()
        {
            this.ReplaceButton.Click += (s, e) => Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.ReplaceImage);//Delegate
            Retouch_Photo2.PhotosPage.ReplaceImageCallBack += (photo) =>
            {
                if (photo == null) return;
                Photocopier photocopier = photo.ToPhotocopier();
                this.SelectionViewModel.Photocopier = photocopier;

                this.MethodViewModel.TLayerChanged<Photocopier, ImageLayer>
                (
                    layerType: LayerType.Image,
                    set: (imageLayer) => imageLayer.Photocopier = photocopier,

                    historyTitle: "Set photocopier",
                    getHistory: (imageLayer) => imageLayer.Photocopier,
                    setHistory: (imageLayer, previous) => imageLayer.Photocopier = previous
                );
            };
        }

    }
}