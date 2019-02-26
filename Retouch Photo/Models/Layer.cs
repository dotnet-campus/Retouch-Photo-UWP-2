﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.ComponentModel;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;
using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Layers;
using Windows.Foundation;
using Retouch_Photo.ViewModels;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Windows.Graphics.Effects;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Microsoft.Graphics.Canvas.UI;
using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.Models.Blends;
using Retouch_Photo.Library;
using static Retouch_Photo.Library.TransformController;

namespace Retouch_Photo.Models
{
    public abstract class Layer
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        public string Name= "Layer";
        
        public double Opacity = 100;

        public bool IsVisual= true;

        public int BlendIndex;
        
        public Transformer Transformer;

        public List<Adjustment> Adjustments = new List<Adjustment>();

        public EffectManager EffectManager = new EffectManager();


        #region Thumbnail


        //@override
        public abstract void ThumbnailDraw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Size controlSize);


        /// <summary> 画布管理 </summary>
        CanvasControlManger CanvasControl;
        /// <summary>初始化</summary>
        public void InitializeCanvasControl(CanvasControl sender) => this.CanvasControl = new CanvasControlManger(sender);


        public void Invalidate()
        {
            if (this.CanvasControl == null) return;

            this.CanvasControl.Invalidate();
        }
        

        public static Rect GetThumbnailSize(float width, float height, Size controlSize)
        {
            double widthScale = controlSize.Width / width;
            double heightScale = controlSize.Height / height;

            double scale = Math.Min(widthScale, heightScale);
            double w = width * scale;
            double h = height * scale;

            double x = (controlSize.Width - w) / 2;
            double y = (controlSize.Height - h) / 2;

            return new Rect(x, y, w, h);
        }
        public static Matrix3x2 GetThumbnailMatrix(float width, float height, Size controlSize)
        {
            double widthScale = controlSize.Width / width;
            double heightScale = controlSize.Height / height;

            double scale = Math.Min(widthScale, heightScale);
            double w = width * scale;
            double h = height * scale;

            double x = (controlSize.Width - w) / 2;
            double y = (controlSize.Height - h) / 2;

            return Matrix3x2.CreateScale((float)scale) *
              Matrix3x2.CreateTranslation((float)x, (float)y);
        }


        #endregion


        //Create
        public static Layer CreateFromXElement(ICanvasResourceCreator creator, XElement element)
        {
            int width = (int)element.Element("LayerWidth");
            int height = (int)element.Element("LayerHeight");

            string strings = element.Element("CanvasRenderTarget").Value;
            byte[] bytes = Convert.FromBase64String(strings);

            ImageLayer layer = ImageLayer.CreateFromBytes(creator, bytes, width, height);
            layer.Name = element.Element("LayerName").Value;
            layer.IsVisual = (bool)element.Element("LayerVisual");
            layer.Opacity = (double)element.Element("LayerOpacity");
            layer.BlendIndex =(int)element.Element("LayerBlendIndex");

            return layer;
        }


        //@override
        public virtual void ColorChanged(Color value) {}
        protected abstract ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix);
    
        //@static
        /// <summary> LayerRender </summary>
        /// <param name="layer">当前图层</param>
        /// <param name="image">从当前图层上面 传下来的 图像</param>
        /// <returns>新的 向下传递的 图像</returns>
        public static ICanvasImage LayerRender(ICanvasResourceCreator creator, Layer layer, ICanvasImage image,Matrix3x2 canvasToVirtualMatrix)
        {
            if (layer.IsVisual == false || layer.Opacity == 0) return image;

            ICanvasImage effect = layer.EffectManager.Render(Adjustment.Render
            (
                adjustments: layer.Adjustments,
                image: layer.GetRender(creator, image, canvasToVirtualMatrix)
            ));

            return Blend.Render
            (
               type: (BlendType)layer.BlendIndex,
               foreground: image,
               background: (layer.Opacity == 100) ? effect : new OpacityEffect
               {
                   Source = effect,
                   Opacity = (float)(layer.Opacity / 100)
               }
            );
        }
        
    }
}
