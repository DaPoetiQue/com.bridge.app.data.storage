using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Bridge.Core.Debug;
using Bridge.Core.App.Events;
using FlatBuffers;
using System.Diagnostics;

#if UNITY_EDITOR

using Bridge.Core.UnityCustomEditor.Debugger;

#endif

namespace Bridge.Core.App.Data.Storage
{
    /// <summary>
    /// This is the main class responsible for saving and loading data and assets.
    /// </summary>
    public static class Storage
    {
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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Save Failed Exception</color>- <color=white>Storage file save failed with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }

            #endregion

            #region Deserialization

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

                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Load Data Failed</color>- <color=white>File failed to load with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }

            #endregion
        }

        /// <summary>
        /// This class holds functions for saving data using Flat Buffers.
        /// </summary>
        public static class FlatBufferData
        {
            #region Serializations

            /// <summary>
            /// Saves app data to a file system using flat buffers.
            /// </summary>
            /// <typeparam name="T">Generic type for saving any type of data.</typeparam>
            /// <param name="storageDataInfo">The files that contains the storage information for the saved data file.</param>
            /// <param name="data">The data that to be serialized.</param>
            /// <param name="callback">The results returned after saving the data</param>
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
#if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Save Failed Exception</color>- <color=white>Storage file save failed with exception message : </color> <color=cyan>{exception.Message}</color>");

#endif

                    throw exception;
                }
            }

            public static void Cache(string cacheDirectory)
            {
                try
                {
                    FlatBufferBuilder cache = new FlatBufferBuilder(1);
                    StringOffset directory = cache.CreateString(cacheDirectory);

                    StorageCache.StartStorageCache(cache);
                    StorageCache.AddCacheDirectory(cache, directory);

                    var dataOffset = StorageCache.EndStorageCache(cache);
                    StorageCache.FinishStorageCacheBuffer(cache, dataOffset);

                    using(var stream = new MemoryStream(cache.DataBuffer.ToFullArray(), cache.DataBuffer.Position, cache.Offset))
                    {
                        File.WriteAllBytes(cacheDirectory, stream.ToArray());
                    }
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Save Failed Exception</color>- <color=white>Storage file save failed with exception message : </color> <color=cyan>{exception.Message}</color>");

#endif

                    throw exception;
                }
            }

            #endregion

            #region Deserialization

            public static void Load(string directory)
            {
                try
                {
                    if(!File.Exists(directory))
                    {
                        return;
                    }

                    ByteBuffer cacheData = new ByteBuffer(File.ReadAllBytes(directory));

                    if(!StorageCache.StorageCacheBufferHasIdentifier(cacheData))
                    {
                        return;
                    }

                    StorageCache data = StorageCache.GetRootAsStorageCache(cacheData);
                }
                catch (Exception exception)
                {
                    #if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Load Failed Exception</color>- <color=white>Storage file load failed with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

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

                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] Save Failed Exception - Storage file save failed with exception message : <color=cyan>{exception.Message}</color>");

                    #endif

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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Load Data Failed</color>- <color=white>File failed to load with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage-Asset Data] <color=red>Create Asset Exception</color>- <color=white>Asset file : </color><color=cyan>{directoryInfo.fileName}</color> <color=white>failed to create at path : </color> <color=cyan>{directoryInfo.filePath}</color> <color=white>, with exception message : </color> <color=red>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }

