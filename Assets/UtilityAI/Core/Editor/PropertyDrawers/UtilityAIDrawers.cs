//namespace UtilityAI
//{
//    using UnityEngine;
//    using UnityEditor;
//    using UnityEditorInternal;
//    using System;
//    using System.Collections.Generic;
//    using System.Reflection;
//    using System.Linq;



//    public static class UtilityAIDrawers
//    {
//        public static string nameID_fieldName = "nameID";
//        public static string logicType_fieldName = "logicType";
//        public static string qualifiers_fieldName = "qualifiers";
//        public static string action_fieldName = "actionOption";
//        public static string isSelected_fieldName = "isSelected";





//        public static void ShowOptionsTypeWindow(Type type, SerializedProperty property, OptionWindowType windowType)
//        {
//            OptionTypesWindow window = new OptionTypesWindow();
//            window.Init(window, type, property, windowType);

//        }



//        /// <summary>
//        /// Draws the Inspector Header for the selected Qualifier or action
//        /// </summary>
//        /// <param name="position">Position.</param>
//        /// <param name="property">Property.</param>
//        public static void DrawInspectorHeader(Rect position, SerializedProperty property)
//        {
//            Rect rect = new Rect(position);

//            SerializedProperty nameID = property.FindPropertyRelative(nameID_fieldName);
//            SerializedProperty logicType = property.FindPropertyRelative(logicType_fieldName);
//            //Type parentType = property.serializedObject.targetObject.GetType();

//            string headerName = logicType.stringValue != "" ? nameID.stringValue : logicType.stringValue;
//            string parentType = property.FindPropertyRelative(logicType_fieldName).stringValue;
//            headerName += " | " + parentType.Replace(typeof(UtilityAIManager).Namespace + ".", "");


//            EditorGUI.LabelField(rect, new GUIContent(headerName), Styles.defaultTextStyle);


//            /*  Change Button in the Header section */
//            rect.x = rect.width - Styles.lineHeight - 18;
//            rect.y += 1;
//            rect.width = rect.height = Styles.lineHeight;
//            if (GUI.Button(rect, new GUIContent(Styles.changeElementIcon), GUIStyle.none))
//            {
//                ShowOptionsTypeWindow(typeof(QualifierBase), logicType, OptionWindowType.ChangeType);
//            }

//            /*  Delete Button in the Header section */
//            rect.x += 18;
//            if (GUI.Button(rect, new GUIContent(Styles.deleteElementIcon), GUIStyle.none))
//            {
//                Debug.Log("Deleting Qualifiers");
//            }
//        }






//        /// <summary>
//        /// Draws the name input field.
//        /// </summary>
//        /// <param name="position">Position.</param>
//        /// <param name="property">Property.</param>
//        public static bool DrawTextField(Rect position, SerializedProperty property)
//        {
//            //rect.y += Styles.lineHeightSpace;
//            Rect rect = new Rect(position);

//            SerializedProperty nameID = property.FindPropertyRelative("nameID");
//            SerializedProperty logicType = property.FindPropertyRelative(logicType_fieldName);

//            EditorGUI.LabelField(rect, new GUIContent("Name: "), Styles.defaultTextStyle);

//            rect.x += 45;
//            rect.width -= 45 + 25;
//            rect.height = Styles.lineHeight;
//            nameID.stringValue = EditorGUI.DelayedTextField(rect, nameID.stringValue.Replace(typeof(UtilityAIManager).Namespace + ".", "") );

//            if (nameID.stringValue == "")
//                nameID.stringValue = logicType.stringValue;
            
//            return true;
//        }


//        public static void GetPropertyInfo(SerializedProperty property)
//        {
//            string strVal = GetStringValue(property);
//            string propDesc = property.propertyPath + " | TYPE: " + property.type + " | NAME:  " + property.name + " | VALUE:  " + strVal;

//            Debug.Log(propDesc);
//        }

//        public static string GetStringValue(SerializedProperty property)
//        {
//            switch (property.propertyType)
//            {
//                case SerializedPropertyType.String:
//                    return property.stringValue;
//                case SerializedPropertyType.Character: //this isn't really a thing, chars are ints!
//                case SerializedPropertyType.Integer:
//                    if (property.type == "char")
//                    {
//                        return System.Convert.ToChar(property.intValue).ToString();
//                    }
//                    return property.intValue.ToString();
//                case SerializedPropertyType.ObjectReference:
//                    if (property.objectReferenceValue != null)
//                    {
//                        return property.objectReferenceValue.ToString();
//                    }
//                    else
//                    {
//                        return "(null)";
//                    }
//                default:
//                    return "";
//            }
//        }


//    }



//    [CustomPropertyDrawer(typeof(QualifierOption))]
//    public class QualifierOptionsDrawer : PropertyDrawer
//    {


//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            property.serializedObject.Update();

//            Rect rect = new Rect(position);
//            rect.x += 20;
//            rect.width -= 25;
//            Rect button_rect = new Rect(rect.width - Styles.lineHeight - 18, rect.y + 1, Styles.lineHeight, Styles.lineHeight);


//            //  HeaderSection
//            UtilityAIDrawers.DrawInspectorHeader(rect, property);
//            //  TextField for Changing the name.
//            rect.y += Styles.lineHeightSpace;
//            UtilityAIDrawers.DrawTextField(rect, property);


//            /*
//            ////  Section for properties of Qualifier.. 
//            ////  Get the reference object and get all properties from it.

//            //SerializedProperty logicType = property.FindPropertyRelative("logicType");
//            //SerializedProperty allFields = property.FindPropertyRelative("allFields");

