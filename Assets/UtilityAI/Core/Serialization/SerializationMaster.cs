namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using Newtonsoft.Json.Serialization;

    public class SerializationMaster
    {

        public object Deserialize(string data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserialize the specified data and requiresInit.
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name = "data" > The serialized from of the item to construct.</param>
        /// <param name = "requiresInit" > A list that will be populated with references to all entities in the graph that require initialization.</param>
        /// <typeparam name="T">The type to construct from the data.</typeparam>
        public object Deserialize<T>(string data, ICollection<T> requiresInit)  //  ICollection<IInitializeAfterDeserialization> requiresInit 
        {
            throw new NotImplementedException();
        }

        public string Serialize<T>(T item, bool prettyPrint)
        {


            return "";
        }

        public MemberData[] SetMemberData(object item)
        {
            
            PropertyInfo[] properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            MemberData[] data = new MemberData[properties.Length];

            for (int i = 0; i < properties.Length; i ++)
            {
                string elementName = properties[i].Name;
                object element = properties[i].GetValue(item, null);
                Type elementType = properties[i].PropertyType;

                data[i] = new MemberData(elementName, element, elementType);

                //if (element != null && element is IEnumerable)
                //{
                //    foreach (IEnumerable obj in element as IEnumerable)
                //    {
                //        Type itemType = obj as Type;
                //        PropertyInfo[] itemInfo = itemType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                //        foreach (PropertyInfo property in itemInfo)
                //        {
                //            //Debug.Log(property.Name + "" + property.GetValue(obj, null) + "" + property.PropertyType + "");
                //        }
                //    }
                //}
            }
            return data;

        }







    }





    [Serializable]
    public struct MemberData
    {
        string name;
        object value;
        Type type;

        public MemberData(string _name, object _value, Type _type)
        {
            name = _name;
            value = _value;
            type = _type;
        }

    }
}


