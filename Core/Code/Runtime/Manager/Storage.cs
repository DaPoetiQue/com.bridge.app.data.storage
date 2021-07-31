using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace Bridge.App.Serializations.Manager
{
    public class Storage
    {
        public static class BinaryFiles
        {
            #region Binary Formatter Data

            public static void Save<T>(StorageData.Info storageDataInfo, T serializationData, Action<StorageData.CallBackResults> callback = null)
            {
                if (string.IsNullOrEmpty(storageDataInfo.fileName) || string.IsNullOrEmpty(storageDataInfo.folderName))
                {
                    var results = new StorageData.CallBackResults();

                    results.success = false;
                    results.error = true;
                    results.errorValue = "-->> Null Exception : Storage data info <color=cyan>[File Name / Folder Name]</color> can't be null.";

                    callback.Invoke(results);
                    return;
                }

                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                //GetDirectory(storageDataInfo.folderName, (dataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange>Data Serialization Failed - </color> <color=white>Serialization Data Directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                //        return;
                //    }

                //    string serializedData = JsonUtility.ToJson(serializationData);
                //    storageDataInfo.path = Path.Combine(storageDataInfo.directory, dataInfo.fileName);

                //    if (File.Exists(storageDataInfo.path) == true)
                //    {
                //        File.Delete(storageDataInfo.path);
                //    }

                //    File.WriteAllText(storageDataInfo.path, serializedData);
                //    callback.Invoke(storageDataInfo.path, File.Exists(storageDataInfo.path));
                //});
            }

            public static void Load<T>(StorageData storageDataInfo, Action<T, bool> callback = null) where T : UnityEngine.Object
            {
                //storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                //storageDataInfo.path = Path.Combine(storageDataInfo.directory, storageDataInfo.fileName);

                //GetPath(storageDataInfo, (dataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange> Load AR Root Config Failed - </color> <color=white>AR root config settings data file missing, not found at path :</color> <color=cyan>{dataInfo.path}</color>");
                //        return;
                //    }

                //    string serializedDataFile = File.ReadAllText(dataInfo.path);
                //    T data = JsonUtility.FromJson<T>(serializedDataFile);

                //    callback.Invoke(data, string.IsNullOrEmpty(serializedDataFile));
                //});
            }

            public static void DeleteFile(StorageData storageDataInfo, Action<string, bool> callback)
            {
                //GetPath(storageDataInfo, (serializationDataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange>Serialization data directory :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist.</color>");
                //        return;
                //    }

                //    if (File.Exists(serializationDataInfo.path) == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange>Serialization data file : </color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist inside directory : </color><color=cyan>{storageDataInfo.directory}</color>");
                //        return;
                //    }

                //    File.Delete(serializationDataInfo.path);

                //    callback.Invoke(serializationDataInfo.directory, File.Exists(serializationDataInfo.path));
                //});
            }

            public static void DeleteDirectory(string folderName, Action<string, bool> callback)
            {
                //GetDirectory(folderName, (serializationDataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange></color> <color=white>AR root config settings directory :</color> <color=cyan>{serializationDataInfo}</color> <color=white>doesn't exist.</color>");
                //        return;
                //    }

                //    Directory.Delete(serializationDataInfo.directory);
                //    callback.Invoke(serializationDataInfo.directory, Directory.Exists(serializationDataInfo.directory));
                //});
            }

            private static void GetDirectory(string folderName, Action<StorageData, bool> callback)
            {
                //StorageData directoryDataInfo = new StorageData();
                //directoryDataInfo.directory = Path.Combine(Application.persistentDataPath, folderName);

                //if (Directory.Exists(directoryDataInfo.directory) == false)
                //{
                //    Directory.CreateDirectory(directoryDataInfo.directory);
                //}

                //callback.Invoke(directoryDataInfo, Directory.Exists(directoryDataInfo.directory));
            }

            private static void GetPath(StorageData storageDataInfo, Action<StorageData, bool> callback)
            {
                //StorageData dataFileInfo = new StorageData();
                //dataFileInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                //GetDirectory(storageDataInfo.folderName, (dataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange></color> <color=white>Serialization data directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                //        return;
                //    }

                //    dataFileInfo.folderName = storageDataInfo.folderName;
                //    dataFileInfo.path = Path.Combine(dataInfo.directory, dataFileInfo.fileName); ;
                //});


                //callback.Invoke(dataFileInfo, File.Exists(dataFileInfo.path));
            }

            #endregion
        }

        public static class JsonFiles
        {
            #region Json Utility Data

            /// <summary>
            /// Saves app data to a file system using a json file.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="serializationData"></param>
            /// <param name="callback"></param>
            public static void Save<T>(StorageData.Info storageDataInfo, T serializationData, Action<StorageData.CallBackResults> callback = null)
            {
                var results = new StorageData.CallBackResults();

                if (string.IsNullOrEmpty(storageDataInfo.fileName) || string.IsNullOrEmpty(storageDataInfo.folderName))
                {
                    results.success = false;
                    results.error = true;
                    results.errorValue = "-->> Null Exception : Storage data info <color=cyan>[File Name / Folder Name]</color> can't be null.";
                }

                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                storageDataInfo.fileName = storageDataInfo.fileName.ToLowerInvariant();

                Debug.Log($"-->> File Name : {storageDataInfo.fileName}, Directory : {storageDataInfo.folderName}");

                GetDirectory(StorageData.StorageRequestType.SaveData, storageDataInfo.folderName, (results) =>
                {
                    if (results.error == true)
                    {
                        Debug.LogWarning(results.errorValue);
                    }

                    if(results.success)
                    {
                        //string serializedData = JsonUtility.ToJson(serializationData);
                        //storageDataInfo.path = Path.Combine(storageDataInfo.directory, dataInfo.fileName);

                        //if (File.Exists(storageDataInfo.path) == true)
                        //{
                        //    File.Delete(storageDataInfo.path);
                        //}

                        //File.WriteAllText(storageDataInfo.path, serializedData);

                        Debug.Log(results.successValue);
                    }

                    callback.Invoke(results);
                });
            }

            public static void Load<T>(StorageData storageDataInfo, Action<T, bool> callback = null) where T : UnityEngine.Object
            {
                //storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                //storageDataInfo.path = Path.Combine(storageDataInfo.directory, storageDataInfo.fileName);

                //GetPath(storageDataInfo, (dataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange> Load AR Root Config Failed - </color> <color=white>AR root config settings data file missing, not found at path :</color> <color=cyan>{dataInfo.path}</color>");
                //        return;
                //    }

                //    string serializedDataFile = File.ReadAllText(dataInfo.path);
                //    T data = JsonUtility.FromJson<T>(serializedDataFile);

                //    callback.Invoke(data, string.IsNullOrEmpty(serializedDataFile));
                //});
            }

            public static void DeleteFile(StorageData storageDataInfo, Action<string, bool> callback)
            {
                //GetPath(storageDataInfo, (serializationDataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange>Serialization data directory :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist.</color>");
                //        return;
                //    }

                //    if (File.Exists(serializationDataInfo.path) == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange>Serialization data file : </color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist inside directory : </color><color=cyan>{storageDataInfo.directory}</color>");
                //        return;
                //    }

                //    File.Delete(serializationDataInfo.path);

                //    callback.Invoke(serializationDataInfo.directory, File.Exists(serializationDataInfo.path));
                //});
            }

            public static void DeleteDirectory(string folderName, Action<string, bool> callback)
            {
                //GetDirectory(folderName, (serializationDataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange></color> <color=white>AR root config settings directory :</color> <color=cyan>{serializationDataInfo}</color> <color=white>doesn't exist.</color>");
                //        return;
                //    }

                //    Directory.Delete(serializationDataInfo.directory);
                //    callback.Invoke(serializationDataInfo.directory, Directory.Exists(serializationDataInfo.directory));
                //});
            }

            private static void GetDirectory(StorageData.StorageRequestType requestType, string folderName, Action<StorageData.CallBackResults> callback)
            {
                var results = new StorageData.CallBackResults();
                results.fileDirectory = Path.Combine(Application.persistentDataPath, folderName);

                switch(requestType)
                {
                    case StorageData.StorageRequestType.SaveData:

                        if (Directory.Exists(results.fileDirectory) == false)
                        {
                            Directory.CreateDirectory(results.fileDirectory);

                            results.error = Directory.Exists(results.fileDirectory) == false;
                            results.success = Directory.Exists(results.fileDirectory) == true;

                            if (results.error)
                            {
                                results.errorValue = $"-->> <color=red>Directory Creation Failed</color> - <color=white>Storage Directory could not be created at :</color>  <color=cyan>{results.fileDirectory}</color>";
                            }

                            if(results.success)
                            {
                                results.successValue = $"-->> <color=green>Success</color> - [Storage] Directory created successfully at : <color=cyan>{results.fileDirectory}</color>";
                            }
                        }

                        if(Directory.Exists(results.fileDirectory) == true)
                        {
                            results.success = true;
                            results.successValue = $"-->> <color=green>Success</color> - [Storage] Directory created successfully at : <color=cyan>{results.fileDirectory}</color>";

                            results.error = false;
                            results.errorValue = "Null";
                        }

                        break;

                    case StorageData.StorageRequestType.LoadData:

                        if (Directory.Exists(results.fileDirectory) == false)
                        {
                            results.error = true;
                            results.errorValue = $"-->> <color=red>Directory Load Failed</color> - <color=white>[Storage] Directory could not be found at :</color>  <color=cyan>{results.fileDirectory}</color>";
                        }

                        if (Directory.Exists(results.fileDirectory) == true)
                        {
                            results.success = true;
                            results.successValue = $"-->> <color=green>Success</color> - Storage Directory found at : <color=cyan>{results.fileDirectory}</color>";
                        }

                        break;

                    case StorageData.StorageRequestType.DeleteDataDirectory:

                        break;
                }

                callback.Invoke(results);
            }

            private static void GetPath(StorageData storageDataInfo, Action<StorageData, bool> callback)
            {
                //StorageData dataFileInfo = new StorageData();
                //dataFileInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                //GetDirectory(storageDataInfo.folderName, (dataInfo, exists) =>
                //{
                //    if (exists == false)
                //    {
                //        Debug.LogWarning($"-->> <color=orange></color> <color=white>Serialization data directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                //        return;
                //    }

                //    dataFileInfo.folderName = storageDataInfo.folderName;
                //    dataFileInfo.path = Path.Combine(dataInfo.directory, dataFileInfo.fileName); ;
                //});


                //callback.Invoke(dataFileInfo, File.Exists(dataFileInfo.path));
            }

            #endregion
        }
    }
    
}
