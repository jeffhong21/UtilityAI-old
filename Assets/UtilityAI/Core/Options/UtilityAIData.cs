namespace UtilityAI
{

    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Reflection;
    using System.Collections.Generic;



    [Serializable]
    public class SelectorOption
    {
        [HideInInspector]
        public bool isSelected;
        public string nameID = "Selector";
        //public List<QualifierOption> qualifiers = new List<QualifierOption>();
        public QualifierOption[] qualifiers;
        /*  Default Qualifier should be created here.   */

        [HideInInspector]
        public string logicType = AIClientNames.HighestScoringQualifier;
        public Type type = typeof(ScoreSelector);


    }




    /// <summary>
    /// For if a Qualifier has an action or points to another Selector.
    /// </summary>
    public enum OutputType
    {
        action,
        Selector,
        None
    }

    [Serializable]
    public class QualifierOption
    {
        
        [HideInInspector]
        public bool isSelected;
        //  Name of the Qualifier.  Used for the editor
        public string nameID = "Qualifier";
        //  The action associated with the Qualifier.
        public ActionOption actionOption;
        /*    Temporary   */
        public List<ScorerEnum> _scorers = new List<ScorerEnum>();
        //  The Scorers associated with the Qualifier.
        public ScorersOption[] scorersOption;


        /* Need to know what class type.  */
        [HideInInspector]  //  What type of Qualifier?
        public string logicType;
        public Type type;


        /* Need to all public fields.  */
        [HideInInspector]  //  All fieldInfo.
        public string[] allFields;
        public FieldInfo[] FieldInfo { get; private set; }


        [HideInInspector]  //  Does this Qualifier lead to an action or a Selector.
        public OutputType outputType = OutputType.action;



        public Type GetLogicType(string class_name)
        {
            var _type = UtilityAIManager.GetTypeFromString(class_name);
            type = _type != null ? _type : typeof(CompositeScoreQualifier);
            return type;
        }


        public QualifierOption(Type type)
        {
            Debug.Log("Constructing Qualifier Option");
            this.isSelected = false;
            this.nameID = type.ToString().Replace(typeof(UtilityAIClient).Namespace + ".", "");
            this.logicType = type.ToString();
            this.type = type;
            this.FieldInfo = UtilityAIManager.GetFieldInfoOfType(this.type);
        }

    }


    [Serializable]
    public class ScorersOption
    {
        [HideInInspector]
        public bool isSelected;
        public string nameID = "Scorers";

        [HideInInspector]
        public string logicType;
        public Type type;

        public ScorersOption(Type type)
        {
            Debug.Log("Constructing Scorers Option");
            this.isSelected = false;
            this.nameID = type.ToString().Replace(typeof(UtilityAIClient).Namespace + ".", "");
            this.logicType = type.ToString();
            this.type = type;
        }
    }







    [Serializable]
    public class ActionOption
    {
        [HideInInspector]
        public bool isSelected;
        public string nameID = "action";
        /*    Temporary   */
        public ActionEnum actionType; // Option selection for what kind of action.
        /*    Temporary   */
        public List<OptionScorerEnum> optionScorers = new List<OptionScorerEnum>(); // Option selection for what kind of OptionScorer.


        //  What type of action?  ActionBase, ActionWithOptions, ContextualAction
        [HideInInspector]
        public string logicType = typeof(ActionBase).ToString();
        public Type type = typeof(ActionBase);
        //[HideInInspector]
        //public List<IOptionScorer<Vector3>> optionScorers = new List<IOptionScorer<Vector3>>();
    }



    /// <summary>
    /// Data class to hold the score of the TOption.
    /// e.g This could hold a Vector3 position data and the score associated with that position.
    /// </summary>
    [Serializable]
    public class ScoredOption<TOption> : IComparable<ScoredOption<TOption>>
    {
        public TOption option;
        public float score;

        public ScoredOption(TOption _option, float _score)
        {
            option = _option;
            score = _score;
        }

        public int CompareTo(ScoredOption<TOption> other)
        {
            //  Current instance is greater than object being compared too.
            if (other == null) return 1;

            return this.score.CompareTo((other.score));
        }
    }





    public enum ActionEnum
    {
        ScanForEntities,
        ScanForPositions,
        ExampleMoveAction,
        TestActionA,
        TestActionB
    }


    public enum OptionScorerEnum
    {
        ExampleOptionScorer
    }


    public enum ScorerEnum
    {
        HasEnemies,
        HasEnemiesInRange,
        TestScorerA,
        TestScorerB
    }

}




//namespace UtilityAI
//{
//    using UnityEngine;
//    using System;
//    using System.Collections.Generic;

//    /// <summary>
//    /// We use this to view the Serialized Data in the Inspector.
//    /// This defines the logic mapping behind the UtilityAIClient.
//    /// This class also creates all the logic definition.
//    /// </summary>
//    [Serializable]
//    public class UtilityAI
//    {
//        ////  Key = ClassName, Value = BaseType
//        //public static Dictionary<Type, Type> ReferencedTypes { get; private set; }



