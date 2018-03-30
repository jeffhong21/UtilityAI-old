namespace UtilityAI
{
    using UnityEngine;
    using System.Collections.Generic;

    public class MockMoveAI : UtilityAI
    {


        List<IScorer[]> scorers = new List<IScorer[]>()
            {
                new IScorer[]{
                new TestScorerA(),
                new TestScorerA() 
            },
                new IScorer[]{
                new HasEnemiesInRange()
            },

            };

        List<IAction> actions = new List<IAction>()
        {
            new TacticalMoveAction( new[] {new ProximityToNearestEnemy()} ),
            new TacticalMoveAction( new[] {new OverRangeToClosestEnemy()} )
            //new PatrolAction(),
            //new RandomMove()

        };


        List<IQualifier> qualifiers = new List<IQualifier>()
            {
                new CompositeScoreQualifier(),
                new CompositeScoreQualifier(),
            };



        private ActionWithOptionsVisualizer _visualizer;
        public ActionWithOptionsVisualizer visualizer{
            get { return _visualizer; }
            set{
                _visualizer = value;
                _visualizer.action = actions[0] as ActionWithOptions<Vector3>;
            }
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





        public MockMoveAI(){
            ConfigureAI(this.rootSelector);
        }

        public MockMoveAI(string aiName) : base(aiName){
            ConfigureAI(this.rootSelector);
        }



    }
}