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

            #region Data Serializations

            /// <summary>
            /// Saves app data to a file system using a json file.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="serializationData"></param>
            /// <param name="callback"></param>
            public static void Save<T>(StorageData.Info storageDataInfo, T serializationData, Action<StorageData.CallBackResults> callback = null)
            {
                if (string.IsNullOrEmpty(storageDataInfo.fileName) || string.IsNullOrEmpty(storageDataInfo.folderName))
                {
                    Debug.LogWarning("-->> Null Exception : Storage data info <color=cyan>[File Name / Folder Name]</color> can't be null.");
                    return;
                }

                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                storageDataInfo.requestType = StorageData.StorageRequestType.SaveData;

                GetPath(storageDataInfo, serializationData, (savedData, storageData, results) => 
                {
                    if (results.error == true)
                    {
                        Debug.LogWarning(results.errorValue);
                    }

                    if (results.success)
                    {
                        Debug.Log(results.successValue);
                    }

                    callback.Invoke(results);
                });
            }

            /// <summary>
            /// Loads app data from a file system using a json file. 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="serializationData"></param>
            /// <param name="callback"></param>
            public static void Load<T>(StorageData.Info storageDataInfo, T serializationData, Action<T, StorageData.CallBackResults> callback = null)
            {
                if (string.IsNullOrEmpty(storageDataInfo.fileName) || string.IsNullOrEmpty(storageDataInfo.folderName))
                {
                    Debug.LogWarning("-->> Null Exception : Storage data info <color=cyan>[File Name / Folder Name]</color> can't be null.");
                    return;
                }

                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                storageDataInfo.requestType = StorageData.StorageRequestType.LoadData;

                GetPath(storageDataInfo, serializationData, (loadedData, storageData, results) =>
                {
                    if (results.error == true)
                    {
                        Debug.LogWarning(results.errorValue);
                    }

                    if (results.success)
                    {
                        Debug.Log(results.successValue);
                    }

                    callback.Invoke(loadedData, results);
                });
            }

            #endregion

            #region Storage References and Accessors

            private static void GetPath(StorageData.Info storageDataInfo, Action<StorageData.Info, StorageData.CallBackResults> callback = null)
            {
                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                GetDirectory(storageDataInfo.requestType, storageDataInfo.folderName, (storageData, results) =>
                {
                    if (results.error == true)
                    {
                        Debug.LogWarning(results.errorValue);
                    }

                    if (results.success)
                    {
                        switch (storageDataInfo.requestType)
                        {
                            case StorageData.StorageRequestType.DeleteDataFile:

                                storageDataInfo.filePath = Path.Combine(storageData.fileDirectory, storageDataInfo.fileName);

                                if(File.Exists(storageData.filePath) == false)
                                {
                                    results.error = true;
                                    results.errorValue = $"-->> <color=red>Delete Data Failed - The file</color> : <color=cyan>{storageData.fileName}</color> <color=orange> was not found/does't exist in directory</color> : <color=cyan>{storageData.fileDirectory}</color>.";

                                    results.success = false;
                                    results.successValue = string.Empty;
                                }

                                if (File.Exists(storageData.filePath) == true)
                                {
                                    File.Delete(storageData.filePath);

                                    if(File.Exists(storageData.filePath) == true)
                                    {
                                        results.error = true;
                                        results.errorValue = $"-->> <color=red>Delete Data Failed - The file</color> : <color=cyan>{storageData.fileName}</color> <color=orange>failed to delete from directory</color> : <color=cyan>{storageData.fileDirectory}</color>.";

                                        results.success = false;
                                        results.successValue = string.Empty;
                                    }

                                    if(File.Exists(storageData.filePath) == false)
                                    {
                                        results.success = true;
                                        results.successValue = $"-->> <color=green>Success</color> - <color=white>The file</color> : <color=cyan>{storageData.fileName}</color> <color=orange>was deleted successfully from directory</color> : <color=cyan>{storageData.fileDirectory}</color>.";

                                        results.error = false;
                                        results.errorValue = string.Empty;
                                    }
                                }

                                break;

                            case StorageData.StorageRequestType.DeleteDataDirectory:

                                if(Directory.Exists(storageDataInfo.fileDirectory) == false)
                                {
                                    results.error = true;
                                    results.errorValue = $"-->> <color=red>Delete Directory Failed - Directory</color> : <color=cyan>{storageDataInfo.fileDirectory}</color> <color=orange> was not found/does't exist</color>.";

                                    results.success = false;
                                    results.successValue = string.Empty;
                                }

                                if (Directory.Exists(storageDataInfo.fileDirectory) == true)
                                {
                                    Directory.Delete(storageDataInfo.fileDirectory);

                                    if(Directory.Exists(storageDataInfo.fileDirectory) == true)
                                    {
                                        results.error = true;
                                        results.errorValue = $"-->> <color=red>Delete Directory Failed - Directory</color> : <color=cyan>{storageDataInfo.fileDirectory}</color> <color=orange> failed to delete.</color>.";

                                        results.success = false;
                                        results.successValue = string.Empty;
                                    }

                                    if (Directory.Exists(storageDataInfo.fileDirectory) == false)
                                    {
                                        results.success = true;
                                        results.successValue = $"-->> <color=green>Success</color> - <color=white>Directory</color> : <color=cyan>{storageDataInfo.fileDirectory}</color> <color=orange>was deleted successfully</color>.";

                                        results.error = false;
                                        results.errorValue = string.Empty;
                                    }
                                }

                                break;
                        }

                        callback.Invoke(storageDataInfo, results);
                    }
                });
            }

            private static void GetPath<T>(StorageData.Info storageDataInfo, T serializableData,  Action<T, StorageData.Info, StorageData.CallBackResults> callback = null)
            {
                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json")? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                GetDirectory(storageDataInfo.requestType, storageDataInfo.folderName, (storageData, results) =>
                {
                    if (results.error == true)
                    {
                        Debug.LogWarning(results.errorValue);
                    }

                    if (results.success)
                    {
                        switch(storageDataInfo.requestType)
                        {
                            case StorageData.StorageRequestType.SaveData:

                                storageDataInfo.serializedData = JsonUtility.ToJson(serializableData);
                                storageDataInfo.filePath = Path.Combine(storageData.fileDirectory, storageDataInfo.fileName);

                                if (File.Exists(storageDataInfo.filePath) == true)
                                {
                                    File.Delete(storageDataInfo.filePath);
                                }

                                File.WriteAllText(storageDataInfo.filePath, storageDataInfo.serializedData);

                                if (File.Exists(storageDataInfo.filePath) == false)
                                {
                                    results.error = true;
                                    results.errorValue = $"-->> File write failed - Couldn't write file : {storageDataInfo.fileName} to disk with path : {storageDataInfo.filePath}";

                                    results.success = false;
                                    results.successValue = string.Empty;
                                }

                                if (File.Exists(storageDataInfo.filePath) == true)
                                {
                                    results.success = true;
                                    results.successValue = $"-->> <color=green>Success</color> <color=white>- Data file :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>Replaced Successfully at path :</color> <color=cyan>{storageDataInfo.folderName}</color>";

                                    results.error = false;
                                    results.errorValue = string.Empty;
                                }

                                break;

                            case StorageData.StorageRequestType.LoadData:

                                storageDataInfo.filePath = Path.Combine(storageData.fileDirectory, storageDataInfo.fileName);

                                if (File.Exists(storageDataInfo.filePath) == false)
                                {
                                    results.error = true;
                                    results.errorValue = $"-->> File read failed - Couldn't read file : {storageDataInfo.fileName} from disk because it is missing/not found at path : {storageDataInfo.filePath}";

                                    results.success = false;
                                    results.successValue = string.Empty;
                                }

                                if(File.Exists(storageDataInfo.filePath) == true)
                                {
                                    results.success = true;
                                    results.successValue = $"-->> <color=green>Success</color> <color=white>- Data file :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>Loaded Successfully from path :</color> <color=cyan>{storageDataInfo.filePath}</color>";

                                    results.error = false;
                                    results.errorValue = string.Empty;
                                }

                                if(results.success == true)
                                {
                                    storageDataInfo.serializedData = File.ReadAllText(storageDataInfo.filePath);
                                    Debug.Log(results.successValue);
                                }

                                break;
                        }

                        T loadedData = JsonUtility.FromJson<T>(storageDataInfo.serializedData);
                        callback.Invoke(loadedData, storageData, results);
                    }
                });
            }

            private static void GetDirectory(StorageData.StorageRequestType requestType, string folderName, Action<StorageData.Info, StorageData.CallBackResults> callback)
            {
                var storageDataInfo = new StorageData.Info();
                storageDataInfo.fileDirectory = Path.Combine(Application.persistentDataPath, folderName);

                var results = new StorageData.CallBackResults();

                switch (requestType)
                {
                    case StorageData.StorageRequestType.SaveData:

                        if (Directory.Exists(storageDataInfo.fileDirectory) == false)
                        {
                            Directory.CreateDirectory(storageDataInfo.fileDirectory);

                            results.error = Directory.Exists(storageDataInfo.fileDirectory) == false;
                            results.success = Directory.Exists(storageDataInfo.fileDirectory) == true;

                            if (results.error)
                            {
                                results.errorValue = $"-->> <color=red>Directory Creation Failed</color> - <color=white>Storage Directory could not be created at :</color>  <color=cyan>{storageDataInfo.fileDirectory}</color>";
                            }

                            if (results.success)
                            {
                                results.successValue = $"-->> <color=green>Success</color> - [Storage] Directory created successfully at : <color=cyan>{storageDataInfo.fileDirectory}</color>";
                            }
                        }

                        if (Directory.Exists(storageDataInfo.fileDirectory) == true)
                        {
                            results.success = true;
                            results.successValue = $"-->> <color=green>Success</color> - [Storage] Directory created successfully at : <color=cyan>{storageDataInfo.fileDirectory}</color>";

                            results.error = false;
                            results.errorValue = "Null";
                        }

                        break;

                    case StorageData.StorageRequestType.LoadData:

                        if (Directory.Exists(storageDataInfo.fileDirectory) == false)
                        {
                            results.error = true;
                            results.errorValue = $"-->> <color=red>Directory Load Failed</color> - <color=white>[Storage] Directory could not be found at :</color>  <color=cyan>{storageDataInfo.fileDirectory}</color>";
                        }

                        if (Directory.Exists(storageDataInfo.fileDirectory) == true)
                        {
                            results.success = true;
                            results.successValue = $"-->> <color=green>Success</color> - Storage Directory found at : <color=cyan>{storageDataInfo.fileDirectory}</color>";
                        }

                        break;

                    case StorageData.StorageRequestType.DeleteDataDirectory:

                        break;
                }

                callback.Invoke(storageDataInfo, results);
            }

            #endregion

            #region Disposals

            /// <summary>
            /// Removes a file from a specified directory.
            /// </summary>
            /// <param name="storageDataInfo"></param>
            /// <param name="callback"></param>
            public static void DeleteFile(StorageData.Info storageDataInfo, Action<StorageData.CallBackResults> callback = null)
            {
                if (string.IsNullOrEmpty(storageDataInfo.fileName) || string.IsNullOrEmpty(storageDataInfo.folderName))
                {
                    Debug.LogWarning("-->> Null Exception : Storage data info <color=cyan>[File Name / Folder Name]</color> can't be null.");
                    return;
                }

                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                storageDataInfo.requestType = StorageData.StorageRequestType.DeleteDataFile;

                GetPath(storageDataInfo, (storageData, results) =>
                {
                    if (results.error == true)
                    {
                        Debug.LogWarning(results.errorValue);
                    }

                    if (results.success)
                    {
                        Debug.Log(results.successValue);
                    }

                    callback.Invoke(results);
                });
            }

            /// <summary>
            /// Removes a specific directory from a given path. 
            /// </summary>
            /// <param name="storageDataInfo"></param>
            /// <param name="callback"></param>
            public static void DeleteDirectory(StorageData.Info storageDataInfo, Action<StorageData.CallBackResults> callback = null)
            {
                if (string.IsNullOrEmpty(storageDataInfo.fileName) || string.IsNullOrEmpty(storageDataInfo.folderName))
                {
                    Debug.LogWarning("-->> Null Exception : Storage data info <color=cyan>[File Name / Folder Name]</color> can't be null.");
                    return;
                }

                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                storageDataInfo.requestType = StorageData.StorageRequestType.DeleteDataDirectory;

                GetPath(storageDataInfo, (storageData, results) =>
                {
                    if (results.error == true)
                    {
                        Debug.LogWarning(results.errorValue);
                    }

                    if (results.success)
                    {
                        Debug.Log(results.successValue);
                    }

                    callback.Invoke(results);
                });
            }

            #endregion

            #endregion
        }
    }
    
}
