using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;

namespace OOP_3
{
    class Area : MapObject
    {
        List<PointLatLng> points = new List<PointLatLng>();

        public Area(string title, List<PointLatLng> points) : base(title)
        {

        }

        public override double getDistance(PointLatLng point)
        {

        }

        public override PointLatLng getFocus()
        {

        }

        public override GMapMarker getMarker()
        {

        }
    }
}
