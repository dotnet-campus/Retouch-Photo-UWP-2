﻿using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> Sets all IGeometryLayer. </summary>     
        private void SetIGeometryLayer(ILayer layer)
        {
            if (layer is GeometryRoundRectLayer roundRectLayer)
            {
                this.GeometryRoundRectCorner = roundRectLayer.Corner;
            }
            if (layer is GeometryTriangleLayer triangleLayer)
            {
                this.GeometryTriangleCenter = triangleLayer.Center;
            }
            if (layer is GeometryDiamondLayer diamondLayer)
            {
                this.GeometryDiamondMid = diamondLayer.Mid;
            }
            if (layer is GeometryPentagonLayer pentagonLayer)
            {
                this.GeometryPentagonPoints = pentagonLayer.Points;
            }
            if (layer is GeometryStarLayer starLayer)
            {
                this.GeometryStarPoints = starLayer.Points;
                this.GeometryStarInnerRadius = starLayer.InnerRadius;
            }
            if (layer is GeometryPieLayer pieLayer)
            {
                this.GeometryPieInnerRadius = pieLayer.InnerRadius;
                this.GeometryPieSweepAngle = pieLayer.SweepAngle;
            }
            if (layer is GeometryCogLayer cogLayer)
            {
                this.GeometryCogCount = cogLayer.Count;
                this.GeometryCogInnerRadius = cogLayer.InnerRadius;
                this.GeometryCogTooth = cogLayer.Tooth;
                this.GeometryCogNotch = cogLayer.Notch;
            }
            if (layer is GeometryArrowLayer arrowLayer)
            {
                this.GeometryArrowValue = arrowLayer.Value;
                this.GeometryArrowLeftTail = arrowLayer.LeftTail;
                this.GeometryArrowRightTail = arrowLayer.RightTail;
            }
            if (layer is GeometryHeartLayer heartLayer)
            {
                this.GeometryHeartSpread = heartLayer.Spread;
            }
        }


        /// <summary> GeometryRoundRectLayer's corner. </summary>     
        public float GeometryRoundRectCorner
        {
            get => this.geometryRoundRectCorner;
            set
            {
                if (this.geometryRoundRectCorner == value) return;
                this.geometryRoundRectCorner = value;
                this.OnPropertyChanged(nameof(this.GeometryRoundRectCorner));//Notify 
            }
        }
        private float geometryRoundRectCorner = 0.12f;
        

        /// <summary> GeometryTriangle's center-point. </summary>     
        public float GeometryTriangleCenter
        {
            get => this.geometryTriangleCenter;
            set
            {
                if (this.geometryTriangleCenter == value) return;
                this.geometryTriangleCenter = value;
                this.OnPropertyChanged(nameof(this.GeometryTriangleCenter));//Notify 
            }
        }
        private float geometryTriangleCenter = 0.5f;


        /// <summary> GeometryDiamond's mid-point. </summary>     
        public float GeometryDiamondMid
        {
            get => this.geometryDiamondMid;
            set
            {
                if (this.geometryDiamondMid == value) return;
                this.geometryDiamondMid = value;
                this.OnPropertyChanged(nameof(this.GeometryDiamondMid));//Notify 
            }
        }
        private float geometryDiamondMid = 0.5f;


        /// <summary> GeometryPentagon's points. </summary>     
        public int GeometryPentagonPoints
        {
            get => this.geometryPentagonPoints;
            set
            {
                if (this.geometryPentagonPoints == value) return;
                this.geometryPentagonPoints = value;
                this.OnPropertyChanged(nameof(this.GeometryPentagonPoints));//Notify 
            }
        }
        private int geometryPentagonPoints = 5;
        

        /// <summary> GeometryStar's points. </summary>     
        public int GeometryStarPoints
        {
            get => this.geometryStarPoints;
            set
            {
                if (this.geometryStarPoints == value) return;
                this.geometryStarPoints = value;
                this.OnPropertyChanged(nameof(this.GeometryStarPoints));//Notify 
            }
        }
        private int geometryStarPoints = 5;
        /// <summary> GeometryStar's inner-radius. </summary>     
        public float GeometryStarInnerRadius
        {
            get => this.geometryStarInnerRadius;
            set
            {
                if (this.geometryStarInnerRadius == value) return;
                this.geometryStarInnerRadius = value;
                this.OnPropertyChanged(nameof(this.GeometryStarInnerRadius));//Notify 
            }
        }
        private float geometryStarInnerRadius = 0.4f;
        

        /// <summary> GeometryPie's inner-radius. </summary>     
        public float GeometryPieInnerRadius
        {
            get => this.geometryPieInnerRadius;
            set
            {
                if (this.geometryPieInnerRadius == value) return;
                this.geometryPieInnerRadius = value;
                this.OnPropertyChanged(nameof(this.GeometryPieInnerRadius));//Notify 
            }
        }
        private float geometryPieInnerRadius = 0.0f;
        /// <summary> GeometryPie's sweep-angle. </summary>     
        public float GeometryPieSweepAngle
        {
            get => this.geometryPieSweepAngle;
            set
            {
                if (this.geometryPieSweepAngle == value) return;
                this.geometryPieSweepAngle = value;
                this.OnPropertyChanged(nameof(this.GeometryPieSweepAngle));//Notify 
            }
        }
        private float geometryPieSweepAngle = FanKit.Math.Pi / 2f;


        /// <summary> GeometryCog's count. </summary>     
        public int GeometryCogCount
        {
            get => this.geometryCogCount;
            set
            {
                if (this.geometryCogCount == value) return;
                this.geometryCogCount = value;
                this.OnPropertyChanged(nameof(this.GeometryCogCount));//Notify 
            }
        }
        private int geometryCogCount = 8;
        /// <summary> GeometryCog's inner-radius. </summary>     
        public float GeometryCogInnerRadius
        {
            get => this.geometryCogInnerRadius;
            set
            {
                if (this.geometryCogInnerRadius == value) return;
                this.geometryCogInnerRadius = value;
                this.OnPropertyChanged(nameof(this.GeometryCogInnerRadius));//Notify 
            }
        }
        private float geometryCogInnerRadius = 0.7f;
        /// <summary> GeometryCog's tooth. </summary>     
        public float GeometryCogTooth
        {
            get => this.geometryCogTooth;
            set
            {
                if (this.geometryCogTooth == value) return;
                this.geometryCogTooth = value;
                this.OnPropertyChanged(nameof(this.GeometryCogTooth));//Notify 
            }
        }
        private float geometryCogTooth = 0.3f;
        /// <summary> GeometryCog's notch. </summary>     
        public float GeometryCogNotch
        {
            get => this.geometryCogNotch;
            set
            {
                if (this.geometryCogNotch == value) return;
                this.geometryCogNotch = value;
                this.OnPropertyChanged(nameof(this.GeometryCogNotch));//Notify 
            }
        }
        private float geometryCogNotch = 0.6f;


        /// <summary> GeometryArrow's value. </summary>     
        public float GeometryArrowValue
        {
            get => this.geometryArrowValue;
            set
            {
                if (this.geometryArrowValue == value) return;
                this.geometryArrowValue = value;
                this.OnPropertyChanged(nameof(this.GeometryArrowValue));//Notify 
            }
        }
        private float geometryArrowValue = 0.5f;
        /// <summary> GeometryArrow's left-tail. </summary>     
        public GeometryArrowTailType GeometryArrowLeftTail
        {
            get => this.geometryArrowLeftTail;
            set
            {
                if (this.geometryArrowLeftTail == value) return;
                this.geometryArrowLeftTail = value;
                this.OnPropertyChanged(nameof(this.GeometryArrowLeftTail));//Notify 
            }
        }
        private GeometryArrowTailType geometryArrowLeftTail = GeometryArrowTailType.None;
        /// <summary> GeometryArrow's right-tail. </summary>     
        public GeometryArrowTailType GeometryArrowRightTail
        {
            get => this.geometryArrowRightTail;
            set
            {
                if (this.geometryArrowRightTail == value) return;
                this.geometryArrowRightTail = value;
                this.OnPropertyChanged(nameof(this.GeometryArrowRightTail));//Notify 
            }
        }
        private GeometryArrowTailType geometryArrowRightTail = GeometryArrowTailType.Arrow;


        /// <summary> GeometryArrow's spread. </summary>     
        public float GeometryHeartSpread
        {
            get => this.geometryHeartSpread;
            set
            {
                if (this.geometryHeartSpread == value) return;
                this.geometryHeartSpread = value;
                this.OnPropertyChanged(nameof(this.GeometryHeartSpread));//Notify 
            }
        }
        private float geometryHeartSpread = 0.8f;



    }
}