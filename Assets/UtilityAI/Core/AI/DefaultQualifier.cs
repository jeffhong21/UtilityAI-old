namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;



    public class DefaultQualifier : QualifierBase, IDefaultQualifier
    {
        public float score { get; set; }

        public DefaultQualifier()
        {
            score = 1f;
        }


        public override float Score(IContext context)
        {
            return score;
        }
    }









}