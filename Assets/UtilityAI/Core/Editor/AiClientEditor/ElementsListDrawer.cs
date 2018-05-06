namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;



    public class ElementsListDrawer : PropertyDrawer
    {
        ReorderableList list;


        float elementHeight;


        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {

            elementHeight = (EditorGUIUtility.singleLineHeight + 4) * 2;



            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                DrawListElement(rect, element);
            };

            list.onChangedCallback = (ReorderableList l) =>
            {
                prop.serializedObject.ApplyModifiedProperties();
                Debug.Log("On Change Call back");
            };

            list.onSelectCallback = (ReorderableList l) =>
            {
                Debug.Log("On Select Call back");
            };

            list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //SerializedProperty list_element = list.serializedProperty.GetArrayElementAtIndex(index);
            };



            list.headerHeight = 2;
            list.showDefaultBackground = false;
            list.elementHeight = (EditorGUIUtility.singleLineHeight + 4) * 2;



        }



        private void DrawListElement(Rect rect, SerializedProperty element)
        {
            //rect.y += 2;

            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                                 element.FindPropertyRelative("element").stringValue, NodeStyles.nodeTextStyle);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUIUtility.singleLineHeight),
                                 element.FindPropertyRelative("item").stringValue, NodeStyles.nodeTextStyle);


            //EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            //                        element.FindPropertyRelative("element"), GUIContent.none);

            //EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUIUtility.singleLineHeight),
            //element.FindPropertyRelative("item"), GUIContent.none);

        }


        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return base.GetPropertyHeight(prop, label);
            //return list.GetHeight();
        }
    }




}

