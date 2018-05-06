namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public class ProjectAsset
    {

        private byte[] data;

        public static byte[] GetData(UtilityAI utilityAI)
        {
            ProjectAsset projectAsset = new ProjectAsset();

            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            //  Serialize utilityAI to memoryStream
            binaryFormatter.Serialize(memoryStream, utilityAI);
            memoryStream.Close();


            //  Store memory stream as byte array
            projectAsset.data = memoryStream.ToArray();

            memoryStream = new MemoryStream();
            binaryFormatter = new BinaryFormatter();
            //  Serialize projectAsset to memoryStream (useful if project asset contains other data)
            binaryFormatter.Serialize(memoryStream, projectAsset);
            memoryStream.Close();


            return memoryStream.ToArray();
        }


        public static object Load(byte[] data)
        {
            object obj = new BinaryFormatter().Deserialize(new MemoryStream(data));

            if (obj is ProjectAsset)
            {
                ProjectAsset asset = obj as ProjectAsset;
                return new BinaryFormatter().Deserialize(new MemoryStream(asset.data)) as UtilityAI;
            }
            else throw new ApplicationException("Unable to deserialize type " + obj.GetType());
        }


        //public static void SaveAsset()
        //{
        //    if(currentAssetLibrary != null){
        //        byte[] newData = currentAssetLibrary.GetData();

        //        if(aiAsset.configuration.Equals(newData))
        //        {
        //            aiAsset.configuration = newData;
        //            EditoryUtility.SetDirty((AIAsset)aiAsset);
        //        }

        //        //Debug.Log("Saving");

        //        //this.currentAssetLibrary.IsDirty = false;
        //        //ActionLibraryExplorer.instance.Repaint();
        //        //ActionNetworkEditor.instance.Repaint();
        //    }
        //}

    }
}

