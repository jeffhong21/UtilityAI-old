namespace UtilityAI
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;



    /// <summary>
    /// Add options window.
    /// </summary>
    public class AddOptionsWindow : OptionsWindow<AddOptionsWindow>
    {
        AddOptionsWindow window;
        protected string windowTitle = "Add Options | AI Object Selector";
        string searchStr;

        Dictionary<Type, Type> displayTypes;



        public override void Init(AddOptionsWindow window, TaskNetworkComponent taskNetwork, Type type)
        {
            displayTypes = GetAllOptions(type);
            windowTitle = string.Format("{0} Search | AI Object Selector", type.Name);
            //windowTitle = string.Format("{0} Search | AI Object Selector", TaskNetworkEditorUtilities.GetAiCategoryName<T>() ); 

            this.taskNetwork = taskNetwork;

            this.window = window;
            this.window.minSize = this.window.maxSize = new Vector2(windowMinSize, windowMaxSize);
            this.window.titleContent = new GUIContent(windowTitle);

            this.window.ShowUtility();
        }


        protected override void OnGUI()
        {
            DrawWindowContents();
        }


        protected override void DrawWindowContents()
        {
            GUIStyle contentStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter
            };


            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                string newSearchStr = EditorGUILayout.TextField(searchStr, new GUIStyle("SearchTextField"), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                if (GUILayout.Button(GUIContent.none, new GUIStyle("SearchCancelButton"), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    Debug.Log("TODO:  Clearing Search Field");
                }
            }
            EditorGUILayout.Space();


            foreach (KeyValuePair<Type, Type> item in displayTypes)
            {
                GUIContent buttonLabel = new GUIContent(item.Key.Name);
                if (GUILayout.Button(buttonLabel, contentStyle, GUILayout.Height(18)))
                {

                    Debug.Log(item.Key.Name);
                    CloseWindow();
                }
            }

        }


        /// <summary>
        /// Get all available classes of given type.
        /// </summary>
        /// <returns>returns the type and the base type</returns>
        /// <param name="optionType">Option type.</param>
        private Dictionary<Type, Type> GetAllOptions(Type optionType)
        {

            Dictionary<Type, Type> availableTypes = new Dictionary<Type, Type>();
            //  Gets all custom Types in this assembly and adds it to a list.
            var optionTypes = Assembly.GetAssembly(optionType).GetTypes()
                                 .Where(t => t.IsClass && t.Namespace == this.GetType().Namespace)
                                 .ToList();
            //optionTypes.ForEach(t => Debug.Log(t));

            //  Go through each item in list and check if its base class equals the Types we defined.
            foreach (Type type in optionTypes)
            {
                if (type.BaseType == optionType && availableTypes.ContainsKey(type.BaseType) == false)
                    availableTypes.Add(type, type.BaseType);
            }
            return availableTypes;
        }


        protected override void CloseWindow()
        {
            window.Close();
        }


    }








}