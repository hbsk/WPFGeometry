using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BuildGeometry
{
    public interface IGeometryData
    {
        void AddNormal(Vector3D normal);
        void AddPoint(Point3D pt);
        void AddTriangleIndices(int first, int second, int third);
        void SetColorInfo();

        Point3DCollection getPositions();
        Int32Collection getTriangleIndices();
        Vector3DCollection getNormals(); 
    }
}
