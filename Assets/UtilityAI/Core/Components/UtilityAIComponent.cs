namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Random = UnityEngine.Random;

    public class UtilityAIComponent : MonoBehaviour
    {

        //  public aiConfig;
        public List<UtilityAIClient> clients = new List<UtilityAIClient>();

        public bool debugNavMesh;


        private IContextProvider contextProvider;
        private IContext context;

        bool isExecuteRunning = true;
        
        void Awake()
        {
            if (contextProvider == null){
                contextProvider = gameObject.GetComponent(typeof(IContextProvider)) as IContextProvider;

                var _contextProvider = contextProvider as ContextProvider;
                _contextProvider.context = new AIContext(GetComponent<Bang.NpcController>());
                context = _contextProvider.GetContext();
            }

        }


        void OnEnable(){
            Initialize();
        }

        public void GetClient(Guid aiId) { }

        public void Initialize()
        {
            //  * Initialize TestClient
            InitializeTestAIClients();

            //  Starting all clients.
            foreach (UtilityAIClient client in clients)
            {
                client.Start();
                var startDelay = Random.Range(client.startDelayMin, client.startDelayMax);
                if(Time.time >= startDelay){
                    StartCoroutine(ExecuteUpdate(client));
                }
            }
        }


        


        public IEnumerator ExecuteUpdate(UtilityAIClient client)
        {
            float nextInterval = 0f;
            IEnumerator activeAction = null;
            IEnumerator onRunning = null;

            yield return new WaitForSeconds(Random.Range(client.startDelayMin, client.startDelayMax));
            while(isExecuteRunning)
            {
                if (client.state == UtilityAIClientState.Stopped){
                    if (Time.time > nextInterval)
                    {
                        //  **  For Debugging
                        if(debugNavMesh){
                            onRunning = OnRunning(client);
                            StartCoroutine(onRunning);
                        }

                        activeAction = client.Execute();
                        yield return StartCoroutine(activeAction);

                        //  **  For Debugging
                        if(onRunning != null)
                            StopCoroutine(onRunning);

                        nextInterval = Time.time + Random.Range(client.intervalMin - 0.5f, client.intervalMax + 1f);
                        Debug.Log("Next interval in:  " + (nextInterval - Time.time));
                        yield return null;
                    }
                }
                else if (client.state == UtilityAIClientState.Running){
                    yield return null;
                }
                else if (client.state == UtilityAIClientState.Pause)
                {
                    while(client.state == UtilityAIClientState.Pause){
                        yield return null;  //  Pause Coroutine.                            
                    }
                }


                yield return null;
            }
        }


        public IEnumerator OnRunning(UtilityAIClient client)
        {
            while (isExecuteRunning)
            {
                if (client.state == UtilityAIClientState.Running)
                {
                    AIContext aiContext = context as AIContext;
                    if(aiContext.navMeshAgent.remainingDistance == Mathf.Infinity){
                        Debug.Log(string.Format("PathPending: < {0} > | PathStatus: < {1} > | Path: < {2} >", aiContext.navMeshAgent.pathPending, aiContext.navMeshAgent.pathStatus, aiContext.navMeshAgent.path));
                        // Debug.Break();
                    }
                    else{
                        Debug.Log(string.Format("DistanceRemaining is: < {0} > | PathStatus:  < {1} > |  < {2} >", aiContext.navMeshAgent.remainingDistance.ToString("n4"), aiContext.navMeshAgent.pathStatus, Time.time));
                    }
                    
                    yield return new WaitForSeconds(1.5f);
                }

                yield return null;
            }
            
        }








        public void InitializeTestAIClients()
        {
            var testAIClient = new Test_UtilityAIClient(this, new UtilityAI(), contextProvider);
            clients.Add(testAIClient);

            foreach (UtilityAIClient c in clients){
                if (c.GetType() == typeof(Test_UtilityAIClient)){
                    var test = c as Test_UtilityAIClient;
                    test.InitializeTestClient();
                }
            }


        }


    }










}