#if UNITY_EDITOR

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

                            #if UNITY_EDITOR

                                DebugConsole.Log(LogLevel.Error, callBackResults.successValue);

                            #endif


                        }
                    });

                }
                catch (Exception exception)
                {

                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage Exeception] <color=red>Get Asset Path Exception</color> <color=white>-Asset file :</color> <color=cyan>{storageAssetPathInfo.fileName}</color> <color=white>failed to load game object at path :</color> <color=orange>{storageAssetPathInfo.assetPath}</color> <color=white>with exception message : </color><color=cyan>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }

            /// <summary>
            /// This function loads an asset from a path.
            /// </summary>
            /// <param name="assetPath">The path to load the assets from.</param>
            /// <returns>Returns a an asset loaded from the given path.</returns>
            public static void LoadAsset<T>(string assetPath, Action<T, AppEventsData.CallBackResults> callBack = null) where T : UnityEngine.Object
            {
                AppEventsData.CallBackResults callBackResults = new AppEventsData.CallBackResults();

                try
                {
                    T asset = null;

                    if (Directory.AssetPathExists(assetPath))
                    {

                        if (assetPath.Length > 0)
                        {
                            asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                            if(asset != null)
                            {
                                callBackResults.success = true;
                                callBackResults.successValue = $"{asset.name} - loaded successfully from path : {assetPath}.";
                            }
                            else
                            {
                                callBackResults.error = true;
                                callBackResults.errorValue = $"Asset failed to load from path : {assetPath}.";
                            }
                        }
                    }
                    else
                    {
                        callBackResults.error = true;
                        callBackResults.errorValue = $"Asset path : {assetPath} not found.";
                    }

                    callBack.Invoke(asset, callBackResults);
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Load Assets Failed</color>- <color=white>File failed to load assets with exception message : </color> <color=cyan>{exception.Message}</color>");

#endif
                    callBackResults.error = true;
                    callBackResults.errorValue = exception.Message;
                    callBack.Invoke(null, callBackResults);
                    throw exception;
                }
            }

            /// <summary>
            /// This function loads assets from an array of paths.
            /// </summary>
            /// <param name="assetPath">The array of paths to load assets from.</param>
            /// <returns>Returns a array of assets loaded from the given paths.</returns>
            public static void LoadAssets<T>(string[] assetPath, Action<T[], AppEventsData.CallBackResults> callBack = null) where T : UnityEngine.Object
            {
                AppEventsData.CallBackResults callBackResults = new AppEventsData.CallBackResults();

                try
                {
                    T[] assets = new T[assetPath.Length];

                    if (assetPath.Length > 0)
                    {
                        for (int i = 0; i < assetPath.Length; i++)
                        {
                            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath[i]);
                        }

                        callBackResults.success = true;
                        callBackResults.successValue = $"{assets.Length} - assets loaded successfully.";
                    }
                    else
                    {
                        callBackResults.error = true;
                        callBackResults.errorValue = "There are no assets paths assigned.";
                    }

                    callBack.Invoke(assets, callBackResults);
                }
                catch(Exception exception)
                {
#if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Load Assets Failed</color>- <color=white>File failed to load assets with exception message : </color> <color=cyan>{exception.Message}</color>");

#endif
                    callBackResults.error = true;
                    callBackResults.errorValue = exception.Message;
                    callBack.Invoke(null, callBackResults);
                    throw exception;
                }
            }
