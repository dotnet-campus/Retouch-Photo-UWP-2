﻿using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public class GeometryTriangleTool : IGeometryTool
    {
        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryTriangleLayer();
        }
        public override ToolType Type => ToolType.GeometryTriangle;
        public override FrameworkElement Icon { get; } = new GeometryTriangleIcon();
        public override IToolButton Button { get; } = new GeometryTriangleButton();
        public override Page Page { get; } = new GeometryTrianglePage();
    }
}