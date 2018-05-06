namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;


    [Serializable]
    public class DefaultQualifier : QualifierBase, IDefaultQualifier
    {
        public float score { get; set; }

        public DefaultQualifier()
        {
            score = 1f;
        }


        public override float Score(IAIContext context)
        {
            return score;
        }
    }









}