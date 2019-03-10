using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKo.Model.General
{
    public interface IFile
    {
        Entity Entity { get; set; }
        byte[] Data { get; set; }
        string Extension { get; set; }
    }

    public class File : Entity,IFile
    {
        #region Properties

        [NotMapped]
        public Entity Entity { get; set; }

        public byte[] Data { get; set; }

        public string Extension { get; set; }

        FileInfo FileInfo { get; set; }

        public string Base64String
        {
            get
            {
                return Convert.ToBase64String(Data);
            }
        }

        public double LengthAsGb
        {
            get
            {
                return Convert.ToDouble(string.Format("{0:0.0000}", (double)LengthAsMb / (double)1024));
            }
        }

        public double LengthAsMb
        {
            get
            {
                return Convert.ToDouble(String.Format("{0:0.0000}", (double)LengthAsKb / (double)1024));
            }
        }

        public double LengthAsKb
        {
            get
            {
                return Convert.ToDouble(String.Format("{0:0.0000}", (double)LengthAsByte / (double)1024));
            }
        }

        public double LengthAsByte
        {
            get
            {
                return Convert.ToDouble(String.Format("{0:0.0000}", (double)FileInfo.Length));
            }
        }

        public static string CurrentUserDesktopPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }

        public static string CurrentProjectBinPath
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        #endregion

        #region Constructor

        public File(string path)
        {
            SetData(path);
        }

        public File(byte[] data)
        {
            SetData(data);
        }

        public File() { }

        #endregion

        #region Functions

        private File SetData(string path)
        {
            try
            {
                if (!Exists(path))
                    throw new Exception(string.Format("The file : '{0}' doesnt exists", path));

                Data = System.IO.File.ReadAllBytes(path);

                Extension = System.IO.Path.GetExtension(path);

                Name = Path.GetFileNameWithoutExtension(path);

                FileInfo = new FileInfo(path);
            }
            catch (Exception ex)
            {
                Log.Create(ex.Message,LogType.Error,Id);
            }

            return this;
        }

        private File SetData(byte[] data)
        {
            Data = data;

            return this;
        }

        public static string CombinePath(params string[] paths)
        {
            var newPath = "";

            foreach (var path in paths)
            {
                newPath = System.IO.Path.Combine(newPath, path);
            }

            return newPath;
        }


        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }


        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                    CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static byte[] Compress(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            return Compress(bytes);
        }


        public static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static void WriteCompressedData(byte[] compressedData)
        {
            System.IO.File.WriteAllBytes(string.Format("{0}-Compressed.gz", DateTime.Now.ToString("[yyyy-MM-dd HH-mm-ss-fff]")), compressedData);
        }

        public static List<string> DirectoryGetFiles(string path)
        {
            var list = new List<string>();

            foreach (string file in Directory.GetFiles(path))
            {
                list.Add(file);
            }

            return list;
        }

        public static bool HasInvalidPathChar(string path)
        {
            return Path.GetInvalidPathChars().ToList().Any(c => path.Contains(c));
        }

        #endregion
    }
}
