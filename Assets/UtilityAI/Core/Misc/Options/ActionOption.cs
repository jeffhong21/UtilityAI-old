//namespace UtilityAI
//{
//    using UnityEngine;
//    using System;
//    using System.Collections.Generic;

//    /// <summary>
//    ///
//    /// </summary>
//    [Serializable]
//    public class ActionOption
//    {
//        public string Name;
//        /*    Temporary   */
//        public ActionEnum actionOption; // { get; set; }
//        public List<OptionScorerEnum> actionScorerOptions = new List<OptionScorerEnum>();


//        private IAction action;
//        //private IQualifier qualifier;
//        private List<IOptionScorer<Vector3>> scorers = new List<IOptionScorer<Vector3>>();


//        private Type optionType = typeof(Vector3);
//        private Type classType;


//        private UtilityAI aiClientMap;


//        public ActionOption(UtilityAI map, ActionOption data)
//        {
//            aiClientMap = map;

//            action = AIClientUtility.CreateInstance<IAction>(data.actionOption.ToString());
//            //  Create any OptionScorers associated with this Aciton.
//            scorers = AIClientUtility.CreateInstanceFromOptions<IOptionScorer<Vector3>, OptionScorerEnum>(data.actionScorerOptions);


//            classType = action.GetType().BaseType;
//            //Debug.Log(classType);
//            AddScorers(classType);
//        }


//        private void AddScorers(Type type)
//        {
//            if (type == typeof(ActionWithOptions<Vector3>))
//            {
//                //  Add OptionScorers
//                var a = action as ActionWithOptions<Vector3>;
//                a.scorers = scorers;
//                //Debug.Log(string.Format("Adding {0} OptionScorers to {1}.", scorers.Count, this.GetType().ToString()));

//                aiClientMap.AddToList(action);
//            }
//            else{
//                //Debug.Log(string.Format("Did not add OptionScorers to {0}.", this.GetType().ToString()));
//            }
//        }


//        public IAction GetAction(){
//            return action;
//        }

//    }
//}