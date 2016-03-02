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
                mFileManager.mTmpFileList.CollectionChanged += TmpFileListOnCollectionChanged;
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

        public string CopyImageToTmp(Bitmap bmp, ImageFormat format)
        {
            var path = GenerateTmp(format);
            bmp.Save(path);
            return path;
        }

        public string GenerateTmp(ImageFormat format)
        {
            return GenerateTmp(format.ToString());
        }

        public string GenerateTmp(string extension)
        {
            var path = Path.GetTempFileName();
            var newPath = Path.ChangeExtension(path, extension);
            File.Move(path, newPath);
            mTmpFileList.Add(newPath);
            return newPath;
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
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

        public void Exit()
        {
            mFileManager.mTmpFileList.CollectionChanged -= TmpFileListOnCollectionChanged;
            ClearTmpFiles();
        }
    }
}