using System;
using System.Collections.Generic;
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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;

namespace OOP_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public class MapPoint
    {
        public string Title { get; set; }
        public double Distance { get; set; }
    }

    public partial class MainWindow : Window
    {
        List<MapObject> objects = new List<MapObject>();
        List<PointLatLng> points = new List<PointLatLng>();
        List<PointLatLng> nearestPointPosition = new List<PointLatLng>();

        List<MapObject> nearestObjects = new List<MapObject>();

        static PointLatLng startOfRoute;
        static PointLatLng endOfRoute;

        static IEnumerable<MapObject> besidedObjects;

        private void AddMarker(MapObject marker)
        {
            objects.Add(marker);
            Map.Markers.Add(marker.getMarker());
            markerTitle.Text = "";
        }

        private void ClearPoints()
        {
            points.Clear();
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            Map.MapProvider = GMapProviders.GoogleMap;
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            Map.Position = new PointLatLng(55.012823, 82.950359);

            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;
        }

        public MainWindow()
        {
            InitializeComponent();
            markerBox.Items.Add("Местоположение");
            markerBox.Items.Add("Человек");
            markerBox.Items.Add("Автомобиль");
            markerBox.Items.Add("Маршрут");
            markerBox.Items.Add("Область");
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if(create.IsChecked == true)
            {
                if ((string)markerBox.SelectedItem == "Человек")
                {
                    Human human = new Human(markerTitle.Text, Map.Position);
                    AddMarker(human);
                }

                if ((string)markerBox.SelectedItem == "Автомобиль")
                {
                    Car car = new Car(markerTitle.Text, Map.Position);
                    AddMarker(car);
                }

                if ((string)markerBox.SelectedItem == "Местоположение")
                {
                    Location location = new Location(markerTitle.Text, Map.Position);
                    AddMarker(location);
                }

                if ((string)markerBox.SelectedItem == "Маршрут")
                {
                    Route route = new Route(markerTitle.Text, points);
                    AddMarker(route);
                }

                if ((string)markerBox.SelectedItem == "Область")
                {
                    Area area = new Area(markerTitle.Text, points);
                    AddMarker(area);
                }
            }

            ClearPoints();
        }

        private void Ok1_Click(object sender, RoutedEventArgs e)
        {
            MapObject mapObject = null;
            searchResults.Items.Clear();
            nearestPointPosition.Clear();
            nearestObjects.Clear();

            foreach (MapObject obj in objects)
            {
                if (obj.getTitle() ==  markerTitle1.Text)
                {
                    mapObject = obj;
                    Map.Position = obj.getFocus();
                    break;
                }
            }

            foreach (MapObject obj in objects)
            {

                if ((mapObject.getDistance(obj.getFocus()) < 500) || (mapObject.getTitle() == obj.getTitle()))
                {
                    nearestObjects.Add(obj);
                }
            }

            besidedObjects = nearestObjects.OrderBy(mapObj => mapObj.getDistance(mapObject.getFocus()));

            foreach (MapObject obj in besidedObjects)
            {

                if ((mapObject.getDistance(obj.getFocus()) < 500) || (mapObject.getTitle() == obj.getTitle()))
                {
                    searchResults.Items.Add(obj.getTitle());
                    nearestPointPosition.Add(obj.getFocus());
                }
            }
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);

            points.Add(point);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ClearPoints();
        }
    }
}
