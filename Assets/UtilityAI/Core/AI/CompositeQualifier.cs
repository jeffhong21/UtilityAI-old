namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A base class for Qualifiers that base their score on a number of child.
    /// </summary>
    public abstract class CompositeQualifier : IQualifier, ICloneFrom
    {
        public bool isDisabled { get; set; }

        private List<IScorer> _scorers = new List<IScorer>();

        public List<IScorer> scorers 
        {
            get { return _scorers; }
            protected set { _scorers = value; }
        }
        public IAction action 
        { 
            get; 
            set; 
        }


        public abstract float Score(IContext context, List<IScorer> scorers);

        public float Score(IContext context)
        {
            
            throw new NotImplementedException();
        }


        public void CloneFrom(object other){
            throw new NotImplementedException();
        }


        //  Used for CompareTo().  
        public float _score { get; protected set; }
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
        
        public override float Score(IContext context, List<IScorer> scorers)
        {
            var score = 0f;
            //Debug.Log(this.GetType().ToString() + " is scoring");

            foreach (IScorer scorer in scorers)
            {
                
                score += scorer.Score(context);
            }


            _score = score;
            return score;
        }
    }










    /// <summary>
    /// Relies on contextual scorer to produce score.  Relies on all its Scorers combined scores to be above the Threshold in which it returns the combined score.  
    /// If below the Threshhold, it returns 0.
    /// </summary>
    public class Q_AllOrNothing : QualifierBase
    {
        [SerializeField]
        public float threshold;

        public override float Score(IContext context)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Simply has a fixed score.  So whatever score is set, it will always return that score.
    /// </summary>
    public class Q_FixedScore : QualifierBase
    {
        [SerializeField]
        public float score;

        public override float Score(IContext context)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Q sum while above threshhold.
    /// </summary>
    public class Q_SumWhileAboveThreshhold : QualifierBase
    {
        [SerializeField]
        public float sum;

        public override float Score(IContext context)
        {
            throw new NotImplementedException();
        }
    }
}

