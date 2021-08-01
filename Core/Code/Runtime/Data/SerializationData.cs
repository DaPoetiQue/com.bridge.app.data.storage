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
            public string fileName;
            public string folderName;

            public string filePath;
            public string fileDirectory;

            public StorageRequestType requestType;
            public string jsonStringFileData;
        }

        public class CallBackResults
        {
            public bool success;
            public string successValue;

            public bool error;
            public string errorValue;
        }
    }
}
