namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Collections.Generic;


    public class UtilityAIDebugger : MonoBehaviour
    {

        private UtilityAIComponent utilityAIComponent;

        private int[] values;
        private bool[] keys;


        void Awake()
        {
            utilityAIComponent = GetComponent<UtilityAIComponent>();


            values = (int[])System.Enum.GetValues(typeof(KeyCode));
            keys = new bool[values.Length];
        }


        void Start()
        {

        }

        void OnEnable()
        {

        }

        void Update()
        {

            DebugSelectorInfo();


        }



        public void DebugSelectorInfo()
        {
            for (int i = 0; i < values.Length; i++)
            {
                keys[i] = Input.GetKeyUp((KeyCode)values[i]);
                if (keys[i])
                {
                    var index = values[i] - 49;
                    if (utilityAIComponent.clients.Count() >= index + 1)
                    {
                        var selectorInfo = SelectorInfo(utilityAIComponent.clients[index].ai.rootSelector);
                        Debug.Log(selectorInfo);
                    }
                }
            }
        }


        public string SelectorInfo(Selector s)
        {
            string selectorInfo = "";
            //  Get Selector Name and Type.
            selectorInfo += s.GetType().ToString();
            selectorInfo += string.Format("Selector Type:  {0}\n\n", s.GetType().Name);

            for (int i = 0; i < s.qualifiers.Count(); i++)
            {
                var qualifier = s.qualifiers[i] as CompositeQualifier;
                string qualifierInfo = "";
                string scorerInfo = "";
                string actionInfo = "";

                qualifierInfo += string.Format(" | {0}", qualifier.GetType().Name);

                foreach (IScorer scorer in qualifier.scorers)
                {
                    scorerInfo += string.Format("    {0}\n", scorer.GetType().Name);
                }
                actionInfo += string.Format(" | {0}", qualifier.action.GetType().Name);

                selectorInfo += string.Format("Qualifier:  {0}\n", qualifierInfo);
                selectorInfo += string.Format("Qualifier Action:  {0}\n", actionInfo);
                selectorInfo += string.Format("Number of Scorers:  {0}\n", qualifier.scorers.Count());
                selectorInfo += scorerInfo;


            }

            return selectorInfo;
        }


    }
}

