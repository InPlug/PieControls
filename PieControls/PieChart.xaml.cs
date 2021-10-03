using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace NetEti.CustomControls
{
    /// <summary>
    /// Stellt ein WPF-Tortendiagramm plus Zusatzinfos zur Verfügung.
    /// Projekt von Kashif Imran auf Code Project - vielen Dank dafür.
    /// https://www.codeproject.com/Articles/442506/Simple-and-Easy-to-Use-Pie-Chart-Controls-in-WPF
    /// </summary>
    public partial class PieChart : UserControl
    {
        /// <summary>
        /// Daten für das aktuelle Tortendiagramm.
        /// </summary>
        public PieDataCollection values;

        /// <summary>
        /// Farbe für Popups mit Zusatzinformationen.
        /// </summary>
        public Brush PopupBrush
        {
            get { return Pie.PopupBrush; }
            set { Pie.PopupBrush = value; }
        }

        /// <summary>
        /// Holt oder setzt die Daten für das aktuelle Tortendiagramm.
        /// </summary>
        public PieDataCollection Data
        {
            get { return values; }
            set
            {
                values = value;
                Pie.Data = value;
                foreach (var v in values)
                {
                    v.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PieSegment_PropertyChanged);
                }
                value.RadialLine.PropertyChanged
                    += new System.ComponentModel.PropertyChangedEventHandler(PieSegment_PropertyChanged); ;
                Dispatcher.Invoke(new Action(() => { InvalidateVisual(); }));
            }
        }

        private void PieSegment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => { InvalidateVisual(); }));
        }

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        public PieChart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Holt oder setzt die Breite des Fensters für das Tortendiagramm.
        /// </summary>
        public double PieWidth
        {
            get { return Pie.Width; }
            set { Pie.Width = value; }
        }

        /// <summary>
        /// Holt oder setzt die Höhe des Fensters für das Tortendiagramm.
        /// </summary>
        public double PieHeight
        {
            get { return Pie.Height; }
            set { Pie.Height = value; }
        }

        //void Pie_PieWasInvalidated(object sender, EventArgs e)
        //{
        //    Dispatcher.Invoke(new Action(() => { InvalidateVisual(); }));
        //}

        /// <summary>
        /// Wird beim Zeichnen des PieCharts angesprungen - fügt PieChart spezifische
        /// Darstellungen hinzu.
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (values != null)
            {
                double height = values.Count * 20;
                double top = (Height - height) / 2;
                foreach (PieSegment ps in values)
                {
                    dc.DrawRectangle(ps.SolidBrush, null, new Rect(Pie.Width + 10, top, 8, 8));
                    dc.DrawText(GetFormattedText(ps.Name + " (" + ps.Value + ")", 12, Brushes.Black), new Point(Pie.Width + 20, top));
                    top += 20;
                }
            }
        }

        /// <summary>
        /// Liefert einen einen entsprechend der "fontSize" und "brush" und der
        /// lokalen Einstellungen formatierten Text zurück.
        /// </summary>
        /// <param name="textToFormat">Der zu formatierende Text.</param>
        /// <param name="fontSize">Die Schriftgröße.</param>
        /// <param name="brush">Zeicheninformationen, Farbe, Liniendicke.</param>
        /// <returns></returns>
        public FormattedText GetFormattedText(string textToFormat, double fontSize, Brush brush)
        {
            Typeface typeface = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            // return new FormattedText(textToFormat, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
            // typeface, fontSize, brush);
            return new FormattedText(textToFormat, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                typeface, fontSize, brush, VisualTreeHelper.GetDpi(this).PixelsPerDip);
        }
    }
}
