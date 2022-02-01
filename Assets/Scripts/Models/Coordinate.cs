using UnityEngine;

namespace Space4x.Models
{
        public struct Coordinate
        {
                public Vector3 Position { get; }

                public float Angle { get; }

                public Coordinate(float x, float z, float angle)
                {
                        this.Angle = angle;
                        this.Position = new Vector3(x, 0f, z);
                }
        }
}
