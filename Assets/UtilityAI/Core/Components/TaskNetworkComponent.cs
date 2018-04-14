namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Random = UnityEngine.Random;

    /// <summary>
    /// TaskNetworkComponent
    /// </summary>

    public class TaskNetworkComponent : MonoBehaviour
    {

        public List<UtilityAIClient> clients = new List<UtilityAIClient>();
        public IContextProvider contextProvider { get; private set; }
        public IAIContext context { get; private set; }


        //public PositionScoreVisualizer visualizer { get; private set; }


        [Header(" -------- Debug -------- ")]
        public bool initializeWithTestAI;
        public bool debugNextIntervalTime;
        //public bool debugNavMesh;



        bool isExecuteRunning = true;
        //TaskNetworkDebugger taskNetworkDebugger;


        void Awake()
        {
            //if (visualizer == null)
                //visualizer = GetComponent<PositionScoreVisualizer>();

            //  Initialize Context
            if (contextProvider == null){
                contextProvider = gameObject.GetComponent(typeof(IContextProvider)) as IContextProvider;

                ContextProvider cp = contextProvider as ContextProvider;
                cp.context = new AIContext(GetComponent<Bang.NpcController>());
                context = cp.GetContext();
            }


            //if(taskNetworkDebugger == null)
                //taskNetworkDebugger = GetComponent<TaskNetworkDebugger>();
        }


        void OnEnable(){
            //if(initializeWithTestAI)
            //    InitializeTestAI();
            //else
                //Initialize();
            Initialize();
        }


        public void GetClient(Guid aiId){
            
        }


        public void Initialize()
        {
            foreach(UtilityAIClient client in clients)
            {
                client.Start();
                StartCoroutine(ExecuteUpdate(client));
            }
        }







        public IEnumerator ExecuteUpdate(UtilityAIClient client)
        {
            float nextInterval = 0f;
            IEnumerator activeAction = null;

            yield return new WaitForSeconds(Random.Range(client.startDelayMin, client.startDelayMax));

            while (isExecuteRunning)
            {   
                if (Time.time > nextInterval)
                {
                    client.Execute();
                    nextInterval = Time.time + Random.Range(client.intervalMin - 0.5f, client.intervalMax + 1f);

                    //if (client.SelectAction()){
                    //    activeAction = client.ExecuteAction();
                    //    yield return StartCoroutine(activeAction);
                    //    activeAction = null;


                    //    nextInterval = Time.time + Random.Range(client.intervalMin - 0.5f, client.intervalMax + 1f);

                    //    if (debugNextIntervalTime) Debug.Log("Current Time:  " + Time.time + " | Next interval in:  " + (nextInterval - Time.time));
                    //}
                    //else{
                    //    //Debug.Log("client has not selected action.");
                    //}

                }

                yield return null;
            }
        }



        [ContextMenu("Create new AI Client")]
        public void CreateAsset()
        {
            Debug.Log("Creating blank asset");
            //var aiClient = new UtilityAIAsset();
            //aiClient.CreateAsset("MockMoveAI", "New Utility AI");
        }











        #region For Testing AI
        //public void InitializeTestAI()
        //{
        //    var scanAi = new MockScanningAI("ScanningAI");
        //    var moveAi = new MockMoveAI("MovementAI");
        //    moveAi.visualizer = visualizer;
        //    //  * Initialize TestClient
        //    var scanAiClient = InitializeAI(scanAi);
        //    var moveAiClient = InitializeAI(moveAi, 1f, 1f, 0f, 0f);

        //    scanAiClient.debug = false;

        //    //  Starting all clients.
        //    foreach (UtilityAIClient client in clients)
        //    {
        //        //client.ActionSelected += taskNetworkDebugger.GetSelectorResults;
        //        client.Start();
        //        StartCoroutine(ExecuteUpdate(client));
        //    }
        //}

        //public UtilityAIClient InitializeAI(UtilityAI utilityAI, float iMin = 1f, float iMax = 1f, float sMin = 0f, float sMax = 0f)
        //{
        //    var aiClient = new UtilityAIClient(this, utilityAI, iMin, iMax, sMin, sMax);
        //    clients.Add(aiClient);
        //    return aiClient;
        //}
        #endregion





        //public IEnumerator _ExecuteUpdate(UtilityAIClient client)
        //{
        //    float nextInterval = 0f;
        //    IEnumerator activeAction = null;
        //    IEnumerator onRunning = null;

        //    yield return new WaitForSeconds(Random.Range(client.startDelayMin, client.startDelayMax));
        //    while(isExecuteRunning)
        //    {
        //        if (client.state == UtilityAIClientState.Stopped){
        //            if (Time.time > nextInterval)
        //            {
        //                //  **  For Debugging
        //                if(debugNavMesh){
        //                    onRunning = OnRunning(client);
        //                    StartCoroutine(onRunning);
        //                }

        //                activeAction = client.ExecuteAction();
        //                yield return StartCoroutine(activeAction);
        //                activeAction = null;

        //                //  **  For Debugging
        //                if(onRunning != null)
        //                    StopCoroutine(onRunning);

        //                nextInterval = Time.time + Random.Range(client.intervalMin - 0.5f, client.intervalMax + 1f);
        //                Debug.Log("Next interval in:  " + (nextInterval - Time.time));
        //                yield return null;
        //            }
        //        }
        //        else if (client.state == UtilityAIClientState.Running){
        //            yield return null;
        //        }
        //        else if (client.state == UtilityAIClientState.Pause)
        //        {
        //            while(client.state == UtilityAIClientState.Pause){
        //                yield return null;  //  Pause Coroutine.                            
        //            }
        //        }


        //        yield return null;
        //    }
        //}


        ///// <summary>
        ///// For Debugging Purposes
        ///// </summary>
        ///// <returns>The running.</returns>
        ///// <param name="client">Client.</param>
        //public IEnumerator OnRunning(UtilityAIClient client)
        //{
        //    while (isExecuteRunning)
        //    {
        //        if (client.state == UtilityAIClientState.Running)
        //        {
        //            AIContext aiContext = context as AIContext;
        //            if(aiContext.navMeshAgent.remainingDistance == Mathf.Infinity){
        //                Debug.Log(string.Format("PathPending: < {0} > | PathStatus: < {1} > | Path: < {2} >", aiContext.navMeshAgent.pathPending, aiContext.navMeshAgent.pathStatus, aiContext.navMeshAgent.path));
        //                // Debug.Break();
        //            }
        //            else{
        //                Debug.Log(string.Format("DistanceRemaining is: < {0} > | PathStatus:  < {1} > |  < {2} >", aiContext.navMeshAgent.remainingDistance.ToString("n4"), aiContext.navMeshAgent.pathStatus, Time.time));
        //            }
                    
        //            yield return new WaitForSeconds(1.5f);
        //        }

        //        yield return null;
        //    }
            
        //}









    }










}

