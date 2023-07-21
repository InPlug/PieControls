using NetEti.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PieControlsDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            PopulateCharts();
            listView.ItemsSource = collection1;
            allListsBox.ItemsSource = (from a in collectionList select a.CollectionName).ToArray();
            allListsBox.SelectedIndex = 0;
        }

        List<PieDataCollection> collectionList = new List<PieDataCollection>();
        PieDataCollection? collection1;
        PieDataCollection? collection2;
        PieDataCollection? collection3;
        PieDataCollection? collection4;

        void PopulateCharts()
        {
            collection1 = new PieDataCollection();
            collection1.CollectionName = "Animals";
            collection1.RadialLine = new PieRadialLine() { Color = Colors.Black, Value = 35, Width = 2 };
            collection1.Add(new PieSegment { Color = Colors.Green, Value = 10, Name = "Dogs" });
            collection1.Add(new PieSegment { Color = Colors.Yellow, Value = 10, Name = "Cats" });
            collection1.Add(new PieSegment { Color = Colors.Red, Value = 10, Name = "Mice" });
            collection1.Add(new PieSegment { Color = Colors.DarkCyan, Value = 10, Name = "Lizards" });

            collection2 = new PieDataCollection();
            collection2.CollectionName = "Foods";
            collection2.RadialLine = new PieRadialLine() { Color = Colors.Transparent, Value = 35, Width = 2 };
            collection2.Add(new PieSegment { Color = Colors.Green, Value = 20, Name = "Dairy" });
            collection2.Add(new PieSegment { Color = Colors.Yellow, Value = 10, Name = "Fruites" });
            collection2.Add(new PieSegment { Color = Colors.Red, Value = 10, Name = "Vegetables" });
            collection2.Add(new PieSegment { Color = Colors.DarkCyan, Value = 18, Name = "Meat" });
            collection2.Add(new PieSegment { Color = Colors.Wheat, Value = 20, Name = "Grains" });
            collection2.Add(new PieSegment { Color = Colors.Gold, Value = 8, Name = "Sweets" });

            collection3 = new PieDataCollection();
            collection3.CollectionName = "Fruites";
            collection3.RadialLine = new PieRadialLine() { Color = Colors.Transparent, Value = 35, Width = 2 };
            collection3.Add(new PieSegment { Color = Colors.Green, Value = 200, Name = "Apples" });
            collection3.Add(new PieSegment { Color = Colors.Yellow, Value = 150, Name = "Oranges" });
            collection3.Add(new PieSegment { Color = Colors.Red, Value = 250, Name = "Grapes" });
            collection3.Add(new PieSegment { Color = Colors.DarkCyan, Value = 100, Name = "Melons" });
            collection3.Add(new PieSegment { Color = Colors.Brown, Value = 140, Name = "Peaches" });

            collection4 = new PieDataCollection();
            collection4.CollectionName = "Furniture";
            collection4.RadialLine = new PieRadialLine() { Color = Colors.Transparent, Value = 35, Width = 2 };
            collection4.Add(new PieSegment { Color = Colors.Green, Value = 8.5, Name = "Tables" });
            collection4.Add(new PieSegment { Color = Colors.Yellow, Value = 7.5, Name = "Chairs" });
            collection4.Add(new PieSegment { Color = Colors.Red, Value = 9.2, Name = "Beds" });

            pie1.Data = collection1;
            pie2.Data = collection2;
            pie1.PopupBrush = Brushes.LightGray;
            chart1.Data = collection3;
            chart1.PopupBrush = Brushes.LightBlue;
            chart2.Data = collection4;
            chart2.PopupBrush = Brushes.LightCoral;
            collectionList.AddRange(new[] { collection1, collection2, collection3, collection4 });
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            for (int i = collectionList.Count - 1; i > 0; i--) //Knuth-Fisher-Yates shuffle algorithm
            {
                int next = rand.Next(i + 1);
                var temp = collectionList[i];
                collectionList[i] = collectionList[next];
                collectionList[next] = temp;
            }
            pie1.Data = collectionList[0];
            pie2.Data = collectionList[1];
            chart1.Data = collectionList[2];
            chart2.Data = collectionList[3];
            //this.InvalidateVisual();
        }

        private void allListsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView.ItemsSource = collectionList[allListsBox.SelectedIndex];
        }
    }
}