#endif
        }

        /// <summary>
        /// This class Holds a function to get data file path.
        /// </summary>
        public static class Directory
        {

            #region Pre-Defined File Directories

            #region Enum Data Types

            /// <summary>
            /// A List of data type to load.
            /// </summary>
            public enum ProjectInfoDataType
            {
                AllUnityEditorData,
                UnityEditorApplicationInfoData,
                UnityEditorApplicationDirectoryData,
                UnityEditorProjectDirectoryData,
            }

            #endregion

            #region Class Data Types

            /// <summary>
            /// This class contains Unity project build directory data. 
            /// </summary>
            public class UnityEditorProjectInfoData
            {
                // <-- BUILD DIRECTORIES -->
                public string projectDirectory;
                public string projectBuildTempDirectory;
                public string projectBuildScriptCompilerDirectory;
                public string projectBuildDirectory;

                // <-- PROJECT DIRECTORIES -->
                public string localUseRootDirectory;
                public string editorApplicationDirectory;
                public string editorApplicationRelativeDirectory;
                public string unityEditorLogFilePath;
            }

            #endregion

            #region Data Info

            /// <summary>
            /// This function returns Unity Editor project directory information.
            /// </summary>
            /// <returns>Unity Editor Project Info Data</returns>
            public static UnityEditorProjectInfoData GetUnityEditorProjectInfoData(string buildScriptFolderName = null)
            {
                UnityEditorProjectInfoData infoData = new UnityEditorProjectInfoData();

                // <-- DIRECTORIES -->
                infoData.localUseRootDirectory = "C:/Program Files/";
                infoData.editorApplicationDirectory = "/Editor/Unity.exe";
                infoData.editorApplicationRelativeDirectory = "\"" + infoData.localUseRootDirectory + Application.unityVersion + infoData.editorApplicationDirectory + "\"";

                string unityEditorLogFileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string unityEditorLogFileRelativeDirectory = $"{unityEditorLogFileDirectory}\\Unity\\Editor\\Editor.log";
                infoData.unityEditorLogFilePath = unityEditorLogFileRelativeDirectory;

                infoData.projectBuildDirectory = $"{GetProjectTempDirectory()}\\Builds";
                infoData.projectBuildTempDirectory = $"\"{GetProjectTempDirectory().Replace("\\", "/")}\"";

                if (string.IsNullOrEmpty(buildScriptFolderName) == false)
                {
                    infoData.projectBuildScriptCompilerDirectory = $"\"{GetProjectTempDirectory().Replace("\\", "/")}{buildScriptFolderName}\"";
                }

                return infoData;
            }

            #endregion

            #region Formatted Directories

            /// <summary>
            /// This batch command function is used to get a formated directory with removed spacing and a backwards Slash/Solidus. 
            /// </summary>
            /// <param name="directory">Directory to be formated.</param>
            /// <returns>Batch command for formating a directory with removed spacing and a backwards Slash/Solidus.</returns>
            public static string GetFormatedDirectoryWithRemovedSpacingAndBackwardsSolidus(string directory)
            {
                return directory.Replace(" ", string.Empty).Replace("/", "\\");
            }

            /// <summary>
            /// This batch command function is used to get a formated directory with removed spacing and a forward Slash/Solidus. 
            /// </summary>
            /// <param name="directory">Directory to be formated.</param>
            /// <returns>Batch command for formating a directory with removed spacing and a forward Slash/Solidus.</returns>
            public static string GetFormatedDirectoryWithRemovedSpacingAndForwardSolidus(string directory)
            {
                return directory.Replace(" ", string.Empty).Replace("\\", "/");
            }

            /// <summary>
            /// This batch command function is used to get a formated directory with removed spacing.
            /// </summary>
            /// <param name="directory">Directory to be formated.</param>
            /// <returns>Batch command for formating a directory with removed spacing.</returns>
            public static string GetFormattedDirectoryWithRemovedSpacing(string directory)
            {
                return directory.Replace(" ", string.Empty);
            }

            /// <summary>
            /// This function reverse/inverse a forwad Slash/Solidus into a backwards Slash/Solidus.
            /// </summary>
            /// <param name="directory">The directory to be reversed/inversed forward.</param>
            /// <returns>Reversed/Inversed Forward Slash/Solidus Directory.</returns>
            public static string GetFormattedDirectoryWithBackwardsSolidus(string directory)
            {
                return directory.Replace("/", "\\"); ;
            }

            /// <summary>
            /// This function reverse/inverse a backwards Slash/Solidus into a forward Slash/Solidus.
            /// </summary>
            /// <param name="directory">The directory to be reversed/inversed forward.</param>
            /// <returns>Reversed/Inversed Forward Slash/Solidus Directory.</returns>
            public static string GetFormattedDirectoryWithForwardSolidus(string directory)
            {
                return directory.Replace("\\", "/"); ;
            }

            /// <summary>
            /// This function reverse/inverse the current Slash/Solidus. 
            /// </summary>
            /// <param name="directory">The directory to be reversed/inversed.</param>
            /// <returns>Reversed/Inversed Slash/Solidus Directory.</returns>
            public static string GetFormattedDirectoryWithInversedSolidus(string directory)
            {
                if (directory.Contains("/"))
                {
                    GetFormattedDirectoryWithBackwardsSolidus(directory);
                }
                else if (directory.Contains("\\"))
                {
                    GetFormattedDirectoryWithForwardSolidus(directory);
                }

                return directory;
            }

            /// <summary>
            /// This function is used to add quotation marks to a given directory.
            /// </summary>
            /// <param name="directory">Directory to be formatted.</param>
            /// <returns>Directory with quotation marks.</returns>
            public static string GetStringFormattedDirectory(string directory)
            {
                return $"\"{directory}\"";
            }

            #endregion

            #endregion

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
                    directoryInfo.fileDirectory = directoryInfo.fileDirectory.Replace("\\", "/");
                    directoryInfo.filePath = Path.Combine(directoryInfo.fileDirectory, directoryInfo.fileName);
                    directoryInfo.filePath = directoryInfo.filePath.Replace("\\", "/");
                    callback.Invoke(directoryInfo);
                }
                catch (Exception exception)
                {

                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Get Path Exception </color><color=white>- Failed to get storage data. Exception message :</color> <color=cyan>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }

