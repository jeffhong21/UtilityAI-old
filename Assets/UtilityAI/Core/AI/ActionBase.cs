namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public abstract class ActionBase : IAction
    {
        public UtilityAIComponent utilityAIComponent {get; set;}

        public event Action OnEndAction;

        public void EndAction(bool debug = false){
            if(debug) Debug.Log("Ending Action");
            OnEndAction();
        }

    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public abstract void Execute(IContext context);

    }



    public abstract class ActionBase<TContext> : IAction
    {
        public UtilityAIComponent utilityAIComponent { get;  set; }

        /// <summary>
        /// Execute methode for custom Context type.
        /// </summary>
        /// <param name="context"></param>
        public abstract void Execute(TContext context);

        /// <summary>
        /// Default execute for generic context.
        /// </summary>
        /// <param name="context"></param>
        public virtual void Execute(IContext context){
            
        }
    }

}