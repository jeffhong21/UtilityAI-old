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
        [SerializeField]
        private List<UtilityAIClient> _clients = new List<UtilityAIClient>();
        public List<UtilityAIClient> clients
        {
            get{
                return _clients;
            }
            set{
                _clients = value;
            }
        }

        [SerializeField]
        private List<UtilityAIAsset> _assets = new List<UtilityAIAsset>();
        public List<UtilityAIAsset> assets{
            get{
                return _assets;
            }
            set{
                _assets = value;
                Debug.Log(string.Format("Clients Count:  {0}\nAssets Count: {1}", _clients.Count, _assets.Count));
            }
        }

        [SerializeField]
        private AIContextProvider _contextProvider;
        public AIContextProvider contextProvider {
            get{
                return _contextProvider;
            }
            set{
                _contextProvider = value;
            }
        }

        [SerializeField]
        public IAIContext _context;
        public IAIContext context {
            get{
                return _context;
            }
            protected set{
                _context = value;
            }
        }


        [Header(" -------- Debug -------- ")]
        public bool initializeAssetConfig;
        public bool debugNextIntervalTime;



        [HideInInspector] public bool showDefaultInspector, showDeleteAssetOption, selectAiAssetOnCreate;
        bool isExecuteRunning = true;



        void InitializeContext()
        {
            if (contextProvider == null){
                contextProvider = gameObject.GetComponent(typeof(IContextProvider)) as AIContextProvider;
                if(contextProvider != null){
                    contextProvider.context = new AIContext(GetComponent<AIEntityController>());
                    context = contextProvider.GetContext();
                }
                else{
                    Debug.LogError("----- No AIContextProvider -----");
                }
            }
            else if (context == null && contextProvider != null){
                if(contextProvider.context == null)
                    contextProvider.context = new AIContext(GetComponent<AIEntityController>());
                context = contextProvider.GetContext();
            }

        }


		void Awake()
        {
            //  Initialize Context
            //contextProvider = null;
            InitializeContext();

        }


		private void OnValidate()
		{
            InitializeContext();
		}


		void OnEnable()
        {
            InitializeContext();
            //Debug.Log("--  Initialize Context via OnEnable");

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
            if(initializeAssetConfig){
                foreach (UtilityAIAsset asset in assets){
                    asset.SetAssetConfig<MoveAIConfig>(asset);
                }
            }


            foreach(UtilityAIClient client in clients)
            {
                client.Start();
                StartCoroutine(ExecuteUpdate(client));
            }
        }


        public IEnumerator ExecuteUpdate(UtilityAIClient client)
        {
            float nextInterval = 0f;
            //IEnumerator activeAction = null;

            yield return new WaitForSeconds(Random.Range(client.startDelayMin, client.startDelayMax));

            while (isExecuteRunning)
            {   
                if (Time.time > nextInterval)
                {
                    client.Execute();
                    nextInterval = Time.time + Random.Range(client.intervalMin - 0.5f, client.intervalMax + 1f);
                    if (debugNextIntervalTime) Debug.Log("Current Time:  " + Time.time + " | Next interval in:  " + (nextInterval - Time.time));

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
		#endregion




	}
}

