﻿using Windows.System;
using Windows.UI.Xaml;
using System.ComponentModel;
using Retouch_Photo2.Elements;
using System;
using System.Numerics;
using FanKit.Transformers;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingViewModel" />. 
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {
        //@Delegate  
        /// <summary> Occurs when the canvas position moved. </summary>
        public Action<Vector2> Move { get; set; }


        //@Construct
        public void ConstructKey()
        {
            Window.Current.CoreWindow.KeyUp += (s, e) =>
            {
                VirtualKey key = e.VirtualKey;
                this.KeyUp(key);
                this.KeyUpAndDown(key);
            };
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                VirtualKey key = e.VirtualKey;
                this.KeyDown(key);
                this.KeyUpAndDown(key);
            };
        }

        private void KeyDown(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Shift:
                    this.SetKeyShift(true);
                    break;
                case VirtualKey.Control:
                    this.SetKeyCtrl(true);
                    break;
                case VirtualKey.Space:
                    this.SetKeyAlt(true);
                    break;

                case VirtualKey.Delete:
                    break;

                case VirtualKey.Escape:
                    this.IsFullScreen = !this.IsFullScreen;
                    break;

                case VirtualKey.Left:
                    this.Move?.Invoke(new Vector2(50, 0));//Delegate
                    break;
                case VirtualKey.Up:
                    this.Move?.Invoke(new Vector2(0, 50));//Delegate
                    break;
                case VirtualKey.Right:
                    this.Move?.Invoke(new Vector2(-50, 0));//Delegate
                    break;
                case VirtualKey.Down:
                    this.Move?.Invoke(new Vector2(0, -50));//Delegate
                    break;


                default:
                    break;
            }
        }

        private void KeyUp(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Shift:
                    this.SetKeyShift(false);
                    break;
                case VirtualKey.Control:
                    this.SetKeyCtrl(false);
                    break;
                case VirtualKey.Space:
                    this.SetKeyAlt(false);
                    break;

                default:
                    break;
            }
        }

        private void KeyUpAndDown(VirtualKey key)
        {
            if (this.KeyCtrl == false && this.KeyShift == false)
                this.CompositeMode = MarqueeCompositeMode.New;//CompositeMode
            else if (this.KeyCtrl == false && this.KeyShift)
                this.CompositeMode = MarqueeCompositeMode.Add;//CompositeMode
            else if (this.KeyCtrl && this.KeyShift == false)
                this.CompositeMode = MarqueeCompositeMode.Subtract;//CompositeMode
            else //if (this.KeyCtrl && this.KeyShift)       
                this.CompositeMode = MarqueeCompositeMode.Intersect;//CompositeMode
        }



        /// <summary> keyboard's the **SHIFT** key. </summary>
        public bool KeyShift;
        private void SetKeyShift(bool value)
        {
            this.IsRatio = value;
            this.IsSquare = value;

            this.KeyShift = value;
            this.OnPropertyChanged(nameof(this.KeyShift));//Notify 
        }


        /// <summary> keyboard's the **CTRL** key. </summary>
        public bool KeyCtrl;
        private void SetKeyCtrl(bool value)
        {
            this.IsCenter = value;

            this.KeyCtrl = value;
            this.OnPropertyChanged(nameof(this.KeyCtrl));//Notify 
        }




        /// <summary> keyboard's the **ALT** key. </summary>
        public bool KeyAlt;
        private void SetKeyAlt(bool value)
        {
            this.IsStepFrequency = value;

            this.KeyAlt = value;
            this.OnPropertyChanged(nameof(this.KeyAlt));//Notify 
        }
                    
    }
}