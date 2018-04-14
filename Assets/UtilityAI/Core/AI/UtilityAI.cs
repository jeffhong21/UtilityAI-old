namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// UtilityAI creates the rootSelector and initializes the tree.
    /// </summary>
    public interface IUtilityAI //: ISerializationCallbackReceiver
    {
        void AddSelector(Selector s);
        void RemoveSelector(Selector s);
        bool ReplaceSelector(Selector current, Selector replacement);
        Selector FindSelector(Selector s);
        IAction Select(IAIContext context);
        void RegenerateIds();
        void PrepareForSerialize();
        void InitializeAfterDeserialize(System.Object rootObject);
        Guid id { get; }
        string name { get; set; }
        Selector rootSelector { get; set; }
        int selectorCount { get; }
        Selector Item { get; }
    }


    [Serializable]
    public class UtilityAI// : IUtilityAI
    {
        public Guid id { get; private set; }
        public string name { get; set; }
        public Selector rootSelector { get; set; }

        public int selectorCount { get; private set; }
        //  Get Selector with specific index.
        public Selector Item { get; private set; }

        public UtilityAI()
        {
            rootSelector = new ScoreSelector();
            RegenerateIds();
        }

        public UtilityAI(string aiName)
        {
            rootSelector = new ScoreSelector();
            name = aiName;
            RegenerateIds();
        }

        public void AddSelector(Selector s)
        {
            throw new NotImplementedException();
        }

        public Selector FindSelector(Selector s)
        {
            throw new NotImplementedException();
        }

        public void RemoveSelector(Selector s)
        {
            throw new NotImplementedException();
        }

        public bool ReplaceSelector(Selector current, Selector replacement)
        {
            throw new NotImplementedException();
        }

        public void RegenerateIds(){
            id = Guid.NewGuid();
            //Debug.Log(id);
        }


        /// <summary>
        /// Selects the action for execution.
        /// </summary>
        /// <returns>The select.</returns>
        /// <param name="context">Context.</param>
        public IAction Select(IAIContext context)
        {
            List<IQualifier> qualifiers = rootSelector.qualifiers;
            IDefaultQualifier defaultQualifier = rootSelector.defaultQualifier;
            IQualifier winner = rootSelector.Select(context, qualifiers, defaultQualifier);

            CompositeQualifier cq = winner as CompositeQualifier;
            // TODO:  What if there are no scoreres?
            //float score = cq.Score(context, cq.scorers);
            IAction action = winner.action;



            return action;
        }






        public void PrepareForSerialize()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Part of ISerializationCallbackReceiver.
        /// </summary>
        public void OnBeforeSerialize()
        {
            PrepareForSerialize();
            //throw new NotImplementedException();
        }


        public void InitializeAfterDeserialize(object rootObject)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Part of ISerializationCallbackReceiver.
        /// </summary>
        public void OnAfterDeserialize()
        {
            // InitializeAfterDeserialize(object rootObject);
            //throw new NotImplementedException();
        }
    }
}

