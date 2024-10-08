using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Owlcat.Runtime.Visual.RenderPipeline.PostProcess
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Color Curves")]
    public sealed class ColorCurves : VolumeComponent, IPostProcessComponent
    {
        public TextureCurveParameter master = new TextureCurveParameter(new TextureCurve(new Keyframe[2]
        {
          new Keyframe(0.0f, 0.0f, 1f, 1f),
          new Keyframe(1f, 1f, 1f, 1f)
        }, 0.0f, false, new Vector2(0.0f, 1f)));
        public TextureCurveParameter red = new TextureCurveParameter(new TextureCurve(new Keyframe[2]
        {
          new Keyframe(0.0f, 0.0f, 1f, 1f),
          new Keyframe(1f, 1f, 1f, 1f)
        }, 0.0f, false, new Vector2(0.0f, 1f)));
        public TextureCurveParameter green = new TextureCurveParameter(new TextureCurve(new Keyframe[2]
        {
          new Keyframe(0.0f, 0.0f, 1f, 1f),
          new Keyframe(1f, 1f, 1f, 1f)
        }, 0.0f, false, new Vector2(0.0f, 1f)));
        public TextureCurveParameter blue = new TextureCurveParameter(new TextureCurve(new Keyframe[2]
        {
          new Keyframe(0.0f, 0.0f, 1f, 1f),
          new Keyframe(1f, 1f, 1f, 1f)
        }, 0.0f, false, new Vector2(0.0f, 1f)));
        public TextureCurveParameter hueVsHue = new TextureCurveParameter(new TextureCurve(new Keyframe[0], 0.5f, true, new Vector2(0.0f, 1f)));
        public TextureCurveParameter hueVsSat = new TextureCurveParameter(new TextureCurve(new Keyframe[0], 0.5f, true, new Vector2(0.0f, 1f)));
        public TextureCurveParameter satVsSat = new TextureCurveParameter(new TextureCurve(new Keyframe[0], 0.5f, false, new Vector2(0.0f, 1f)));
        public TextureCurveParameter lumVsSat = new TextureCurveParameter(new TextureCurve(new Keyframe[0], 0.5f, false, new Vector2(0.0f, 1f)));

        public bool IsActive()
        {
            return true;
        }

        public bool IsTileCompatible()
        {
            return true;
        }
    }
}
