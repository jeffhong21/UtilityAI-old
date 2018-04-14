namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;


    [Serializable]
    public class ContainerNode
    {
        //  Name of the node.
        public string name { get; set; }
        //  e.g. Qualifier should know its parent is a Selector, or a Scorer's is a Qualifier.
        public string parent { get; protected set; }
        //  Elements would be fields and properties.
        public object[] elements;
        //  Items would be a list of Scorers for a Qualifier for example.
        public Type[] items;

        //<summary>
        //Removes the element from its parent.If not parented nothing will happen.
        //</summary>
        public void Remove() { }

        public ContainerNode(string name) { }

        public ContainerNode(string name, object[] items) { }


    }




    #region AI blocks

    public class Container
    {
        Element[] qualifierNodes;
    }

    public class Element
    {
        QualifierNode qualifierNode;
        ActionNode actionNode;
    }


    public class SelectorNode
    {
        public List<IQualifier> qualifiers;
        public IDefaultQualifier defaultQualifier;
    }

    public class QualifierNode
    {
        public IAction action;
    }

    public class CompositeQualifierNode : QualifierNode
    {
        public List<IScorer> scorers;
    }


    public class DefaultQualiferNode : QualifierNode
    {
    }

    public class ActionNode
    {
    }

    public class ActionSequenceNode : ActionNode
    {
        //  Action Sequence
        public List<IAction> actions;  
    }

    public class ActionWithOptionNode<TOption> : ActionNode
    {
        public List<IOptionScorer<TOption>> scorers;
    }



    public class ScorerNode
    {

    }


    #endregion








    [Serializable]
    public class StageItem
    {
        public StageItem(string name)
        {
        }

        //<summary>
        //Removes the element from its parent.If not parented nothing will happen.
        //</summary>
        public void Remove() { }

        public string name { get; set; }
        public string parent { get; protected set; }

    }

    [Serializable]
    public class StageElement
    {
        object[] elements;
        object[] items;

        public StageElement(string name) { }


        public StageElement(string name, object[] items) { }

        /// <summary>
        /// Gets all child items with the specified name.
        /// </summary>
        public StageItem[] Items(string name){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all child elements with the specified name.
        /// </summary>
        public StageItem[] Elements(string name){
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns all descendant items
        /// </summary>
        public StageItem[] Descendants(string name){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all descendants of type.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Descendants<T>() { }


        public void Add(object item) { }
    }






}