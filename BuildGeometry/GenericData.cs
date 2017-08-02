using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildGeometry
{
    public interface GeometricData
    {
        void AddPoint();

        void AddNormal();

        /// <summary>
        /// A set of 3 indices for one triangle are sent in
        /// </summary>
        void AddTriangleIndices();

        void SetColorInfo();

    }
}