//            //Type class_type = UtilityAIManager.GetTypeFromString(logicType.stringValue);
//            //string[] propertyFields = UtilityAIManager.GetFieldInfoOfTypeAsString(class_type);

//            //allFields.ClearArray();
//            //allFields.arraySize = propertyFields.Length;
//            //var arraySize = allFields.arraySize;

//            ////  This draws out the fields.  
//            //for (int index = 0; index < arraySize; index++)
//            //{
//            //    SerializedProperty element = allFields.GetArrayElementAtIndex(index);
//            //    //element.stringValue = propertyFields[index];
//            //    rect.y += Styles.lineHeightSpace;
//            //    EditorGUI.PropertyField(rect, element, new GUIContent(propertyFields[index] ));
//            //    //EditorGUI.PropertyField(rect, element, new GUIContent(element.type));
//            //}

//            */



//            /*  --------------------------------------------------------------------------------  */
//            /*  Section for the scorers. */
//            /*  --------------------------------------------------------------------------------  */

//            rect.y += Styles.lineHeightSpace * 2;
//            EditorGUI.LabelField(rect, new GUIContent("Scorers: "), Styles.defaultTextStyle);
//            //  Button for deleting Scorers
//            button_rect.y = rect.y;
//            if (GUI.Button(button_rect, new GUIContent(Styles.changeElementIcon), GUIStyle.none))
//            {
//                Type scorersType = typeof(ScorerBase);
//                UtilityAIDrawers.ShowOptionsTypeWindow(scorersType, property, OptionWindowType.Add);
//            }



//            /*  --------------------------------------------------------------------------------  */
//            /*  Debugging Info. */
//            /*  --------------------------------------------------------------------------------  */
//            rect.height = Styles.lineHeightSpace;
//            rect.y = position.y + (GetPropertyHeight(property, label) - rect.height) - 4;
//            if (GUI.Button(rect, new GUIContent("Property Debug Info")))
//            {
//                //GetPropertyInfo(allFields);
//                //Debug.Log(property.propertyType);
//                Debug.Log(property.serializedObject.targetObject);
//                Debug.Log(property.propertyPath);
//                Debug.Log(property.FindPropertyRelative("scorersOption").isArray);
//                Debug.Log(property.FindPropertyRelative("scorersOption").arraySize);
//            }


//            property.serializedObject.ApplyModifiedProperties();
//            EditorGUI.EndProperty();
//        }


//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
//            return Styles.lineHeightSpace * 10; ;
//        }






//    }


//    [CustomPropertyDrawer(typeof(ActionOption))]
//    public class ActionOptionDrawer : PropertyDrawer
//    {
        
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            property.serializedObject.Update();

//            Rect rect = new Rect(position);
//            Rect button_rect = new Rect(rect.width - Styles.lineHeight - 18, rect.y + 1, Styles.lineHeight, Styles.lineHeight);

//            rect.x += 20;
//            rect.width -= 25;
//            /*  Header  */

//            UtilityAIDrawers.DrawInspectorHeader(rect, property);
//            /*  TextField for Changing the name. */
//            rect.y += Styles.lineHeightSpace;
//            UtilityAIDrawers.DrawTextField(rect, property);





//            /*  --------------------------------------------------------------------------------  */
//            /*  Debugging Info. */
//            /*  --------------------------------------------------------------------------------  */
//            rect.height = Styles.lineHeightSpace;
//            rect.y = position.y + (GetPropertyHeight(property, label) - rect.height) - 4;
//            if (GUI.Button(rect, new GUIContent("Property Debug Info")))
//            {
//                //GetPropertyInfo(allFields);
//                //Debug.Log(property.propertyType);
//            }


//            property.serializedObject.ApplyModifiedProperties();
//            EditorGUI.EndProperty();
//        }



//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return Styles.lineHeightSpace * 10;;
//            //return base.GetPropertyHeight(property, label) * 6;
//        }

//    }



//    [CustomPropertyDrawer(typeof(ScorersOption))]
//    public class ScorerOptionDrawer : PropertyDrawer
//    {

//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            property.serializedObject.Update();

//            Rect rect = new Rect(position);
//            Rect button_rect = new Rect(rect.width - Styles.lineHeight - 18, rect.y + 1, Styles.lineHeight, Styles.lineHeight);

//            rect.x += 20;
//            rect.width -= 25;


//            ///*  Header  */
//            //EditorGUI.LabelField(rect, new GUIContent("scorersOption"), Styles.defaultTextStyle);
//            ////  Button for deleting Scorers
//            //button_rect.y = rect.y;
//            //if (GUI.Button(button_rect, new GUIContent(Styles.changeElementIcon), GUIStyle.none))
//            //{
//            //    Type scorersType = typeof(ScorerBase);
//            //    UtilityAIDrawers.ShowOptionsTypeWindow(scorersType, property, OptionWindowType.Add);
//            //}




//            //for (int index = 0; index < property.arraySize; index++)
//            //{
//            //    SerializedProperty element = property.GetArrayElementAtIndex(index);
//            //    rect.y += Styles.lineHeightSpace;
//            //    EditorGUI.PropertyField(rect, element, new GUIContent(element.FindPropertyRelative("nameID").stringValue));
//            //}



//            property.serializedObject.ApplyModifiedProperties();
//            EditorGUI.EndProperty();
//        }



//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            //return Styles.lineHeightSpace * 10; ;
//            return base.GetPropertyHeight(property, label) ;
//        }

//    }

//}

