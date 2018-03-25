namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public enum UtilityAIClientState
    {
        Unregistered,
        Running,
        Stopped,
        Pause
    }
    [Serializable]
    public class UtilityAIClient
    {
        private UtilityAIComponent agent;
        public IUtilityAI ai { get; private set; }

        [SerializeField]
        private UtilityAIClientState _state;
        /// <summary>
        /// The Current state of the client.
        /// </summary>
        public UtilityAIClientState state { get { return _state; } set { _state = value; } }

        [SerializeField]
        private float _intervalMin;
        /// <summary>
        /// 
        /// </summary>
        public float intervalMin { get { return _intervalMin; } set { _intervalMin = value; } }
        [SerializeField]
        private float _intervalMax;
        /// <summary>
        /// 
        /// </summary>
        public float intervalMax { get { return _intervalMax; } set { _intervalMax = value; } }
        [SerializeField]
        private float _startDelayMin;
        /// <summary>
        /// 
        /// </summary>
        public float startDelayMin { get { return _startDelayMin; } set { _startDelayMin = value; } }
        [SerializeField]
        private float _startDelayMax;
        /// <summary>
        /// 
        /// </summary>
        public float startDelayMax { get { return _startDelayMax; } set { _startDelayMax = value; } }


        private IContextProvider contextProvider;
        private IContext context;


        public IAction currentAction { get; private set; }


        public UtilityAIClient(Guid aiId, IContextProvider contextProvider) {}

        public UtilityAIClient(UtilityAIComponent agent, IUtilityAI ai, IContextProvider contextProvider)
        {
            this.agent = agent;
            this.ai = ai;
            this.contextProvider = contextProvider;
            context = contextProvider.GetContext();
            
            this.intervalMin = this.intervalMax = 1f;
            this.startDelayMin = this.startDelayMax = 0f;
            state = UtilityAIClientState.Unregistered;
        }

        public UtilityAIClient(IUtilityAI ai, IContextProvider contextProvider, float intervalMin, float intervalMax, float startDelayMin, float startDelayMax)
        {
            this.ai = ai;
            this.contextProvider = contextProvider;
            context = contextProvider.GetContext();

            this.intervalMin = intervalMin;
            this.intervalMax = intervalMax;
            this.startDelayMin = startDelayMin;
            this.startDelayMax = startDelayMax;
            state = UtilityAIClientState.Unregistered;
        }


        /// <summary>
        /// This gets the best Action to perform and executes it.
        /// </summary>
        /// <returns></returns>
        public IEnumerator Execute()
        {
            //  Select the action to be executed.
            currentAction = ai.Select(context);
            currentAction.utilityAIComponent = agent;
            //  Set the current state to "Running".
            Resume();
            //  Execute the current action.
            currentAction.Execute(context);

            while(state == UtilityAIClientState.Running){
                if(state == UtilityAIClientState.Stopped)
                    break;
                yield return null;
            }

            yield return null;
        }

        /// <summary>
        /// Called to initialize AIClient
        /// </summary>
        public void Start()
        {
            if(state == UtilityAIClientState.Unregistered){
                foreach (IQualifier qualifier in ai.rootSelector.qualifiers){
                    ActionBase action = qualifier.action as ActionBase;
                    action.OnEndAction += Stop;
                }
            }
            state = UtilityAIClientState.Stopped;
            OnStart();
        }

        /// <summary>
        /// Called when action has finished executing.  Client remains in Stopped state until it performs a Execute().
        /// </summary>
        public void Stop()
        {
            state = UtilityAIClientState.Stopped;
            OnStop();
        }

        /// <summary>
        /// Called when the ai is performing a Execute().  Client stays in Running state until it finishes its action.
        /// </summary>
        public void Resume()
        {
            state = UtilityAIClientState.Running;
            OnResume();
        }

        public void Pause()
        {
            state = UtilityAIClientState.Pause;
            OnPause();
        }

        
        private void UnRegister()
        {
            foreach (IQualifier qualifier in ai.rootSelector.qualifiers){
                ActionBase action = qualifier.action as ActionBase;
                action.OnEndAction -= Pause;
            }
        }


        protected virtual void OnStart()
        {
            //Debug.Log(string.Format("Executing action:  {0} | {1}", currentAction.GetType().Name, Time.time));
        }

        protected virtual void OnStop()
        {
            //Debug.Log(string.Format("{0} is finished executing.  | {1}", currentAction.GetType().Name, Time.time));
            //Debug.Log("Current State:  " + state);
        }

        protected virtual void OnPause()
        {
            Debug.Log("Current State:  " + state);
        }

        protected virtual void OnResume()
        {
            //Debug.Log("Current State:  " + state);
        }








    }
}
