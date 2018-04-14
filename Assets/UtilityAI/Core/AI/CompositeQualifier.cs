namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A base class for Qualifiers that base their score on a number of child.
    /// </summary>
    public abstract class CompositeQualifier : IQualifier
    {
        // (IQualifier)
        public bool isDisabled { get; set; }
        //  Used for CompareTo().  
        public float _score { get; protected set; }  // (IQualifier)
        // (IQualifier)
        public IAction action { get; set; }

        protected List<IScorer> _scorers = new List<IScorer>();

        public List<IScorer> scorers {
            get { return _scorers; }
            protected set { _scorers = value; }
        }

        public abstract float Score(IAIContext context, List<IScorer> scorers);

        // (IQualifier)
        public float Score(IAIContext context){
            return Score(context, scorers);
        }

        public int CompareTo(IQualifier other)
        {
            //  Current instance is greater than object being compared too.
            if (other == null) return 1;

            return this._score.CompareTo(other._score);
            //return this.Score(context).CompareTo(other.Score(context));
        }



        protected CompositeQualifier()
        {
            scorers = new List<IScorer>();
        }

    }


    /// <summary>
    /// This Qualifier adds up the sum of all the Scorers attached to this Qualifier.
    /// </summary>
    public class CompositeScoreQualifier : CompositeQualifier
    {
        
        public override float Score(IAIContext context, List<IScorer> scorers)
        {
            var score = 0f;
            if (scorers.Count == 0)
                return score;
            //Debug.Log(this.GetType().ToString() + " is scoring");

            foreach (IScorer scorer in scorers){
                score += scorer.Score(context);
            }


            _score = score;
            return score;
        }
    }











}

