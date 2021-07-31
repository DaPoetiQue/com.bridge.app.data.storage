using System;
using System.IO;
using UnityEngine;

namespace com.bridge.app.serializations.manager
{
    public static class AppDataSerializationManager
    {
        #region Persistant Data

        public static void SaveData<T>(SerializationData serializationDataInfo, T serializationData, Action<string, bool> saveCallBack = null) where T : UnityEngine.Object
        {
            serializationDataInfo.fileName = serializationDataInfo.fileName.Contains(".json") ? serializationDataInfo.fileName : serializationDataInfo.fileName + ".json";

            GetSerializedDataDirectory(serializationDataInfo.folderName, (dataInfo, exists) =>
            {
                if (exists == false)
                {
                    Debug.LogWarning($"-->> <color=orange>Data Serialization Failed - </color> <color=white>Serialization Data Directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                    return;
                }

                string serializedData = JsonUtility.ToJson(serializationData);
                serializationDataInfo.path = Path.Combine(serializationDataInfo.directory, dataInfo.fileName);

                if (File.Exists(serializationDataInfo.path) == true)
                {
                    File.Delete(serializationDataInfo.path);
                }

                File.WriteAllText(serializationDataInfo.path, serializedData);
                saveCallBack.Invoke(serializationDataInfo.path, File.Exists(serializationDataInfo.path));
            });
        }

        public static void LoadData<T>(SerializationData serializationDataInfo, Action<T, bool> callback = null) where T : UnityEngine.Object
        {
            serializationDataInfo.fileName = serializationDataInfo.fileName.Contains(".json") ? serializationDataInfo.fileName : serializationDataInfo.fileName + ".json";
            serializationDataInfo.path = Path.Combine(serializationDataInfo.directory, serializationDataInfo.fileName);

            GetSerializedDataPath(serializationDataInfo, (dataInfo, exists) =>
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

        public static void DeleteDataFile(SerializationData serializedDataInfo, Action<string, bool> callback)
        {
            GetSerializedDataPath(serializedDataInfo, (serializationDataInfo, exists) =>
            {
                if (exists == false)
                {
                    Debug.LogWarning($"-->> <color=orange>Serialization data directory :</color> <color=cyan>{serializedDataInfo.fileName}</color> <color=white>doesn't exist.</color>");
                    return;
                }

                if(File.Exists(serializationDataInfo.path) == false)
                {
                    Debug.LogWarning($"-->> <color=orange>Serialization data file : </color> <color=cyan>{serializedDataInfo.fileName}</color> <color=white>doesn't exist inside directory : </color><color=cyan>{serializedDataInfo.directory}</color>");
                    return;
                }

                File.Delete(serializationDataInfo.path);

                callback.Invoke(serializationDataInfo.directory, File.Exists(serializationDataInfo.path));
            });
        }

        public static void DeleteDirectory(string folderName, Action<string, bool> callback)
        {
            GetSerializedDataDirectory(folderName, (serializationDataInfo, exists) =>
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

        private static void GetSerializedDataDirectory(string folderName, Action<SerializationData, bool> callback)
        {
            SerializationData directoryDataInfo = new SerializationData();
            directoryDataInfo.directory = Path.Combine(Application.persistentDataPath, folderName);

            if (Directory.Exists(directoryDataInfo.directory) == false)
            {
                Directory.CreateDirectory(directoryDataInfo.directory);
            }

            callback.Invoke(directoryDataInfo, Directory.Exists(directoryDataInfo.directory));
        }

        private static void GetSerializedDataPath(SerializationData serializedDataInfo, Action<SerializationData, bool> callback)
        {
            SerializationData dataFileInfo = new SerializationData();
            dataFileInfo.fileName = serializedDataInfo.fileName.Contains(".json")? serializedDataInfo.fileName : serializedDataInfo.fileName + ".json";

            GetSerializedDataDirectory(serializedDataInfo.folderName, (dataInfo, exists) =>
            {
                if (exists == false)
                {
                    Debug.LogWarning($"-->> <color=orange></color> <color=white>Serialization data directory :</color> <color=cyan>{dataInfo.directory}</color> <color=white>doesn't exist.</color>");
                    return;
                }

                dataFileInfo.folderName = serializedDataInfo.folderName;
                dataFileInfo.path = Path.Combine(dataInfo.directory, dataFileInfo.fileName); ;
            });


            callback.Invoke(dataFileInfo, File.Exists(dataFileInfo.path));
        }

        #endregion
    }
}
