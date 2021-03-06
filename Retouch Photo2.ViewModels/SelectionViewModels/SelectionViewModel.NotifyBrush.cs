﻿using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Styles;
using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {
        
        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                if (this.fillOrStroke == value) return;
                this.fillOrStroke = value;
                this.OnPropertyChanged(nameof(this.FillOrStroke));//Notify 
            }
        }
        private FillOrStroke fillOrStroke = FillOrStroke.Fill;
                        

        /// <summary> Gets or sets the current color. </summary>
        public Color Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.OnPropertyChanged(nameof(this.Color));//Notify 
            }
        }
        private Color color = Colors.LightGray;


        //////////////////////////


        /// <summary> Gets or sets the style.IsFollowTransform. </summary>
        public bool IsFollowTransform
        {
            get => this.isFollowTransform;
            set
            {
                if (this.isFollowTransform == value) return;
                this.isFollowTransform = value;
                this.OnPropertyChanged(nameof(this.IsFollowTransform));//Notify 
            }
        }
        private bool isFollowTransform = true;

        /// <summary> Gets or sets whether the stroke is behind the fill.. </summary>
        public bool IsStrokeBehindFill
        {
            get => this.isStrokeBehindFill;
            set
            {
                if (this.isStrokeBehindFill == value) return;
                this.isStrokeBehindFill = value;
                this.OnPropertyChanged(nameof(this.IsStrokeBehindFill));//Notify 
            }
        }
        private bool isStrokeBehindFill = false;

        /// <summary> Gets or sets the current fill. </summary>
        public IBrush Fill
        {
            get => this.fill;
            set
            {
                this.fill = value;
                this.OnPropertyChanged(nameof(this.Fill));//Notify 
            }
        }
        private IBrush fill = BrushBase.ColorBrush(Colors.LightGray);

        /// <summary> Gets or sets the current stroke. </summary>
        public IBrush Stroke
        {
            get => this.stroke;
            set
            {
                this.stroke = value;
                this.OnPropertyChanged(nameof(this.Stroke));//Notify 
            }
        }
        private IBrush stroke = new BrushBase();
        
        /// <summary> Gets or sets the style.IsStrokeWidthFollowScale. </summary>
        public bool IsStrokeWidthFollowScale
        {
            get => this.isStrokeWidthFollowScale;
            set
            {
                if (this.isStrokeWidthFollowScale == value) return;
                this.isStrokeWidthFollowScale = value;
                this.OnPropertyChanged(nameof(this.IsStrokeWidthFollowScale));//Notify 
            }
        }
        private bool isStrokeWidthFollowScale = false;
                
        /// <summary> Gets or sets the current stroke-width. </summary>
        public float StrokeWidth
        {
            get => this.strokeWidth;
            set
            {
                this.strokeWidth = value;
                this.OnPropertyChanged(nameof(this.StrokeWidth));//Notify 
            }
        }
        private float strokeWidth = 1;


        //////////////////////////


        /// <summary> Gets the current stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle { get; private set; } = new CanvasStrokeStyle();


        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashStyle"/>. </summary>
        public CanvasDashStyle StrokeStyleDash
        {
            get => this.strokeStyleDash;
            set
            {
                this.strokeStyleDash = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleDash));//Notify 
                this.StrokeStyle.DashStyle = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasDashStyle strokeStyleDash = CanvasDashStyle.Solid;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashCap"/>. </summary>
        public CanvasCapStyle StrokeStyleCap
        {
            get => this.strokeStyleCap;
            set
            {
                this.strokeStyleCap = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleCap));//Notify 
                this.StrokeStyle.DashCap = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasCapStyle strokeStyleCap = CanvasCapStyle.Flat;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.LineJoin"/>. </summary>
        public CanvasLineJoin StrokeStyleJoin
        {
            get => this.strokeStyleJoin;
            set
            {
                this.strokeStyleJoin = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleJoin));//Notify 
                this.StrokeStyle.LineJoin = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasLineJoin strokeStyleJoin = CanvasLineJoin.Miter;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashOffset"/>. </summary>
        public float StrokeStyleOffset
        {
            get => this.strokeStyleOffset;
            set
            {
                this.strokeStyleOffset = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleOffset));//Notify 
                this.StrokeStyle.DashOffset = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private float strokeStyleOffset = 0.0f;


        //////////////////////////


        /// <summary> Gets or sets the current transparency. </summary>
        public IBrush Transparency
        {
            get => this.transparency;
            set
            {
                this.transparency = value;
                this.OnPropertyChanged(nameof(this.Transparency));//Notify 
            }
        }
        private IBrush transparency = new BrushBase();


        //////////////////////////


        /// <summary> Sets the style. </summary>  
        public void SetStyle(IStyle style)
        {
            if (style == null) return;


            this.IsFollowTransform = style.IsFollowTransform;

            this.IsStrokeBehindFill = style.IsStrokeBehindFill;
            
            this.Fill = style.Fill;
            this.Stroke = style.Stroke;

            this.IsStrokeWidthFollowScale = style.IsStrokeWidthFollowScale;

            this.StrokeWidth = style.StrokeWidth;

            this.StrokeStyle = style.StrokeStyle;

            this.StrokeStyleDash = style.StrokeStyle.DashStyle;
            this.StrokeStyleCap = style.StrokeStyle.DashCap;
            this.strokeStyleJoin = style.StrokeStyle.LineJoin;
            this.StrokeStyleOffset = style.StrokeStyle.DashOffset;

            this.Transparency = style.Transparency;


            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    if (style.Fill.Type == BrushType.Color)
                        this.Color = style.Fill.Color;
                    break;
                case FillOrStroke.Stroke:
                    if (style.Stroke.Type == BrushType.Color)
                        this.Color = style.Stroke.Color;
                    break;
            }
        }

    }
}