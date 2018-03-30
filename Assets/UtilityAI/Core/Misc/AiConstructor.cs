namespace UtilityAI
{
    using UnityEngine;
    using System.Collections.Generic;

    public abstract class AiConstructor
    {

        protected IAction action;
        protected IScorer scorer;
        protected IQualifier qualifier;
        protected Selector selector;

        protected IUtilityAI utilityAI;


        protected abstract void DefineActions();
        protected abstract void DefineScorers();
        protected abstract void DefineQualifiers();
        protected abstract void DefineSelectors();

        protected abstract void ConfigureAI();


        protected AiConstructor(){
            
        }

        void Initialize()
        {
            DefineActions();
            DefineScorers();
            DefineQualifiers();
            DefineSelectors();
            ConfigureAI();
        }

    }
}