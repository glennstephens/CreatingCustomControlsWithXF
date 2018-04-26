using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace FormsControlExample
{
    public class SimpleGauge : SKCanvasView
    {
        public SimpleGauge() : base()
        {
            WidthRequest = 500;
            HeightRequest = 500;
        }

        // Properties for the Values
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create("Value", typeof(float), typeof(SimpleGauge), 0.0f);

        public float Value
        {
            get { return (float)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty StartValueProperty =
            BindableProperty.Create("StartValue", typeof(float), typeof(SimpleGauge), 0.0f);

        public float StartValue
        {
            get { return (float)GetValue(StartValueProperty); }
            set { SetValue(StartValueProperty, value); }
        }

        public static readonly BindableProperty EndValueProperty =
            BindableProperty.Create("EndValue", typeof(float), typeof(SimpleGauge), 100.0f);

        public float EndValue
        {
            get { return (float)GetValue(EndValueProperty); }
            set { SetValue(EndValueProperty, value); }
        }

        // Properties for the Range Display

        public static readonly BindableProperty RangeIsVisibleProperty =
            BindableProperty.Create("RangeIsVisible", typeof(bool), typeof(SimpleGauge), true);

        public bool RangeIsVisible
        {
            get { return (bool)GetValue(RangeIsVisibleProperty); }
            set { SetValue(RangeIsVisibleProperty, value); }
        }

        public static readonly BindableProperty HighlightRangeStartValueProperty =
            BindableProperty.Create("HighlightRangeStartValue", typeof(float), typeof(SimpleGauge), 70.0f);

        public float HighlightRangeStartValue
        {
            get { return (float)GetValue(HighlightRangeStartValueProperty); }
            set { SetValue(HighlightRangeStartValueProperty, value); }
        }

        public static readonly BindableProperty HighlightRangeEndValueProperty =
            BindableProperty.Create("HighlightRangeEndValue", typeof(float), typeof(SimpleGauge), 100.0f);

        public float HighlightRangeEndValue
        {
            get { return (float)GetValue(HighlightRangeEndValueProperty); }
            set { SetValue(HighlightRangeEndValueProperty, value); }
        }

        // Properties for the Colors
        public static readonly BindableProperty GaugeLineColorProperty =
            BindableProperty.Create("GaugeLineColor", typeof(Color), typeof(SimpleGauge), Color.FromHex("#70CBE6"));

        public Color GaugeLineColor
        {
            get { return (Color)GetValue(GaugeLineColorProperty); }
            set { SetValue(GaugeLineColorProperty, value); }
        }

        public static readonly BindableProperty ValueColorProperty =
            BindableProperty.Create("ValueColor", typeof(Color), typeof(SimpleGauge), Color.FromHex("FF9A52"));

        public Color ValueColor
        {
            get { return (Color)GetValue(ValueColorProperty); }
            set { SetValue(ValueColorProperty, value); }
        }

        public static readonly BindableProperty RangeColorProperty =
            BindableProperty.Create("RangeColor", typeof(Color), typeof(SimpleGauge), Color.FromHex("#E6F4F7"));

        public Color RangeColor
        {
            get { return (Color)GetValue(RangeColorProperty); }
            set { SetValue(RangeColorProperty, value); }
        }

        // Properties for the Units

        public static readonly BindableProperty UnitsTextProperty =
            BindableProperty.Create("UnitsText", typeof(string), typeof(SimpleGauge), "");

        public string UnitsText
        {
            get { return (string)GetValue(UnitsTextProperty); }
            set { SetValue(UnitsTextProperty, value); }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;

            SKPaint backPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.WhiteSmoke.ToSKColor(),
            };

            int width = e.Info.Width;
            int height = e.Info.Height;

            canvas.DrawRect(new SKRect(0, 0, width, height), backPaint);

            canvas.Save();

            canvas.Translate(width / 2, height / 2);
            canvas.Scale(Math.Min(width / 210f, height / 520f));

            SKPaint GaugeMainLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = GaugeLineColor.ToSKColor(),
                StrokeWidth = 10
            };

            SKPaint GaugePointPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = ValueColor.ToSKColor()
            };

            SKPaint HighlightRangePaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = RangeColor.ToSKColor()
            };

            SKPoint center = new SKPoint(0, 0);

            var rect = new SKRect(-100, -100, 100, 100);

            // Add a buffer for the rectangle
            rect.Inflate(-10, -10);

            // Draw the range of values
            if (RangeIsVisible)
            {
                var rangeStartAngle = AmountToAngle(HighlightRangeStartValue);
                var rangeEndAngle = AmountToAngle(HighlightRangeEndValue);
                var angleDistance = rangeEndAngle - rangeStartAngle;

                using (SKPath path = new SKPath())
                {
                    path.AddArc(rect, rangeStartAngle, angleDistance);
                    path.LineTo(center);
                    canvas.DrawPath(path, HighlightRangePaint);
                }
            }

            // Draw the main gauge line/arc
            var startAngle = 135;
            var sweepAngle = 270;
            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawPath(path, GaugeMainLinePaint);
            }

            // Draw the current value on the arc
            var valueAsAngle = AmountToAngle(Value);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, valueAsAngle, 2);
                canvas.DrawCircle(path.Bounds.Left + path.Bounds.Width / 2,
                                  path.Bounds.Top + path.Bounds.Height / 2,
                                  10f,
                                  GaugePointPaint);
            }

            // Draw the Units of Measurement Text on the display
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Black
            };

            float textWidth = textPaint.MeasureText(UnitsText);
            textPaint.TextSize = 12f;

            SKRect textBounds = SKRect.Empty;
            textPaint.MeasureText(UnitsText, ref textBounds);

            float xText = -1 * textBounds.MidX;
            float yText = 95 - textBounds.Height;

            // And draw the text
            canvas.DrawText(UnitsText, xText, yText, textPaint);

            // Draw the Value on the display
            var valueText = Value.ToString("F1");
            float valueTextWidth = textPaint.MeasureText(valueText);
            textPaint.TextSize = 35f;

            textPaint.MeasureText(valueText, ref textBounds);

            xText = -1 * textBounds.MidX;
            yText = 85 - textBounds.Height;

            // And draw the text
            canvas.DrawText(valueText, xText, yText, textPaint);

            canvas.Restore();
        }

        float AmountToAngle(float value)
        {
            return 135f + (value / (EndValue - StartValue)) * 270f;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            // Determine when to change. Basically on any of the properties that we've added that affect
            // the visualization, including the size of the control, we'll repaint
            if (propertyName == HighlightRangeEndValueProperty.PropertyName ||
                propertyName == HighlightRangeStartValueProperty.PropertyName ||
                propertyName == ValueProperty.PropertyName ||
                propertyName == WidthProperty.PropertyName ||
                propertyName == HeightProperty.PropertyName ||
                propertyName == StartValueProperty.PropertyName ||
                propertyName == EndValueProperty.PropertyName ||
                propertyName == RangeIsVisibleProperty.PropertyName ||
                propertyName == GaugeLineColorProperty.PropertyName ||
                propertyName == ValueColorProperty.PropertyName ||
                propertyName == RangeColorProperty.PropertyName ||
                propertyName == UnitsTextProperty.PropertyName)
            {
                this.InvalidateSurface();
            }
        }
    }
}
