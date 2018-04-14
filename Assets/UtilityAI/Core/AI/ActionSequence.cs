namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Composite action.
    /// </summary>
    public sealed class ActionSequence : ActionBase
    {

        private List<IAction> _actions = new List<IAction>();

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



        protected override void Execute(IAIContext context)
        {
            for (int i = 0; i < actions.Count; i++){
                actions[i].ExecuteAction(context);
            }
        }


        public ActionSequence(){
            
        }

        public ActionSequence(ActionSequence other){
            actions = other.actions;
        }


    }



}