using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace Bridge.App.Serializations.Manager
{
    public class SerializationDataPersistentStorage
    {
        public static class BinaryFormatterDataStorage
        {
            #region Binary Formatter Data

            public static void SaveData<T>(StorageDataInfo storageDataInfo, T storedData, Action<string, bool> saveCallBack = null) where T : UnityEngine.Object
            {
                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                GetDataDirectory(storageDataInfo.folderName, (dataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange>Data Serialization Failed - </color> <color=white>Serialization Data Directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    string serializedData = JsonUtility.ToJson(storedData);
                    storageDataInfo.path = Path.Combine(storageDataInfo.directory, dataInfo.fileName);

                    if (File.Exists(storageDataInfo.path) == true)
                    {
                        File.Delete(storageDataInfo.path);
                    }

                    File.WriteAllText(storageDataInfo.path, serializedData);
                    saveCallBack.Invoke(storageDataInfo.path, File.Exists(storageDataInfo.path));
                });
            }

            public static void LoadData<T>(StorageDataInfo storageDataInfo, Action<T, bool> callback = null) where T : UnityEngine.Object
            {
                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                storageDataInfo.path = Path.Combine(storageDataInfo.directory, storageDataInfo.fileName);

                GetDataPath(storageDataInfo, (dataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange> Load AR Root Config Failed - </color> <color=white>AR root config settings data file missing, not found at path :</color> <color=cyan>{dataInfo.path}</color>");
                        return;
                    }

                    string serializedDataFile = File.ReadAllText(dataInfo.path);
                    T data = JsonUtility.FromJson<T>(serializedDataFile);

                    callback.Invoke(data, string.IsNullOrEmpty(serializedDataFile));
                });
            }

            public static void DeleteData(StorageDataInfo storageDataInfo, Action<string, bool> callback)
            {
                GetDataPath(storageDataInfo, (serializationDataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange>Serialization data directory :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    if (File.Exists(serializationDataInfo.path) == false)
                    {
                        Debug.LogWarning($"-->> <color=orange>Serialization data file : </color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist inside directory : </color><color=cyan>{storageDataInfo.directory}</color>");
                        return;
                    }

                    File.Delete(serializationDataInfo.path);

                    callback.Invoke(serializationDataInfo.directory, File.Exists(serializationDataInfo.path));
                });
            }

            public static void DeleteDataDirectory(string folderName, Action<string, bool> callback)
            {
                GetDataDirectory(folderName, (serializationDataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange></color> <color=white>AR root config settings directory :</color> <color=cyan>{serializationDataInfo}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    Directory.Delete(serializationDataInfo.directory);
                    callback.Invoke(serializationDataInfo.directory, Directory.Exists(serializationDataInfo.directory));
                });
            }

            private static void GetDataDirectory(string folderName, Action<StorageDataInfo, bool> callback)
            {
                StorageDataInfo directoryDataInfo = new StorageDataInfo();
                directoryDataInfo.directory = Path.Combine(Application.persistentDataPath, folderName);

                if (Directory.Exists(directoryDataInfo.directory) == false)
                {
                    Directory.CreateDirectory(directoryDataInfo.directory);
                }

                callback.Invoke(directoryDataInfo, Directory.Exists(directoryDataInfo.directory));
            }

            private static void GetDataPath(StorageDataInfo storageDataInfo, Action<StorageDataInfo, bool> callback)
            {
                StorageDataInfo dataFileInfo = new StorageDataInfo();
                dataFileInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                GetDataDirectory(storageDataInfo.folderName, (dataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange></color> <color=white>Serialization data directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    dataFileInfo.folderName = storageDataInfo.folderName;
                    dataFileInfo.path = Path.Combine(dataInfo.directory, dataFileInfo.fileName); ;
                });


                callback.Invoke(dataFileInfo, File.Exists(dataFileInfo.path));
            }

            #endregion
        }

        public static class JsonDataStorage
        {
            #region Json Utility Data

            public static void SaveData<T>(StorageDataInfo storageDataInfo, T serializationData, Action<string, bool> saveCallBack = null) where T : UnityEngine.Object
            {
                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                GetDataDirectory(storageDataInfo.folderName, (dataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange>Data Serialization Failed - </color> <color=white>Serialization Data Directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    string serializedData = JsonUtility.ToJson(serializationData);
                    storageDataInfo.path = Path.Combine(storageDataInfo.directory, dataInfo.fileName);

                    if (File.Exists(storageDataInfo.path) == true)
                    {
                        File.Delete(storageDataInfo.path);
                    }

                    File.WriteAllText(storageDataInfo.path, serializedData);
                    saveCallBack.Invoke(storageDataInfo.path, File.Exists(storageDataInfo.path));
                });
            }

            public static void LoadData<T>(StorageDataInfo storageDataInfo, Action<T, bool> callback = null) where T : UnityEngine.Object
            {
                storageDataInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";
                storageDataInfo.path = Path.Combine(storageDataInfo.directory, storageDataInfo.fileName);

                GetDataPath(storageDataInfo, (dataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange> Load AR Root Config Failed - </color> <color=white>AR root config settings data file missing, not found at path :</color> <color=cyan>{dataInfo.path}</color>");
                        return;
                    }

                    string serializedDataFile = File.ReadAllText(dataInfo.path);
                    T data = JsonUtility.FromJson<T>(serializedDataFile);

                    callback.Invoke(data, string.IsNullOrEmpty(serializedDataFile));
                });
            }

            public static void DeleteData(StorageDataInfo storageDataInfo, Action<string, bool> callback)
            {
                GetDataPath(storageDataInfo, (serializationDataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange>Serialization data directory :</color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    if (File.Exists(serializationDataInfo.path) == false)
                    {
                        Debug.LogWarning($"-->> <color=orange>Serialization data file : </color> <color=cyan>{storageDataInfo.fileName}</color> <color=white>doesn't exist inside directory : </color><color=cyan>{storageDataInfo.directory}</color>");
                        return;
                    }

                    File.Delete(serializationDataInfo.path);

                    callback.Invoke(serializationDataInfo.directory, File.Exists(serializationDataInfo.path));
                });
            }

            public static void DeleteDataDirectory(string folderName, Action<string, bool> callback)
            {
                GetDataDirectory(folderName, (serializationDataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange></color> <color=white>AR root config settings directory :</color> <color=cyan>{serializationDataInfo}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    Directory.Delete(serializationDataInfo.directory);
                    callback.Invoke(serializationDataInfo.directory, Directory.Exists(serializationDataInfo.directory));
                });
            }

            private static void GetDataDirectory(string folderName, Action<StorageDataInfo, bool> callback)
            {
                StorageDataInfo directoryDataInfo = new StorageDataInfo();
                directoryDataInfo.directory = Path.Combine(Application.persistentDataPath, folderName);

                if (Directory.Exists(directoryDataInfo.directory) == false)
                {
                    Directory.CreateDirectory(directoryDataInfo.directory);
                }

                callback.Invoke(directoryDataInfo, Directory.Exists(directoryDataInfo.directory));
            }

            private static void GetDataPath(StorageDataInfo storageDataInfo, Action<StorageDataInfo, bool> callback)
            {
                StorageDataInfo dataFileInfo = new StorageDataInfo();
                dataFileInfo.fileName = storageDataInfo.fileName.Contains(".json") ? storageDataInfo.fileName : storageDataInfo.fileName + ".json";

                GetDataDirectory(storageDataInfo.folderName, (dataInfo, exists) =>
                {
                    if (exists == false)
                    {
                        Debug.LogWarning($"-->> <color=orange></color> <color=white>Serialization data directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                        return;
                    }

                    dataFileInfo.folderName = storageDataInfo.folderName;
                    dataFileInfo.path = Path.Combine(dataInfo.directory, dataFileInfo.fileName); ;
                });


                callback.Invoke(dataFileInfo, File.Exists(dataFileInfo.path));
            }

            #endregion
        }
    }
    
}
