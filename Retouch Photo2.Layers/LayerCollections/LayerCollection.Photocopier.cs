﻿using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Gets all photocopiers, which in ( layer and children )'s style manager.
        /// </summary>
        /// <returns> The yield photocopiers. </returns>
        public IEnumerable<Photocopier> GetPhotocopiers() => this._getPhotocopiers(this.RootLayers);
        private IEnumerable<Photocopier> _getPhotocopiers(IEnumerable<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                foreach (Photocopier photocopier in this._getPhotocopiers(child.Children))
                {
                    yield return photocopier;
                }

                //ImageLayer
                if (child.Type== LayerType.Image)
                {
                    ImageLayer imageLayer = (ImageLayer)child;
                    yield return imageLayer.Photocopier;
                }

                //Fill
                if (child.Style.Fill.Type == BrushType.Image)
                {
                    yield return child.Style.Fill.Photocopier;
                }
                //Stroke
                if (child.Style.Stroke.Type == BrushType.Image)
                {
                    yield return child.Style.Stroke.Photocopier;
                }
            }
        }

    }
}