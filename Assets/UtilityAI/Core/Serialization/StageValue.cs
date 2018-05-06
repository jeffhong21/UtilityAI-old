namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;



    public class StageValue : StageItem
    {

        protected string _value;
        public string value
        {
            get { return _value; }
            set { _value = value; }
        }

        protected bool _isText;
        public bool isText
        {
            get { return _isText; }
            protected set { _isText = value; }
        }

        public virtual void Init(string name, string value, bool isText = false)
        {
            displayName = name;

        }




    }


}

