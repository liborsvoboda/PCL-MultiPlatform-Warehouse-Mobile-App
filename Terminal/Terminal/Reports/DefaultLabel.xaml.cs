using SkiaSharp;
using SkiaSharp.Views.Forms;
using Splat;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Terminal.DbModels;
using Terminal.Languages;
using Terminal.Singleton;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Rendering;
using static Terminal.Database.SapDefinitions;

namespace Terminal.SKReports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultLabelPage : ContentPage
    {
        private SapUIID SapUIIDGenerated = new SapUIID();
        readonly SKPaint blackPaint = new SKPaint { Color = SKColors.Black, TextSize = 16 };
        readonly SKPaint backgroundPaint = new SKPaint { Color = SKColors.White, TextSize = 16 };
        readonly SKPaint blackPaintMiddle = new SKPaint { Color = SKColors.Black, TextSize = 20 };
        readonly SKPaint BlackPaintBold = new SKPaint { Color = SKColors.Black, TextSize = 40 };
        readonly SKPaint vWhitePaint = new SKPaint { Color = SKColors.White };
        private string typeLabel = "PMS";

        //<zxing:ZXingBarcodeImageView WidthRequest="700" x:Name="barCode" BarcodeValue="" BarcodeFormat="PDF_417"/>
        public DefaultLabelPage(SapUIID sapUIIDGenerated)
        {
            InitializeComponent();
            SapUIIDGenerated = sapUIIDGenerated;
            if (String.IsNullOrWhiteSpace(SapUIIDGenerated.snInserted) && !String.IsNullOrWhiteSpace(SapUIIDGenerated.pnInserted) && !String.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted)) typeLabel = "PM";

        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKSurface vSurface = args.Surface;
            SKCanvas vCanvas = vSurface.Canvas;

            var vWhiteRectangle = new SKRect(0, 0, 450, 200);
            vCanvas.DrawRect(vWhiteRectangle, backgroundPaint);

            vCanvas.DrawText(LangResources.MN + ":", 20, 50, blackPaint);
            vCanvas.DrawText(SapUIIDGenerated.mnInserted.TrimStart('0'), 20, 90, BlackPaintBold);
            vCanvas.DrawText("C/N:", 20, 110, blackPaint);
            if (typeLabel == "PM")
            {
                vCanvas.DrawText(LangResources.PN + ": " + SapUIIDGenerated.pnInserted, 20, 130, blackPaint);
            }
            if (typeLabel == "PMS")
            {
                vCanvas.DrawText(LangResources.SN + ": " + SapUIIDGenerated.snInserted, 20, 130, blackPaint);
                vCanvas.DrawText(LangResources.PN + ": " + SapUIIDGenerated.pnInserted, 220, 130, blackPaint);
            }

            vCanvas.DrawText(SapUIIDGenerated.Text, 20, 150, blackPaintMiddle);
            barCodeResult.BarcodeValue = geterateBarcode(SapUIIDGenerated);
        }

        void Print_Clicked(object sender, EventArgs e)
        {

            var stream = new SKFileWStream(Path.Combine(AppStatus.BasePath, "label.pdf"));
            var document = SKDocument.CreatePdf(stream);

            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.PDF_417,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 200,
                    Height = 40,
                    PureBarcode = true,
                    Margin = 0
                },
                Renderer = new PixelDataRenderer() { Foreground = PixelDataRenderer.Color.Black, Background = PixelDataRenderer.Color.White }
            };
            var writeableBitmap = writer.Write(geterateBarcode(SapUIIDGenerated));

            uint[] decoded = new uint[writeableBitmap.Pixels.Length / 4];
            Buffer.BlockCopy(writeableBitmap.Pixels, 0, decoded, 0, writeableBitmap.Pixels.Length);
            SKBitmap bitmap = new SKBitmap();
            var gcHandle = GCHandle.Alloc(decoded, GCHandleType.Pinned);
            var info = new SKImageInfo(writeableBitmap.Width, writeableBitmap.Height, SKColorType.Rgb888x, SKAlphaType.Opaque);
            bitmap.InstallPixels(info, gcHandle.AddrOfPinnedObject(), info.RowBytes, null, delegate { gcHandle.Free(); }, null);


            using (var pdfCanvas = document.BeginPage(450, 200))
            {
                pdfCanvas.DrawRect(new SKRect(0, 0, 450, 200), backgroundPaint);
                pdfCanvas.DrawText(LangResources.MN + ":", 20, 50, blackPaint);
                pdfCanvas.DrawText(SapUIIDGenerated.mnInserted.TrimStart('0'), 20, 90, BlackPaintBold);
                pdfCanvas.DrawText("C/N:", 20, 110, blackPaint);
                if (typeLabel == "PM")
                {
                    pdfCanvas.DrawText(LangResources.PN + ": " + SapUIIDGenerated.pnInserted, 20, 130, blackPaint);
                }
                if (typeLabel == "PMS")
                {
                    pdfCanvas.DrawText(LangResources.SN + ": " + SapUIIDGenerated.snInserted, 20, 130, blackPaint);
                    pdfCanvas.DrawText(LangResources.PN + ": " + SapUIIDGenerated.pnInserted, 220, 130, blackPaint);
                }
                pdfCanvas.DrawText(SapUIIDGenerated.Text, 20, 150, blackPaintMiddle);
                pdfCanvas.DrawBitmap(bitmap, new SKPoint() { X = 220, Y = 60 });
                pdfCanvas.Save();
            }

            document.EndPage();
            document.Close();
            stream.Dispose();
            Navigation.PopModalAsync();

            PclPrintService.PclPrintService.PrintFromFile(Path.Combine(AppStatus.BasePath, "label.pdf"));
        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private string geterateBarcode(SapUIID SapUIIDGenerated)
        {
            LabelDefinitions selectedDefinition = App.LabelDefinitions.Where(item => item.labelName == typeLabel).First();

            string labelCode = selectedDefinition.definition
                .Replace("*1*", (SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue1) != null ? (string)SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue1).GetValue(SapUIIDGenerated, null) : "").TrimStart(selectedDefinition.removeStartChar1 != null ? selectedDefinition.removeStartChar1[0] : new char()))
                .Replace("*2*", (SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue2) != null ? (string)SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue2).GetValue(SapUIIDGenerated, null) : "").TrimStart(selectedDefinition.removeStartChar2 != null ? selectedDefinition.removeStartChar2[0] : new char()))
                .Replace("*3*", (SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue3) != null ? (string)SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue3).GetValue(SapUIIDGenerated, null) : "").TrimStart(selectedDefinition.removeStartChar3 != null ? selectedDefinition.removeStartChar3[0] : new char()))
                .Replace("*4*", (SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue4) != null ? (string)SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue4).GetValue(SapUIIDGenerated, null) : "").TrimStart(selectedDefinition.removeStartChar4 != null ? selectedDefinition.removeStartChar4[0] : new char()))
                .Replace("*5*", (SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue5) != null ? (string)SapUIIDGenerated.GetType().GetProperty(selectedDefinition.wsdlValue5).GetValue(SapUIIDGenerated, null) : "").TrimStart(selectedDefinition.removeStartChar5 != null ? selectedDefinition.removeStartChar5[0] : new char()))
                ;
            return labelCode;
        }
    }
}