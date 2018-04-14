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



    public abstract class TaskNetworkOptionsWindow : EditorWindow
    {
        protected bool IsUtilityWindow = true;
        protected int windowMinSize = 250;
        protected int windowMaxSize = 350;


        protected TaskNetworkEditor taskNetworkEditor { get; set; }
        protected TaskNetworkComponent taskNetwork { get; set; }



        public abstract void Init(TaskNetworkEditor editor);
        public abstract void Init(TaskNetworkEditor editor, Type typeOption);
        protected abstract void CloseWindow();

        protected abstract void DrawWindowContents(TaskNetworkComponent taskNetwork);
        protected virtual void OnGUI(){
            DrawWindowContents(taskNetwork);
        }
    }


    public class AddClientWindow : TaskNetworkOptionsWindow
    {
        static AddClientWindow window;
        const string filterType = "t:UtilityAIAsset";

        protected string windowTitle = "Add Clients";
        protected string defaultAiID = "NewUtilityAI";
        protected string defaultAiName = "New Utility AI";
        //  Name for the AI file.
        string aiName { get; set; }

        GUIStyle header;

        public override void Init(TaskNetworkEditor editor)
        {
            header = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter
            };

            taskNetworkEditor = editor;
            taskNetwork = taskNetworkEditor.target as TaskNetworkComponent;

            window = EditorWindow.GetWindow<AddClientWindow>(IsUtilityWindow);
            window.minSize = window.maxSize = new Vector2(windowMinSize, windowMaxSize);
            window.titleContent = new GUIContent(windowTitle);


            window.ShowUtility();
        }

        public override void Init(TaskNetworkEditor editor, Type typeOption){
            
        }


        protected override void DrawWindowContents(TaskNetworkComponent taskNetwork)
        {
            GUIStyle header = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter
            };

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("New AI Name", header);
                using (new EditorGUILayout.HorizontalScope()){
                    GUILayout.Label("Name: ", GUILayout.Width(windowMinSize * 0.18f));
                    aiName = GUILayout.TextField(aiName);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Ok")){
                        var utilityAIAsset = new UtilityAIAsset();
                        var aiAsset = utilityAIAsset.CreateAsset(String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiID : aiName,
                                                                 String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? defaultAiName : aiName);

                        taskNetworkEditor.AddUtilityAIAsset(aiAsset);
                        CloseWindow();
                    }
                    if (GUILayout.Button("Cancel")){
                        CloseWindow();
                    }
                }
            }

            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope())
            {
                foreach (var guid in AssetDatabase.FindAssets(filterType)){
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    GUIContent buttonLabel = new GUIContent(Path.GetFileNameWithoutExtension(assetPath));

                    if (GUILayout.Button(buttonLabel, GUILayout.Height(18))){
                        UtilityAIAsset aiAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as UtilityAIAsset;

                        taskNetworkEditor.AddUtilityAIAsset(aiAsset);
                        CloseWindow();
                    }
                }
            }
        }


        protected override void CloseWindow(){
            window.Close();
        }


    }



    public class AddOptionsWindow : TaskNetworkOptionsWindow
    {
        static AddOptionsWindow window;
        protected string windowTitle = "Add Options | AI Object Selector";
        string searchStr;

        Dictionary<Type, Type> displayTypes;


        public override void Init(TaskNetworkEditor editor){
            Debug.LogWarning("Need to speicify a type");
        }

        public override void Init(TaskNetworkEditor editor, Type typeOption)
        {
            taskNetworkEditor = editor;
            taskNetwork = taskNetworkEditor.target as TaskNetworkComponent;

            displayTypes = GetAllOptions(typeOption);
            windowTitle = string.Format("{0} Search | AI Object Selector", typeOption.Name);


            window = EditorWindow.GetWindow<AddOptionsWindow>(IsUtilityWindow);
            window.minSize = window.maxSize = new Vector2(windowMinSize, windowMaxSize);
            window.titleContent = new GUIContent(windowTitle);

            window.ShowUtility();
        }


        protected override void DrawWindowContents(TaskNetworkComponent taskNetwork)
        {
            GUIStyle contentStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter
            };


            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                string newSearchStr = EditorGUILayout.TextField(searchStr, new GUIStyle("SearchTextField"), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                if (GUILayout.Button(GUIContent.none, new GUIStyle("SearchCancelButton"), GUILayout.Height(EditorGUIUtility.singleLineHeight ))){
                    searchStr = "";
                }
            }
            EditorGUILayout.Space();


            foreach (KeyValuePair<Type, Type> item in displayTypes)
            {
                GUIContent buttonLabel = new GUIContent(item.Key.Name);
                if (GUILayout.Button(buttonLabel, contentStyle, GUILayout.Height(18))){
                    Debug.Log(item.Key.Name);
                    CloseWindow();
                }
            }

        }


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




    //public class AddOptionsWindow<TOption> : TaskNetworkOptionsWindow
    //{
    //    static AddOptionsWindow<TOption> window;
    //    protected string windowTitle = "Add Options | AI Object Selector";
    //    string searchStr;

    //    Dictionary<Type, Type> displayTypes;


    //    public override void Init(TaskNetworkEditor editor)
    //    {
    //        taskNetworkEditor = editor;
    //        taskNetwork = taskNetworkEditor.target as TaskNetworkComponent;

    //        displayTypes = GetAllOptions(typeof(TOption));
    //        windowTitle = string.Format("{0} Search | AI Object Selector", typeof(TOption).BaseType);


    //        //window = EditorWindow.GetWindow<AddOptionsWindow<TOption>>(IsUtilityWindow);
    //        window = EditorWindow.GetWindow(typeof(AddOptionsWindow<TOption>), true) as AddOptionsWindow<TOption>;
    //        window.minSize = window.maxSize = new Vector2(windowMinSize, windowMaxSize);
    //        window.titleContent = new GUIContent(windowTitle);

    //        window.Show();
    //    }



    //    protected override void DrawWindowContents(TaskNetworkComponent taskNetwork)
    //    {
    //        GUIStyle contentStyle = new GUIStyle(GUI.skin.button)
    //        {
    //            alignment = TextAnchor.MiddleCenter
    //        };


    //        EditorGUILayout.Space();
    //        string newSearchStr = EditorGUILayout.TextField(searchStr, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.Width(windowMinSize));
    //        if (GUILayout.Button(GUIContent.none, new GUIStyle("SearchCancelButton"),GUILayout.Height(18)))
    //        {
    //            Debug.Log("Clear");
    //        }

    //        EditorGUILayout.Space();

    //        foreach (KeyValuePair<Type, Type> item in displayTypes){
    //            GUIContent buttonLabel = new GUIContent(item.Key.Name);
    //            if (GUILayout.Button(buttonLabel, contentStyle, GUILayout.Height(18))) 
    //            {
    //                Debug.Log(item.Key.Name);
    //                CloseWindow();
    //            }
    //        }



    //    }


    //    private Dictionary<Type, Type> GetAllOptions(Type optionType){
            
    //        Dictionary<Type, Type> availableTypes = new Dictionary<Type, Type>();
    //        //  Gets all custom Types in this assembly and adds it to a list.
    //        var optionTypes = Assembly.GetAssembly(optionType).GetTypes()
    //                             .Where(t => t.IsClass && t.Namespace == this.GetType().Namespace)
    //                             .ToList();
    //        //optionTypes.ForEach(t => Debug.Log(t));

    //        //  Go through each item in list and check if its base class equals the Types we defined.
    //        foreach (Type type in optionTypes){
    //            if (type.BaseType == optionType && availableTypes.ContainsKey(type.BaseType) == false) 
    //                availableTypes.Add(type, type.BaseType);
    //        }
    //        return availableTypes;
    //    }



    //    protected override void CloseWindow()
    //    {
    //        window.Close();
    //    }


    //}











    //public class ConstructMockAIs : TaskNetworkOptionsWindow<ConstructMockAIs>
    //{
    //    static ConstructMockAIs window;
    //    static string windowTitle = "Construct MockAI's";





    //    public override void Init(TaskNetworkComponent tn){
    //        window = EditorWindow.GetWindow<ConstructMockAIs>(IsUtilityWindow);
    //        window.titleContent = new GUIContent(windowTitle);
    //        window.minSize = window.maxSize = new Vector2(windowMinSize, windowMaxSize);
    //        window.Show();

    //    }


    //    protected override void DrawWindowContents(TaskNetworkComponent taskNetwork)
    //    {
    //        if (GUILayout.Button(new GUIContent("MockMoveAI")))
    //        {
    //            var aiClient = new UtilityAIAsset();
    //            aiClient.CreateAsset("MockMoveAI", "MovementAI", typeof(MockMoveAI));
    //            CloseWindow();
    //        }

    //        if (GUILayout.Button(new GUIContent("MockScanAI")))
    //        {
    //            var aiClient = new UtilityAIAsset();
    //            aiClient.CreateAsset("MockScanningAI", "ScanningAI", typeof(MockScanningAI));
    //            CloseWindow();
    //        }
    //    }



    //    protected override void CloseWindow(){
    //        window.Close();
    //    }

    //}



}