namespace UtilityAI
{
    using System;
    using System.IO;
    using UnityEngine;
    using UnityEditor;


    /// <summary>
    /// Add client window.
    /// </summary>
    public class AddClientWindow : OptionsWindow<AddClientWindow>
    {
        AddClientWindow window;
        const string filterType = "t:UtilityAIAsset";

        protected string windowTitle = "Add Clients";
        protected string defaultAiID = "NewUtilityAI";
        protected string defaultAiName = "New Utility AI";
        //  Name for the AI file.
        string aiName { get; set; }

        protected string defaultDemoAiID = "NewDemoMockAI";
        protected string defaultDemoAiName = "NewDemoMockAI";


        void AddAIAsset(UtilityAIAsset aiAsset)
        {
            //  Add asset and client to TaskNetwork
            UtilityAIClient client = new UtilityAIClient(aiAsset.configuration, taskNetwork.contextProvider);
            taskNetwork.clients.Add(client);
            taskNetwork.assets.Add(aiAsset);

            aiAsset.configuration.OnBeforeSerialize();
            aiAsset.configuration.OnAfterDeserialize();

            EditorUtility.SetDirty(taskNetwork);
            
        }


        public override void Init(AddClientWindow window, TaskNetworkComponent taskNetwork)
        {
            this.taskNetwork = taskNetwork;
            this.window = window;
            this.window.minSize = this.window.maxSize = new Vector2(windowMinSize, windowMaxSize);
            this.window.titleContent = new GUIContent(windowTitle);

            this.window.ShowUtility();
        }


        protected override void OnGUI()
        {
            //  Section for new name
            CreateClientDrawer();

            //  Al Clients
            DrawWindowContents();

            //  Draw Create Demo AI's.
            EditorGUILayout.Space();
            DrawCreateDemoAIContents();
        }


        protected override void DrawWindowContents()
        {
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

                        //  Add asset and client to TaskNetwork
                        AddAIAsset(aiAsset);
                        // -----------------------------------

                        //taskNetworkEditor.AddUtilityAIAsset(aiAsset);
                        CloseWindow();
                    }
                }
            }

        }


        protected void DrawCreateDemoAIContents()
        {
            //  Section for new name
            CreateClientDrawer(true);

            EditorGUILayout.Space();


            if (GUILayout.Button("Create Scan AI"))
            {
                var utilityAIAsset = new UtilityAIAsset();
                var aiAsset = utilityAIAsset.CreateAsset<ScanAIConfig>("DemoScanAI", "DemoScanAI", taskNetwork.selectAiAssetOnCreate);
                //  Add asset and client to TaskNetwork
                AddAIAsset(aiAsset);
                CloseWindow();
            }
            if (GUILayout.Button("Create Move AI"))
            {
                var utilityAIAsset = new UtilityAIAsset();
                var aiAsset = utilityAIAsset.CreateAsset<MoveAIConfig>("DemoMoveAI", "DemoMoveAI", taskNetwork.selectAiAssetOnCreate);
                //  Add asset and client to TaskNetwork
                AddAIAsset(aiAsset);
                CloseWindow();
            }

        }




        protected override void CloseWindow()
        {
            window.Close();
        }



        void CreateClientDrawer(bool isDemoAI = false)
        {
            string _defaultAiID = isDemoAI == false ? defaultDemoAiID : defaultAiID;
            string _defaultAiName = isDemoAI == false ? defaultDemoAiName : defaultAiName;
            UtilityAIAsset utilityAIAsset;

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
                        //utilityAIAsset = isDemoAI == false ? new UtilityAIAsset() : new UtilityAIConfig();
                        utilityAIAsset = new UtilityAIAsset();
                        var aiAsset = utilityAIAsset.CreateAsset(String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? _defaultAiID : aiName,
                                                                 String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? _defaultAiName : aiName,
                                                                 taskNetwork.selectAiAssetOnCreate);
                        //  Add asset and client to TaskNetwork
                        AddAIAsset(aiAsset);
                        // -----------------------------------

                        //editor.AddUtilityAIAsset(aiAsset);
                        CloseWindow();
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        CloseWindow();
                    }
                }
            }
        }









    }
}