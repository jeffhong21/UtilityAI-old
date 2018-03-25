namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Composite action.
    /// </summary>
    public sealed class CompositeAction : IAction
    {
        public UtilityAIComponent utilityAIComponent { get; set; }
        
        //{ get; private set; }
        public List<IAction> _actions = new List<IAction>();

        public List<IAction> actions 
        {
            get{
                if (_actions == null){
                    _actions = new List<IAction>();
                }
                return _actions;
            }
            private set{ _actions = value; }
        }


        public void Execute(IContext context)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].Execute(context);
            }
        }

    }



}