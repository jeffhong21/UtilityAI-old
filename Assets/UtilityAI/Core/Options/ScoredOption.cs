//namespace UtilityAI
//{
//    using UnityEngine;
//    using System;
//    /// <summary>
//    /// Data class to hold the score of the TOption.
//    /// e.g This could hold a Vector3 position data and the score associated with that position.
//    /// </summary>
//    public class ScoredOption<TOption> : IComparable<ScoredOption<TOption>>
//    {
        
//        public TOption option;
//        public float score;


//        public ScoredOption(TOption _option, float _score)
//        {
//            option = _option;
//            score = _score;
//        }

//        public int CompareTo(ScoredOption<TOption> other)
//        {
//            //  Current instance is greater than object being compared too.
//            if (other == null) return 1;

//            return this.score.CompareTo((other.score));
//        }
//    }
//}