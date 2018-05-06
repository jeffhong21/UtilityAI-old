namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;


    //public interface IStageItem{}


    public class StageItem : ScriptableObject
    {


        protected string _displayName;
        public string displayName
        { 
            get { return _displayName; } 
            set { _displayName = value; } 
        }

        //  Parent
        protected string _parent;
        public string parent
        {
            get { return _parent; }
            protected set { _parent = value; }
        }


        public virtual void Init(string name)
        {
            displayName = name;

        }

        /// <summary>
        /// Removes the element from its parent. If not parented nothing will happen.
        /// </summary>
        public virtual void Remove()
        {
            //  Find parent

        }

    }



    public static class StageUtilities<U> where U : ScriptableObject
    {
        public static T CreateStageItem<T>(string parent, string name, params StageItem[] items) where T : StageItem
        {
            T obj = ScriptableObject.CreateInstance<T>();
            if (items == null)
                obj.Init(name);
            return obj;
        }
    }

}

