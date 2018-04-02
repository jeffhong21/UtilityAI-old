namespace UtilityAI
{
    using UnityEngine;
    using System.Collections.Generic;

    public interface AiConstructor
    {

        IAction a { get; }
        IScorer scorer { get; }
        List<IScorer> scorers { get; }
        IQualifier q { get; }
        Selector s { get; }


        void DefineActions();
        void DefineScorers();
        void DefineQualifiers();
        void DefineSelector();

        /// <summary>
        /// Calls all Define methods
        /// </summary>
        void Initialize();


    }


    public struct SelectorConstructor
    {
        public List<IQualifier> qualifiers { get; set; }
        public List<IScorer[]> allScorers { get; set; }
        public List<IAction> actions { get; set; }


        public SelectorConstructor(List<IQualifier> qualifiers, List<IScorer[]> allScorers, List<IAction> actions)
        {
            this.actions = actions;
            this.allScorers = allScorers;
            this.qualifiers = qualifiers;
        }

    }
}