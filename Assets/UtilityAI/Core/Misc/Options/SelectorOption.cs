//namespace UtilityAI
//{
//    using UnityEngine;
//    using System;
//    using System.Collections.Generic;


//    /// <summary>
//    /// Selector data.
//    /// </summary>
//    [System.Serializable]
//    public class SelectorOption
//    {
        
//        public string Name;
//        public Selector Selector;
//        public List<QualifierOption> Qualifiers = new List<QualifierOption>();
//        /*  Default Qualifier should be created here.   */
//        private UtilityAI aiClientMap;



//        private string type = AIClientNames.HighestScoringQualifier;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="T:UtilityAI.SelectorOption"/> class.
//        /// </summary>
//        /// <param name="data">Data.</param>
//        public SelectorOption(UtilityAI map, SelectorOption data)
//        {
//            aiClientMap = map;
//            //  Create Selector.
//            Selector = AIClientUtility.CreateInstance<Selector>(type);
//            //  Create QualifierOption
//            Qualifiers = CreateQualifiers(data.Qualifiers);


//            Name = GetName(data.Name);
//        }



//        private string GetName(string data)
//        {
//            if (String.IsNullOrEmpty(data))
//                return Selector.GetType().ToString();
//            else
//                return data;
//        }


//        /// <summary>
//        /// Creates all the QualifierOptions
//        /// </summary>
//        /// <returns>The Qualifiers.</returns>
//        /// <param name="data">Data.</param>
//        private List<QualifierOption> CreateQualifiers(List<QualifierOption> data)
//        {
//            List<QualifierOption> q = new List<QualifierOption>();

//            for (int index = 0; index < data.Count; index++)
//            {
//                q.Add(new QualifierOption(aiClientMap, Selector, data[index]) );
//            }
//            return q;
//        }
//    }


//}