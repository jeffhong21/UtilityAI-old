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
        public abstract float Score(IContext context);




    }


}