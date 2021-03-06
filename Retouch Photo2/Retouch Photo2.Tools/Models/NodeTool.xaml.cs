﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s NodeTool.
    /// </summary>
    public partial class NodeTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode SelectionMode => this.SelectionViewModel.SelectionMode;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Content
        public ToolType Type => ToolType.Node;
        public FrameworkElement Icon { get; } = new NodeIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new NodeIcon()
        };
        public FrameworkElement Page => this.NodePage;
        NodePage NodePage = new NodePage();
        

        //@Construct
        /// <summary>
        /// Initializes a NodeTool. 
        /// </summary>
        public NodeTool()
        {
            this.ConstructStrings();
        }
        

        Layerage Layerage;
        NodeCollectionMode NodeCollectionMode;
        TransformerRect TransformerRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            this.Layerage = this.GetNodeCollectionLayer(startingPoint, matrix);
            if (this.Layerage == null)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.NodeCollectionMode = NodeCollectionMode.RectChoose;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                return;
            }

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.Move:
                    this.MoveStarted();
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.MoveSingleNodePointStarted(startingPoint, matrix);
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this.MoveSingleNodeControlPointStarted();
                    break;
                case NodeCollectionMode.RectChoose:
                    this.RectChooseStarted(startingPoint, point);
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.Layerage == null)
            {
                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.ViewModel.Invalidate();//Invalidate
                return;
            }

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.Move:
                    this.MoveDelta(canvasStartingPoint, canvasPoint);
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.MoveSingleNodePointDelta(canvasPoint);
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    this.MoveSingleNodeControlPointDelta(canvasPoint, isLeftControlPoint: true);
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this.MoveSingleNodeControlPointDelta(canvasPoint, isLeftControlPoint: false);
                    break;
                case NodeCollectionMode.RectChoose:
                    this.RectChooseDelta(canvasStartingPoint, canvasPoint);
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.Layerage == null)
            {
                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.NodeCollectionMode = NodeCollectionMode.None;
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                return;
            }

            if (isOutNodeDistance)
            {
                switch (this.NodeCollectionMode)
                {
                    case NodeCollectionMode.Move:
                        this.MoveComplete(canvasStartingPoint, canvasPoint);
                        break;
                    case NodeCollectionMode.MoveSingleNodePoint:
                        this.MoveSingleNodePointComplete(canvasPoint);
                        break;
                    case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                        this.MoveSingleNodeControlPointComplete(canvasPoint, isLeftControlPoint: true);
                        break;
                    case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                        this.MoveSingleNodeControlPointComplete(canvasPoint, isLeftControlPoint: false);
                        break;
                    case NodeCollectionMode.RectChoose:
                        this.RectChooseComplete(canvasStartingPoint, canvasPoint);
                        break;
                }
            }

            this.NodeCollectionMode = NodeCollectionMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set nodes is checked");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.Curve)
                {
                    layer.Nodes.CacheTransform();
                    layer.Nodes.SelectionOnlyOne(point, matrix);

                    //History
                    var previous = layer.Nodes.NodesStartingClone().ToList();
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.Nodes.NodesReplace(previous);
                    };
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        
        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None: break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;

                    if (layer2.Type == LayerType.Curve)
                    {
                        drawingSession.DrawLayerBound(layer2, matrix, this.ViewModel.AccentColor);
                        drawingSession.DrawNodeCollection(layer2.Nodes, matrix, this.ViewModel.AccentColor);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                            drawingSession.DrawNodeCollection(layer.Nodes, matrix, this.ViewModel.AccentColor);
                        }
                    }
                    break;
            }

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.Move:
                case NodeCollectionMode.MoveSingleNodePoint:
                    {
                        //Snapping
                        if (this.IsSnap)
                        {
                            this.Snap.Draw(drawingSession, matrix);
                            this.Snap.DrawNode2(drawingSession, matrix);
                        }
                    }
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        CanvasGeometry canvasGeometry = this.TransformerRect.ToRectangle(this.ViewModel.CanvasDevice);
                        CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                        drawingSession.DrawGeometryDodgerBlue(canvasGeometryTransform);
                    }
                    break;
            }
        }

        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Node");
        }

    }


    /// <summary>
    /// Page of <see cref="NodeTool"/>.
    /// </summary>
    internal partial class NodePage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        /// <summary> PenPage's Flyout. </summary>
        public PenModeControl PenFlyout => this._PenFlyout;


        //@Construct
        /// <summary>
        /// Initializes a NodePage. 
        /// </summary>
        public NodePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructNodes();
            this.ConstructSmooth();
            this.MoreButton.Click += (s, e) => this.Flyout.ShowAt(this);
        }

    }

    /// <summary>
    /// Page of <see cref="NodeTool"/>.
    /// </summary>
    internal partial class NodePage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RemoveTextBlock.Text = resource.GetString("/Tools/Node_Remove");
            this.InsertTextBlock.Text = resource.GetString("/Tools/Node_Insert");
            this.SharpTextBlock.Text = resource.GetString("/Tools/Node_Sharp");
            this.SmoothTextBlock.Text = resource.GetString("/Tools/Node_Smooth");
        }

        private void ConstructNodes()
        {
            this.RemoveButton.Click += (s, e) =>
            {
                IList<Layerage> removeLayerage = new List<Layerage>();


                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Remove nodes");

                    //Selection
                    this.SelectionViewModel.SetValue((layerage) =>
                    {
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            NodeBorderCollection nodeBorderCollection = new NodeBorderCollection(layer.Nodes);
                            NodeRemoveMode removeMode = nodeBorderCollection.GetRemoveMode();

                            switch (removeMode)
                            {
                                case NodeRemoveMode.RemoveCurve:
                                    {
                                        removeLayerage.Add(layerage);
                                    }
                                    break;

                                case NodeRemoveMode.RemovedNodes:
                                    {
                                        var previous = layer.Nodes.NodesClone().ToList();
                                        history.UndoAction += () =>
                                        {
                                            //Refactoring
                                            layer.IsRefactoringTransformer = true;
                                            layer.IsRefactoringRender = true;
                                            layer.IsRefactoringIconRender = true;
                                            layerage.RefactoringParentsTransformer();
                                            layerage.RefactoringParentsRender();
                                            layerage.RefactoringParentsIconRender();
                                            layer.Nodes.NodesReplace(previous);
                                        };

                                        //Refactoring
                                        layer.IsRefactoringTransformer = true;
                                        layer.IsRefactoringRender = true;
                                        layer.IsRefactoringIconRender = true;
                                        layerage.RefactoringParentsTransformer();
                                        layerage.RefactoringParentsRender();
                                        layerage.RefactoringParentsIconRender();
                                        IEnumerable<Node> uncheckedNodes = nodeBorderCollection.GetUnCheckedNodes();
                                        layer.Nodes.NodesReplace(uncheckedNodes);
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    });

                    //History
                    this.ViewModel.HistoryPush(history);
                }


                //Remove
                if (removeLayerage.Count != 0)
                {
                    //History
                    LayeragesArrangeHistory history = new LayeragesArrangeHistory("Remove layers", this.ViewModel.LayerageCollection);
                    this.ViewModel.HistoryPush(history);

                    foreach (Layerage remove in removeLayerage)
                    {
                        LayerageCollection.Remove(this.ViewModel.LayerageCollection, remove);
                    }

                    //Selection
                    this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                    LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                }

                this.ViewModel.Invalidate();//Invalidate
            };


            this.InsertButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Insert nodes");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.Curve)
                    {
                        var previous = layer.Nodes.NodesStartingClone().ToList();
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            layer.IsRefactoringTransformer = true;
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layerage.RefactoringParentsTransformer();
                            layerage.RefactoringParentsRender();
                            layerage.RefactoringParentsIconRender();
                            layer.Nodes.NodesReplace(previous);
                        };

                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        NodeCollection.InterpolationCheckedNodes(layer.Nodes);
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        private void ConstructSmooth()
        {
            this.SharpButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Sharp nodes");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.Curve)
                    {
                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        layer.Nodes.CacheTransformOnlySelected();
                        bool isSuccessful = NodeCollection.SharpCheckedNodes(layer.Nodes);

                        if (isSuccessful)
                        {
                            //History
                            var previous = layer.Nodes.NodesStartingClone().ToList();
                            history.UndoAction += () =>
                            {
                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                layer.Nodes.NodesReplace(previous);
                            };
                        }
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };


            this.SmoothButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Smooth nodes");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.Curve)
                    {
                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        layer.Nodes.CacheTransformOnlySelected();
                        bool isSuccessful = NodeCollection.SmoothCheckedNodes(layer.Nodes);

                        if (isSuccessful)
                        {
                            //History
                            var previous = layer.Nodes.NodesStartingClone().ToList();
                            history.UndoAction += () =>
                            {
                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                layer.Nodes.NodesReplace(previous);
                            };
                        }
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}