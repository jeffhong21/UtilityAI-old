namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   The Qualifiers job is to get just a score.  
    /// </summary>
    [Serializable]
    public abstract class QualifierBase : IQualifier  // IComparable<IQualifier>, 
    {
        public bool isDisabled { get; set; }
        public IAction action { get; set; }

        public float _score { get; protected set; }

        public int CompareTo(IQualifier other)
        {
            //  Current instance is greater than object being compared too.
            if (other == null) return 1;

            return this._score.CompareTo(other._score);
            //return this.Score(context).CompareTo(other.Score(context));
        }

        //  Generates a score from all Scorers.
        public abstract float Score(IAIContext context);




    }

    /// <summary>
    /// Relies on contextual scorer to produce score.  Relies on all its Scorers combined scores to be above the Threshold in which it returns the combined score.  
    /// If below the Threshhold, it returns 0.
    /// </summary>
    [Serializable]
    public class Q_AllOrNothing : QualifierBase
    {
        [SerializeField]
        public float threshold;

        public override float Score(IAIContext context)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Simply has a fixed score.  So whatever score is set, it will always return that score.
    /// </summary>
    [Serializable]
    public class Q_FixedScore : QualifierBase
    {
        [SerializeField]
        public float score;

        public override float Score(IAIContext context)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Q sum while above threshhold.
    /// </summary>
    [Serializable]
    public class Q_SumWhileAboveThreshhold : QualifierBase
    {
        [SerializeField]
        public float sum;

        public override float Score(IAIContext context)
        {
            throw new NotImplementedException();
        }
    }
}