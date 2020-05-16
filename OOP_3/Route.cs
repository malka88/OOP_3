﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;

namespace OOP_3
{
    class Route : MapObject
    {
        List<PointLatLng> points = new List<PointLatLng>();

        public Route(string title, List<PointLatLng> points) : base(title)
        {
            foreach (PointLatLng point in points)
            {
                this.points.Add(point);
            }
        }

        public override double getDistance(PointLatLng point)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(points[0].Lat, points[0].Lng);

            return c1.GetDistanceTo(c2);
        }

        public override PointLatLng getFocus()
        {
            return points[0];
        }

        public override GMapMarker getMarker()
        {
            GMapMarker marker = new GMapRoute(points)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.DarkBlue,
                    Fill = Brushes.DarkBlue,
                    StrokeThickness = 4
                }
            };

            return marker;
        }

        public List<PointLatLng> getPoints()
        {
            return points;
        }
    }
}
