namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;




    public static class Styles
    {
        public static GUIStyle defaultNodeStyle;


        static Styles()
        {
            defaultNodeStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                contentOffset = new Vector2(25, -3)
            };


        }
    }



    #region ContainerDrawer

    [CustomPropertyDrawer(typeof(ContainerNode), true)]
    public class ContainerDrawer : PropertyDrawer
    {
        ReorderableList list;
        float element_buffer = 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty items = property.FindPropertyRelative("items");

            if (list == null)
                list = new ReorderableList(property.serializedObject, items, true, false, false, false);


            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                float offset = 20;
                rect.xMin -= offset;
                rect.xMax += 5;
                rect.yMin -= 1;
                rect.yMax += 1;
                rect.x += 1;
                rect.width -= 1;


                SerializedProperty element = items.GetArrayElementAtIndex(index);
                Rect elementRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight + element_buffer);

                //EditorGUI.LabelField(listElementRect, listElement.name);
                GUI.Box(elementRect, element.name, Styles.defaultNodeStyle);

                //  Make sure to apply changes to serialized object.
                property.serializedObject.ApplyModifiedProperties();
            };


            list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //SerializedProperty list_element = list.serializedProperty.GetArrayElementAtIndex(index);
            };


            list.onSelectCallback = (ReorderableList l) =>
            {
                //property.serializedObject.ApplyModifiedProperties();
            };

            list.elementHeight = EditorGUIUtility.singleLineHeight + element_buffer;
            list.showDefaultBackground = true;
            list.headerHeight = 0;

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //return base.GetPropertyHeight(property, label);
            return list.GetHeight();
        }


    }

    #endregion











    #region Selector

    //[CustomPropertyDrawer(typeof(Selector), true)]
    //public class SelectorDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        Rect rect = new Rect(position);
    //        EditorGUI.LabelField(rect, "Selector Property Test");


    //        GUI.Box(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, rect.height), property.displayName, EditorStyles.helpBox);
    //    }

    //    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //    {
    //        return base.GetPropertyHeight(property, label);
    //    }

    //}

    #endregion


    #region UtilityAIClient
    //[CustomPropertyDrawer(typeof(UtilityAIClient))]
    //public class UtilityAIClientDrawer : PropertyDrawer
    //{
    //    GUIContent IntervalLabel = new GUIContent("Interval:");
    //    GUIContent StartDelayLabel = new GUIContent("Start Delay:");

    //    int lineHeightBuffer = 4;
    //    int intFieldWidth = 48;
    //    int fieldSeperatorWidth = 24;
    //    float fieldPosition = 0.35f;


    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        EditorGUI.BeginProperty(position, label, property);
    //        property.serializedObject.Update();

    //        Rect rect = new Rect(position);

    //        SerializedProperty intervalMin = property.FindPropertyRelative("_intervalMin");
    //        SerializedProperty intervalMax = property.FindPropertyRelative("_intervalMax");
    //        SerializedProperty startDelayMin = property.FindPropertyRelative("_startDelayMin");
    //        SerializedProperty startDelayMax = property.FindPropertyRelative("_startDelayMax");


    //        rect.x = position.x;
    //        //EditorGUI.ToggleLeft(rect, GUIContent.none, true);

    //        rect.x = rect.width - 36 - 28;
    //        rect.y -= 2;
    //        rect.width = 36;
    //        rect.height = EditorGUIUtility.singleLineHeight;
    //        if (GUI.Button(rect, new GUIContent("C"))){
    //            Debug.Log(property.serializedObject.context);
    //        }

    //        rect.x += 40;
    //        if (GUI.Button(rect, new GUIContent("-"))){

    //            int index = 0;
    //            string path = property.propertyPath.Replace(".Array.data", "");
    //            string[] fieldStructure = path.Split('.');
    //            Regex rgx = new Regex(@"\[\d+\]");
    //            for (int i = 0; i < fieldStructure.Length; i++){
    //                if (fieldStructure[i].Contains("["))
    //                    index = System.Convert.ToInt32(new string(fieldStructure[i].Where(c => char.IsDigit(c)).ToArray()));
    //            }
    //            Debug.Log(index);
    //            Debug.Log(property.serializedObject);


    //            property.serializedObject.FindProperty("clients").DeleteArrayElementAtIndex(index);
    //            property.serializedObject.ApplyModifiedProperties();

    //            return;
    //        }


    //        rect.x = position.x;
    //        rect.y += EditorGUIUtility.singleLineHeight + lineHeightBuffer;
    //        rect.width = position.width;
    //        rect.height = position.height;
    //        //rect.y += EditorGUIUtility.singleLineHeight + lineHeightBuffer;
    //        rect.height = EditorGUIUtility.singleLineHeight;
    //        EditorGUI.LabelField(rect, "AI:");
    //        rect.x += rect.width * fieldPosition;
    //        EditorGUI.LabelField(rect, "TempAIName");


    //        rect.x = position.x;
    //        rect.y += EditorGUIUtility.singleLineHeight;
    //        EditorGUI.LabelField(rect, IntervalLabel);
    //        rect.x += rect.width * fieldPosition;
    //        EditorGUI.PropertyField(new Rect(rect.x, rect.y, intFieldWidth, rect.height), intervalMin, GUIContent.none);
    //        rect.x += intFieldWidth + 2;
    //        EditorGUI.LabelField(new Rect(rect.x, rect.y, fieldSeperatorWidth, rect.height), "to ");
    //        rect.x += fieldSeperatorWidth + 2;
    //        EditorGUI.PropertyField(new Rect(rect.x, rect.y, intFieldWidth, rect.height), intervalMax, GUIContent.none);


    //        rect.x = position.x;
    //        rect.y += EditorGUIUtility.singleLineHeight + lineHeightBuffer;
    //        EditorGUI.LabelField(rect, StartDelayLabel);
    //        rect.x += rect.width * fieldPosition;
    //        EditorGUI.PropertyField(new Rect(rect.x, rect.y, intFieldWidth, rect.height), startDelayMin, GUIContent.none);
    //        rect.x += intFieldWidth + 2;
    //        EditorGUI.LabelField(new Rect(rect.x, rect.y, fieldSeperatorWidth, rect.height), "to ");
    //        rect.x += fieldSeperatorWidth + 2;
    //        EditorGUI.PropertyField(new Rect(rect.x, rect.y, intFieldWidth, rect.height), startDelayMax, GUIContent.none);

    //        property.serializedObject.ApplyModifiedProperties();
    //        EditorGUI.EndProperty();
    //    }




    //    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //    {
    //        //return base.GetPropertyHeight(property, label);
    //        int lines = 3;
    //        return (EditorGUIUtility.singleLineHeight + lineHeightBuffer) * lines + (lineHeightBuffer * lines);


    //    }


    //}
    #endregion


    #region SelectorDrawer
    //[CustomPropertyDrawer(typeof(Selector), true)]
    //public class SelectorDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        Rect rect = new Rect(position);
    //        EditorGUI.LabelField(rect, "Selector Property Test");


    //        GUI.Box(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, rect.height), property.displayName, EditorStyles.helpBox);
    //    }

    //    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //    {
    //        return base.GetPropertyHeight(property, label);
    //    }

    //}
    #endregion




}