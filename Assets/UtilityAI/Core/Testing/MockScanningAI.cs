namespace UtilityAI
{
    using UnityEngine;
    using System.Collections.Generic;

    public class MockScanningAI : UtilityAI
    {

        //protected IAction action;
        //protected IScorer scorer;
        //protected IScorer[] scorers;
        //protected IQualifier qualifier;
        //protected Selector selector;

        List<IQualifier> qualifiers = new List<IQualifier>()
            {
                new CompositeScoreQualifier(),
                new CompositeScoreQualifier(), 
            };

        List<IScorer[]> scorers = new List<IScorer[]>()
            {
                new IScorer[]{
                new TestScorerA()
            },
                new IScorer[]{
                new TestScorerB()
            },

            };

        List<IAction> actions = new List<IAction>()
        {
            new ScanForEntities(),
            new ScanForPositions()
        };



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
                foreach (IScorer scorer in scorers[index])
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



        public MockScanningAI(){
            ConfigureAI(this.rootSelector);
        }

        public MockScanningAI(string aiName) : base(aiName){
            ConfigureAI(this.rootSelector);
        }



    }
}