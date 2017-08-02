using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BuildGeometry
{
    public class STLFileReader : FileReader
    {
        public override IGeometryData getParsedData()
        {
            throw new System.NotImplementedException();
        }

        public STLFileReader(string filename, IGeometryData fGeomData) : base(filename, fGeomData) { }
        public override void ReadFile()
        {
            throw new System.NotImplementedException();
        }
    }
}
