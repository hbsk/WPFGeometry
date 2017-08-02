using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BuildGeometry
{
    class CustomMeshGeometry
    {
        public CustomMeshGeometry()
        {
        }

        // return the coordinate points of the box whose size is 'boxSize'
        public Point3DCollection getBoxPositions(Point3D center, double basePosZ, double boxSize)
        {
            Point3DCollection points = new Point3DCollection();

            double cx = center.X;
            double cy = center.Y;
            double cz = center.Z + basePosZ;

            double lenb2 = boxSize / 2;
            points.Add(new Point3D( cx + lenb2, cy + lenb2, cz));
            points.Add(new Point3D( cx + lenb2, cy - lenb2, cz));
            points.Add(new Point3D( cx - lenb2, cy - lenb2, cz));
            points.Add(new Point3D( cx - lenb2, cy + lenb2, cz));

            points.Add(new Point3D(cx + lenb2, cy + lenb2, cz + boxSize));
            points.Add(new Point3D(cx + lenb2, cy - lenb2, cz + boxSize));
            points.Add(new Point3D(cx - lenb2, cy - lenb2, cz + boxSize));
            points.Add(new Point3D(cx - lenb2, cy + lenb2, cz + boxSize));

            return points;
        }

        public Int32Collection getBoxTriangleIndices(int baseIndex)
        {
            Int32Collection indices = new Int32Collection();

            // lateral faces
            indices.Add(0 + baseIndex);
            indices.Add(1 + baseIndex);
            indices.Add(4 + baseIndex);
            indices.Add(1 + baseIndex);
            indices.Add(5 + baseIndex);
            indices.Add(4 + baseIndex);

            indices.Add(1 + baseIndex);
            indices.Add(2 + baseIndex);
            indices.Add(5 + baseIndex);
            indices.Add(2 + baseIndex);
            indices.Add(6 + baseIndex);
            indices.Add(5 + baseIndex);

            indices.Add(2 + baseIndex);
            indices.Add(3 + baseIndex);
            indices.Add(6 + baseIndex);
            indices.Add(3 + baseIndex);
            indices.Add(7 + baseIndex);
            indices.Add(6 + baseIndex);

            indices.Add(3 + baseIndex);
            indices.Add(0 + baseIndex);
            indices.Add(7 + baseIndex);
            indices.Add(0 + baseIndex);
            indices.Add(4 + baseIndex);
            indices.Add(7 + baseIndex);

            // top face
            indices.Add(4 + baseIndex);
            indices.Add(5 + baseIndex);
            indices.Add(7 + baseIndex);
            indices.Add(5 + baseIndex);
            indices.Add(6 + baseIndex);
            indices.Add(7 + baseIndex);

            // bottom face
            indices.Add(0 + baseIndex);
            indices.Add(3 + baseIndex);
            indices.Add(1 + baseIndex);
            indices.Add(3 + baseIndex);
            indices.Add(2 + baseIndex);
            indices.Add(1 + baseIndex);

            return indices;
        }

        // Returns the coordinate points of the cylinder whose height, base radius and number of 
        // sides are specified. It is always centered at the origin
        //
        public Point3DCollection getCylinderPositions(int numSides, double height, int baseRadius)
        {
            Point3D center = new Point3D(0, 0, 0);

            Point3DCollection points = new Point3DCollection();

            // Cylinder center is at the origin..
            Point3D baseCenter = new Point3D(center.X, center.Y, center.Z - height / 2);
            Point3D topCenter = new Point3D(center.X, center.Y, center.Z + height / 2);

            points.Add(baseCenter);
            double theta = (2.0 * Math.PI) / numSides;

            double lht = -height / 2;
            for (int num = 0; num < 2; ++num)
            {
                for (int i = 0; i < numSides; ++i)
                {
                    double x = baseRadius * Math.Cos(i * theta);
                    double y = baseRadius * Math.Sin(i * theta);

                    Point3D pt = new Point3D(center.X + x, center.Y + y, center.Z + lht);
                    points.Add(pt);
                }
                lht = height / 2;
                if (num == 0)
                    points.Add(topCenter);
            }

            return points;
        }

        // Returns the connectivity information for triangles on the cylinder
        //
        public Int32Collection getCyinderTriangleIndices(int numSides)
        {

            Int32Collection indices = new Int32Collection();
            Int32Collection bottomFaceIndices = new Int32Collection();

            // Bottom face triangles' indices
            for (int i = 0; i < numSides; ++i)
            {
                indices.Add(0);
                indices.Add(((i + 2) % (numSides)) + 1);
                indices.Add(((i + 1) % (numSides)) + 1);

                bottomFaceIndices.Add(0);
                bottomFaceIndices.Add(((i + 1) % (numSides)) + 1);
                bottomFaceIndices.Add(((i + 2) % (numSides)) + 1);
            }

            // Top face triangles' indices
            int indlen = indices.Count();
            for (int i = 0; i < indlen; ++i)
            {
                indices.Add(bottomFaceIndices[i] + numSides + 1);
            }

            // Lateral triangles' indices
            indlen = indices.Count();
            for (int i = 0; i < numSides; ++i)
            {
                int id1 = i + 1;
                int id2 = ((i + 1) % numSides) + 1;

                int id3 = id1 + numSides + 1;
                int id4 = id2 + numSides + 1;

                indices.Add(id1);
                indices.Add(id2);
                indices.Add(id3);

                indices.Add(id2);
                indices.Add(id4);
                indices.Add(id3);
            }

            return indices;
        }

        public Point3DCollection getGeometryPositions(int nSides, double height, int bRadius, int boxSize)
        {
            // get cylinder positions
            Point3DCollection points = getCylinderPositions(nSides, height, bRadius);

            // get box positions
            Point3DCollection boxPts = getBoxPositions(new Point3D(0, 0, 0), height / 2, boxSize);

            // append the box positions to cylinder positions container
            foreach (Point3D pos in boxPts)
            {
                points.Add(pos);
            }

            return points;
        }

        public Int32Collection getGeometryIndices(int nSides)
        {
            // Get the triangle indices of Cylinder
            Int32Collection indices = getCyinderTriangleIndices(nSides);

            // Get the triangle indices of Box
            Int32Collection boxIndices = getBoxTriangleIndices(2 * nSides + 2);

            // concatenate the indices
            foreach (Int32 id in boxIndices)
            {
                indices.Add(id);
            }

            return indices;
        }

    }
}
