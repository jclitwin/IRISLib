using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRISLib
{
    public class FileManager
    {
        protected static byte[] _bytes;
        protected static int _size;

        public static void OpenFile(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // Read the source file into a byte array.
                    _bytes = new byte[fs.Length];
                    _size = (int)fs.Length;

                    int numBytesToRead = _size;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fs.Read(_bytes, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = _bytes.Length;
                }
            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
        }

        public static void SaveFile(string path, byte[] buffer)
        {
            if (buffer.Length == 0)
                return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(path, (int)buffer.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = buffer;

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
    }
}
