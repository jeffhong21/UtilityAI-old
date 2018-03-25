namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Reflection;
    using System.Linq;
    using System.Collections.Generic;


    public static class UtilityAIManager
    {
        //  Namespace
        public static string ns = typeof(UtilityAIManager).Namespace;
        //  AVailibe Types.
        private static readonly string[] availible_types =
        {
            ns + "." + "Selector",
            ns + "." + "QualifierBase",
            ns + "." + "ActionBase",
            ns + "." + "ScorerBase"
        };

        //  Key = ClassName, Value = BaseType  |  TKey = QualifierBase, TValue = SumOfChildren 
        public static Dictionary<Type, Type> AvailibleTypes { get; private set; }
        //  Key = LogicType, Value = OptionType  |  TKey = QualifierBase, TValue = QualifierOption 
        public static Dictionary<Type, Type> OptionsClassMapping { get; private set; }
        //  Key = ClassName, Value = All Fields visible in editor
        public static Dictionary<Type, FieldInfo[]> LogicTypeFieldInfo { get; private set; }




        /// <summary>
        /// Initializes the <see cref="T:UtilityAI.UtilityAIManager"/> class.
        /// </summary>
        static UtilityAIManager()
        {
            AvailibleTypes = new Dictionary<Type, Type>();
            OptionsClassMapping = new Dictionary<Type, Type>()
            {
                {typeof(Selector), typeof(SelectorOption)},
                {typeof(QualifierBase), typeof(QualifierOption)},
                {typeof(ActionBase), typeof(ActionOption)},
                {typeof(ScorerBase), typeof(ScorersOption)}
            };

            UpdateAvailibleTypes();
            UpdateOptionClassMapping();
        }


        /// <summary>
        /// Updates the availible logic types that can be built.
        /// </summary>
        public static void UpdateAvailibleTypes(bool debug = false)
        {
            //  Gets all custom Types in this assembly and adds it to a list.
            var all_types = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(t => t.IsClass && t.Namespace == ns)
                                .ToList();
            //all_types.ForEach(t => Debug.Log(t));

            //  Go through each item in list and check if its base class equals the Types we defined.
            foreach(Type type in all_types){
                if (availible_types.Contains(type.BaseType.ToString()) && AvailibleTypes.ContainsKey(type) == false)
                    AvailibleTypes.Add(type, type.BaseType);
            }

            if (debug){
                var debug_log = "";
                foreach (KeyValuePair<Type, Type> item in AvailibleTypes){
                    debug_log += string.Format("Key: {0}, Value: {1}\n", item.Key, item.Value);
                }
                Debug.Log(debug_log);
            }
        }


        /// <summary>
        /// Gets all available Logic types of given type.  Used in Editor
        /// </summary>
        /// <returns>The all availible of type.</returns>
        /// <param name="type">Type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> GetAllAvailibleOfType<T> (T type) where T : class
        {
            List<T> list = new List<T>();

            foreach (KeyValuePair<Type, Type> item in AvailibleTypes){
                //Debug.Log(string.Format("ValueType: {0}, ComparisonType: {1}", item.Value, type));
                if(item.Value == type){
                    var obj = item.Key as T;
                    list.Add(obj);
                }
            }
            return list;
        }


        /// <summary>
        /// Map out the ClassTypes with the appropriate OptionsType.  Used in the Editor, 
        /// so when creating the BaseClass, it knows what OptionsClass to update.
        /// </summary>
        /// <param name="debug">If set to <c>true</c> debug.</param>
        public static void UpdateOptionClassMapping(bool debug = false)
        {
            //UpdateAvailibleTypes();


            //  Add all the derived classes.
            foreach (KeyValuePair<Type, Type> item in AvailibleTypes)
            {
                if ( OptionsClassMapping.ContainsKey(item.Key) == false && 
                     item.Value == typeof(Selector) )
                {
                    OptionsClassMapping.Add(item.Key, typeof(SelectorOption) );
                }
                else if (OptionsClassMapping.ContainsKey(item.Key) == false && 
                         item.Value == typeof(QualifierBase) ||
                         item.Value == typeof(DefaultQualifier))
                {
                    OptionsClassMapping.Add(item.Key, typeof(QualifierOption));
                }
                else if (OptionsClassMapping.ContainsKey(item.Key) == false && 
                         item.Value == typeof(ActionBase) )
                {
                    OptionsClassMapping.Add(item.Key, typeof(ActionOption));
                }
                else if (OptionsClassMapping.ContainsKey(item.Key) == false && 
                         item.Value == typeof(ScorerBase) )
                {
                    OptionsClassMapping.Add(item.Key, typeof(ScorersOption));
                }
                //else{
                //    Debug.Log(string.Format("No match for {0} ", item.Key ));
                //}
            }


            if (debug){
                var debug_log = "";
                foreach (KeyValuePair<Type, Type> item in OptionsClassMapping){
                    debug_log += string.Format("Key: {0}, Value: {1}\n", item.Key, item.Value);
                }
                Debug.Log(debug_log);
            }
        }

        /// <summary>
        /// Get the OptionsType equivilent of the provided Type.
        /// </summary>
        /// <returns>The option type.</returns>
        /// <param name="type">Type.</param>
        public static Type GetOptionType(Type type)
        {
            Type otherType;
            if (OptionsClassMapping.TryGetValue(type, out otherType))
                return otherType;
            else
                return null;
        }



        /// <summary>
        /// Get the Type of a string name.
        /// </summary>
        /// <returns>The type from string.</returns>
        /// <param name="type_name">Type name.</param>
        public static Type GetTypeFromString(string type_name)
        {
            var all_types = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(t => t.IsClass && t.Namespace == ns)
                                .ToList();
            
            foreach (Type type in all_types)
            {
                Type found_type = Type.GetType(type_name);
                if (found_type != null)
                    return found_type;
            }
            return null;
        }


        /// <summary>
        /// Gets all public fields of class.
        /// </summary>
        /// <returns>The field info of type.</returns>
        /// <param name="type">Type.</param>
        /// <param name="debugLog">If set to <c>true</c> debug log.</param>
        public static FieldInfo[] GetFieldInfoOfType(Type type, bool debugLog = false)
        {
            FieldInfo[] fieldInfo = type.GetFields();

            if(debugLog){
                foreach (FieldInfo field in fieldInfo){
                    Debug.Log(field.Name);
                }
            }
            return fieldInfo;
        }




        //  Same as GetFieldInfoOfType except returns an Array of strings.
        public static string[] GetFieldInfoOfTypeAsString(Type type)
        {
            FieldInfo[] fieldInfo = type.GetFields();
            return fieldInfo.Select(x => x.Name).ToArray();
        }


        public static Dictionary<string, ValueType> GetFieldInfoAndValue(Type type)
        {
            Dictionary<string, ValueType> _dict = new Dictionary<string, ValueType>();

            object classObj = Activator.CreateInstance(type);
            FieldInfo[] fieldInfo = type.GetFields();

            for (int i = 0; i < fieldInfo.Length; i ++)
            {
                string TKey = fieldInfo[i].Name;
                ValueType TValue = fieldInfo[i].GetValue(classObj) as ValueType;
                //object TValue = fieldInfo[i].GetValue(classObj);
                _dict.Add(TKey, TValue);
            }

            //foreach (KeyValuePair<string, ValueType> item in _dict){
            //    Debug.Log(string.Format("Key: {0}, Value: {1}, ValueType:T {2}", item.Key, item.Value, item.Value.GetType()));
            //}

            return _dict;
        }


        //public static T GetElementType<T>(T type)
        //{
        //    T obj = (T)type.GetType();
        //    return obj;
        //}

    }











    public static class AIClientNames
    {
        //  The Name Map is a generated class used to reference AI's by name.
        public static readonly string NameMapLocation = "Assets/AI";

        //  Names of Selectors
        public static readonly string HighestScoringQualifier = "ScoreSelector";
        //  Names of Qualifieres
        public static readonly string SumOfChildren = "CompositeScoreQualifier";

    }
}