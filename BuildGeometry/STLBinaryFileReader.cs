using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace BuildGeometry
{
    public class STLBinaryFileReader : STLFileReader
    {
        private BinaryReader bFileReader;
        public STLBinaryFileReader(string fName, IGeometryData fGeomData) : base(fName, fGeomData)
        {
        }

        private Point3D getPoint(BinaryReader bReader)
        {
            Point3D pt = new Point3D();

            pt.X = bReader.ReadSingle();
            pt.Y = bReader.ReadSingle();
            pt.Z = bReader.ReadSingle();

            return pt;
        }

        private Vector3D getVector(BinaryReader bReader)
        {
            Vector3D pt = new Vector3D();

            pt.X = bReader.ReadSingle();
            pt.Y = bReader.ReadSingle();
            pt.Z = bReader.ReadSingle();

            return pt;
        }

        public override void ReadFile()
        {
            byte[] headBytes = bFileReader.ReadBytes(80);
            UInt32 numTriangles = bFileReader.ReadUInt32();

            if (numTriangles == 0)
                return;

            int idx = 0;
            for (UInt32 i = 0; i < numTriangles; ++i)
            {
                Vector3D nm = getVector(bFileReader);
                fileData.AddNormal(nm);

                Point3D p1 = getPoint(bFileReader);
                Point3D p2 = getPoint(bFileReader);
                Point3D p3 = getPoint(bFileReader);

                fileData.AddPoint(p1);
                fileData.AddPoint(p2);
                fileData.AddPoint(p3);

                fileData.AddTriangleIndices(idx, idx + 1, idx + 2);
                idx += 3;

                byte[] atr;
                atr = bFileReader.ReadBytes(2); // discard as this is not being used

                //int tt = 0; // just to add break point..
            }
        }


        public override IGeometryData getParsedData()
        {
            if (!OpenFile(filename))
                return null;

            bFileReader = new BinaryReader(fStream);
            ReadFile();
            CloseFile();

            return fileData;
        }

    }
}
