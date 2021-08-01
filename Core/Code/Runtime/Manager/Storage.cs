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
            /// <param name="data"></param>
            /// <param name="callback"></param>
            public static void Save<T>(StorageData.Info storageDataInfo, T data, Action<StorageData.CallBackResults> callback = null)
            {
                try
                {
                    GetDataFilePath(storageDataInfo, (storageDataResults) =>
                    {
                        StorageData.CallBackResults callBackResults = new StorageData.CallBackResults();

                        if (Directory.Exists(storageDataResults.fileDirectory) == false) Directory.CreateDirectory(storageDataResults.fileDirectory);

                        if (Directory.Exists(storageDataResults.fileDirectory) == true)
                        {
                            string jsonStringData = JsonUtility.ToJson(data);
                            File.WriteAllText(storageDataResults.filePath, jsonStringData);

                            if (File.Exists(storageDataResults.filePath) == false)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"-->> <color=green>Success</color> <color=white>- Data file :</color> <color=cyan>{storageDataResults.fileName}</color> <color=white>Replaced Successfully at path :</color> <color=cyan>{storageDataResults.folderName}</color>";
                            }

                            if(File.Exists(storageDataResults.filePath) == true)
                            {
                                callBackResults.error = true;
                                callBackResults.errorValue = $"-->> <color=red>File write failed</color> <color=white>-Couldn't write file :</color> <color=cyan>{storageDataResults.fileName}</color> <color=white>, to path :</color> <color=orange>{storageDataResults.filePath}</color>";
                            }
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"-->> <color=red>File write failed</color> <color=white>-Couldn't write file :</color> <color=cyan>{storageDataResults.fileName}</color> <color=white>, to path :</color> <color=orange>{storageDataResults.filePath}</color>";
                        }

                        callback.Invoke(callBackResults);
                    });
                }
                catch(Exception exception)
                {
                    Debug.LogError($"-->> <color=red>Save Failed</color>- <color=white>Storage file save failed with exception message : </color> <color=cyan>{exception.Message}</color>");
                }
            }

            /// <summary>
            /// Loads app data from a file system using a json file. 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="serializationData"></param>
            /// <param name="callback"></param>
            public static void Load<T>(StorageData.Info storageDataInfo, Action<T, StorageData.CallBackResults> callback = null)
            {
                try
                {
                    GetDataFilePath(storageDataInfo, (storageDataResults) =>
                    {
                        StorageData.CallBackResults callBackResults = new StorageData.CallBackResults();

                        if (File.Exists(storageDataResults.filePath) == true)
                        {
                            callBackResults.success = true;
                            callBackResults.successValue = $"-->> [Storage]<color=green>Load Data Success</color> <color=white>- Stoarge data file :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>has been loaded Successfully form path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"-->> [Storage]<color=red>File Load Failed</color> <color=white>-The system couldn't find a file to read at path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                        }

                        if (callBackResults.success == true)
                        {
                            storageDataInfo.jsonStringFileData = File.ReadAllText(storageDataResults.filePath);
                            T loadedResults = JsonUtility.FromJson<T>(storageDataInfo.jsonStringFileData);

                            callback.Invoke(loadedResults, callBackResults);
                        }
                    });
                }
                catch (Exception exception)
                {
                    Debug.LogError($"-->> [Storage]<color=red>Load Data Failed</color>- <color=white>File failed to load with exception message : </color> <color=cyan>{exception.Message}</color>");
                }
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
                try
                {
                    GetDataFilePath(storageDataInfo, (storageDataResults) =>
                    {
                        StorageData.CallBackResults callBackResults = new StorageData.CallBackResults();

                        if (File.Exists(storageDataResults.filePath) == true)
                        {
                            File.Delete(storageDataResults.filePath);

                            if (File.Exists(storageDataResults.filePath) == false)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"-->> [Storage]<color=green>Load Data Success</color> <color=white>- Stoarge data file :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>has been loaded Successfully form path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                            }
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"-->> [Storage]<color=red>File Load Failed</color> <color=white>-There is no file to read at path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                        }

                        callback.Invoke(callBackResults);
                    });
                }
                catch (Exception exception)
                {
                    Debug.LogError($"-->> [Storage]<color=red>Delete File Failed</color>- <color=white>File failed to delete with exception message : </color> <color=cyan>{exception.Message}</color>");
                }
            }

            /// <summary>
            /// Removes a specific directory from a given path. 
            /// </summary>
            /// <param name="storageDataInfo"></param>
            /// <param name="callback"></param>
            public static void DeleteDirectory(StorageData.Info storageDataInfo, Action<StorageData.CallBackResults> callback = null)
            {
                try
                {
                    GetDataFilePath(storageDataInfo, (storageDataResults) =>
                    {
                        StorageData.CallBackResults callBackResults = new StorageData.CallBackResults();

                        if (Directory.Exists(storageDataResults.fileDirectory) == true)
                        {
                            Directory.Delete(storageDataResults.fileDirectory);

                            if (Directory.Exists(storageDataResults.fileDirectory) == false)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"-->> [Storage]<color=green>Delete Data Directory Success</color> <color=white>- Storage data director :</color> <color=cyan>{storageDataInfo.fileDirectory}</color> <color=white>has been deleted Successfully.</color>";
                            }
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"-->> [Storage]<color=red>Delete Data Directory Failed</color> <color=white>-There is no Director to remove at path :</color> <color=cyan>{storageDataInfo.fileDirectory}</color>";
                        }

                        callback.Invoke(callBackResults);
                    });
                }
                catch (Exception exception)
                {
                    Debug.LogError($"-->> [Storage]<color=red>Delete Directory Failed</color>- <color=white>Directory failed to delete with exception message : </color> <color=cyan>{exception.Message}</color>");
                }
            }

            #endregion

            #region Storage Directories

            /// <summary>
            /// This function gets the path to the storage.
            /// </summary>
            /// <param name="storageDataInfo"></param>
            /// <param name="callback"></param>
            private static void GetDataFilePath(StorageData.Info storageDataInfo, Action<StorageData.Info> callback = null)
            {
                try
                {
                    if (string.IsNullOrEmpty(storageDataInfo.fileName) || string.IsNullOrEmpty(storageDataInfo.folderName))
                    {
                        throw new NullReferenceException("-->> <color=red>Null Exception</color> <color=white>: Storage data info</color> <color=cyan>[File Name / Folder Name]</color> <color=white>can't be null.</color>");
                    }

                    storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                    storageDataInfo.fileDirectory = Path.Combine(Application.persistentDataPath, storageDataInfo.folderName);
                    storageDataInfo.filePath = Path.Combine(storageDataInfo.fileDirectory, storageDataInfo.fileName);

                    callback.Invoke(storageDataInfo);
                }
                catch(Exception exception)
                {
                    Debug.LogError($"-->> <color=red>Get Path Exception </color><color=white>- Failed to get storage data. Exception message :</color> <color=cyan>{exception.Message}</color>");
                }
            }

            #endregion

            #endregion
        }
    }
    
}
