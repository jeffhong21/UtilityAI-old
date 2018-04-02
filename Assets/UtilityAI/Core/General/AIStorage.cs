namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.IO;

    [Serializable]
    public class AIStorage : ScriptableObject
    {
        [Multiline]
        public string description;
        public int version;
        public Guid aiId;
        public UtilityAIClient configuration;


        public void Create(string aiId, string aiName)
        {
            
            var utilityAI = Type.GetType(typeof(AIStorage).Namespace + "." + aiId);
            if (utilityAI == null){
                Debug.Log(string.Format("Could not find {0}", aiId));
                return;
            }
                
            var instance = (UtilityAI)Activator.CreateInstance(utilityAI, aiName);
            configuration = new UtilityAIClient(instance);


            var asset = ScriptableObject.CreateInstance<AIStorage>();
            string path = AiManager.StorageFolder;


            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + aiName + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();

            Debug.Log(configuration);
            Debug.Log(description);
        }



    }
}

