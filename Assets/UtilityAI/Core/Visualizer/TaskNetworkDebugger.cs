//namespace UtilityAI
//{
//    using UnityEngine;
//    using System;
//    using System.Linq;
//    using System.Collections.Generic;


//    public class TaskNetworkDebugger : MonoBehaviour
//    {

//        private TaskNetworkComponent taskNetwork;

//        private Dictionary<CompositeQualifier, float> selectorResults = new Dictionary<CompositeQualifier, float>();

//        private GUIStyle textStyle;


//        private bool toggleDisplay;
//        private bool toggleAiDisplay;
//        private int clientIndex;


//        private int[] values;
//        private bool[] keys;


//        void Awake()
//        {
//            taskNetwork = GetComponent<TaskNetworkComponent>();

//            values = (int[])System.Enum.GetValues(typeof(KeyCode));
//            keys = new bool[values.Length];



//            textStyle = new GUIStyle();
//            textStyle.normal.textColor = Color.white;
//            textStyle.fontSize = 12;
//            textStyle.richText = true;
//        }


//        void Update()
//        {
//            for (int i = 0; i < values.Length; i++){
//                keys[i] = Input.GetKeyUp((KeyCode)values[i]);
//                if (keys[i]){
//                    var index = values[i] - 49;
//                    if (taskNetwork.clients.Count() >= index + 1){
//                        if (toggleDisplay && clientIndex == index)
//                            toggleDisplay = false;
//                        else if(toggleDisplay == false && clientIndex != index){
//                            toggleDisplay = true;
//                            clientIndex = index;
//                        }
//                        else if (toggleDisplay && clientIndex != index)
//                            clientIndex = index;
//                        else
//                            toggleDisplay = !toggleDisplay;
//                    }
//                }
//            }

//            if(Input.GetKeyUp(KeyCode.Q)){
//                toggleAiDisplay = !toggleAiDisplay;
//            }

//        }


//        void OnGUI()
//        {
//            //if(toggleDisplay && clientIndex < taskNetwork.clients.Count()){
//            //    AiClientDisplay(clientIndex);
//            //}

//            if (toggleAiDisplay && taskNetwork.clients.Any())
//                TaskNetworkInfo();

//        }



//        private void AiClientDisplay(int index)
//        {
//            var selectorInfo = SelectorInfo(taskNetwork.clients[index].ai);

//            GUILayout.BeginArea(new Rect(5f, 5f, 350, 500), GUI.skin.box);
//            GUILayout.Label(selectorInfo);
//            GUILayout.EndArea();
//        }




//        private void TaskNetworkInfo( )
//        {
//            GUILayout.BeginArea(new Rect(5f, 5f, Screen.width * 0.2f, Screen.height * 0.5f), GUI.skin.box);

//            foreach(UtilityAIClient client in taskNetwork.clients)
//            {
//                string clientInfo = client.ai.name + " | State: " + client.state + "\n";
//                //var lastAction = client.currentAction;
//                //if(client.currentAction != lastAction)
//                    //selectorResults = GetSelectorResults(taskNetwork.context, client.taskNetwork.rootSelector.qualifiers);

//                foreach (KeyValuePair<CompositeQualifier, float> item in client.selectorResults)
//                {
//                    CompositeQualifier qualifier = item.Key;
//                    float score = item.Value;

//                    var action = qualifier.action;
//                    var actionName = action.GetType().Name;
//                    if (action is ActionWithOptions<Vector3>){
//                        var _action = action as ActionWithOptions<Vector3>;
//                        action = _action;
//                        actionName = _action.name;
//                    }
                        
                    

//                    if (client.currentAction == action)
//                        clientInfo += string.Format(" <b>Qualifier:</b> {0} | <b>Score:</b>: <color=lime>{1}</color>\n <b>Action:</b>:  <color=lime>{2}</color>\n", qualifier.GetType().Name, score, actionName);
//                    else
//                        clientInfo += string.Format(" <b>Qualifier:</b> {0} | <b>Score:</b>: {1}\n <b>Action:</b>:  {2}\n", qualifier.GetType().Name, score, actionName);
//                }

//                GUILayout.Label(clientInfo, textStyle);
//                GUILayout.Space(16);
//            }


//            GUILayout.EndArea();

//        }


//        private void GetSelectorResults(IAIContext context, List<IQualifier> qualifiers)
//        {
//            selectorResults.Clear();
//            for (int index = 0; index < qualifiers.Count; index++){
//                CompositeQualifier qualifier = qualifiers[index] as CompositeQualifier;
//                var score = qualifier.Score(context, qualifier.scorers);
//                selectorResults.Add(qualifier, score);
//            }

//            //return selectorResults;
//        }



//        public string SelectorInfo(UtilityAI s)
//        {
//            var selector = s.rootSelector;

//            string selectorInfo = "";
//            selectorInfo += string.Format("** taskNetwork Name: :  {0} **\n\n", s.GetType().Name);
//            selectorInfo += string.Format("  Selector Type:  {0}\n", selector.GetType().Name);

//            //  Get Selector Name and Type.
//            for (int i = 0; i < selector.qualifiers.Count(); i++)
//            {
//                var qualifier = selector.qualifiers[i] as CompositeQualifier;
//                string qualifierInfo = "";
//                string scorerInfo = "";
//                string actionInfo = "";

//                qualifierInfo += string.Format("{0}", qualifier.GetType().Name);

//                foreach (IScorer scorer in qualifier.scorers)
//                {
//                    scorerInfo += string.Format("    - {0}\n", scorer.GetType().Name);
//                }
//                actionInfo += string.Format("{0}  |  status:  {1}", qualifier.action.GetType().Name, qualifier.action.actionStatus);



//                selectorInfo += string.Format("  Qualifier:    {0}\n", qualifierInfo);
//                selectorInfo += string.Format("  Action:       {0}\n", actionInfo);
//                selectorInfo += string.Format("  Number of Scorers:  {0}\n", qualifier.scorers.Count());
//                selectorInfo += scorerInfo;
//                selectorInfo += "\n";
//            }

//            return selectorInfo;
//        }



//        public string ClientStatus(UtilityAIClient client)
//        {
//            string clientInfo = "";
//            return clientInfo;
//        }



//    }
//}

