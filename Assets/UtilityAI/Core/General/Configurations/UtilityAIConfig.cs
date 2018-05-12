namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;


    [Serializable]
    public class UtilityAIConfig
    {
        public UtilityAIAsset asset;

        protected IAction a;
        protected IScorer scorer;
        protected List<IScorer> scorers;
        protected IQualifier q;
        protected Selector s;

        protected List<IQualifier> qualifiers;
        protected List<IScorer[]> allScorers;
        protected List<IAction> actions;


        public UtilityAIConfig(UtilityAIAsset asset) { }

        protected virtual void Init() {}

        public virtual void ConfigureAI(Selector rs) {}






    }

}

