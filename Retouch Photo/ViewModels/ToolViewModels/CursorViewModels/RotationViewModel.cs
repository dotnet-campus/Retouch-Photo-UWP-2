﻿using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels
{
    public class RotationViewModel : IToolViewModel
    {    
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        bool IsStepFrequency
        {
            get => this.ViewModel.KeyShift;
            set => this.ViewModel.KeyShift=value;
        }

        Vector2 Center;
        
        float StartTransformerRadian;
        float StartRadian;

        float Radian;

        public void Start(Vector2 point, Layer layer)
        {
            Matrix3x2 matrix = layer.Transformer.Matrix * this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;

            this.Center = layer.Transformer.TransformCenter(matrix);

            this.StartTransformerRadian = layer.Transformer.Radian;
            this.StartRadian = Transformer.VectorToRadians(point - this.Center);
        }
        public void Delta(Vector2 point, Layer layer)
        {
            this.Radian = Transformer.VectorToRadians(point - this.Center);

            float radian = this.StartTransformerRadian - this.StartRadian + this.Radian;

            layer.Transformer.Radian = this.IsStepFrequency ? Transformer.RadiansStepFrequency(radian) : radian;
        }
        public void Complete(Vector2 point, Layer layer)
        {
            this.IsStepFrequency = false;
        }

        public void Draw(CanvasDrawingSession ds, Layer layer)
        {
            Transformer.DrawBoundNodesWithRotation(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);

            ds.DrawLine(this.Center, Transformer.RadiansToVector(this.StartRadian, this.Center), Colors.Gray);
            ds.DrawLine(this.Center, Transformer.RadiansToVector(this.Radian, this.Center), Colors.Gray);
        }
    }
}
