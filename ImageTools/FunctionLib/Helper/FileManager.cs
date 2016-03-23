using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace FunctionLib.Helper
{
    public class FileManager
    {
        private static FileManager mFileManager;
        private readonly ObservableCollection<string> mTmpFileList = new ObservableCollection<string>();

        public static FileManager GetInstance()
        {
            return mFileManager ?? (mFileManager = new FileManager());
        }

        public string CopyImageToTmp(string path)
        {
            string result;
            using (var bmp = Image.FromFile(path))
            {
                result = CopyImageToTmp(bmp);
            }
            return result;
        }

        public string CopyImageToTmp(Image bmp)
        {
            var format = GetImageFormat(bmp);
            var path = GenerateTmp(format);
            bmp.Save(path);
            return path;
        }

        public ImageFormat GetImageFormat(string path)
        {
            ImageFormat result;
            using (var img = Image.FromFile(path))
            {
                result = GetImageFormat(img);
            }
            return result;
        }

        private ImageFormat GetImageFormat(Image img)
        {
            return GetImageFormat(img.RawFormat);
        }

        public ImageFormat GetImageFormat(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (format.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (format.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (format.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (format.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (format.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (format.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (format.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp;
            if (format.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            return ImageFormat.Wmf;
        }

        public string GenerateTmp(ImageFormat format)
        {
            return GenerateTmp(GetImageFormat(format).ToString());
        }

        public string GenerateTmp(string extension = "")
        {
            var path = Path.GetTempFileName();
            var newPath = Path.ChangeExtension(path, extension);
            File.Move(path, newPath);
            mTmpFileList.Add(newPath);
            return newPath;
        }

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        private void ClearTmpFiles()
        {
            var currentFile = "";
            try
            {
                foreach (var tmpFile in mTmpFileList)
                {
                    currentFile = tmpFile;
                    File.Delete(tmpFile);
                }
            }
            catch
            {
                MessageBox.Show(string.Format("File {0} could not be deleted. Maybe it is in use.", currentFile),
                    "Error");
            }
        }

        public void CleanUp()
        {
            ClearTmpFiles();
        }
    }
}