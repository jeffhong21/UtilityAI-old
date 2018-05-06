namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public class SerializationMaster
    {
        class SelectorFields
        {

            Guid _id;
            List<IQualifier> _qualifiers;
            IDefaultQualifier _defaultQualifier;
            Guid id { get; set; }
            List<IQualifier> qualifiers { get; set; }
            IDefaultQualifier defaultQualifier { get; set; }

        }

        class QualifierFields
        {
            bool isDisabled { get; set; }
            public float _score { get; protected set; }

            IAction action { get; set; }

        }

        class CompositeQualifierFields
        {
            // (IQualifier)
            bool isDisabled { get; set; }
            public float _score { get; protected set; }

            public IAction action { get; set; }
            protected List<IScorer> _scorers;
            public List<IScorer> scorers { get; protected set; }
        }


        class ActionSequenceFields
        {
            private List<IAction> _actions;
            public List<IAction> actions { get; set; }
        }

        class ActionWithOptionFields<TOption>
        {
            //  All the OptionScorers attached to this action.
            public List<IOptionScorer<TOption>> _scorers;
            public List<IOptionScorer<TOption>> scorers { get; set; }
            //  All the TOptions and its score.
            public List<ScoredOption<TOption>> scoredOptions { get; protected set; }
        }


        //public void PrepareForSerialize()
        //{
        //    //throw new NotImplementedException();
        //    var root = rootSelector;
        //    IQualifier[] qualifiers = root.qualifiers.ToArray();

        //    //  Loop through each qualifier.
        //    for (int i = 0; i < qualifiers.Length; i++)
        //    {
        //        IQualifier qualifier = qualifiers[i];
        //        IAction action = qualifier.action;
        //        IAction[] actionSequence;
        //        IScorer[] scorers;

        //        //  Qualifiers
        //        if (qualifiers.GetType().BaseType == typeof(QualifierBase))
        //        {
        //            var q = qualifier as QualifierBase;
        //            //action = q.action;
        //        }
        //        else if (qualifiers.GetType().BaseType == typeof(CompositeQualifier))
        //        {
        //            var q = qualifier as CompositeQualifier;
        //            //action = q.action;
        //            scorers = new IScorer[q.scorers.Count];
        //            for (int k = 0; k < q.scorers.Count; k++)
        //            {
        //                scorers[k] = q.scorers[k];
        //            }
        //        }
        //        else if (qualifier.GetType().BaseType == typeof(DefaultQualifier))
        //        {
        //            var q = qualifier as DefaultQualifier;
        //            //action = q.action;
        //        }
        //        else
        //        {
        //            Debug.LogError(qualifier.GetType().BaseType + " is wrong Qualifier Type.");
        //        }

        //        //  Actions
        //        if (action.GetType().BaseType == typeof(ActionBase))
        //        {
        //            var a = action as ActionBase;
        //        }
        //        else if (action.GetType().BaseType == typeof(ActionSequence))
        //        {
        //            var a = action as ActionSequence;
        //            actionSequence = new IAction[a.actions.Count];
        //            for (int k = 0; k < a.actions.Count; k++)
        //            {
        //                actionSequence[k] = a.actions[k];
        //            }
        //        }
        //        else if (action.GetType().BaseType == typeof(ActionWithOptions<>))
        //        {
        //            //var typesList = typeof(ActionWithOptions<>).GetTypeInfo();
        //            //var a = action as ActionWithOptions<>;
        //        }
        //        else
        //        {
        //            Debug.LogError(action.GetType().BaseType + " is wrong Action Type.");
        //        }
        //    }

        //}


    }
}