//        public Selector CreateLogicDefinition(UtilityAIClient utilityAI)
//        {
//            SelectorOption selector_option = utilityAI.selectorOption;
//            QualifierOption[] qualifier_option = selector_option.qualifiers;
//            List<IQualifier> qualifier_list = new List<IQualifier>();


//            for (int i = 0; i < qualifier_option.Length; i++)
//            {
//                IQualifier qualifier = InitializeQualifier(qualifier_option[i]);
//                qualifier_list.Add(qualifier);
//            }

//            Selector root_selector = CreateSelector(selector_option.type, qualifier_list, selector_option.nameID);

//            return root_selector;
//        }


//        private IQualifier InitializeQualifier(QualifierOption qualifier_option)
//        {
//            QualifierOption qualifier = qualifier_option;
//            Type qualifier_type = qualifier.type != null ? qualifier.type : typeof(CompositeScoreQualifier);
//            IAction action = null;
//            List<IScorer> scorers_list = new List<IScorer>();


//            var scorers_all = qualifier._scorers;
//            for (int k = 0; k < scorers_all.Count; k++)
//            {
//                IScorer scorer_obj = CreateScorer(scorers_all[k].ToString());
//                scorers_list.Add(scorer_obj);
//            }

//            /*   Check Qualifier output type.  */
//            action = InitializeAction(qualifier.actionOption);

//            //  CReate Qualifier.
//            IQualifier qualifier_obj = CreateQualifier(qualifier_type, action, scorers_list, qualifier.nameID);

//            return qualifier_obj;
//        }


//        private IAction InitializeAction(ActionOption action_option)
//        {
//            IAction action = action_option.type != null ? CreateAction(action_option.type) : null;
//            return action;
//        }


//        public void UpdateReferencedTypes()
//        {

//        }



//        private Selector CreateSelector(Type type, List<IQualifier> objects, string nameID = "")
//        {
//            var name = (String.IsNullOrEmpty(nameID)) ? type.Name : nameID;
//            var _type = type.Name;

//            Selector obj = CreateInstance<Selector>(_type);
//            obj.NameID = name;
//            obj.AddQualifiers(objects);
//            /* 
//             * Perform some mapping logic here
//            */

//            return obj;
//        }

//        private IQualifier CreateQualifier(Type type, IAction action, List<IScorer> objects, string nameID = "")
//        {
//            var name = (String.IsNullOrEmpty(nameID)) ? type.Name : nameID;
//            var _type = type.Name;

//            IQualifier obj = CreateInstance<IQualifier>(_type);
//            obj.action = action;

//            if (obj == typeof(QualifierBase))
//            {
//                var o = obj as QualifierBase;
//                o.NameID = name;
//                o.AddScorers(objects);
//            }


//            /* 
//             * Perform some mapping logic here
//            */

//            return obj;
//        }


//        private IAction CreateAction(Type type)
//        {
//            string _type = type.Name;
//            IAction obj = CreateInstance<IAction>(_type);
//            /* 
//             * Perform some mapping logic here
//            */
//            return obj;
//        }

//        private ActionWithOptions<TOption> CreateActionWithOptions<TOption>(Type type, params IOptionScorer<TOption>[] objects)
//        {
//            string _type = type.Name;
//            ActionWithOptions<TOption> obj = CreateInstance<ActionWithOptions<TOption>>(_type, objects);
//            /* 
//             * Perform some mapping logic here
//            */
//            return obj;
//        }

//        private IScorer CreateScorer(Type type)
//        {
//            return CreateInstance<IScorer>(type.Name);
//        }

//        private IScorer CreateScorer(string type)
//        {
//            return CreateInstance<IScorer>(type);
//        }

//        private IOptionScorer<TOption> CreateScorer<TOption>(Type type)
//        {
//            IOptionScorer<TOption> obj = CreateInstance<IOptionScorer<TOption>>(type.Name);
//            return obj;

//        }




//        /// <summary>
//        /// Creates the instance.
//        /// </summary>
//        /// <returns>The instance.</returns>
//        /// <param name="type_name">Type name.</param>
//        /// <param name="args">Arguments.</param>
//        /// <typeparam name="T">The 1st type parameter.</typeparam>
//        private static T CreateInstance<T>(string type_name, params object[] args)
//        {
//            var name = type_name;
//            var type = Type.GetType(typeof(UtilityAI).Namespace + "." + name);
//            var count = args.Length;

//            var instance = count > 0 ? (T)Activator.CreateInstance(type, args) : (T)Activator.CreateInstance(type);

//            //return (T)Convert.ChangeType(_selector, typeof(T));
//            return instance;
//        }



//    }



//}

