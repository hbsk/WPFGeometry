using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BuildGeometry
{
    public abstract class FileReader
    {
        protected IGeometryData fileData;
        protected FileStream fStream;
        protected string filename;

        public FileReader(string fname, IGeometryData geomData)
        {
            filename = fname;
            fileData = geomData;
        }

        public IGeometryData IGeometryData
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public abstract IGeometryData getParsedData();

        private bool CanOpenFile(string filename)
        {
            if (!File.Exists(filename))
                return false;

            return true;
        }

        public bool OpenFile(string filename)
        {
            if ( !CanOpenFile(filename) )
            {         
                return false;
            }

            fStream = File.Open(filename, FileMode.Open);
            return true;
        }

        public virtual void ReadFile()
        {
            throw new System.NotImplementedException();
        }

        public void CloseFile()
        {
            fStream.Close();
        }
    }
}