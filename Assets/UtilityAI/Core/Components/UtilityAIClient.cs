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
    public class UtilityAIClient
    {
        private UtilityAIComponent agent;
        public IUtilityAI ai { get; private set; }

        [SerializeField]
        private UtilityAIClientState _state;
        /// <summary>
        /// The Current state of the client.
        /// </summary>
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


        private IContextProvider contextProvider;
        private IContext context;
        public IAction currentAction { get; protected set; }

        //  For Debuging.
        public bool debug;


        public UtilityAIClient(Guid aiId, IContextProvider contextProvider) {}

        public UtilityAIClient(UtilityAIComponent agent, IUtilityAI ai)
        {
            this.agent = agent;
            this.ai = ai;
            contextProvider = agent.contextProvider;
            context = agent.context;
            
            this.intervalMin = this.intervalMax = 1f;
            this.startDelayMin = this.startDelayMax = 0f;
            state = UtilityAIClientState.Stopped;
        }

        public UtilityAIClient(UtilityAIComponent agent, IUtilityAI ai, float intervalMin, float intervalMax, float startDelayMin, float startDelayMax)
        {
            this.agent = agent;
            this.ai = ai;
            contextProvider = agent.contextProvider;
            context = agent.context;

            this.intervalMin = intervalMin;
            this.intervalMax = intervalMax;
            this.startDelayMin = startDelayMin;
            this.startDelayMax = startDelayMax;
            state = UtilityAIClientState.Stopped;
        }


        /// <summary>
        /// Selects the action.
        /// </summary>
        /// <returns><c>true</c>, if action was selected, <c>false</c> otherwise.</returns>
        public bool SelectAction()
        {
            //  Check if Action is still running.
            if (IsActionStillRunning())
                return false;

            //  Select the action to be executed.
            currentAction = ai.Select(context);
            currentAction.utilityAIComponent = agent;
            return currentAction != null;
        }


        bool IsActionStillRunning(){
            if (currentAction == null)
                return false;
            return currentAction.actionStatus == ActionStatus.Running;
        }


        /// <summary>
        ///  This gets the best Action to perform and executes it.
        /// </summary>
        /// <returns></returns>
        public IEnumerator ExecuteAction()
        {
            if (currentAction == null)
                yield break;

            if(debug) Debug.Log(string.Format("{0} is executing action. |  {1}", ai.name, Time.time));
            //  Set the current state to "Running".
            Start();

            //  Execute the current action.
            currentAction.ExecuteAction(context);

            while(currentAction.actionStatus == ActionStatus.Running){
                if(currentAction.actionStatus != ActionStatus.Running)
                    break;
                yield return null;
            }
            //Debug.Log("Action Done Executing at:  " + Time.time);
            Stop();
            yield return null;
        }


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








    }
}
