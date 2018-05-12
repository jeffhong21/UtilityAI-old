namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;


    [Serializable]
    public class ScanAIConfig : UtilityAIConfig
    {
        
        //private IAction a;
        //private IScorer scorer;
        //private List<IScorer> scorers;
        //private IQualifier q;
        //private Selector s;

        //private List<IQualifier> qualifiers;
        //private List<IScorer[]> allScorers;
        //private List<IAction> actions;


        public ScanAIConfig(UtilityAIAsset asset) : base(asset)
        {
            this.asset = asset;
            Init();
        }

        protected override void Init()
        {
            //ConfigureAI(this.asset.configuration.selector);
            ConfigureAI(this.asset.configuration.rootSelector);
            AssetDatabase.SaveAssets();
        }


        public override void ConfigureAI(Selector rs)
        {
            if (qualifiers == null) qualifiers = new List<IQualifier>();
            if (allScorers == null) allScorers = new List<IScorer[]>();
            if (actions == null) actions = new List<IAction>();


            a = new ScanForEntities();
            a.name = "Scan_For_Entities";
            actions.Add(a);
            a = new ScanForPositions();
            a.name = "Scan_For_Positions";
            actions.Add(a);

            scorers = new List<IScorer>();
            scorer = new HasEnemies();
            scorers.Add(scorer);
            scorer = new TestScorerA();
            scorers.Add(scorer);
            allScorers.Add(scorers.ToArray());

            scorers = new List<IScorer>();
            scorer = new HasEnemiesInRange();
            scorers.Add(scorer);
            scorer = new TestScorerB();
            scorers.Add(scorer);
            allScorers.Add(scorers.ToArray());

            q = new CompositeScoreQualifier();
            qualifiers.Add(q);
            q = new CompositeScoreQualifier();
            qualifiers.Add(q);

            rs.defaultQualifier.action = new ScanForEntities();

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

            Debug.Log("Finish Initializing Scan AI");
        }





    }

}

