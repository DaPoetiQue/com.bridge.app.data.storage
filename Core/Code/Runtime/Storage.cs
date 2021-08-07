using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Bridge.Core.Debug;
using Bridge.Core.App.Events;

namespace Bridge.Core.App.Data.Storage
{
    /// <summary>
    /// This is the main class responsible for saving and loading data and assets.
    /// </summary>
    public static class Storage
    {
        #region Global Save

        /// <summary>
        /// Saves data to the defined storage data info's directories.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storageDataInfo"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public static void SaveJsonFile<T>(StorageData.DirectoryInfoData storageDataInfo, T data, Action<StorageData.DirectoryInfoData, AppEventsData.CallBackResults> callback = null)
        {
            JsonData.Save(storageDataInfo, data, (directoryInfoResults, callBackResults) => 
            {
                callback.Invoke(directoryInfoResults, callBackResults);
            });
        }

        #endregion

        #region Main Storage

        /// <summary>
        /// This class holds functions for saving data using Binary formatters.
        /// </summary>
        public static class BinaryFormatJasonData
        {
            #region Serializations

            /// <summary>
            /// Saves app data to a file system using a binary file.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="data"></param>
            /// <param name="callback"></param>
            public static void Save<T>(StorageData.DirectoryInfoData storageDataInfo, T data, Action<AppEventsData.CallBackResults> callback = null)
            {
                try
                {
                    Directory.GetDataPath(storageDataInfo, (storageDataResults) =>
                    {
                        var callBackResults = new AppEventsData.CallBackResults();

                        if (System.IO.Directory.Exists(storageDataResults.fileDirectory) == false)
                        {
                            System.IO.Directory.CreateDirectory(storageDataResults.fileDirectory);
                        }

                        if (System.IO.Directory.Exists(storageDataResults.fileDirectory) == true)
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            FileStream stream = new FileStream(storageDataResults.filePath, FileMode.Open, FileAccess.Write);

                            bf.Serialize(stream, data);
                            stream.Close();

                            if (File.Exists(storageDataResults.filePath) == true)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"[Storage] <color=green>Success</color> <color=white>- Data file :</color> <color=cyan>{storageDataResults.fileName}</color> <color=white>Replaced Successfully at path :</color> <color=cyan>{storageDataResults.folderName}</color>";
                            }

                            if (File.Exists(storageDataResults.filePath) == false)
                            {
                                callBackResults.error = true;
                                callBackResults.errorValue = $"[Storage] <color=red>File write failed</color> <color=white>-Couldn't write file :</color> <color=cyan>{storageDataResults.fileName}</color> <color=white>, to path :</color> <color=orange>{storageDataResults.filePath}</color>";
                            }
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"[Storage] <color=red>File write failed</color> <color=white>-Couldn't write file :</color> <color=cyan>{storageDataResults.fileName}</color> <color=white>, to path :</color> <color=orange>{storageDataResults.filePath}</color>, <color=white>File storage directory not found.</color>";
                        }

                        callback.Invoke(callBackResults);
                    });
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Save Failed Exception</color>- <color=white>Storage file save failed with exception message : </color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            /// Loads app data from a file system using a binary file. 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="serializationData"></param>
            /// <param name="callback"></param>
            public static void Load<T>(StorageData.DirectoryInfoData storageDataInfo, Action<T, AppEventsData.CallBackResults> callback = null)
            {
                try
                {
                    Directory.GetDataPath(storageDataInfo, (storageDataResults) =>
                    {
                        var callBackResults = new AppEventsData.CallBackResults();

                        if (File.Exists(storageDataResults.filePath) == true)
                        {
                            callBackResults.success = true;
                            callBackResults.successValue = $"[Storage] <color=green>Load Data Success</color> <color=white>- Stoarge data file :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>has been loaded Successfully form path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"[Storage] <color=red>File Load Failed</color> <color=white>-The system couldn't find a file to read at path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                        }

                        if (callBackResults.success == true)
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            FileStream stream = new FileStream(storageDataResults.filePath, FileMode.Open, FileAccess.Read);
                            T loadedResults = (T)formatter.Deserialize(stream);

                            callback.Invoke(loadedResults, callBackResults);
                        }
                    });
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Load Data Failed</color>- <color=white>File failed to load with exception message : </color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            #endregion
        }

        /// <summary>
        /// This class holds functions for saving data using json.
        /// </summary>
        public static class JsonData
        {
            #region Json Utility Data Serializations

            /// <summary>
            /// Saves app data to a file system using a json file.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="data"></param>
            /// <param name="callback"></param>
            public static void Save<T>(StorageData.DirectoryInfoData storageDataInfo, T data, Action<StorageData.DirectoryInfoData, AppEventsData.CallBackResults> callback = null)
            {
                try
                {
                    Directory.GetDataPath(storageDataInfo, (storageDataResults) =>
                    {
                        var callBackResults = new AppEventsData.CallBackResults();

                        if (System.IO.Directory.Exists(storageDataResults.fileDirectory) == false)
                        {
                            System.IO.Directory.CreateDirectory(storageDataResults.fileDirectory);
                        }

                        if (System.IO.Directory.Exists(storageDataResults.fileDirectory) == true)
                        {
                            string jsonStringData = JsonUtility.ToJson(data);
                            File.WriteAllText(storageDataResults.filePath, jsonStringData);

                            if (File.Exists(storageDataResults.filePath) == true)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"[Storage] Data file : <color=cyan>{storageDataResults.fileName}</color> <color=white>Has been successfully replaced at path :</color> <color=orange>{storageDataResults.filePath}</color>";
                            }
                            else
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"[Storage] Data file : <color=cyan>{storageDataResults.fileName}</color> <color=white>Created successfully at path :</color> <color=orange>{storageDataResults.filePath}</color>";
                            }

                            if(File.Exists(storageDataResults.filePath) == false)
                            {
                                callBackResults.error = true;
                                callBackResults.errorValue = $"[Storage] File write failed Couldn't write file : <color=cyan>{storageDataResults.fileName}</color> <color=white>, to path :</color> <color=orange>{storageDataResults.filePath}</color>";
                            }
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"[Storage] File write failed - Couldn't write file : <color=cyan>{storageDataResults.fileName}</color> <color=white>, to path :</color> <color=orange>{storageDataResults.filePath}</color>, <color=white>File storage directory not found.</color>";
                        }

                        callback.Invoke(storageDataResults, callBackResults);
                    });
                }
                catch(Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] Save Failed Exception - Storage file save failed with exception message : <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            /// Loads app data from a file system using a json file. 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="storageDataInfo"></param>
            /// <param name="serializationData"></param>
            /// <param name="callback"></param>
            public static void Load<T>(StorageData.DirectoryInfoData storageDataInfo, Action<T, AppEventsData.CallBackResults> callback = null)
            {
                try
                {
                    Directory.GetDataPath(storageDataInfo, (storageDataResults) =>
                    {
                        var callBackResults = new AppEventsData.CallBackResults();

                        if (File.Exists(storageDataResults.filePath) == false)
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"[Storage] <color=red>File Load Failed</color> <color=white>-The system couldn't find a file to read at path :</color> <color=cyan>{storageDataInfo.filePath}</color>";

                            callBackResults.success = false;
                        }

                        if (File.Exists(storageDataResults.filePath) == true)
                        {
                            callBackResults.success = true;
                            callBackResults.successValue = $"[Storage] <color=green>Load Data Success</color> <color=white>- Stoarge data file :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>has been loaded Successfully form path :</color> <color=cyan>{storageDataInfo.filePath}</color>";

                            callBackResults.error = false;
                        }

                        storageDataInfo.jsonStringFileData = File.ReadAllText(storageDataResults.filePath);
                        T loadedResults = JsonUtility.FromJson<T>(storageDataInfo.jsonStringFileData);

                        callback.Invoke(loadedResults, callBackResults);
                    });
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Load Data Failed</color>- <color=white>File failed to load with exception message : </color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            #endregion
        }

        /// <summary>
        /// This class Holds functions for loading scene assets.
        /// </summary>
        public static class AssetData
        {
            public static void CreateSceneAsset(UnityEngine.Object sceneObject, StorageData.DirectoryInfoData directoryInfo, Action<AppEventsData.CallBackResults> callBack = null)
            {
                try
                {

                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage-Asset Data] <color=red>Create Asset Exception</color>- <color=white>Asset file : </color><color=cyan>{directoryInfo.fileName}</color> <color=white>failed to create at path : </color> <color=cyan>{directoryInfo.filePath}</color> <color=white>, with exception message : </color> <color=red>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            /// Loads a scene item from a given path.
            /// </summary>
            /// <param name="storageAssetPathInfo"></param>
            /// <param name="callBack"></param>
            public static void LoadSceneAsset(StorageData.DirectoryInfoData storageAssetPathInfo, Action<UnityEngine.Object, AppEventsData.CallBackResults> callBack = null)
            {
                try
                {
                    var results = new AppEventsData.CallBackResults();

                    Directory.AssetPathExists(storageAssetPathInfo, callBackResults => 
                    {
                        if(callBackResults.success)
                        {
                            Transform parentObject = null;

                            if (AssetDatabase.IsMainAssetAtPathLoaded(storageAssetPathInfo.assetPath) == true)
                            {
                                Transform parentObjectResults = AssetDatabase.LoadAssetAtPath<Transform>(storageAssetPathInfo.assetPath);

                                if (parentObjectResults != null)
                                {
                                    parentObject = parentObjectResults;
                                }

                                if (parentObjectResults != null)
                                {
                                    results.error = true;
                                    results.errorValue = $"[Storage] <color=red>Load Scene Asset Failed</color>- <color=white>The scene asset path</color> <color=cyan>{storageAssetPathInfo.assetPath}</color> <color=white>doesn't exist in the list of assets available.</color>";
                                }
                            }
                            else
                            {
                                results.error = true;
                                results.errorValue = $"[Storage] <color=red>Load Scene Asset Failed</color>- <color=white>The scene asset path</color> <color=cyan>{storageAssetPathInfo.assetPath}</color> <color=white>doesn't exist in the list of assets available.</color>";
                            }

                            if (parentObject != null)
                            {
                                results.success = true;
                                results.successValue = $"[Storage] <color=green>Success</color> <color=white>-Scene Asset : <color=cyan>{parentObject.name}'s</color> <color=white> was loaded successfully form path : </color> <color=orange>{storageAssetPathInfo.assetPath}</color>";
                            }

                            callBack.Invoke(parentObject, results);

                            DebugConsole.Log(LogLevel.Error, callBackResults.successValue);
                        }
                    });

                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage Exeception] <color=red>Get Asset Path Exception</color> <color=white>-Asset file :</color> <color=cyan>{storageAssetPathInfo.fileName}</color> <color=white>failed to load game object at path :</color> <color=orange>{storageAssetPathInfo.assetPath}</color> <color=white>with exception message : </color><color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }
        }

        /// <summary>
        /// This class Holds a function to get data file path.
        /// </summary>
        public static class Directory
        {
            #region Storage Directories

            /// <summary>
            /// This function gets the path to the storage.
            /// </summary>
            /// <param name="directoryInfo"></param>
            /// <param name="callback"></param>
            public static void GetDataPath(StorageData.DirectoryInfoData directoryInfo, Action<StorageData.DirectoryInfoData> callback = null)
            {
                try
                {
                    if (string.IsNullOrEmpty(directoryInfo.fileName) || string.IsNullOrEmpty(directoryInfo.folderName))
                    {
                        throw new NullReferenceException("[Storage] <color=red>Null Exception</color> <color=white>: Storage data info</color> <color=cyan>[File Name / Folder Name]</color> <color=white>can't be null.</color>");
                    }

                    directoryInfo.fileName = directoryInfo.fileName.Contains($".{GetFileExtensionType(directoryInfo.extensionType)}") ? directoryInfo.fileName : directoryInfo.fileName + $".{GetFileExtensionType(directoryInfo.extensionType)}";
                    directoryInfo.fileDirectory = Path.Combine(Application.persistentDataPath, directoryInfo.folderName);
                    directoryInfo.filePath = Path.Combine(directoryInfo.fileDirectory, directoryInfo.fileName);

                    callback.Invoke(directoryInfo);
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Get Path Exception </color><color=white>- Failed to get storage data. Exception message :</color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            /// Gets an asset path.
            /// </summary>
            /// <param name="sceneAssetObject"></param>
            /// <param name="callBack"></param>
            public static void GetAssetPath(UnityEngine.Object sceneAssetObject, Action<StorageData.DirectoryInfoData, AppEventsData.CallBackResults> callBack = null)
            {
                try
                {
                    var storageData = new StorageData.DirectoryInfoData();
                    var results = new AppEventsData.CallBackResults();

                    if (sceneAssetObject == null)
                    {
                        results.error = true;
                        results.errorValue = $"[Storage] Get Asset Path Failed - Asset/Object is not found/assigned";

                        results.success = false;
                    }

                    if (sceneAssetObject != null)
                    {
                        storageData.assetPath = AssetDatabase.GetAssetOrScenePath(sceneAssetObject);

                        if (string.IsNullOrEmpty(storageData.assetPath) == true)
                        {
                            results.error = true;
                            results.errorValue = $"[Storage] Get Asset Path Failed - Asset/Object path is null/not found/not assigned";

                            results.success = false;
                        }

                        if (string.IsNullOrEmpty(storageData.assetPath) == false)
                        {
                            storageData.assetGUID = AssetDatabase.GUIDFromAssetPath(storageData.assetPath);

                            if (storageData.assetGUID == null)
                            {
                                results.error = true;
                                results.errorValue = $"[Storage] Get Asset GUID Failed - Asset/Object path couldn't convert from asset path to guid";

                                results.success = false;
                            }

                            if (storageData.assetGUID != null)
                            {
                                results.success = true;
                                results.successValue = $"[Storage] Asset file : <color=cyan>{sceneAssetObject.name}</color><color=white>'s path : </color> <color=orange>{storageData.assetPath}</color>";

                                results.error = false;
                            }
                        }
                    }

                    callBack.Invoke(storageData, results);
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Get Asset Path Exception</color>- <color=white>Asset file : {sceneAssetObject.name} failed to get asset path with exception message : </color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            /// Checks if a file exists in a given directory.
            /// </summary>
            /// <param name="directoryInfo"></param>
            /// <param name="callback"></param>
            public static void DataPathExists(StorageData.DirectoryInfoData directoryInfo, Action<StorageData.DirectoryInfoData, AppEventsData.CallBackResults> callback = null)
            {
                try
                {
                    Directory.GetDataPath(directoryInfo, (storageDataResults) =>
                    {
                        var callBackResults = new AppEventsData.CallBackResults();

                        if (File.Exists(storageDataResults.filePath) == true)
                        {
                            callBackResults.success = true;
                            callBackResults.successValue = $"[Storage] <color=green>Success</color> <color=white>- File :</color> <color=cyan>{directoryInfo.fileName}</color> <color=white>exists at path :</color> <color=cyan>{directoryInfo.filePath}</color>";
                        }

                        if (File.Exists(storageDataResults.filePath) == false)
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"[Storage] <color=red>Get File Failed</color> <color=white>-There is no file to get at path :</color> <color=cyan>{directoryInfo.filePath}</color>";
                        }

                        callback.Invoke(storageDataResults, callBackResults);
                    });
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Get File Failed</color>- <color=white>Failed to access file :</color> <color=cyan>{directoryInfo.fileName}</color> <color=white>with exception message : </color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            ///  This function checks if scene assets path exists.
            ///  Gets all the asset path.
            /// </summary>
            /// <param name="assetPath"></param>
            /// <param name="callBack"></param>
            public static bool AssetPathExists(string assetPath)
            {
                try
                {
                    string[] pathFilter = AssetDatabase.GetAllAssetPaths();
                    return pathFilter.Contains(assetPath);
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Scene Asset Exist Exception </color><color=white>- Failed to check if scene asset path exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            ///  This function checks if scene assets path exists.
            ///  Gets all the asset path.
            /// </summary>
            /// <param name="assetPath"></param>
            /// <param name="callBack"></param>
            public static void AssetPathExists(string assetPath, Action<AppEventsData.CallBackResults> callBack = null)
            {
                try
                {
                    var callBackResults = new AppEventsData.CallBackResults();

                    string[] pathFilter = AssetDatabase.GetAllAssetPaths();

                    if (pathFilter.Contains(assetPath) == true)
                    {
                        callBackResults.success = true;
                        callBackResults.successValue = $"-->> <color=white>[Storage]</color><color=green>Check Scene Asset Success</color> <color=white>- Scene asset path :</color> <color=cyan>{assetPath}</color> <color=white>has been found Successfully at path :</color> <color=cyan>{assetPath}</color>";
                    }

                    if (pathFilter.Contains(assetPath) == false)
                    {
                        callBackResults.error = true;
                        callBackResults.errorValue = $"-->> <color=white>[Storage]</color><color=red>Check Asset Path Exists Failed</color> <color=white>-Failed to check if scene asset path exist or not at path :</color> <color=cyan>{assetPath}</color>";
                    }

                    callBack.Invoke(callBackResults);
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage]</color> Scene Asset Exist Exception </color><color=white>- Failed to check if scene asset path exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            ///  This function checks if scene assets path exists.
            ///  Gets all the asset path.
            /// </summary>
            /// <param name="directoryInfo"></param>
            /// <param name="callBack"></param>
            public static void AssetPathExists(StorageData.DirectoryInfoData directoryInfo, Action<AppEventsData.CallBackResults> callBack = null)
            {
                try
                {
                    var callBackResults = new AppEventsData.CallBackResults();

                    string[] pathFilter = AssetDatabase.GetAllAssetPaths();

                    if (pathFilter.Contains(directoryInfo.assetPath) == true)
                    {
                        callBackResults.success = true;
                        callBackResults.successValue = $"-->> <color=white>[Storage]</color><color=green>Check Scene Asset Success</color> <color=white>- Scene asset path :</color> <color=cyan>{directoryInfo.fileName}</color> <color=white>has been found Successfully at path :</color> <color=cyan>{directoryInfo.assetPath}</color>";
                    }

                    if (pathFilter.Contains(directoryInfo.assetPath) == false)
                    {
                        callBackResults.error = true;
                        callBackResults.errorValue = $"-->> <color=white>[Storage]</color><color=red>Check Asset Path Exists Failed</color> <color=white>-Failed to check if scene asset path exist or not at path :</color> <color=cyan>{directoryInfo.assetPath}</color>";
                    }

                    callBack.Invoke(callBackResults);
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Scene Asset Exist Exception </color><color=white>- Failed to check if scene asset path exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            /// Converts the file type enum to a string.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private static string GetFileExtensionType(StorageData.ExtensionType type)
            {
                return type.ToString().ToLowerInvariant();
            }

            #endregion

            #region Disposables

            /// <summary>
            /// Removes a file from a specified directory.
            /// </summary>
            /// <param name="storageDataInfo"></param>
            /// <param name="callback"></param>
            public static void DeleteFile(StorageData.DirectoryInfoData storageDataInfo, Action<AppEventsData.CallBackResults> callback = null)
            {
                try
                {
                    Directory.GetDataPath(storageDataInfo, (storageDataResults) =>
                    {
                        var callBackResults = new AppEventsData.CallBackResults();

                        if (File.Exists(storageDataResults.filePath) == true)
                        {
                            File.Delete(storageDataResults.filePath);

                            if (File.Exists(storageDataResults.filePath) == false)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"[Storage] <color=green>Load Data Success</color> <color=white>- Stoarge data file :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>has been loaded Successfully form path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                            }
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"[Storage] <color=red>File Load Failed</color> <color=white>-There is no file to read at path :</color> <color=cyan>{storageDataInfo.filePath}</color>";
                        }

                        callback.Invoke(callBackResults);
                    });
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Delete File Failed</color>- <color=white>File failed to delete with exception message : </color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            /// <summary>
            /// Removes a specific directory from a given path. 
            /// </summary>
            /// <param name="storageDataInfo"></param>
            /// <param name="callback"></param>
            public static void DeleteDirectory(StorageData.DirectoryInfoData storageDataInfo, Action<AppEventsData.CallBackResults> callback = null)
            {
                try
                {
                    Directory.GetDataPath(storageDataInfo, (storageDataResults) =>
                    {
                        var callBackResults = new AppEventsData.CallBackResults();

                        if (System.IO.Directory.Exists(storageDataResults.fileDirectory) == true)
                        {
                            storageDataInfo.directorDataFileList = System.IO.Directory.GetFiles(storageDataResults.fileDirectory);

                            foreach (var dataFile in storageDataInfo.directorDataFileList)
                            {
                                File.Delete(dataFile);
                            }

                            System.IO.Directory.Delete(storageDataResults.fileDirectory);

                            if (System.IO.Directory.Exists(storageDataResults.fileDirectory) == false)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"-[Storage] <color=green>Delete Data Directory Success</color> <color=white>- Storage data director :</color> <color=cyan>{storageDataInfo.fileDirectory}</color> <color=white>has been deleted Successfully.</color>";
                            }

                            if (System.IO.Directory.Exists(storageDataResults.fileDirectory) == true)
                            {
                                callBackResults.error = true;
                                callBackResults.errorValue = $"[Storage] <color=red>Delete Data Directory Failed</color> <color=white>-Couldn't dleted directory :</color> <color=cyan>{storageDataInfo.fileDirectory}</color>";
                            }
                        }
                        else
                        {
                            callBackResults.error = true;
                            callBackResults.errorValue = $"[Storage] <color=red>Delete Data Directory Failed</color> <color=white>-There is no Director to remove at path :</color> <color=cyan>{storageDataInfo.fileDirectory}</color>";
                        }

                        callback.Invoke(callBackResults);
                    });
                }
                catch (Exception exception)
                {
                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Delete Directory Failed</color>- <color=white>Directory failed to delete with exception message : </color> <color=cyan>{exception.Message}</color>");
                    throw exception;
                }
            }

            #endregion
        }

        #endregion
    }
}
