namespace UtilityAI
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    
    public class AIAssetEditor : EditorWindow
    {
        UtilityAIAsset currentClient;
        SerializedObject serializedObject;
        //List<ContainerNode> containerNodes = new List<ContainerNode>();



        GenericMenu clientList;
        Rect menuBarRect,containerRect, inspectorRect, debugRect;


        float menuBarHeight = 18;
        bool debugEditorFoldout = true;

        [MenuItem("AI TaskNetwork/Client Editor")]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<AIAssetEditor>();
            window.minSize = new Vector2(330f, 360f);
            window.maxSize = new Vector2(600f, 4000f);
            window.titleContent = new GUIContent("AI Client");
            window.Show();
        }


        void UpdateClientList(){
            clientList = new GenericMenu();
            foreach (UtilityAIAsset asset in InspectorUtility.GetAllClients()){
                clientList.AddItem(new GUIContent(asset.configuration.name), false, SetActiveClient, asset);
            }
        }

        void SetActiveClient(object c){
            currentClient = c as UtilityAIAsset;
            serializedObject = new SerializedObject(currentClient);
        }




		void OnGUI()
        {
            menuBarRect = new Rect(0, 2, position.width, menuBarHeight);
            DrawMenuBar(menuBarRect);


            Rect rect = new Rect(0, menuBarRect.y + menuBarHeight + 2 ,position.width, position.height );
            GUILayout.BeginArea(rect);

            //  --Header
            using (new EditorGUILayout.HorizontalScope())
            {
                var activeClientName = currentClient == null ? "<None>" : currentClient.name;
                GUILayout.Label("Active Client: " + activeClientName);
            }

            //  -- Container
            using (new EditorGUILayout.VerticalScope())
            {
                if (currentClient != null)
                {
                    var count = currentClient.configuration.selector.qualifiers.Count;
                    GUILayout.Label("Number of Qualifiers:  " + count.ToString());
                }

                //ContainerNode container = new ContainerNode();
                //container.DrawNode(new Vector2(rect.width / 2 - 100, rect.y + 24));
            }


            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Client Editor Inspector");
            }

            DrawDebug();

            GUILayout.EndArea();

            //DrawContainer(containerRect);
            //DrawInspector(inspectorRect);
            //DrawDebug(debugRect);

            if (GUI.changed) Repaint();
        }



        private void DrawMenuBar(Rect rect)
        {
            GUILayout.BeginArea(rect, EditorStyles.toolbar);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (ToolbarButton(new GUIContent("New"))){
                    AddNewClientWindow window = new AddNewClientWindow();
                    window.Init(window);
                }
                GUILayout.Space(5);

                if(ToolbarButton(new GUIContent("Save")))
                {
                    //if(currentClient != null){
                    //    byte[] newData = ProjectAsset.GetData()
                    //
                    //    if(currentClient.configuration.Equals(newData))
                    //    {
                    //        currentClient.configuration = newData;
                    //        EditorUtility.SetDirty((UtilityAIAsset)currentClient);
                    //    }
                    //
                    //    //Debug.Log("Saving");
                    //}
                    Debug.Log("Save");
                }
                GUILayout.Space(5);

                if (ToolbarButton(new GUIContent("Load"))){
                    UpdateClientList();
                    clientList.ShowAsContext();
                }
                //GUILayout.Space(5);
                //if (ToolbarButton(new GUIContent("Reload"))){
                //    //container = new ContainerNode();
                //}
            }
            GUILayout.EndArea();
        }



        #region DebugInspector

        private void DrawDebug()
        {
            GUILayout.Space(150);
            debugEditorFoldout = EditorGUILayout.Foldout(debugEditorFoldout, "< Debug >");
            if (debugEditorFoldout)
            {
                //  -- Debug
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(" Print Root Selector ", EditorStyles.miniButton, GUILayout.Height(18)))
                    {
                        Debug.Log(currentClient.configuration.rootSelector + "\n" + currentClient.configuration.rootSelector.qualifiers.Count);
                        Debug.Log(currentClient.configuration.selector + "\n" + currentClient.configuration.selector.qualifiers.Count);
                        //Debug.Log(serializedObject.FindProperty("configuration.selector").type);
                    }

                    if (GUILayout.Button("Debug AI RootSelector Properties", EditorStyles.miniButton, GUILayout.Height(18)))
                    {
                        Debug.Log("Active Client" + currentClient.configuration.rootSelector.GetType());
                        var entity = currentClient.configuration.rootSelector;
                        var obj = TaskNetworkUtilities.GetAllProperties(entity);
                        foreach (PropertyInfo info in obj)
                        {
                            //info.GetValue(entity);
                            Debug.Log(info.GetValue(entity).GetType());
                            Debug.Log(info.GetValue(entity));
                        }
                    }

                }

                EditorGUILayout.Space();
                string selectorConfigInfo = currentClient != null ? DebugEditorUtilities.DebugSelectorInfo(currentClient.configuration.selector) : "No Selected Selector";
                EditorGUILayout.HelpBox(selectorConfigInfo, MessageType.Info);
            }

        }

        #endregion



        public static bool ToolbarButton(GUIContent content){
            bool clicked;
            clicked = GUILayout.Button(content, EditorStyles.toolbarButton, GUILayout.Width(48));
            return clicked;
        }
    }
}