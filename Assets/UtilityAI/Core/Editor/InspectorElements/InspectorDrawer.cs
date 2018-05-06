namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;


    /// <summary>
    /// Inspector drawer for Client Editor
    /// </summary>
    public static class InspectorDrawer
    {


        public static UtilityAIAsset ElementInspector(UtilityAIAsset activeClient)
        {
            if (activeClient == null){
                return null;
            }

            Selector rootSelector = activeClient.configuration.rootSelector;

            bool elementIsDisable;
            string elementType = rootSelector.GetType().Name;
            string elementName;
            string elementDisplayName = "Qualifiers";
            string elementDescription;

            List<Type> items = new List<Type>();
            ReorderableList itemsList;


            //var attr = activeClient.rootSelector.GetType().GetProperty("FriendlyNameAttribute").GetCustomAttribute(typeof(FriendlyNameAttribute));
            //var friendlyName = attr as FriendlyNameAttribute;
            using (new EditorGUILayout.HorizontalScope())
            {
                elementIsDisable = EditorGUILayout.ToggleLeft(new GUIContent(elementType + " | TASKNETWORK AI"), true);

                //  Change Element
                if (InspectorUtility.OptionsPopupButton(InspectorUtility.ChangeContent)){
                    //ShowOptionsWindow<AddOptionsWindow>(typeof(Selector));
                    Debug.Log("Show Option Popup Window");
                }
                //  Delete Element
                if (InspectorUtility.OptionsPopupButton(InspectorUtility.DeleteContent)){
                    Debug.Log("Deleting");
                }
            }


            //  NameField of Selected Selector, Qualifier or Action
            elementName = InspectorUtility.NameField("Test Name");
            //  Description of Selector.
            elementDescription = InspectorUtility.DescriptionField("", 2);


            //  Custom Attribute Fields.
            EditorGUILayout.LabelField(" <Custom Attributes> ");
            EditorGUILayout.IntField("First Test Field", 1);
            EditorGUILayout.IntField("Second Test Field", 2);



            //  Header for list of itemsList.
            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(new GUIContent(elementDisplayName));

                if (InspectorUtility.OptionsPopupButton(InspectorUtility.AddContent))
                {
                    Debug.Log("Show Option Popup Window");
                    //ShowOptionsWindow<AddOptionsWindow>(typeof(QualifierBase));
                    //activeClient.rootSelector.qualifiers.Add(new CompositeScoreQualifier());
                    //aiAssets[0].ApplyModifiedProperties();
                }
            }

            //  list of itemsList.
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (rootSelector.qualifiers.Count != 0)
                {
                    itemsList = new ReorderableList(rootSelector.qualifiers, typeof(IQualifier), true, false, false, false);
                    itemsList.showDefaultBackground = false;
                    itemsList.DoLayoutList();
                }

                //  Default Qualifier
                EditorGUILayout.LabelField(new GUIContent(rootSelector.defaultQualifier.GetType().Name));
            }


            return activeClient;
        }







    }




}