namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public enum UtilityAIClientState
    {
        Running,  ///   The associated AI is running.
        Stopped,  ///   The associated AI is not running.
        Pause     ///   The associated AI is paused.
    }

    /// <summary>
    /// This is the decision maker.
    /// </summary>
    [Serializable]
    public class UtilityAIClient
    {
        private TaskNetworkComponent taskNetwork;
        [SerializeField]
        private UtilityAI _ai;
        public UtilityAI ai{
            get{
                return _ai;
            }
            set{
                _ai = value;
            }
        }


        [SerializeField]
        private UtilityAIClientState _state;
        public UtilityAIClientState state { get { return _state; } protected set { _state = value; } }

        [SerializeField]
        private float _intervalMin;
        public float intervalMin { get { return _intervalMin; } set { _intervalMin = value; } }
        [SerializeField]
        private float _intervalMax;
        public float intervalMax { get { return _intervalMax; } set { _intervalMax = value; } }
        [SerializeField]
        private float _startDelayMin;
        public float startDelayMin { get { return _startDelayMin; } set { _startDelayMin = value; } }
        [SerializeField]
        private float _startDelayMax;
        public float startDelayMax { get { return _startDelayMax; } set { _startDelayMax = value; } }

        [SerializeField] [HideInInspector]
        private AIContextProvider contextProvider;

        public IAction currentAction { get; protected set; }

        //  For Debuging.
        public Dictionary<IQualifier, float> selectorResults;




        public UtilityAIClient(Guid aiId, IContextProvider contextProvider) 
        {
            //this.ai = ai;
            this.contextProvider = contextProvider as AIContextProvider;

            this.intervalMin = this.intervalMax = 1f;
            this.startDelayMin = this.startDelayMax = 0f;
        }

        public UtilityAIClient(UtilityAI ai, IContextProvider contextProvider)
        {
            this.ai = ai;
            this.contextProvider = contextProvider as AIContextProvider;

            this.intervalMin = this.intervalMax = 1f;
            this.startDelayMin = this.startDelayMax = 0f;
            state = UtilityAIClientState.Stopped;
        }

        public UtilityAIClient(UtilityAI ai, IContextProvider contextProvider, float intervalMin, float intervalMax, float startDelayMin, float startDelayMax)
        {
            this.ai = ai;
            this.contextProvider = contextProvider as AIContextProvider;

            this.intervalMin = intervalMin;
            this.intervalMax = intervalMax;
            this.startDelayMin = startDelayMin;
            this.startDelayMax = startDelayMax;
            state = UtilityAIClientState.Stopped;
        }



        /// <summary>
        /// For Debugging
        /// </summary>
        public Dictionary<IQualifier, float> GetSelectorResults(IAIContext context, IList<IQualifier> qualifiers, IDefaultQualifier defaultQualifer)
        {
            if (selectorResults == null)
                selectorResults = new Dictionary<IQualifier, float>();
            else
                selectorResults.Clear();
            
            for (int index = 0; index < qualifiers.Count; index++){
                CompositeQualifier qualifier = qualifiers[index] as CompositeQualifier;
                var score = qualifier.Score(context, qualifier.scorers);
                selectorResults.Add(qualifier, score);
            }


            var dq = defaultQualifer as IQualifier;
            selectorResults.Add(dq, defaultQualifer.Score(context));

            return selectorResults;
        }



        public void Execute()
        {
            IAction newAction = ai.Select(contextProvider.GetContext());
            currentAction = newAction;
            //  For Debug
            selectorResults = GetSelectorResults(contextProvider.GetContext(), ai.rootSelector.qualifiers, ai.rootSelector.defaultQualifier);
            // --------------
            if (currentAction == null)
                return;

            //  Execute the current action.
            //Debug.Log("Executing " + currentAction.name);
            currentAction.ExecuteAction(contextProvider.GetContext());

        }



        //public void Execute2()
        //{
        //    IAction newAction = ai.Select(context);
        //    Debug.Log(string.Format("Current Action:  {0}\nNew Action: {1}", currentAction, newAction));
        //
        //    if (currentAction != newAction){
        //        currentAction = newAction;
        //        //currentAction.utilityAIComponent = taskNetwork;
        //        //if (ActionSelected != null){
        //        //    ActionSelected(context, ai.rootSelector.qualifiers);
        //        //    Debug.Log(ai.name + " just pinged debugger");
        //        //}
        //        selectorResults = GetSelectorResults(context, ai.rootSelector.qualifiers, ai.rootSelector.defaultQualifier);
        //    }
        //    else{
        //        return;
        //    }
        //
        //    if (currentAction == null)
        //        return;
        //   
        //    //  Execute the current action.
        //    currentAction.ExecuteAction(context);
        //    Debug.Log("Executing " + currentAction.GetType().Name);
        //}



        /// <summary>
        /// Called to initialize AIClient
        /// </summary>
        public void Start(){
            if (state != UtilityAIClientState.Stopped)
                return;

            state = UtilityAIClientState.Running;
            OnStart();
            //Debug.Log(string.Format("Executing action:  {0} | {1}", currentAction.GetType().Name, Time.time));
        }

        /// <summary>
        /// Called when action has finished executing.  Client remains in Stopped state until it performs a Execute().
        /// </summary>
        public void Stop(){
            if (state == UtilityAIClientState.Stopped)
                return;
            
            state = UtilityAIClientState.Stopped;
            OnStop();
            //Debug.Log(string.Format("{0} is finished executing.  | {1}", currentAction.GetType().Name, Time.time));
        }

        /// <summary>
        /// Called when the ai is performing a Execute().  Client stays in Running state until it finishes its action.
        /// </summary>
        public void Resume(){
            if (state != UtilityAIClientState.Pause)
                return;
            
            state = UtilityAIClientState.Running;
            OnResume();
        }


        public void Pause(){
            if (state != UtilityAIClientState.Running)
                return;
            
            state = UtilityAIClientState.Pause;
            OnPause();
        }






        protected virtual void OnStart()
        {

        }

        protected virtual void OnStop()
        {

        }

        protected virtual void OnPause()
        {

        }

        protected virtual void OnResume()
        {

        }





        ///// <summary>
        ///// Selects the action.
        ///// </summary>
        ///// <returns><c>true</c>, if action was selected, <c>false</c> otherwise.</returns>
        //public bool SelectAction()
        //{
        //    //  Check if Action is still running.
        //    if (IsActionStillRunning())
        //        return false;

        //    //  Select the action to be executed.
        //    currentAction = ai.Select(context);
        //    currentAction.utilityAIComponent = taskNetwork;
        //    //  For debugging.
        //    selectorResults = GetSelectorResults(context, ai.rootSelector.qualifiers);

        //    return currentAction != null;
        //}


        //private bool IsActionStillRunning(){
        //    if (currentAction == null)
        //        return false;
        //    return currentAction.actionStatus == ActionStatus.Running;
        //}


        ///// <summary>
        /////  This gets the best Action to perform and executes it.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerator ExecuteAction()
        //{
        //    if (currentAction == null)
        //        yield break;

        //    if(debug) Debug.Log(string.Format("{0} is executing action. |  {1}", ai.name, Time.time));
        //    //  Set the current state to "Running".
        //    Start();

        //    //  Execute the current action.
        //    currentAction.ExecuteAction(context);

        //    while(currentAction.actionStatus == ActionStatus.Running){
        //        if(currentAction.actionStatus != ActionStatus.Running)
        //            break;
        //        yield return null;
        //    }
        //    //Debug.Log("Action Done Executing at:  " + Time.time);
        //    Stop();
        //    yield return null;
        //}


    }
}
