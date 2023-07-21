using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NetEti.CustomControls
{
    /// <summary>
    /// Einfache Aufzählung für Himmelsrichtungen.
    /// </summary>
    public enum CompassDirection
    {
        /// <summary>Nord.</summary>
        N,
        /// <summary>Nord.</summary>
        North,
        /// <summary>Nordost.</summary>
        NE,
        /// <summary>Nordost.</summary>
        NorthEast,
        /// <summary>Ost.</summary>
        E,
        /// <summary>Ost.</summary>
        East,
        /// <summary>Südost.</summary>
        SE,
        /// <summary>Südost.</summary>
        SouthEast,
        /// <summary>Süd.</summary>
        S,
        /// <summary>Süd.</summary>
        South,
        /// <summary>Südwest.</summary>
        SW,
        /// <summary>Südwest.</summary>
        SouthWest,
        /// <summary>West.</summary>
        W,
        /// <summary>West.</summary>
        West,
        /// <summary>Nordwest.</summary>
        NW,
        /// <summary>Nordwest.</summary>
        NorthWest
    }

    /// <summary>
    /// Stellt ein WPF-Tortendiagramm plus Zusatzinfos zur Verfügung.
    /// Projekt von Kashif Imran auf Code Project - vielen Dank dafür.
    /// https://www.codeproject.com/Articles/442506/Simple-and-Easy-to-Use-Pie-Chart-Controls-in-WPF
    /// </summary> 
    public partial class PieControl : UserControl
    {
        PieDataCollection? values;
        Dictionary<Path, PieSegment> pathDictionary = new Dictionary<Path, PieSegment>();

        /// <summary>
        /// Definiert, in welcher Himmelsrichtung das erste Segment des PieChart beginnt (linke Kante).
        /// </summary>
        public static readonly DependencyProperty StartSegmentCompassProperty
          = DependencyProperty.Register("StartSegmentCompass", typeof(CompassDirection), typeof(PieControl));

        /// <summary>
        /// Definiert, in welcher Himmelsrichtung das erste Segment des PieChart beginnt (linke Kante).
        /// </summary>
        public CompassDirection StartSegmentCompass
        {
            get { return (CompassDirection)GetValue(StartSegmentCompassProperty); }
            set { SetValue(StartSegmentCompassProperty, value); }
        }

        /// <summary>
        /// Holt oder setzt die Schrift- und Farbeinstellungen für Popup-Informationen.
        /// </summary>
        public static readonly DependencyProperty PopupBrushProperty
          = DependencyProperty.Register("PopupBrush", typeof(Brush), typeof(PieControl));

        /// <summary>
        /// Holt oder setzt die Schrift- und Farbeinstellungen für Popup-Informationen.
        /// </summary>
        public Brush PopupBrush
        {
            get
            {
                return (Brush)GetValue(PopupBrushProperty);
            }
            set
            {
                SetValue(PopupBrushProperty, value);
            }
        }

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        public PieControl()
        {
            DataContext = this;
            PopupBrush = Brushes.Orange;
            InitializeComponent();
        }

        /// <summary>
        /// Daten für das aktuelle PieChart.
        /// </summary>
        public PieDataCollection? Data
        {
            get { return values; }
            set
            {
                values = value;
                if (values != null)
                {
                    values.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(values_CollectionChanged);
                    foreach (var v in values)
                    {
                        v.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(pieSegment_PropertyChanged);
                    }
                }
                ResetPie();
            }
        }

        private void AddPathToDictionary(Path path, PieSegment ps)
        {
            pathDictionary.Add(path, ps);
            path.MouseEnter += new MouseEventHandler(Path_MouseEnter);
            path.MouseMove += new MouseEventHandler(Path_MouseMove);
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = Mouse.GetPosition(this);
            piePopup.Margin = new Thickness(point.X - piePopup.ActualWidth / 4, point.Y - (18 + piePopup.ActualHeight), 0, 0);
        }

        private void PieControl_MouseLeave(object sender, MouseEventArgs e)
        {
            piePopup.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            piePopup.Visibility = System.Windows.Visibility.Visible;
            PieSegment seg = pathDictionary[(Path)sender];
            popupData.Text = seg.Name + " : " + ((seg.Value / Data?.GetTotal() ?? 1) * 100).ToString("N2") + "%";
            Point point = Mouse.GetPosition(this);
            piePopup.Margin = new Thickness(point.X - piePopup.ActualWidth / 4, point.Y - (18 + piePopup.ActualHeight), 0, 0);
        }

        private void ClearPathDictionary()
        {
            foreach (Path path in pathDictionary.Keys)
            {
                path.MouseEnter -= Path_MouseEnter;
                path.MouseMove -= Path_MouseMove;
            }
            pathDictionary.Clear();
        }

        private void CreatePiePathAndGeometries()
        {
            ClearPathDictionary();
            drawingCanvas.Children.Clear();
            drawingCanvas.Children.Add(piePopup);
            if (Data != null)
            {
                double total = Data.GetTotal();
                if (total > 0)
                {
                    double angle = 0;
                    switch (this.StartSegmentCompass)
                    {
                        case CompassDirection.N:
                        case CompassDirection.North:
                            angle = -Math.PI / 2; break;
                        case CompassDirection.NE:
                        case CompassDirection.NorthEast:
                            angle = -Math.PI / 4; break;
                        case CompassDirection.E:
                        case CompassDirection.East:
                            angle = 0; break;
                        case CompassDirection.SE:
                        case CompassDirection.SouthEast:
                            angle = Math.PI / 4; break;
                        case CompassDirection.S:
                        case CompassDirection.South:
                            angle = Math.PI / 2; break;
                        case CompassDirection.SW:
                        case CompassDirection.SouthWest:
                            angle = Math.PI / 4 * 3; break;
                        case CompassDirection.W:
                        case CompassDirection.West:
                            angle = Math.PI * 1; break;
                        case CompassDirection.NW:
                        case CompassDirection.NorthWest:
                            angle = -Math.PI / 4 * 3; break;
                        default:
                            angle = -Math.PI / 2; break;
                    }
                    double startAngle = angle;
                    foreach (PieSegment ps in Data)
                    {
                        //PieSegment ps = Data[1];
                        Geometry geometry;
                        Path path = new Path();
                        if (ps.Value == total)
                        {
                            geometry = new EllipseGeometry(new Point(this.Width / 2, this.Height / 2), this.Width / 2, this.Height / 2);
                        }
                        else
                        {
                            geometry = new PathGeometry();
                            double x = Math.Cos(angle) * Width / 2 + Width / 2;
                            double y = Math.Sin(angle) * Height / 2 + Height / 2;
                            LineSegment lingeSeg = new LineSegment(new Point(x, y), true);
                            double angleShare = (ps.Value / total) * 360;
                            angle += DegreeToRadian(angleShare);
                            x = Math.Cos(angle) * Width / 2 + Width / 2;
                            y = Math.Sin(angle) * Height / 2 + Height / 2;
                            ArcSegment arcSeg = new ArcSegment(new Point(x, y), new Size(Width / 2, Height / 2), angleShare, angleShare > 180, SweepDirection.Clockwise, false);
                            LineSegment lingeSeg2 = new LineSegment(new Point(Width / 2, Height / 2), true);
                            PathFigure fig = new PathFigure(new Point(Width / 2, Height / 2), new PathSegment[] { lingeSeg, arcSeg, lingeSeg2 }, false);
                            ((PathGeometry)geometry).Figures.Add(fig);
                        }
                        path.Fill = ps.GradientBrush;
                        path.Data = geometry;
                        AddPathToDictionary(path, ps);
                        drawingCanvas.Children.Add(path);
                    }
                    if (Data.RadialLine != null)
                    {
                        double angleShare = (Data.RadialLine.Value / total) * 360;
                        angle = DegreeToRadian(angleShare) + startAngle;
                        double x = Math.Cos(angle) * Width / 2 + Width / 2;
                        double y = Math.Sin(angle) * Height / 2 + Height / 2;
                        Point center = new Point(this.Width / 2, this.Height / 2);
                        Point arc = new Point(x, y);
                        Line line = new Line();
                        line.X1 = center.X;
                        line.Y1 = center.Y;
                        line.X2 = arc.X;
                        line.Y2 = arc.Y;
                        line.StrokeThickness = Data.RadialLine.Width;
                        line.Stroke = Data.RadialLine.SolidBrush;
                        line.Visibility = System.Windows.Visibility.Visible;
                        drawingCanvas.Children.Add(line);
                        // InfoController.Say(String.Format($"#PieControl# Children: {drawingCanvas.Children.Count}"));
                    }
                }
            }
        }

        private void ResetPie()
        {
            Dispatcher.Invoke(new Action(CreatePiePathAndGeometries));
        }

        private void pieSegment_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ResetPie();
        }

        private void values_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ResetPie();
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
