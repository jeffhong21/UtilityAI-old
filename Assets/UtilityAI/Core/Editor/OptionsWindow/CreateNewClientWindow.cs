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
    /// Add client window.
    /// </summary>
    public class CreateNewClientWindow : EditorWindow
    {
        CreateNewClientWindow window;
        const string filterType = "t:UtilityAIAsset";

        protected string windowTitle = "Add Clients";
        protected string defaultAiID = "NewUtilityAI";
        protected string defaultAiName = "NewUtilityAI";
        //  Name for the AI file.
        string aiName { get; set; }



        public void Init(CreateNewClientWindow window)
        {
            this.window = window;
            this.window.minSize = this.window.maxSize = new Vector2(250, 100);
            this.window.titleContent = new GUIContent("Add Clients");
            this.window.ShowUtility();
        }


        protected virtual void OnGUI()
        {
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




            //var oldColor = GUI.backgroundColor;
            //GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Create Scan AI"))
            {
                var utilityAIAsset = new UtilityAIAsset();
                var aiAsset = utilityAIAsset.CreateAsset<ScanAIConfig>("DemoMockAI", "DemoMockAI");
                CloseWindow();
            }
            if (GUILayout.Button("Create Move AI"))
            {
                var utilityAIAsset = new UtilityAIAsset();
                var aiAsset = utilityAIAsset.CreateAsset<MoveAIConfig>("DemoMockAI", "DemoMockAI");
                CloseWindow();
            }
            //GUI.backgroundColor = oldColor;
        }


        protected void CloseWindow()
        {
            window.Close();
        }


    }






}