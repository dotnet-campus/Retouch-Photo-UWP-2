﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties.
    /// </summary>
    public interface ILayer : ICacheTransform
    {

        #region Property


        /// <summary> Gets ILayer's type name. </summary>
        string Type { get; }
        /// <summary> Gets or sets ILayer's name. </summary>
        string Name { get; set; }
        /// <summary> Gets or sets ILayer's opacity. </summary>
        float Opacity { get; set; }
        /// <summary> Gets or sets ILayer's blend type. </summary>
        BlendType BlendType { get; set; }
        /// <summary> Gets or sets ILayer's visibility. </summary>
        Visibility Visibility { get; set; }

        /// <summary> Gets or sets ILayer is need to refactoring transformer. </summary>
        bool IsRefactoringTransformer { get; set; }
        /// <summary> Gets ILayer's actually transformer. </summary>
        Transformer GetActualDestinationWithRefactoringTransformer { get; }

        /// <summary> Gets or sets ILayer's transformer. </summary>
        TransformManager TransformManager { get; set; }
        /// <summary> Gets or sets ILayer's effect manager. </summary>
        EffectManager EffectManager { get; set; }
        /// <summary> Gets or sets ILayer's adjustment manager. </summary>
        AdjustmentManager AdjustmentManager { get; set; }
        

        /// <summary> Gets or sets ILayer's fill-color. </summary>
        Color? FillColor { get; set; }
        /// <summary> Gets or sets ILayer's stroke-color. </summary>
        Color? StrokeColor { get; set; }


        /// <summary>
        /// Get ILayer own copy.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The cloned ILayer. </returns>
        ILayer Clone(ICanvasResourceCreator resourceCreator);


        #endregion


        #region Children


        /// <summary> Gets or sets ILayer's children layers. </summary>
        IList<ILayer> Children { get; set; }
        /// <summary> Gets or sets ILayer's parent layer. </summary>
        ILayer Parents { get; set; }


        /// <summary>
        /// Add a layer into children.
        /// </summary>
        /// <param name="layer"> The source layer. </param>
        void Add(ILayer layer);
        /// <summary>
        /// Add some layers into children.
        /// </summary>
        /// <param name="layers"> The source layers. </param>
        void AddRange(IList<ILayer> layers);

        /// <summary>
        /// As a child, to disengage with your parents.
        /// </summary>
        /// <param name="layerCollection"> The layer collection. </param>
        void Disengage(LayerCollection layerCollection);


        #endregion


        #region Control


        /// <summary> Gets or sets ILayer's control. </summary>
        ILayerControl Control { get; }


        /// <summary> Gets or sets ILayer's overlay-mode. </summary>
        OverlayMode OverlayMode { get; set; }

        /// <summary> Gets or sets ILayer's expand-mode. </summary>
        ExpandMode ExpandMode { get; set; }
        /// <summary> Changed the expand-mode. </summary>
        void Expaned();

        /// <summary> Gets or sets ILayer's selected-mode. </summary>
        SelectMode SelectMode { get; set; }
        /// <summary> Changed the select-mode. </summary>
        void Selected();
        

        #endregion
               

        #region Render


        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-to-virtual matrix. </param>
        /// <returns> The rendered layer. </returns>
        ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix);
   
        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor);


        #endregion
        
    }
}