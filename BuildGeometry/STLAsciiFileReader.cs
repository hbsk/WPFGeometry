using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BuildGeometry
{
    public class STLAsciiFileReader : STLFileReader
    {
        private StreamReader sReader = null;
        public STLAsciiFileReader(string fname, IGeometryData fGeomData) : base(fname, fGeomData)
        {
        }

        public override void ReadFile()
        {
            throw new System.NotImplementedException();
        }

        public override IGeometryData getParsedData()
        {
            throw new System.NotImplementedException();
        }
    }
}
