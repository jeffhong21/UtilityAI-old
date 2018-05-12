namespace UtilityAI
{
    using UnityEngine;
    using System;


    [System.Serializable]
    public struct ScoredOption<TOption> : IEquatable<ScoredOption<TOption>>, IComparable<ScoredOption<TOption>>
    {
        private TOption _option;
        private float _score;

        public TOption option { 
            get { return _option; }
            set { _option = value; }
        }

        public float score{
            get { return _score; }
            set { _score = value; }
        }


        public int CompareTo(ScoredOption<TOption> other){
            //  Current instance is greater than object being compared too.
            //if (other == null) return 1;
            return this.score.CompareTo((other.score));
        }


        public bool Equals(ScoredOption<TOption> other){
            return score == other.score;
        }

		public override bool Equals(object obj){
            if (obj == null)
                return false;

            var scoredOption = (ScoredOption<TOption>)obj;
            return Equals(scoredOption);
		}

        public override int GetHashCode(){
            return score.GetHashCode();
        }

		public ScoredOption(TOption o, float s){
            _option = o;
            _score = s;
        }
    }


    /// <summary>
    /// Data class to hold the score of the TOption.
    /// e.g This could hold a Vector3 position data and the score associated with that position.
    /// </summary>
    //public class ScoredOption<TOption> //: IComparable<ScoredOption<TOption>>
    //{

    //    public TOption option;
    //    public float score;


    //    public ScoredOption(TOption _option, float _score)
    //    {
    //        option = _option;
    //        score = _score;
    //    }

    //    //public int CompareTo(ScoredOption<TOption> other)
    //    //{
    //    //    //  Current instance is greater than object being compared too.
    //    //    if (other == null) return 1;

    //    //    return this.score.CompareTo((other.score));
    //    //}
    //}

}