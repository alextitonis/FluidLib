using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluidLib.Physics
{
    #region 3D
    public class Box
    {
        public Vector3 min { get; private set; }
        public Vector3 max { get; private set; }
        public Vector3 center { get; private set; }

        public Box(Vector3 min, Vector3 max, Vector3 center)
        {
            this.min = min;
            this.max = max;
            this.center = center;
        }
    }
    public class Sphere
    {
        public Vector3 center { get; private set; }
        public float radius { get; private set; }

        public Sphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }

    public class Vector3
    {
        public float x { get; private set; }
        public float y { get; private set; }
        public float z { get; private set; }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    #endregion
    #region 2D
    public class Box2D
    {
        public Vector2 min { get; private set; }
        public Vector2 max { get; private set; }
        public Vector2 center { get; private set; }

        public Box2D(Vector2 min, Vector2 max, Vector2 center)
        {
            this.min = min;
            this.max = max;
            this.center = center;
        }
    }
    public class Circle
    {
        public Vector2 center { get; private set; }
        public float radius { get; private set; }

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }

    public class Vector2
    {
        public float x { get; private set; }
        public float y { get; private set; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    #endregion
}