#if UNITY_EDITOR

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
                            storageData.assetGUID = AssetDatabase.AssetPathToGUID(storageData.assetPath);

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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Get Asset Path Exception</color>- <color=white>Asset file : {sceneAssetObject.name} failed to get asset path with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }


            /// <summary>
            /// Returns the path for the asset.
            /// </summary>
            /// <typeparam name="T">The type of asset.</typeparam>
            /// <param name="asset">The asset that the path is requested for.</param>
            /// <param name="callBack"></param>
            public static void GetAssetPath<T>(T asset, Action<string, AppEventsData.CallBackResults> callBack = null) where T : UnityEngine.Object
            {
                var callBackResults = new AppEventsData.CallBackResults();

                try
                {
                    string path = AssetDatabase.GetAssetPath(asset);

                    if(string.IsNullOrEmpty(path) == false)
                    {
                        callBackResults.success = true;
                        callBackResults.successValue = $"Asset path successfully found @ : {path}";
                    }
                    else
                    {
                        callBackResults.error = true;
                        callBackResults.errorValue = "Get asset path failed.";
                    }

                    callBack.Invoke(path, callBackResults);
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Scene Asset Exist Exception </color><color=white>- Failed to check if scene asset path exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");

#endif

                    callBackResults.error = true;
                    callBackResults.errorValue = exception.Message;
                    callBack.Invoke(string.Empty,callBackResults);

                    throw exception;
                }
            }

            public static void GetAssetsPaths<T>(T[] assets, Action<string[], AppEventsData.CallBackResults> callBack = null) where T : UnityEngine.Object
            {
                AppEventsData.CallBackResults callBackResults = new AppEventsData.CallBackResults();

                try
                {
                    string[] paths = new string[assets.Length];

                    if (assets.Length > 0)
                    {
                        for (int i = 0; i < assets.Length; i++)
                        {
                            paths[i] = AssetDatabase.GetAssetPath(assets[i]);
                        }

                        callBackResults.success = true;
                        callBackResults.successValue = "Assets paths has been loaded successfully.";
                    }
                    else
                    {
                        callBackResults.error = true;
                        callBackResults.errorValue = "Get assets paths failed.";
                    }

                    callBack.Invoke(paths, callBackResults);
                }
                catch (Exception exception)
                {
                    callBackResults.error = true;
                    callBackResults.errorValue = exception.Message;
                    callBack.Invoke(null, callBackResults);
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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Scene Asset Exist Exception </color><color=white>- Failed to check if scene asset path exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");

                    #endif

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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage]</color> Scene Asset Exist Exception </color><color=white>- Failed to check if scene asset path exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");

                    #endif

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
                var callBackResults = new AppEventsData.CallBackResults();

                try
                {
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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Scene Asset Exist Exception </color><color=white>- Failed to check if scene asset path exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");

#endif

                    callBackResults.error = true;
                    callBackResults.errorValue = exception.Message;
                    callBack.Invoke(callBackResults);

                    throw exception;
                }
            }

            /// <summary>
            /// Checks if a folder exists in the given directory path.
            /// </summary>
            /// <param name="directory">The directory to check.</param>
            /// <param name="callBack">The results from the folder exists check.</param>
            public static void FolderExist(string directory, Action<AppEventsData.CallBackResults> callBack = null)
            {
                var callBackResults = new AppEventsData.CallBackResults();

                try
                {
                    if(System.IO.Directory.Exists(directory))
                    {
                        callBackResults.success = true;
                        callBackResults.successValue = $"Folder forund @ : {directory}";
                    }
                    else
                    {
                        callBackResults.error = true;
                        callBackResults.errorValue = $"Folder not forund @ : {directory}"; 
                    }

                    callBack.Invoke(callBackResults);
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Folder Exist Exception </color><color=white>- Failed to check if folder at path : {directory} exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");

#endif

                    callBackResults.error = true;
                    callBackResults.errorValue = exception.Message;
                    callBack.Invoke(callBackResults);

                    throw exception;
                }
            }

            public static bool FolderExist(string directory)
            {
                try
                {
                    if (System.IO.Directory.Exists(directory))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR

                    DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Folder Exist Exception </color><color=white>- Failed to check if folder at path : {directory} exist or not. Exception message :</color> <color=cyan>{exception.Message}</color>");

#endif

                    throw exception;
                }
            }

            /// <summary>
            /// Opens a diven folder from a directory path.
            /// </summary>
            /// <param name="directory"></param>
            public static void OpenFolder(string directory)
            {
                if (System.IO.Directory.Exists(directory))
                {
                    Process.Start("explorer.exe", directory.Replace("/", "\\"));
                }
            }
#endif

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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Get File Failed</color>- <color=white>Failed to access file :</color> <color=cyan>{directoryInfo.fileName}</color> <color=white>with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }

            public static string GetProjectTempDirectory()
            {
                string targetDest = Path.GetTempPath();
                string targetDestDir = Path.Combine(targetDest, Application.productName.Replace(" ", string.Empty));
                return targetDestDir;
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

            #region Storage Data Commands

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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Delete File Failed</color>- <color=white>File failed to delete with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

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
                    #if UNITY_EDITOR

                        DebugConsole.Log(LogLevel.Error, $"[Storage] <color=red>Delete Directory Failed</color>- <color=white>Directory failed to delete with exception message : </color> <color=cyan>{exception.Message}</color>");

                    #endif

                    throw exception;
                }
            }

#endregion
        }

#endregion
    }
}
