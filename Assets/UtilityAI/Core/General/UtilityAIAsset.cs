namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.IO;

    [Serializable]
    public class UtilityAIAsset : ScriptableObject
    {
        public string friendlyName;
        [Multiline]
        public string description;
        public int version;
        public string aiId;


        public UtilityAI configuration;
        public string[] editorConfiguration;

        ///<summary>
        ///Creates an instance for the specified AI.
        ///</summary>
        ///<param name = "aiId" > The AI ID.</param>
        ///<param name = "aiName" > Name of the AI.</param>
        ///<returns></returns>
        public UtilityAIAsset CreateAsset(string aiId, string aiName)
        {
            string dir = AiManager.StorageFolder;
            string assetDir = AssetDatabase.GenerateUniqueAssetPath(dir + "/" + aiName + ".asset");
            aiName = Path.GetFileNameWithoutExtension(assetDir);

            UtilityAIAsset asset = ScriptableObject.CreateInstance<UtilityAIAsset>();
            asset.friendlyName = aiName;
            asset.configuration = new UtilityAI(){
                name = aiName
            };
            asset.aiId = aiId;




            AssetDatabase.CreateAsset(asset, assetDir);
            AssetDatabase.SaveAssets();

            //Debug.Log(configuration.ai.name);
            //Debug.Log(asset.configuration.name);
            return asset;
        }






        #region Selector and UtilityAI
        //public Selector configuration;
        //public UtilityAI editorConfiguration;

        /////<summary>
        /////Creates an instance for the specified AI.
        /////</summary>
        /////<param name = "aiId" > The AI ID.</param>
        /////<param name = "aiName" > Name of the AI.</param>
        /////<returns></returns>
        //public UtilityAIAsset CreateAsset(string aiId, string aiName)
        //{
        //    string dir = AiManager.StorageFolder;
        //    string assetDir = AssetDatabase.GenerateUniqueAssetPath(dir + "/" + aiName + ".asset");
        //    aiName = Path.GetFileNameWithoutExtension(assetDir);

        //    UtilityAIAsset asset = ScriptableObject.CreateInstance<UtilityAIAsset>();
        //    asset.configuration = new ScoreSelector();
        //    asset.editorConfiguration = new UtilityAI(aiName)
        //    {
        //        rootSelector = asset.configuration
        //    };
        //    asset.aiId = aiId;

        //    //asset.configuration = new UtilityAIClient(new UtilityAI(aiName));
        //    //asset.aiId = aiId;

        //    AssetDatabase.CreateAsset(asset, assetDir);
        //    AssetDatabase.SaveAssets();


        //    return asset;
        //}
        #endregion


        #region MockAIs
        public void CreateAsset(string aiId, string aiName, Type type)
        {
            var utilityAI = Type.GetType(typeof(UtilityAIAsset).Namespace + "." + aiId);
            if (utilityAI == null)
            {
                Debug.LogWarning(string.Format("Could not find {0}", aiId));
                return;
            }

            var instance = (UtilityAI)Activator.CreateInstance(utilityAI, aiName);
            //configuration = new UtilityAIClient(instance);


            var asset = ScriptableObject.CreateInstance(type);


            string dir = AiManager.StorageFolder;
            string assetDir = AssetDatabase.GenerateUniqueAssetPath(dir + "/" + aiName + ".asset");
            AssetDatabase.CreateAsset(asset, assetDir);
            AssetDatabase.SaveAssets();

            //Debug.Log(configuration);
            //Debug.Log(description);
        }
#endregion

    }

}

