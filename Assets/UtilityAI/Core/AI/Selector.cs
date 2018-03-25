﻿namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;



    /// <summary>
    ///   Selectors select the best Qualifier from the qualifiers attached to the Selector.
    ///   Selector Gets the Highest Score from the list of qualifiers.
    ///   It needs to know all qualifiers attached to it.
    /// </summary>
    public abstract class Selector
    {
        private int _id;
        private List<IQualifier> _qualifiers;
        private IDefaultQualifier _defaultQualifier;

        //  Gets the id of this selector.
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        //  Gets the qualifiers of this selector.
        public List<IQualifier> qualifiers 
        {
            get{
                if (_qualifiers == null){
                    _qualifiers = new List<IQualifier>();
                }
                return _qualifiers;
            }
            
            private set { _qualifiers = value; }
        }

        //  Gets or sets the default qualifier.
        public IDefaultQualifier defaultQualifier  
        {
            get { return _defaultQualifier; }
            set { _defaultQualifier = value; }
        }


        /// <summary>
        /// Selects the action for execution.
        /// </summary>
        /// <returns>The action to execute.</returns>
        /// <param name="context">Context.</param>
        public virtual IAction Select(IContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   This function selects the best score from a list of qualifiers.
        /// </summary>
        public abstract IQualifier Select(IContext context, List<IQualifier> qualifiers);
        //public abstract IQualifier Select(IContext context, List<IQualifier> qualifiers, IDefaultQualifier defaultQualifier);


        protected void RegenerateId()
        {
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:UtilityAI.Selector"/> class.
        /// </summary>
        protected Selector()
        {
            //Debug.Log(string.Format("Selector:  {0} : Abstract Constructor Message", this.GetType().Name));
            defaultQualifier = new DefaultQualifier();
        }




    }





    /// <summary>
    ///   Selector Gets the Highest Score from the list of qualifiers.
    /// </summary>
    public class ScoreSelector : Selector
    {
        
        public override IQualifier Select(IContext context, List<IQualifier> qualifiers)  //  Need default qualifier.  Final return value should be default Qualifier.
        {
            
            List<IQualifier> qList = new List<IQualifier>(qualifiers);

            //  Get score for all qualifiers
            for (int index = 0; index < qList.Count; index++)
            {
                CompositeQualifier q = qList[index] as CompositeQualifier;
                q.Score(context, q.scorers);

            }

            //  Sort list of qualifiers.
            qList.Sort();   //  Sorts in accending order.
            qList.Reverse();//  Sorts in decending order.


            //DebugSelectorWinner(context, qList);

            return qList[0];
        }


        /// <summary>
        /// Used just for Debugging
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="qualifiers">Qualifiers.</param>
        private void DebugSelectorWinner(IContext context, List<IQualifier> qualifiers)
        {
            var winnerInfo = "";

            winnerInfo += "Winner is:   " + qualifiers[0] + "\n";
            for (int index = 0; index < qualifiers.Count; index++)
            {
                CompositeQualifier q = qualifiers[index] as CompositeQualifier;
                var score = q.Score(context, q.scorers);
                winnerInfo += q.GetType().Name + " | " + score + "\n";
            }

            Debug.Log(winnerInfo);
        }

    }


}