using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BuildGeometry
{
    public class Mesh3DData : IGeometryData
    {
        private Vector3DCollection normals = new Vector3DCollection();
        private Point3DCollection positions = new Point3DCollection();
        private Int32Collection triangleIndices = new Int32Collection();
        //private int colorInfo;

        public void AddPoint(Point3D position)
        {
            positions.Add(position);
        }

        public void AddNormal(Vector3D normal)
        {
            normals.Add(normal);
        }

        public void AddTriangleIndices(int first, int second, int third)
        {
            triangleIndices.Add(first);
            triangleIndices.Add(second);
            triangleIndices.Add(third);
        }

        public void SetColorInfo()
        {
            throw new System.NotImplementedException();
        }

        public Point3DCollection getPositions()
        {
            return positions;
        }

        public Int32Collection getTriangleIndices()
        {
            return triangleIndices;
        }

        public Vector3DCollection getNormals()
        {
            return normals;
        }
    }
}
