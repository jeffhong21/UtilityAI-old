namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;




    public class StageElement : StageContainer
    {
        //protected string _displayName;
        //protected string _parent;

        StageItem[] items;
        StageItem[] elements;
        
        public override void Init(string name)
        {


        }

        public void Init(string name, StageItem[] items)
        {
            
        }



        /// <summary>
        /// Gets all child items with the specified name.
        /// </summary>
        public StageItem[] Items(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all child elements with the specified name.
        /// </summary>
        public StageItem[] Elements(string name)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns all descendant items
        /// </summary>
        public StageItem[] Descendants(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all descendants of type.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public StageItem[] Descendants<T>()
        {
            throw new NotImplementedException();
        }


        public override void Add(StageItem item)
        {


        }

    }



}

