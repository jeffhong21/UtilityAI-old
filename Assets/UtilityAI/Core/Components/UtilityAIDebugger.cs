namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Collections.Generic;


    public class UtilityAIDebugger : MonoBehaviour
    {

        UtilityAIComponent utilityAIComponent;

        //[SerializeField]
        private bool toggleDisplay;
        //[SerializeField]
        private int clientIndex;


        private int[] values;
        private bool[] keys;


        void Awake()
        {
            utilityAIComponent = GetComponent<UtilityAIComponent>();

            values = (int[])System.Enum.GetValues(typeof(KeyCode));
            keys = new bool[values.Length];
        }


        void Update()
        {
            
            for (int i = 0; i < values.Length; i++){
                keys[i] = Input.GetKeyUp((KeyCode)values[i]);
                if (keys[i]){
                    var index = values[i] - 49;
                    if (utilityAIComponent.clients.Count() >= index + 1){
                        if (toggleDisplay && clientIndex == index)
                            toggleDisplay = false;
                        else if(toggleDisplay == false && clientIndex != index){
                            toggleDisplay = true;
                            clientIndex = index;
                        }
                        else if (toggleDisplay && clientIndex != index)
                            clientIndex = index;
                        else
                            toggleDisplay = !toggleDisplay;
                    }
                }
            }

            if(Input.GetKeyUp(KeyCode.Q)){
                toggleDisplay = false;
            }

        }


        void OnGUI()
        {
            if(toggleDisplay && clientIndex < utilityAIComponent.clients.Count()){
                AiClientDisplay(clientIndex);
            }


        }


        void AiClientDisplay(int index)
        {
            var selectorInfo = SelectorInfo(utilityAIComponent.clients[index].ai);

            GUILayout.BeginArea(new Rect(5f, 5f, 350, 500), GUI.skin.box);
            GUILayout.Label(selectorInfo);
            GUILayout.EndArea();
        }


        public void DebugSelectorInfo()
        {
            for (int i = 0; i < values.Length; i++){
                keys[i] = Input.GetKeyUp((KeyCode)values[i]);
                if (keys[i]){
                    var index = values[i] - 49;
                    if (utilityAIComponent.clients.Count() >= index + 1){
                        var selectorInfo = SelectorInfo(utilityAIComponent.clients[index].ai);
                        Debug.Log(selectorInfo);
                        Debug.Break();
                    }
                }
            }
        }


        public string ClientInfo()
        {
            string clientInfo = "";
            //utilityAIComponent.clients
            return clientInfo;
        }


        public string SelectorInfo(IUtilityAI s)
        {
            var selector = s.rootSelector;

            string selectorInfo = "";
            selectorInfo += string.Format("** Ai Name: :  {0} **\n\n", s.GetType().Name);
            selectorInfo += string.Format("  Selector Type:  {0}\n", selector.GetType().Name);

            //  Get Selector Name and Type.
            for (int i = 0; i < selector.qualifiers.Count(); i++)
            {
                var qualifier = selector.qualifiers[i] as CompositeQualifier;
                string qualifierInfo = "";
                string scorerInfo = "";
                string actionInfo = "";

                qualifierInfo += string.Format("{0}", qualifier.GetType().Name);

                foreach (IScorer scorer in qualifier.scorers)
                {
                    scorerInfo += string.Format("    - {0}\n", scorer.GetType().Name);
                }
                actionInfo += string.Format("{0}  |  status:  {1}", qualifier.action.GetType().Name, qualifier.action.actionStatus);



                selectorInfo += string.Format("  Qualifier:    {0}\n", qualifierInfo);
                selectorInfo += string.Format("  Action:       {0}\n", actionInfo);
                selectorInfo += string.Format("  Number of Scorers:  {0}\n", qualifier.scorers.Count());
                selectorInfo += scorerInfo;
                selectorInfo += "\n";
            }

            return selectorInfo;
        }



        public string ClientStatus(UtilityAIClient client)
        {
            string clientInfo = "";

            return clientInfo;
        }


    }
}

