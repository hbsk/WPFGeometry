using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BuildGeometry
{
    public class FileFormatFactory
    {

        /// <summary>
        /// Returns the extension of the given file.
        /// </summary>
        private static string CheckFileExtension(string filename)
        {
            string exten = "";

            if (File.Exists(filename))
            {
                string[] parts = filename.Split('.');
                int len = parts.Count();

                if (len > 0)
                    if (parts[len - 1].Contains("STL") || parts[len - 1].Contains("stl"))
                        exten = "STL";
            }

            return exten;
        }

        public static bool isBinary(string filename)
        {
            // TODO: implement this function..
            //
            return true;
        }

        /// <summary>
        /// Creates and returns the appropriate FileReader object for the given file format.
        /// </summary>
        public static FileReader GetFileReader(string filename)
        {
            FileReader fileReader = null;
            if (File.Exists(filename))
            {
                string exten = CheckFileExtension(filename);
                if (exten.Contains("STL"))
                {
                    IGeometryData geomData = new Mesh3DData();

                    // now check if it is Binary or Ascii
                    // because the reader is different
                    //
                    bool bBinaryFile = true; // get this info from another function..
                    if (bBinaryFile)
                    {
                        fileReader = new STLBinaryFileReader(filename, geomData);
                    }
                    else
                    {
                        fileReader = new STLAsciiFileReader(filename, geomData);
                    }
                }
                else if (exten.Contains("OBJ"))
                {
                    // TODO: Mesh3DData may not be the right type for OBJ format !!!
                    // We need to create another mesh3D variant to handle OBJ format
                    //
                    IGeometryData geomData = new Mesh3DData();
                    fileReader = new OBJFileReader(filename, geomData);
                }
                else
                {
                    throw new System.NotImplementedException("This format is not supporteed yet...");
                }
            }

            return fileReader;
        }
    }
}