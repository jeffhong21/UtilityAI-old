namespace UtilityAI
{
    using UnityEngine;
    using System.Collections.Generic;


    public class MockAI : UtilityAI
    {
        IAction a;
        IScorer scorer;
        List<IScorer> scorers;
        IQualifier q;
        Selector s;

        List<IQualifier> qualifiers = new List<IQualifier>();
        List<IScorer[]> allScorers = new List<IScorer[]>();
        List<IAction> actions = new List<IAction>();


        void DefineActions()
        {
            a = new ScanForEntities();
            actions.Add(a);
            a = new ScanForPositions();
            actions.Add(a);
        }

        void DefineScorers()
        {
            scorers = new List<IScorer>();
            scorer = new TestScorerA();
            scorers.Add(scorer);

            scorers = new List<IScorer>();
            scorer = new TestScorerB();
            scorers.Add(scorer);
        }

        void DefineQualifiers()
        {
            q = new CompositeScoreQualifier();
            qualifiers.Add(q);


            q = new CompositeScoreQualifier();
            qualifiers.Add(q);
        }

        void DefineSelectors()
        {
            
        }

        void Initialize()
        {
            DefineActions();
            DefineScorers();
            DefineQualifiers();
            DefineSelectors();
        }


        public void ConfigureAI(Selector rs)
        {
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

            Debug.Log(string.Format("Finished Initializing {0}", this.GetType().Name));
        }


        public MockAI()
        {
            Initialize();
            ConfigureAI(this.rootSelector);
        }

        public MockAI(string aiName) : base(aiName)
        {
            Initialize();
            ConfigureAI(this.rootSelector);
        }

    }
}