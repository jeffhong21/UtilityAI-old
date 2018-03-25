namespace UtilityAI
{
    using UnityEngine;
    using System.Collections.Generic;

    public class Test_UtilityAIClient : UtilityAIClient
    {
        Selector rootSelector;


        List<IQualifier> qualifiers = new List<IQualifier>()
            {
                new CompositeScoreQualifier()
                // , new CompositeScoreQualifier()
            };

        List<IScorer[]> scorers = new List<IScorer[]>()
            {
                new IScorer[]
                {
                    new TestScorerA()
                }
                // , new IScorer[]
                // {
                //     new TestScorerB()
                // },
            };

        List<IAction> actions = new List<IAction>()
            {
                new PatrolAction()
                // ,new PatrolAction()
            };



        /// <summary>
        /// Initializes all the qualifiers, actions, and scorers.
        /// </summary>
        public void InitializeTestClient()
        {
            
            rootSelector = this.ai.rootSelector;

            //  Setup each qualifiers action and scorers.
            for (int index = 0; index < qualifiers.Count; index++)
            {
                //  Add qualifier to rootSelector.
                rootSelector.qualifiers.Add(qualifiers[index]);
                var qualifier = rootSelector.qualifiers[index];

                //  Set qualifier's action.
                qualifier.action = actions[index];

                //  Add scorers to qualifier.
                foreach (IScorer scorer in scorers[index])
                {
                    if (qualifier is CompositeQualifier)
                    {
                        var q = qualifier as CompositeQualifier;
                        q.scorers.Add(scorer);
                    }
                }

            }



            Debug.Log("Finished Initializing TestAICLient");
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="T:UtilityAI.Test_UtilityAIClient"/> class.
        /// </summary>
        /// <param name="ai">Ai.</param>
        public Test_UtilityAIClient(UtilityAIComponent agent, IUtilityAI ai, IContextProvider contextProvider) : base (agent, ai, contextProvider)
        {
        }

        
    }
}