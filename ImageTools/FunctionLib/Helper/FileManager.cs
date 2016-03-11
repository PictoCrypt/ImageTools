using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            if (mFileManager == null)
            {
                mFileManager = new FileManager();
                //mFileManager.mTmpFileList.CollectionChanged += TmpFileListOnCollectionChanged;
            }
            return mFileManager;
        }

        private static void TmpFileListOnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var collection = sender as ObservableCollection<string>;
            foreach (var item in collection)
            {
                if (!IsFileLocked(new FileInfo(item)))
                {
                    File.Delete(item);
                }
            }
        }

        public string CopyImageToTmp(string path)
        {
            string result;
            using (var bmp = new Bitmap(path))
            {
                result = CopyImageToTmp(bmp, bmp.RawFormat);
            }
            return result;
        }

        public string CopyImageToTmp(Bitmap bmp, ImageFormat format)
        {
            //TODO Umbauen als USING bmp..?
            var path = GenerateTmp(format);
            bmp.Save(path);
            return path;
        }

        public string GenerateTmp(ImageFormat format)
        {
            return GenerateTmp(format.ToString());
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
            //mFileManager.mTmpFileList.CollectionChanged -= TmpFileListOnCollectionChanged;
            ClearTmpFiles();
        }
    }
}