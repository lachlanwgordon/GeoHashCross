using SkiaSharp;

namespace GeohashCross.Views
{
    public partial class HomePage
    {
        public static class Paint
        {


            public static SKPaint RedPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Red
            };


            public static SKPaint BluePaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Blue
            };


            public static SKPath NeedlyPath = SKPath.ParseSvgPathData("M 0 -80 C 0 -30 20 -30 5 -20 L 10 10 C 5 7.5 -5 7.5 -10 10 L -5 -20 C -20 -30 0 -30 0 -80");

            public static SKPaint WhiteFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White,
            };

            public static SKPaint GreyFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.DimGray,
            };

            public static SKPaint WhiteStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };
        }


    }
}