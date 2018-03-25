namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class AIStorage : ScriptableObject
    {
        public string description;
        public int version;
        public Guid aiId;
        public string configuration;


        public void Create(string aiId, string aiName)
        {
            ScriptableObject ai = CreateInstance(aiId);
            var path = string.Format("Assets/{0}.asset", aiName);

            AssetDatabase.CreateAsset(ai, path);
            AssetDatabase.SaveAssets();
        }
    }
}

