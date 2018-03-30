//namespace UtilityAI
//{
//    using UnityEngine;
//    using System;
//    using System.Collections.Generic;



//    [Serializable]
//    public class QualifierOption
//    {
//        //  The Qualifier.  Not used in PropertyDrawer.  Doesn't have a value until it is created.
//        public IQualifier Qualifier;

//        //  Name of the Qualifier.
//        public string Name;
//        //  The action  associated with the Qualifier.
//        public ActionOption ActionOption;
//        //  The Scorers associated with the Qualifier.
//        public List<IScorer> Scorers;




//        /*    Temporary   */
//        public List<ScorerEnum> scorerOptions = new List<ScorerEnum>();         


//        private UtilityAI aiClientMap;


//        /// <summary>
//        /// Initializes a new instance of the <see cref="T:UtilityAI.QualifierOption"/> class.
//        /// </summary>
//        /// <param name="data">Data.</param>
//        public QualifierOption(UtilityAI map, Selector selector, QualifierOption data)
//        {
//            aiClientMap = map;
//            //  Create Qualifier.
//            Qualifier = AIClientUtility.CreateInstance<IQualifier>(AIClientNames.SumOfChildren);
//            //  Create the action.
//            ActionOption = new ActionOption(aiClientMap, data.ActionOption);
//            //  Creating the Scorers associated wit this Qualifier.
//            Scorers = new List<IScorer>();
//            Scorers = AIClientUtility.CreateInstanceFromOptions<IScorer, ScorerEnum>(data.scorerOptions);


//            Name = GetName(data.Name);
//            //  Type of the Qualifier.  Use this to get the class name.
//            var baseType = Qualifier.GetType().BaseType;
//            //  Set the properties of this Qualifier.
//            SetProperties(baseType, selector, data);
//        }


//        private string GetName(string data)
//        {
//            if (String.IsNullOrEmpty(data))
//                return Qualifier.GetType().ToString();
//            else
//                return data;
//        }


//        private void SetProperties(Type type, Selector selector, QualifierOption data)
//        {
//            //  Add this Qualifier to the correct Selector.
//            selector.AddQualifier(Qualifier);
//            //  Set Qualifier action.
//            Qualifier.action = ActionOption.GetAction();


//            if (type == typeof(QualifierBase))
//            {
//                var q = Qualifier as QualifierBase;
//                q.AddScorers(Scorers);
//                q.NameID = Name;


//                //Debug.Log(q.NameID + ":  After properties are set." + q.Scorers.Count);
//                //Debug.Log(string.Format("Adding {0} Properties to {1}.", Scorers.Count, this.GetType().ToString()));
//            }
//            else
//            {
//                //Debug.Log(string.Format("Did not add Scorers to {0}.", this.GetType().ToString()));
//            }
//        }






//        private T CreateInstance<T>(string instance)  //  Another argument could be object[] args
//        {
//            var _name = instance;
//            var _type = Type.GetType(this.GetType().Namespace + "." + _name);


//            //object _args = new object[] {args};
//            var _object = (T)Activator.CreateInstance(_type);

//            //return (T)Convert.ChangeType(_selector, typeof(T));
//            return _object;
//        }

//        private T1 CreateInstanceFromOption<T1, T2>(T2 option)
//        {
//            var optionName = option.ToString();
//            T1 returnObj = CreateInstance<T1>(optionName);

//            return returnObj;
//        }

//        private List<T1> CreateInstanceFromOptions<T1, T2> (List<T2> options)
//        {
//            List<T1> _scorers = new List<T1>();

//            for (int index = 0; index < options.Count; index++)
//            {
//                var optionName = options[index].ToString();
//                var scorer = CreateInstance<T1>(optionName);

//                _scorers.Add(scorer);
//            }

//            return _scorers;
//        }





//    }



//}