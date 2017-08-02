using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildGeometry
{
    public class OBJFileReader : FileReader
    {
        public OBJFileReader(string filename, IGeometryData fGeomData) : base(filename, fGeomData)
        {
        }

        public override IGeometryData getParsedData()
        {
            throw new System.NotImplementedException();
        }
    }
}
