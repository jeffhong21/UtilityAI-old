namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.IO;

    #region Original UtilityAI
  //  [Serializable]
  //  public class UtilityAIAsset : ScriptableObject
  //  {
  //      public string friendlyName;
  //      [Multiline]
  //      public string description;
  //      [ShowOnly]
  //      public float version = 1.0f;
  //      public string aiId;

  //      public UtilityAI configuration;
  //      public string editorConfiguration;

  //      ///<summary>
  //      ///Creates an instance for the specified AI.
  //      ///</summary>
  //      ///<param name = "aiId" > The name of the ai that gets registered.</param>
  //      ///<param name = "aiName" > Name of the asset file.</param>
  //      ///<returns></returns>
  //      public UtilityAIAsset CreateAsset(string aiId, string aiName, bool isMockAI = false)
  //      {
  //          string dir = AiManager.StorageFolder;
  //          string assetDir = AssetDatabase.GenerateUniqueAssetPath(dir + "/" + aiName + ".asset");
  //          aiName = Path.GetFileNameWithoutExtension(assetDir);

  //          UtilityAIAsset asset = ScriptableObject.CreateInstance<UtilityAIAsset>();
  //          asset.friendlyName = aiName;
  //          asset.aiId = aiId;
  //          asset.configuration = new UtilityAI(aiId);

  //          if(isMockAI)
  //              ConfigureAI(asset.configuration.selector);


  //          AssetDatabase.CreateAsset(asset, assetDir);
  //          AssetDatabase.SaveAssets();


  //          return asset;
  //      }


		//#region MockAIs

    //    IAction a;
    //    IScorer scorer;
    //    List<IScorer> scorers;
    //    IQualifier q;
    //    Selector s;

    //    List<IQualifier> qualifiers;
    //    List<IScorer[]> allScorers;
    //    List<IAction> actions;

    //    void ConfigureAI(Selector rs)
    //    {
    //        if(qualifiers == null) qualifiers = new List<IQualifier>();
    //        if (allScorers == null) allScorers = new List<IScorer[]>();
    //        if (actions == null) actions = new List<IAction>();


    //        a = new ScanForEntities();
    //        actions.Add(a);
    //        a = new ScanForPositions();
    //        actions.Add(a);
    //        scorers = new List<IScorer>();
    //        scorer = new HasEnemies();
    //        scorers.Add(scorer);
    //        allScorers.Add(scorers.ToArray());
    //        scorers = new List<IScorer>();
    //        scorer = new TestScorerB();
    //        scorers.Add(scorer);
    //        allScorers.Add(scorers.ToArray());
    //        q = new CompositeScoreQualifier();
    //        qualifiers.Add(q);
    //        q = new CompositeScoreQualifier();
    //        qualifiers.Add(q);

    //        //  Setup each qualifiers action and scorers.
    //        for (int index = 0; index < qualifiers.Count; index++)
    //        {
    //            //  Add qualifier to rootSelector.
    //            rs.qualifiers.Add(qualifiers[index]);
    //            var qualifier = rs.qualifiers[index];
    //            //  Set qualifier's action.
    //            qualifier.action = actions[index];
    //            //  Add scorers to qualifier.
    //            foreach (IScorer scorer in allScorers[index]){
    //                if (qualifier is CompositeQualifier){
    //                    var q = qualifier as CompositeQualifier;
    //                    q.scorers.Add(scorer);
    //                }
    //            }
    //        }
    //    }

    //    #endregion

    //}

    #endregion


    [Serializable]
    public class UtilityAIAsset : ScriptableObject
    {
        public string friendlyName;
        [Multiline]
        public string description;
        [ShowOnly]
        public float version = 1.0f;
        public string aiId;
        [HideInInspector]
        public byte[] aiConfig;
        [HideInInspector]
        public byte[] editorConfig;
        [HideInInspector]
        public UtilityAI configuration;

        ///<summary>
        ///Creates an instance for the specified AI.
        ///</summary>
        ///<param name = "aiId" > The name of the ai that gets registered.</param>
        ///<param name = "aiName" > Name of the asset file.</param>
        ///<returns></returns>
        public UtilityAIAsset CreateAsset(string aiId, string aiName, bool isMockAI = false)
        {
            UtilityAIAsset asset = ScriptableObject.CreateInstance<UtilityAIAsset>();

            string assetDir = AssetDatabase.GenerateUniqueAssetPath(AiManager.StorageFolder + "/" + aiName + ".asset");


            asset.friendlyName = Path.GetFileNameWithoutExtension(assetDir);
            asset.aiId = aiId;
            asset.configuration = new UtilityAI(asset.friendlyName);

            asset.aiConfig = ProjectAsset.GetData(asset.configuration);

            if (isMockAI){
                ConfigureAI(asset.configuration.selector);
            }
                

            AssetDatabase.CreateAsset(asset, assetDir);
            AssetDatabase.SaveAssets();


            return asset;
        }


        #region MockAIs

        IAction a;
        IScorer scorer;
        List<IScorer> scorers;
        IQualifier q;
        Selector s;

        List<IQualifier> qualifiers;
        List<IScorer[]> allScorers;
        List<IAction> actions;

        void ConfigureAI(Selector rs)
        {
            if (qualifiers == null) qualifiers = new List<IQualifier>();
            if (allScorers == null) allScorers = new List<IScorer[]>();
            if (actions == null) actions = new List<IAction>();


            a = new ScanForEntities();
            actions.Add(a);
            a = new ScanForPositions();
            actions.Add(a);
            scorers = new List<IScorer>();
            scorer = new HasEnemies();
            scorers.Add(scorer);
            allScorers.Add(scorers.ToArray());
            scorers = new List<IScorer>();
            scorer = new TestScorerB();
            scorers.Add(scorer);
            allScorers.Add(scorers.ToArray());
            q = new CompositeScoreQualifier();
            qualifiers.Add(q);
            q = new CompositeScoreQualifier();
            qualifiers.Add(q);

            //  Setup each qualifiers action and scorers.
            for (int index = 0; index < qualifiers.Count; index++)
            {
                //  Add qualifier to rootSelector.
                rs.qualifiers.Add(qualifiers[index]);
                var qualifier = rs.qualifiers[index];
                //  Set qualifier's action.
                qualifier.action = actions[index];
                //  Add scorers to qualifier.
                foreach (IScorer scorer in allScorers[index])
                {
                    if (qualifier is CompositeQualifier)
                    {
                        var q = qualifier as CompositeQualifier;
                        q.scorers.Add(scorer);
                    }
                }
            }
        }

        #endregion
    }

}

