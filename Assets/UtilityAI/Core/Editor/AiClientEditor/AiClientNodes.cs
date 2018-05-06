namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    #region ContainerDrawer

    //[Serializable]
    //public class ContainerNode
    //{
    //    //  Name of the node.
    //    public string name { get; set; }
    //    //  e.g. Qualifier should know its parent is a Selector, or a Scorer's is a Qualifier.
    //    public string parent { get; protected set; }
    //    //<summary>
    //    //Removes the element from its parent.If not parented nothing will happen.
    //    //</summary>
    //    public void Remove() { }

    //    //  Elements would be fields and properties.
    //    public object[] elements;
    //    //  Items would be a list of Scorers for a Qualifier for example.
    //    public object[] items;


    //    public ContainerNode(string name) { }

    //    public ContainerNode(object item) { }

    //    /// <summary>
    //    /// Gets all child items with the specified name.
    //    /// </summary>
    //    public StageItem1[] Items()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Gets all child elements with the specified name.
    //    /// </summary>
    //    public StageItem1[] Elements()
    //    {
    //        throw new NotImplementedException();
    //    }


    //    /// <summary>
    //    /// Returns all descendant items
    //    /// </summary>
    //    public StageItem1[] Descendants()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Returns all descendants of type.
    //    /// </summary>
    //    /// <typeparam name="T">The 1st type parameter.</typeparam>
    //    public void Descendants<T>()
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public void Add(object item) { }

    //}


    #endregion




    [Serializable]
    public class Item
    {
        public string name { get; set; }
        public string parent { get; protected set; }

        //<summary>
        //Removes the element from its parent.If not parented nothing will happen.
        //</summary>
        public void Remove() { }

        public Item(string name)
        {
            this.name = name;
        }
    }


    [Serializable]
    public class ClassElement : Item
    {
        public List<Type> items = new List<Type>();

        public ClassElement(string name) : base(name)
        {
            
        }
    }



    public class SelectorElement
    {
        public List<IQualifier> qualifiers;
        public IDefaultQualifier defaultQualifier;
    }

    public class QualifierElement
    {
        public IAction action;
    }

    public class CompositeQualifierElement : QualifierElement
    {
        public List<IScorer> scorers;
    }


    public class DefaultQualiferElement : QualifierElement
    {
        
    }

    public class ActionElement
    {
        
    }

    public class ActionSequenceElement : ActionElement
    {
        //  Action Sequence
        public List<IAction> actions;
    }

    public class ActionWithOptionElement<TOption> : ActionElement
    {
        public List<IOptionScorer<TOption>> scorers;
    }



    public class ScorerElement
    {

    }











}