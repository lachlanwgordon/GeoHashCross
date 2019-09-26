using System;
using System.Diagnostics;
using GeohashCross.ViewModels;
using SkiaSharp;
using Xamarin.Forms;
using static GeohashCross.Views.HomePage;

namespace GeohashCross.Views
{
    public class Compass : SkiaSharp.Views.Forms.SKCanvasView
    {
        public Compass()
        {
            PaintSurface += Handle_PaintSurface;
            Device.StartTimer(TimeSpan.FromSeconds(1f / 30), UpdateCanvas);
        }

        public static readonly BindableProperty NeedleDirectionProperty = BindableProperty.Create(
                                                                  nameof(NeedleDirection), //Public name to use
                                                                  typeof(float), //this type
                                                                  typeof(Compass), //parent type (tihs control)
                                                                  0f); //default value
        public float NeedleDirection
        {
            get { return (float)GetValue(NeedleDirectionProperty); }
            set { SetValue(NeedleDirectionProperty, value); }
        }


        void Handle_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            if (!IsVisible)
                return;

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            var height = e.Info.Height;
            var width = e.Info.Width;

            //Setup canvas with transforms based at the center
            canvas.Translate(width / 2, height / 2);

            var smallerSide = Math.Min(height, width);
            canvas.Scale(smallerSide / 220);//Using smaller side makes sure it's a circle that fits without cropping regardless of orientation. I don't remember what 220 means

            canvas.Save();

            //Markers around circle
            for (int angle = 0; angle < 360; angle += 15)
            {
                canvas.DrawCircle(0, -90, angle % 90 == 0 ? 5 : 2, Paint.WhiteFill);
                canvas.RotateDegrees(15);
            }

            canvas.Restore();


            //Target Needle
            canvas.Save();
            
            canvas.RotateDegrees(HomePageViewModel.Instance.TargetNeedleDirection);
            canvas.DrawPath(Paint.NeedlyPath, Paint.BluePaint);
            canvas.DrawPath(Paint.NeedlyPath, Paint.WhiteStrokePaint);
            canvas.Restore();
        }


        bool UpdateCanvas()
        {
            InvalidateSurface();
            return true;
        }



    }
}

