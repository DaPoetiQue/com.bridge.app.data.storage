namespace Bridge.App.Serializations.Manager
{
    public class StorageData
    {
        public enum StorageRequestType
        {
            SaveData, LoadData, DeleteDataFile, DeleteDataDirectory
        }

        public class Info
        {
            // Files.
            public string fileName;
            public string folderName;

            // Directory.
            public string directory;
            public string path;
        }

        public class CallBackResults
        {
            public string filePath;
            public string fileDirectory;

            public bool success;
            public string successValue;

            public bool error;
            public string errorValue;
        }
    }
}
