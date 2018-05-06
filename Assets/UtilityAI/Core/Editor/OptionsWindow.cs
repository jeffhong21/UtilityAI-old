namespace UtilityAI
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;



    public abstract class OptionsWindow<T> : EditorWindow
    {
        //protected T window;
        protected int windowMinSize = 250;
        protected int windowMaxSize = 350;


        //  Need to know so we can add UtilityAIAssets
        protected TaskNetworkEditor taskNetworkEditor { get; set; }


        public abstract void Init(T window, TaskNetworkEditor editor);
        public abstract void Init(T window, TaskNetworkEditor editor, Type typeOption);
        protected abstract void CloseWindow();
        protected abstract void DrawWindowContents();


        protected virtual void OnGUI(){
            DrawWindowContents();
        }

    }


    /// <summary>
    /// Add client window.
    /// </summary>
    public class ChangeClientWindow : OptionsWindow<ChangeClientWindow>
    {
        ChangeClientWindow window;
        const string filterType = "t:UtilityAIAsset";

        protected string windowTitle = "Add Clients";
        protected string defaultAiID = "NewUtilityAI";
        protected string defaultAiName = "New Utility AI";
        //  Name for the AI file.
        string aiName { get; set; }



        public override void Init(ChangeClientWindow window, TaskNetworkEditor editor)
        {
            
            taskNetworkEditor = editor;
            //this.window = EditorWindow.GetWindow<ChangeClientWindow>(IsUtilityWindow);
            this.window = window;
            this.window.minSize = this.window.maxSize = new Vector2(windowMinSize, windowMaxSize);
            this.window.titleContent = new GUIContent(windowTitle);

            this.window.ShowUtility();
        }

        public override void Init(ChangeClientWindow window, TaskNetworkEditor editor, Type typeOption) { }


        protected override void DrawWindowContents()
        {
            
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("New AI Name", Styles.TextCenterStyle);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Name: ", GUILayout.Width(windowMinSize * 0.18f));
                    aiName = GUILayout.TextField(aiName);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Ok"))
                    {
                        var utilityAIAsset = new UtilityAIAsset();
                        var aiAsset = utilityAIAsset.CreateAsset(String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiID : aiName,
                                                                 String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiName : aiName);

                        taskNetworkEditor.AddUtilityAIAsset(aiAsset);
                        CloseWindow();
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        CloseWindow();
                    }
                }
            }

            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope())
            {
                foreach (var guid in AssetDatabase.FindAssets(filterType))
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    GUIContent buttonLabel = new GUIContent(Path.GetFileNameWithoutExtension(assetPath));

                    if (GUILayout.Button(buttonLabel, GUILayout.Height(18)))
                    {
                        UtilityAIAsset aiAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as UtilityAIAsset;

                        taskNetworkEditor.AddUtilityAIAsset(aiAsset);
                        CloseWindow();
                    }
                }
            }

            EditorGUILayout.Space();
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Create Mock AI"))
            {
                var utilityAIAsset = new UtilityAIAsset();
                var aiAsset = utilityAIAsset.CreateAsset("DemoMockAI", "DemoMockAI", true);

                taskNetworkEditor.AddUtilityAIAsset(aiAsset);
                CloseWindow();
            }
            GUI.backgroundColor = oldColor;




        }


        protected override void CloseWindow()
        {
            window.Close();
        }


    }


    /// <summary>
    /// Add options window.
    /// </summary>
    public class AddOptionsWindow : OptionsWindow<AddOptionsWindow>
    {
        AddOptionsWindow window;
        protected string windowTitle = "Add Options | AI Object Selector";
        string searchStr;

        Dictionary<Type, Type> displayTypes;


        public override void Init(AddOptionsWindow window, TaskNetworkEditor editor){
            Debug.LogWarning("Need to specifiy a type");
        }

        public override void Init(AddOptionsWindow window, TaskNetworkEditor editor, Type typeOption)
        {
            //Type type = typeof(T);
            Type type = typeOption;
            displayTypes = GetAllOptions(type);
            windowTitle = string.Format("{0} Search | AI Object Selector", type.Name);  
            //windowTitle = string.Format("{0} Search | AI Object Selector", TaskNetworkEditorUtilities.GetAiCategoryName<T>() ); 

            taskNetworkEditor = editor;
            //this.window = EditorWindow.GetWindow<AddOptionsWindow>(IsUtilityWindow);
            this.window = window;
            this.window.minSize = this.window.maxSize = new Vector2(windowMinSize, windowMaxSize);
            this.window.titleContent = new GUIContent(windowTitle);

            this.window.ShowUtility();
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
                if (GUILayout.Button(GUIContent.none, new GUIStyle("SearchCancelButton"), GUILayout.Height(EditorGUIUtility.singleLineHeight))){
                    Debug.Log("TODO:  Clearing Search Field");
                }
            }
            EditorGUILayout.Space();


            foreach (KeyValuePair<Type, Type> item in displayTypes){
                GUIContent buttonLabel = new GUIContent(item.Key.Name);
                if (GUILayout.Button(buttonLabel, contentStyle, GUILayout.Height(18))){
                    
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




    /// <summary>
    /// Add client window.
    /// </summary>
    public class AddNewClientWindow : EditorWindow
    {
        AddNewClientWindow window;
        const string filterType = "t:UtilityAIAsset";

        protected string windowTitle = "Add Clients";
        protected string defaultAiID = "NewUtilityAI";
        protected string defaultAiName = "NewUtilityAI";
        //  Name for the AI file.
        string aiName { get; set; }



        public void Init(AddNewClientWindow window)
        {
            this.window = window;
            this.window.minSize = this.window.maxSize = new Vector2(250, 100);
            this.window.titleContent = new GUIContent("Add Clients");
            this.window.ShowUtility();
        }


        protected virtual void OnGUI(){
            DrawWindowContents();
        }


        protected void DrawWindowContents()
        {

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("New AI Name", Styles.TextCenterStyle);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Name: ", GUILayout.Width(window.minSize.x * 0.18f));
                    aiName = GUILayout.TextField(aiName);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Ok"))
                    {
                        var utilityAIAsset = new UtilityAIAsset();
                        var aiAsset = utilityAIAsset.CreateAsset(String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiID : aiName,
                                                                 String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiName : aiName);

                        CloseWindow();
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        CloseWindow();
                    }
                }
            }



            EditorGUILayout.Space();
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Create Mock AI"))
            {
                var utilityAIAsset = new UtilityAIAsset();
                var aiAsset = utilityAIAsset.CreateAsset("DemoMockAI", "DemoMockAI", true);
                CloseWindow();
            }
            GUI.backgroundColor = oldColor;
        }


        protected void CloseWindow()
        {
            window.Close();
        }


    }






}