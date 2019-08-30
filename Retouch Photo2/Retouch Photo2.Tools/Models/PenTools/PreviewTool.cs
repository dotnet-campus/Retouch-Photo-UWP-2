﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// <see cref="PenTool"/>'s PenPreviewNodeTool.
    /// </summary>
    public class PreviewTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        NodeCollection NodeCollection => this.SelectionViewModel.CurveLayer.NodeCollection;

        Vector2 _left;
        Vector2 _right;

        /// <summary> Only the left point. </summary>
        bool _hasTempLeftPoint;

        public void Start(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this._hasTempLeftPoint == false) this._left = canvasPoint;
            this._right = canvasPoint;

            this.ViewModel.Invalidate();
        }
        public void Delta(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            this._right = canvasPoint;

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this._hasTempLeftPoint)
            {
                this._hasTempLeftPoint = false;
                this.CreateLayer(this._left, canvasPoint);
            }
            else if (isSingleStarted)
            {
                this._hasTempLeftPoint = false;
                this.CreateLayer(canvasStartingPoint, canvasPoint);
            }
            else
            {
                this._hasTempLeftPoint = true;
            }
        }

        /// <summary>
        /// Draw a line before creating a curve layer.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2 lineLeft = Vector2.Transform(this._left, matrix);

            if (this._hasTempLeftPoint)
            {
                drawingSession.DrawNode3(lineLeft);
            }
            else
            {
                Vector2 lineRight = Vector2.Transform(this._right, matrix);

                drawingSession.DrawLineDodgerBlue(lineLeft, lineRight);
                drawingSession.DrawNode3(lineLeft);
                drawingSession.DrawNode3(lineRight);
            }
        }

        private void CreateLayer(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Transformer
            Transformer transformer = new Transformer(canvasPoint, canvasStartingPoint);

            //Layer
            CurveLayer curveLayer = new CurveLayer(canvasStartingPoint, canvasPoint)
            {
                IsChecked = true,
                StrokeWidth = 1,
                StrokeBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = this.SelectionViewModel.FillColor,
                },

                Source = transformer,
                Destination = transformer,
            };

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.IsChecked = false;
            });

            //Insert
            int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
            this.ViewModel.Layers.Insert(index, curveLayer);

            this.SelectionViewModel.SetModeSingle(curveLayer);//Selection
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}