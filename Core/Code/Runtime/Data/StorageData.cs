namespace Bridge.App.Serializations.Manager
{
    public class StorageData
    {
        public class DirectoryInfoData
        {
            public string fileName;
            public string folderName;
            public string filePath;
            public string fileDirectory;
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
