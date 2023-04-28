using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Singleton;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terminal.SKReports
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Report : ContentPage
	{
        public Report()
        {
            InitializeComponent();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            //Initialize the Canvas  
            SKSurface vSurface = args.Surface;
            SKCanvas vCanvas = vSurface.Canvas;
            //Clear the Canvas  
            vCanvas.Clear();
            //Creating the Paint object to color the Items  
            SKPaint vBlackPaint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 5,
                StrokeCap = SKStrokeCap.Round,
                TextSize = 60
            };
            SKPaint vWhitePaint = new SKPaint
            {
                Color = SKColors.White
            };
            var vRectangle = new SKRect(100, 100, 900, 900);
            vCanvas.DrawRect(vRectangle, vBlackPaint);
            var vWhiteRectangle = new SKRect(105, 105, 895, 895);
            vCanvas.DrawRect(vWhiteRectangle, vWhitePaint);
            //Diagnol Lines  
            vCanvas.DrawLine(100, 100, 900, 900, vBlackPaint);
            vCanvas.DrawLine(900, 100, 100, 900, vBlackPaint);
            //Drawing the Numbers between the lines  
            vCanvas.DrawText("1", 500, 300, vBlackPaint);
            vCanvas.DrawText("2", 260, 200, vBlackPaint);
            vCanvas.DrawText("3", 150, 350, vBlackPaint);
            vCanvas.DrawText("4", 250, 500, vBlackPaint);
            vCanvas.DrawText("5", 150, 700, vBlackPaint);
            vCanvas.DrawText("6", 260, 800, vBlackPaint);
            vCanvas.DrawText("7", 500, 700, vBlackPaint);
            vCanvas.DrawText("8", 670, 830, vBlackPaint);
            vCanvas.DrawText("9", 780, 700, vBlackPaint);
            vCanvas.DrawText("10", 670, 500, vBlackPaint);
            vCanvas.DrawText("11", 780, 350, vBlackPaint);
            vCanvas.DrawText("12", 670, 200, vBlackPaint);
            //The Side lines for creating other boxes  
            vCanvas.DrawLine(100, 500, 500, 100, vBlackPaint);
            vCanvas.DrawLine(100, 500, 500, 900, vBlackPaint);
            vCanvas.DrawLine(500, 100, 900, 500, vBlackPaint);
            vCanvas.DrawLine(500, 900, 900, 500, vBlackPaint);
        }

        void Save_Clicked(object sender, EventArgs e)
        {
            var stream = new SKFileWStream(Path.Combine(AppStatus.BasePath, "document.pdf"));
            var document = SKDocument.CreatePdf(stream);

  
            var canvas = document.BeginPage(256, 256);

            SKPaint vBlackPaint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 5,
                StrokeCap = SKStrokeCap.Round,
                TextSize = 60
            };
            var vRectangle = new SKRect(100, 100, 200, 200);
            canvas.DrawRect(vRectangle, vBlackPaint);

           
            document.EndPage();
            document.Close();
            Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}