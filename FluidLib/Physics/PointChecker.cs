using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluidLib.Physics
{
    public class PointChecker
    {
        public static bool PointIsInsideBox(Vector3 point, Box box)
        {
            return (point.x >= box.min.x && point.x <= box.max.x) &&
           (point.y >= box.min.y && point.y <= box.max.y) &&
           (point.z >= box.min.z && point.z <= box.max.z);
        }
        public static bool PointIsInsideSphere(Vector3 point, Sphere sphere)
        {
            double distance = Math.Pow((point.x - sphere.center.x), 2) + Math.Pow((point.y - sphere.center.y), 2) + Math.Pow((point.z - sphere.center.z), 2);

            return distance <= Math.Pow(sphere.radius, 2);
        }

        public static bool PointIsInside(Vector2 point, Box2D box)
        {
            return (point.x >= box.min.x && point.x <= box.max.x) &&
                (point.y >= box.min.y && point.y <= box.max.y);
        }
        public static bool PointIsInsideCircle(Vector2 point, Circle circle)
        {
            double distance = Math.Pow(point.x - circle.center.x, 2) * Math.Pow(point.y - circle.center.y, 2);

            return distance <= Math.Pow(circle.radius, 2);
        }
    }
}
