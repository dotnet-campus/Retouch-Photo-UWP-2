﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Effects
{
    /// <summary> 
    /// <see cref = "IEffectPage" />'s manager. 
    /// </summary>
    public class Effect
    {
        //GaussianBlur
        public bool GaussianBlur_IsOn;
        public float GaussianBlur_Radius = 0;

        //DirectionalBlur
        public bool DirectionalBlur_IsOn;
        public float DirectionalBlur_Radius = 0;
        public float DirectionalBlur_Angle = 0;

        //Sharpen
        public bool Sharpen_IsOn;
        public float Sharpen_Amount = 0;

        //OuterShadow
        public bool OuterShadow_IsOn;
        public float OuterShadow_Radius = 0;
        public float OuterShadow_Opacity = 0.5f;
        public Color OuterShadow_Color = Colors.Black;

        private float OuterShadow_offset = 0;
        public float OuterShadow_Offset
        {
            get => this.OuterShadow_offset;
            set
            {
                this.OuterShadow_Position = new Vector2((float)Math.Cos(this.OuterShadow_Angle), (float)Math.Sin(this.OuterShadow_Angle)) * value;
                this.OuterShadow_offset = value;
            }
        }
        private float OuterShadow_angle = 0.78539816339744830961566084581988f;// 1/4 π
        public float OuterShadow_Angle
        {
            get => this.OuterShadow_angle;
            set
            {
                this.OuterShadow_Position = new Vector2((float)Math.Cos(value), (float)Math.Sin(value)) * this.OuterShadow_Offset;
                this.OuterShadow_angle = value;
            }
        }
        Vector2 OuterShadow_Position = Vector2.Zero;

        //Outline
        public bool Outline_IsOn;
        private int Outline_size = 1;
        public int Outline_Size
        {
            get => this.Outline_size;
            set
            {
                this.Outline_Mode = (value > 0) ? MorphologyEffectMode.Dilate : MorphologyEffectMode.Erode;

                int s = Math.Abs(value);
                this.Outline_Height = this.Outline_Width = s > 90 ? 90 : s;

                this.Outline_size = value;
            }
        }

        public MorphologyEffectMode Outline_Mode { get; private set; }
        public int Outline_Width { get; private set; } = 1;
        public int Outline_Height { get; private set; } = 1;

        //Emboss
        public bool Emboss_IsOn;
        public float Emboss_Radius = 1;
        public float Emboss_Angle = 0;

        //Straighten
        public bool Straighten_IsOn;
        public float Straighten_Angle = 0;

        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned effect. </returns>
        public Effect Clone()
        {
            return new Effect
            {
                //GaussianBlur
                GaussianBlur_IsOn = this.GaussianBlur_IsOn,
                GaussianBlur_Radius = this.GaussianBlur_Radius,

                //DirectionalBlur
                DirectionalBlur_IsOn = this.DirectionalBlur_IsOn,
                DirectionalBlur_Radius = this.DirectionalBlur_Radius,
                DirectionalBlur_Angle = this.DirectionalBlur_Angle,

                //Sharpen
                Sharpen_IsOn = this.Sharpen_IsOn,
                Sharpen_Amount = this.Sharpen_Amount,

                //OuterShadow
                OuterShadow_IsOn = this.OuterShadow_IsOn,
                OuterShadow_Radius = this.OuterShadow_Radius,
                OuterShadow_Opacity = this.OuterShadow_Opacity,
                OuterShadow_Color = this.OuterShadow_Color,

                OuterShadow_Offset = this.OuterShadow_Offset,
                OuterShadow_Angle = this.OuterShadow_Angle,
                OuterShadow_Position = this.OuterShadow_Position,

                //Outline
                Outline_IsOn = this.Outline_IsOn,
                Outline_Size = this.Outline_Size,

                //Emboss
                Emboss_IsOn = this.Emboss_IsOn,
                Emboss_Radius = this.Emboss_Radius,
                Emboss_Angle = this.Emboss_Angle,

                //Straighten
                Straighten_IsOn = this.Straighten_IsOn,
                Straighten_Angle = this.Straighten_Angle,
            };
        }

        //@Static
        public static ICanvasImage Render(Effect effect, ICanvasImage image)
        {
            //GaussianBlur
            if (effect.GaussianBlur_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect
                {
                    Source = image,
                    BlurAmount = effect.GaussianBlur_Radius
                };
            }

            //DirectionalBlur
            if (effect.DirectionalBlur_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.DirectionalBlurEffect
                {
                    Source = image,
                    BlurAmount = effect.DirectionalBlur_Radius,
                    Angle = -effect.DirectionalBlur_Angle,
                };
            }

            //Sharpen
            if (effect.Sharpen_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.SharpenEffect
                {
                    Source = image,
                    Amount = effect.Sharpen_Amount,
                };
            }

            //OuterShadow
            if (effect.OuterShadow_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.CompositeEffect
                {
                    Sources =
                {
                     new Microsoft.Graphics.Canvas.Effects.Transform2DEffect
                     {
                          TransformMatrix = Matrix3x2.CreateTranslation(effect.OuterShadow_Position),
                          Source = new Microsoft.Graphics.Canvas.Effects.OpacityEffect
                          {
                              Opacity =effect.OuterShadow_Opacity,
                              Source=new Microsoft.Graphics.Canvas.Effects.ShadowEffect
                              {
                                   Source = image,
                                   BlurAmount = effect.OuterShadow_Radius,
                                   ShadowColor = effect.OuterShadow_Color,
                              }
                          }
                     },
                    image
                }
                };
            }

            //Outline
            if (effect.Outline_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.MorphologyEffect
                {
                    Source = image,
                    Mode = effect.Outline_Mode,
                    Width = effect.Outline_Width,
                    Height = effect.Outline_Height,
                };
            }

            //Emboss
            if (effect.Emboss_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.EmbossEffect
                {
                    Source = image,
                    Amount = effect.Emboss_Radius,
                    Angle = -effect.Emboss_Angle,
                };
            }

            //Straighten
            if (effect.Straighten_IsOn)
            {
                image = new StraightenEffect
                {
                    Source = image,
                    MaintainSize = true,
                    Angle = effect.Straighten_Angle,
                };
            }

            return image;
        }
    }
}