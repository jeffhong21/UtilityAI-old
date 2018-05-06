namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections;


    /// <summary>
    /// Used to make a readonly property.  (Field cannot be edited)
    /// </summary>
    public class ShowOnlyAttribute : PropertyAttribute
    {
    }


    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true)]
    public class AICategoryAttribute : Attribute
    {
        public string name { get; set; }

        public AICategoryAttribute(string name){
            this.name = name;
        }
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FriendlyNameAttribute : PropertyAttribute
    {
        public string name { get; private set; }
        public string description { get; set; }
        public int sortOrder { get; set; }


        public FriendlyNameAttribute(string name){
            this.name = name;
        }

        public FriendlyNameAttribute(string name, string description){
            this.name = name;
            this.description = description;
        }

    }



    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class AISerializationAttribute : Attribute
    {
        public string name { get; set; }

        public AISerializationAttribute(string name)
        {
            this.name = name;
        }
    }


    public class MemberEditorAttribute : Attribute
    {

    }

    ///<summary>
    /// An attribute to decorate AI entity fields and properties to assign them to a category when displayed in the AI editor inspector.
    ///</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class MemberCategoryAttribute : Attribute
    {
        public string name { get; private set; }
        public int sortOrder { get; private set; }

        public MemberCategoryAttribute(string name){
            this.name = name;
        }

        public MemberCategoryAttribute(string name, int sortOrder){
            this.name = name;
            this.sortOrder = sortOrder;
        }

    }




    public class DefaultAttribute : Attribute
    {
        
    }

}

