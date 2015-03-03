﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FastWpfGridTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grid1.Model = new GridModel1();
            grid2.Model = new GridModel2();
        }

        private void tabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab.SelectedIndex == 2) Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Action) SetBitmap);
        }

        private void SetBitmap()
        {
            int width = (int) textbox.ActualWidth;
            int height = (int) textbox.ActualHeight;

            if (width <= 0 || height <= 0) return;
            WriteableBitmap buffer;

            var bgColor = Color.FromRgb(0xF9, 0xF9, 0xF9);

            textImage1.Width = width;
            textImage1.Height = height;
            buffer = BitmapFactory.New(width, height);
            buffer.Clear(bgColor);
            //buffer.DrawString(0, 0, Colors.Black, new Typeface("Arial"), 12, "ABC 123 Gfijdr");
            buffer.DrawString(0, 0, Colors.Black, new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 11, "Transparency");
            buffer.DrawLine(0, 10, 50, 10, Colors.Black);
            buffer.DrawLine(50, 0, 50, 40, Colors.Black);
            textImage1.Source = buffer;

            textImage2.Width = width;
            textImage2.Height = height;
            buffer = BitmapFactory.New(width, height);
            buffer.Clear(bgColor);
            buffer.DrawString(0, 0, Colors.Black, bgColor, new Typeface("Arial"), 12, "ABC 123 Gfijdr");
            textImage2.Source = buffer;

            textImage3.Width = width;
            textImage3.Height = height;


            var text = new FormattedText("ABC l 123 Gfijdr",
                                         CultureInfo.InvariantCulture,
                                         FlowDirection.LeftToRight,
                                         new Typeface("Arial"),
                                         12,
                                         new SolidColorBrush(Colors.Black));


            var drawingVisual = new DrawingVisual();
            //TextOptions.SetTextRenderingMode(drawingVisual, TextRenderingMode.ClearType);
            using (var drawingContext = drawingVisual.RenderOpen())
            {

                //var brush = new VisualBrush(new TextBlock {Text = "ABC 123 Gfijdr"});
                //drawingContext.DrawRectangle(brush, null, new Rect(0, 0, width, height));

                //Double halfPenWidth = 0.5;

                //var face = new Typeface("Arial");
                //GlyphTypeface gt;
                //face.TryGetGlyphTypeface(out gt);
                //var indexes = new ushort[] {gt.CharacterToGlyphMap['A']};
                //var widths = new double[] {gt.AdvanceWidths['A']*12};

                //var glyphRun = new GlyphRun(gt, 0, false, 12, indexes, new Point(0, 14), widths, null, null, null, null, null, null);
                //var rect = glyphRun.ComputeAlignmentBox();

                //Double halfPenWidth = 0.5;
                ////var rect = new Rect(0, 0, width, height);
                //GuidelineSet guidelines = new GuidelineSet();
                //guidelines.GuidelinesX.Add(rect.Left + halfPenWidth);
                //guidelines.GuidelinesX.Add(rect.Right + halfPenWidth);
                //guidelines.GuidelinesY.Add(rect.Top + halfPenWidth);
                //guidelines.GuidelinesY.Add(rect.Bottom + halfPenWidth);

                drawingContext.DrawRectangle(new SolidColorBrush(bgColor), null, new Rect(0, 0, width, height));

                drawingContext.DrawText(text, new Point(0, 0));
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(0.5, 10.5), new Point(50.5, 10.5));
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(35.5, 0.5), new Point(35.5, 40.5));

                //drawingContext.PushGuidelineSet(guidelines);
                //drawingContext.DrawGlyphRun(Brushes.Black, glyphRun);
                //drawingContext.Pop();
            }
            //drawingVisual.Transform = new ScaleTransform(0.9, 0.9);
            //drawingVisual.Transform = new ScaleTransform(1, 1);
            //drawingVisual.Transform = new TranslateTransform(0, -0.25);

            var tb = new TextBlock { Text = "ABC l 123 Gfijdr" };
            tb.Measure(new Size(width, height));
            tb.Arrange(new Rect(new Point(0, 0), new Size(width, height)));

            var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);

            RenderOptions.SetBitmapScalingMode(drawingVisual, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetBitmapScalingMode(bmp, BitmapScalingMode.NearestNeighbor);

            //RenderOptions.SetCachingHint(bmp, CachingHint.Cache);
            //RenderOptions.SetBitmapScalingMode(bmp, BitmapScalingMode.LowQuality);
            bmp.Render(drawingVisual);
            //bmp.Render(tb);
            //bmp.Freeze();


            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            var ms = new MemoryStream();
            encoder.Save(ms);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (var fs = System.IO.File.OpenWrite("c:/test/file1.png"))
            {
                encoder.Save(fs);
            }

            textImage3.Stretch = Stretch.None;
            textImage3.Source = bmp;
        }
    }
}
