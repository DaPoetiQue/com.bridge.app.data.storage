using System;
using UnityEditor;
using UnityEngine;

namespace Bridge.Core.App.Data.Storage
{
    public class StorageData
    {
        #region Options

        public enum ExtensionType
        {
            bridge,
            json,
            txt,
            xml,
            asset
        }

        #endregion

        public class DirectoryInfoData
        {
            public string fileName;
            public string folderName;

            public string filePath;
            public string fileDirectory;

            public string jsonStringFileData;
            public bool isLoaded;

            public string[] directorDataFileList;
            public string assetPath;
            public GUID assetGUID;

            public ExtensionType extensionType;
        }

        #region Serializable Data

        #region Serializable Vector

        [Serializable]
        public class SerializableVector
        {
            #region Components

            public float x;
            public float y;
            public float z;
            public float w;

            #endregion

            #region Constructors

            public SerializableVector()
            {

            }

            public SerializableVector(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public SerializableVector(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public SerializableVector(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            #endregion
        }

        #endregion

        #region Serializable Quaternion

        [Serializable]
        public class SerializableQuaternion
        {
            #region Components

            public float x;
            public float y;
            public float z;
            public float w;

            #endregion

            #region Constructors

            /// <summary>
            /// Empty constuct for default setup.
            /// </summary>
            public SerializableQuaternion()
            {
            }

            public SerializableQuaternion(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// This static class holds functions for converting vectors and quaternion to serializable.
        /// </summary>
        [Serializable]
        public static class SerializableData
        {
            #region Vectors

            #region Serialized Vectors

            /// <summary>
            /// A custom serializable vector. 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <param name="w"></param>
            /// <returns></returns>
            public static SerializableVector GetVector(float x, float y, float? z, float? w)
            {
                return new SerializableVector(x, y, (float)z, (float)w);
            }

            /// <summary>
            /// A custom vector that is dynamic.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <param name="w"></param>
            /// <returns></returns>
            public static Vector4 GetCustomVector(float x, float y, float? z = null, float? w = null)
            {
                // Returns a Vector 4 is all parameters are set.
                if(z != null && w == null)
                {
                    return new Vector3(x, y, (float)z);
                }

                // Returns a Vector 3 is parameters are set accordingly.
                if (z != null && w != null)
                {
                    return new Vector4(x, y, (float)z, (float)w);
                }

                // Return a vector 2 as a default.
                return new Vector2(x, y);
            }

            /// <summary>
            /// Convert a default unity vector 2 struct to a serializable vector 2 class.
            /// </summary>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static SerializableVector GetVector(Vector2 vector)
            {
                return new SerializableVector(vector.x, vector.y);
            }

            /// <summary>
            /// Convert a default unity vector 3 struct to a serializable vector 3 class.
            /// </summary>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static SerializableVector GetVector(Vector3 vector)
            {
                return new SerializableVector(vector.x, vector.y, vector.z);
            }

            /// <summary>
            /// Convert a default unity vector 4 struct to a serializable vector 4 class.
            /// </summary>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static SerializableVector GetVector(Vector4 vector)
            {
                return new SerializableVector(vector.x, vector.y, vector.z, vector.w);
            }

            #endregion

            #region Un Serialized Vectors

            /// <summary>
            /// Convert a serializable vector 2 class to a default unity vector 2 struct.
            /// </summary>
            /// <param name="serializedVector"></param>
            /// <returns></returns>
            public static Vector2 Vector2(SerializableVector serializedVector)
            {
                return new Vector2(serializedVector.x, serializedVector.y);
            }

            /// <summary>
            /// Convert a serializable vector 3 class to a default unity vector 3 struct.
            /// </summary>
            /// <param name="serializedVector"></param>
            /// <returns></returns>
            public static Vector3 Vector3(SerializableVector serializedVector)
            {
                return new Vector3(serializedVector.x, serializedVector.y, serializedVector.z);
            }

            /// <summary>
            /// Convert a serializable vector 4 class to a default unity vector 4 struct.
            /// </summary>
            /// <param name="serializedVector"></param>
            /// <returns></returns>
            public static Vector4 Vector4(SerializableVector serializedVector)
            {
                return new Vector4(serializedVector.x, serializedVector.y, serializedVector.z, serializedVector.w);
            }

            #endregion

            #endregion

            #region Quaternions

            #region Serialized Quaternion

            /// <summary>
            /// Convert a unity quaternion to a serializable quaternion struct.
            /// </summary>
            /// <param name="quaternion"></param>
            /// <returns></returns>
            public static SerializableQuaternion GetQuaternion(Quaternion quaternion)
            {
                return new SerializableQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            }

            #endregion

            #region Un-Serialized Quarternion

            /// <summary>
            /// Convert a serializable quaternion class to a default unity quaternion struct.
            /// </summary>
            /// <param name="quaternion"></param>
            /// <returns></returns>
            public static Quaternion Quaternion(SerializableQuaternion quaternion)
            {
                return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            }

            #endregion

            #endregion
        }

        #endregion

        #region Interfaces

        public interface IStorable
        {
            
        }

        #endregion
    }
}
