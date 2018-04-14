namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;



    public static class TaskNetworkEditorUtilities
    {

        public static FieldInfo[] GetAllFields(object obj){
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return fields;
        }

        public static PropertyInfo[] GetAllProperties(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return properties;
        }


    }

















    public static class DebugEditorUtilities
    {

        /// <summary>
        /// Debugs the client info.
        /// </summary>
        /// <returns>The client info.</returns>
        /// <param name="taskNetwork">Task network.</param>
        public static string DebugClientInfo(TaskNetworkComponent taskNetwork)
        {
            string clientInfo = "";
            foreach (UtilityAIClient client in taskNetwork.clients)
            {
                clientInfo = client.ai.name + " | State: " + client.state + "\n";
                foreach (KeyValuePair<CompositeQualifier, float> item in client.selectorResults)
                {
                    CompositeQualifier qualifier = item.Key;
                    float score = item.Value;

                    var action = qualifier.action;
                    var actionName = action.GetType().Name;
                    if (action is ActionWithOptions<Vector3>)
                    {
                        var _action = action as ActionWithOptions<Vector3>;
                        action = _action;
                        actionName = _action.name;
                    }



                    if (client.currentAction == action)
                        clientInfo += string.Format(" <b>Qualifier:</b> {0} | <b>Score:</b>: <color=lime>{1}</color>\n <b>Action:</b>:  <color=lime>{2}</color>\n", qualifier.GetType().Name, score, actionName);
                    else
                        clientInfo += string.Format(" <b>Qualifier:</b> {0} | <b>Score:</b>: {1}\n <b>Action:</b>:  {2}\n", qualifier.GetType().Name, score, actionName);
                }

            }

            return clientInfo;
        }


        /// <summary>
        /// Debugs the selector info.
        /// </summary>
        /// <returns>The selector info.</returns>
        /// <param name="s">S.</param>
        public static string DebugSelectorInfo(UtilityAI s)
        {
            var selector = s.rootSelector;

            string selectorInfo = "";
            selectorInfo += string.Format("** taskNetwork Name: :  {0} **\n\n", s.GetType().Name);
            selectorInfo += string.Format("  Selector Type:  {0}\n", selector.GetType().Name);

            //  Get Selector Name and Type.
            for (int i = 0; i < selector.qualifiers.Count; i++)
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
                //actionInfo += string.Format("{0}  |  status:  {1}", qualifier.action.GetType().Name, qualifier.action.actionStatus);



                selectorInfo += string.Format("  Qualifier:    {0}\n", qualifierInfo);
                //selectorInfo += string.Format("  Action:       {0}\n", actionInfo);
                selectorInfo += string.Format("  Number of Scorers:  {0}\n", qualifier.scorers.Count);
                selectorInfo += scorerInfo;
                selectorInfo += "\n";
            }

            return selectorInfo;
        }




    }

}

