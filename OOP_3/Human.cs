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

namespace OOP_3
{
    class Human : MapObject
    {
        private PointLatLng point;
        private PointLatLng destination;
        public GMapMarker humanMarker;

        public event EventHandler passSeated;

        public Human(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }

        public PointLatLng getDestination()
        {
            return destination;
        }

        public void setPosition(PointLatLng point)
        {
            this.point = point;
        }

        public void moveTo(PointLatLng dest)
        {
            destination = dest;
        }

        public PointLatLng getPosition()
        {
            return point;
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
                    Width = 32,
                    Height = 32,
                    ToolTip = getTitle(),
                    Source = new BitmapImage(new Uri("C:\\Users\\malka\\source\\repos\\OOP_3\\OOP_3\\icons\\Human.png"))
                }
            };

            humanMarker = marker;

            return marker;
        }

        public void CarArrived(object sender, EventArgs e)
        {
            passSeated?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("tut");
        }

    }
}
