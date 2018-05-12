namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;



    public static class TaskNetworkUtilities
    {

        public static FieldInfo[] GetAllFields(object obj)
        {
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


        public static string GetAiCategoryName<T>()
        {
            Type type = typeof(T).BaseType;
            switch(type.ToString())
            {
                case "Selector":
                    return "Selector";
                case "QualifierBase":
                    return "Qualifier";
                case "ActionBase":
                    return "Action";
                case "ScorerBase":
                    return "Scorer";
                default:
                    return "<None>";
            }
        }


        /// <summary>
        /// Get all available classes of type.
        /// </summary>
        /// <returns> All available options. </returns>
        /// <typeparam name="T"> Is one the AI blocks. </typeparam>
        public static List<Type> GetAllOptions<T>() //where T: Selector, IQualifier, IAction, IScorer
        {
            List<Type> availableTypes = new List<Type>();
            Type type = typeof(T);
            //  Gets all custom Types in this assembly and adds it to a list.
            var optionTypes = Assembly.GetAssembly(type).GetTypes()
                                      .Where(t => t.IsClass && t.Namespace == typeof(UtilityAIManager).Namespace)
                                      .ToList();
            //optionTypes.ForEach(t => Debug.Log(t));

            foreach (Type option in optionTypes){
                if (option.BaseType == type)
                    availableTypes.Add(option);
            }
            //availableTypes.ForEach(t => Debug.Log(t));
            return availableTypes;
        }



    }















    #region DebugMessages

    public static class DebugEditorUtilities
    {

        /// <summary>
        /// Debugs the client info.
        /// </summary>
        /// <returns>The client info.</returns>
        /// <param name="taskNetwork">Task network.</param>
        public static string ClientStateInfo(TaskNetworkComponent taskNetwork)
        {
            string clientInfo = "";
            foreach (UtilityAIClient client in taskNetwork.clients)
            {
                clientInfo = client.ai + " | State: " + client.state + "\n";
                foreach (KeyValuePair<IQualifier, float> item in client.selectorResults)
                {
                    IQualifier qualifier = item.Key;
                    float score = item.Value;

                    var action = qualifier.action;
                    var actionName = action.GetType().Name;
                    if (action is ActionWithOptions<Vector3>)
                    {
                        var _action = action as ActionWithOptions<Vector3>;
                        action = _action;
                        //actionName = _action.name;
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
        public static string SelectorConfig(Selector selector)
        {
            //var selector = s.rootSelector;

            string selectorInfo = "Displays configuration of a Selector\n";
            //selectorInfo += string.Format("** taskNetwork Name: :  {0} **\n\n", s.GetType().Name);
            selectorInfo += string.Format("  Selector Type:  {0}\n\n", selector.GetType().Name);

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
                actionInfo += string.Format("{0}", qualifier.action.GetType().Name);


                selectorInfo += string.Format("  Qualifier:    {0}\n", qualifierInfo);
                selectorInfo += string.Format("  Action:       {0}\n", actionInfo);
                selectorInfo += string.Format("  Number of Scorers:  {0}\n", qualifier.scorers.Count);
                selectorInfo += scorerInfo;
                selectorInfo += "\n";
            }

            string defaultQualifierAction;
            if (selector.defaultQualifier.action != null)
                defaultQualifierAction = selector.defaultQualifier.action.GetType().Name;
            else
                defaultQualifierAction = "<None>";


            selectorInfo += string.Format("  DefaultQualifier:    {0}\n", selector.defaultQualifier);
            selectorInfo += string.Format("  Action:       {0}\n", defaultQualifierAction);
            selectorInfo += "\n";

            return selectorInfo;
        }



    }

    #endregion

}

