using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace OOP_3
{
    class Car : MapObject
    {
        private PointLatLng point;
        private Route route;
        public Human pass;
        Thread newThread;

        GMapMarker carMarker;

        public event EventHandler Arrived;

        public Car(string title, PointLatLng point) : base(title)
        {
            this.point = point;
            pass = null;
        }

        public override double getDistance(PointLatLng point)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(this.point.Lat, this.point.Lng);

            return c1.GetDistanceTo(c2);
        }

        public override PointLatLng getFocus()
        {
            return point;
        }

        public override GMapMarker getMarker()
        {
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 40,
                    Height = 40,
                    ToolTip = getTitle(),
                    Source = new BitmapImage(new Uri("C:\\Users\\malka\\source\\repos\\OOP_3\\OOP_3\\icons\\Car.png"))
                }
            };

            carMarker = marker;

            return marker;
        }

        public GMapMarker moveTo(PointLatLng dest)
        {
            RoutingProvider routingProvider = GMapProviders.OpenStreetMap;
            // определение маршрута
            MapRoute route = routingProvider.GetRoute(
             point, // начальная точка маршрута
             dest, // конечная точка маршрута
             false, // поиск по шоссе (false - включен)
             false, // режим пешехода (false - выключен)
             (int)15);
            // получение точек маршрута
            List<PointLatLng> routePoints = route.Points;

            this.route = new Route("", routePoints);

            newThread = new Thread(new ThreadStart(MoveByRoute));
            newThread.Start();

            return this.route.getMarker();
        }

        private void MoveByRoute()
        {
            // последовательный перебор точек маршрута
            foreach (var point in route.getPoints())
            {
                // делегат, возвращающий управление в главный поток
                Application.Current.Dispatcher.Invoke(delegate {
                    // изменение позиции маркера
                    carMarker.Position = point;

                    this.point = point;

                    if(pass != null)
                    {
                        pass.setPosition(point);
                        pass.humanMarker.Position = point;
                    }
                });
                // задержка 500 мс
                Thread.Sleep(500);
            }
            if (pass == null)
            {
                newThread.Abort();
                Arrived?.Invoke(this, null);
            }
            else
            {
                pass = null;
                newThread.Abort();
            }
        }

        public void passSeated(object sender, EventArgs e)
        {
            pass = (Human)sender;
            Application.Current.Dispatcher.Invoke(delegate { moveTo(pass.getDestination()); });
        }
    }
}
