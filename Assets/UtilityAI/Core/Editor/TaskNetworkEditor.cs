namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;


    [CustomEditor(typeof(TaskNetworkComponent))]
    public class TaskNetworkEditor : Editor
    {
        TaskNetworkComponent taskNetwork;
        SerializedObject taskNetworkSO;

        //  Currently selected Client.  Used for the Editor tab.
        UtilityAIAsset activeClient;
        SerializedObject activeClientSO;
        GenericMenu clientList;

        bool showDefaultInspector, showDeleteAssetOption;
        bool debugEditorFoldout = true;
        string selectorConfigInfo;
        int currentTab;



        void OnEnable()
        {
            taskNetwork = target as TaskNetworkComponent;
            taskNetworkSO = new SerializedObject(taskNetwork);

            //UpdateClientList();

            //selectorConfigInfo = activeClient != null ? DebugEditorUtilities.SelectorConfig(activeClient.configuration.selector) : "No Selected Selector";
            selectorConfigInfo = activeClient != null ? DebugEditorUtilities.SelectorConfig(activeClient.configuration.rootSelector) : "No Selected Selector";
        }



		public virtual void ShowOptionsWindow<T>(Type optionType = null) where T : OptionsWindow<T>, new(){
            T window = new T();
            if (optionType == null)
                window.Init(window, taskNetwork);
            else
                window.Init(window, taskNetwork, optionType);
        }


        //void UpdateClientList(){
        //    clientList = new GenericMenu();
        //    foreach (UtilityAIClient client in taskNetwork.clients){
        //        clientList.AddItem(new GUIContent(client.ai.name), false, SetActiveClient, client);
        //    }
        //}


        //public void AddUtilityAIAsset(UtilityAIAsset aiAsset){
        //    UtilityAIClient client = new UtilityAIClient(aiAsset.configuration, taskNetwork.contextProvider);
        //    //  Add to Lists
        //    taskNetwork.clients.Add(client);
        //    taskNetwork.assets.Add(aiAsset);
        //    //  Update Editor.
        //    EditorUtility.SetDirty(target);
        //    UpdateClientList();
        //    Repaint();

        //}


        public void RemoveUtilityAIAsset(int index){
            activeClient = null;
            taskNetwork.clients.RemoveAt(index);
            taskNetwork.assets.RemoveAt(index);
            //  Update Editor.
            EditorUtility.SetDirty(target);
            //UpdateClientList();
            Repaint();
        }


        //public virtual void SetActiveClient(object c){
        //    UtilityAIClient client = c as UtilityAIClient;
        //    int index = taskNetwork.clients.IndexOf(client);
        //    activeClient = taskNetwork.assets[index];
        //    activeClientSO = new SerializedObject(activeClient);
        //}

        //  Debug
        private void ActiveClientMessageBox(){
            string _activeClient = activeClient == null ? "<None>" : activeClient.name;
            string _activeClientSO = activeClient == null ? "<None>" : activeClientSO.ToString();
            string activeClientMsg = string.Format("Active Client: {0}\nSerializeObject: {1}", _activeClient, _activeClientSO);
            EditorGUILayout.HelpBox(activeClientMsg, MessageType.Info);
            EditorGUILayout.Space();
        }


        private void ContextMessageBox(){
            string context = taskNetwork.context == null ? "<None>" : taskNetwork.context.GetType().ToString();
            string contextProvider = taskNetwork.contextProvider == null ? "<None>" : taskNetwork.contextProvider.GetType().ToString();
            string contextMsg = string.Format("Context: {0}\nContextProvider: {1}", context, contextProvider);
            EditorGUILayout.HelpBox(contextMsg, MessageType.Info);
            EditorGUILayout.Space();
        }


        #region AI Client Inspector

        /// <summary>
        /// Client Inspector
        /// </summary>
        protected virtual void DrawTaskNetworkInspector()
        {

            //  Displaying header options.
            using (new EditorGUILayout.HorizontalScope()){
                EditorGUILayout.LabelField("AIs", EditorStyles.boldLabel);

                //  Add a new UtilityAIClient
                if (GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.Width(65f))){
                    InspectorUtility.ShowOptionsWindow<AddClientWindow>(taskNetwork);
                    //ShowOptionsWindow<AddClientWindow>();
                }
            }


            //  Displaying the AI Clients
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (taskNetwork.clients.Count == 0){
                    EditorGUILayout.HelpBox("There are no AI's attached to this TaskNetworkComponent.", MessageType.Info);
                }


                for (int i = 0; i < taskNetwork.clients.Count; i++)  
                {
                    UtilityAIClient client = taskNetwork.clients[i];
                    UtilityAIAsset asset = taskNetwork.assets[i];
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (asset != null && client != null){
                            //  For Client Options
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.ToggleLeft(GUIContent.none, true);  // GUILayout.Width(Screen.width * 0.6f)

                                if (GUILayout.Button("Debug", EditorStyles.miniButton, GUILayout.Width(48f))){  //  GUILayout.Width(Screen.width * 0.15f)
                                    Debug.Log(client.ai.name);
                                    Selection.activeObject = asset;
                                }

                                if (InspectorUtility.OptionsPopupButton(InspectorUtility.DeleteContent)){
                                    RemoveUtilityAIAsset(i);
                                }
                            }


                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(new GUIContent("AI: "), GUILayout.Width(Screen.width * 0.33f));
                                EditorGUILayout.LabelField(new GUIContent(asset.friendlyName));
                                //EditorGUILayout.LabelField(new GUIContent(client.ai.name));  //  Name resets after scene reload
                            }


                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField("Interval: ", GUILayout.Width(Screen.width * 0.33f));
                                client.intervalMin = EditorGUILayout.FloatField(client.intervalMin, GUILayout.Width(35f));
                                EditorGUILayout.LabelField("to ", GUILayout.Width(20f));
                                client.intervalMax = EditorGUILayout.FloatField(client.intervalMax, GUILayout.Width(35f));
                            }

                            //  For Client StartDelay
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                //EditorGUILayout.LabelField("Start Delay: ", GUILayout.Width(Screen.width * 0.33f));
                                //client.startDelayMin = EditorGUILayout.FloatField(client.startDelayMin, GUILayout.Width(35f));
                                //EditorGUILayout.LabelField("to ", GUILayout.Width(20f));
                                //client.startDelayMax = EditorGUILayout.FloatField(client.startDelayMax, GUILayout.Width(35f));
                                float min = client.startDelayMin;
                                float max = client.startDelayMax;
                                InspectorUtility.MinMaxInputField(ref min, ref max, new GUIContent("Start Delay: "));
                            }
                            EditorGUILayout.Space();
                        }
                    }
                }

            }  // The group is now ended



            //  -- Active Client Info 
            if(taskNetwork.assets.Count > 0){
                ContextMessageBox();
                //ActiveClientMessageBox();
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Editor", EditorStyles.miniButton, GUILayout.Width(65f))){
                    AIAssetEditor.Init();
                }
            }
            GUILayout.Space(8);



        }


        #endregion



        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            using (new EditorGUILayout.HorizontalScope())
            {
                taskNetwork.showDefaultInspector = EditorGUILayout.ToggleLeft("Show Default Inspector", taskNetwork.showDefaultInspector);

                if (GUILayout.Button("Clear", EditorStyles.miniButton, GUILayout.Width(65f))){
                    taskNetwork.clients.Clear();
                    taskNetwork.assets.Clear();
                    activeClient = null;
                    //Repaint();
                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                taskNetwork.showDeleteAssetOption = EditorGUILayout.ToggleLeft("Show Delete Asset Btn", taskNetwork.showDeleteAssetOption);
                if(taskNetwork.showDeleteAssetOption){
                    if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(65f))){
                        activeClient = null;
                        taskNetwork.clients.Clear();
                        taskNetwork.assets.Clear();
                        var results = AssetDatabase.FindAssets("t:UtilityAIAsset", new string[]{AiManager.StorageFolder} );
                        foreach(string guid in results){
                            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
                        }
                        //Repaint();
                    }
                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                taskNetwork.selectAiAssetOnCreate = EditorGUILayout.ToggleLeft("Select Asset On Create", taskNetwork.selectAiAssetOnCreate);
            }

            GUILayout.Space(8);
            if (taskNetwork.showDefaultInspector){
                DrawDefaultInspector();
                GUILayout.Space(8);
            } 
            DrawTaskNetworkInspector();


            //currentTab = GUILayout.Toolbar(currentTab, new string[] { "Clients", "Client Editor", "Preferences" });
            //switch (currentTab)
            //{
            //    case 0:
            //        if(taskNetwork.showDefaultInspector) DrawDefaultInspector();
            //        DrawTaskNetworkInspector();
            //        break;
            //    case 1:
            //        DrawTaskNetworkClientInspector();
            //        break;
            //    case 2:
            //        DrawPreferenceInspector();
            //        break;
            //}


            serializedObject.ApplyModifiedProperties();
        }









        #region AI Client Editor Inspector

        ///// <summary>
        ///// Draws the task network client inspector.
        ///// </summary>
        //protected virtual void DrawTaskNetworkClientInspector()
        //{
        //    //  -- Header
        //    using (new EditorGUILayout.HorizontalScope())
        //    {
        //        EditorGUILayout.LabelField("Client Editor", EditorStyles.boldLabel);
        //        GUIContent buttonLabel = activeClient != null ? new GUIContent("Selected Client:  " + activeClient.name) : new GUIContent("No Selected Client");
        //        if (GUILayout.Button(buttonLabel, EditorStyles.popup, GUILayout.Height(24)))
        //        {
        //            clientList.ShowAsContext();
        //        }
        //    }
        //    //  -- Active Client Info 
        //    ActiveClientMessageBox();


        //    // -- Editor Inspector
        //    activeClient = InspectorDrawer.ElementInspector(activeClient);


        //    #region EditorInspector
        //    //  -- Inspector 
        //    //if (activeClient != null)
        //    //{
        //    //    bool elementIsDisable;
        //    //    string elementName;
        //    //    string elementDescription;
        //    //    string elementDisplayName;
        //    //    ReorderableList list;
        //    //    Selector rootSelector = activeClient.rootSelector;

        //    //    EditorGUILayout.LabelField(activeClient.rootSelector.GetType().ToString(), EditorStyles.boldLabel);
        //    //    EditorGUILayout.Space();

        //    //    //var attr = activeClient.rootSelector.GetType().GetProperty("FriendlyNameAttribute").GetCustomAttribute(typeof(FriendlyNameAttribute));
        //    //    //var friendlyName = attr as FriendlyNameAttribute;
        //    //    using (new EditorGUILayout.HorizontalScope())
        //    //    {
        //    //        elementIsDisable = EditorGUILayout.ToggleLeft(new GUIContent(rootSelector.GetType().Name + " | TASKNETWORK AI"), true);

        //    //        if(InspectorUtility.OptionsPopupButton(InspectorUtility.ChangeContent)){
        //    //            ShowOptionsWindow<AddOptionsWindow>(typeof(Selector));
        //    //        }
        //    //        if (InspectorUtility.OptionsPopupButton(InspectorUtility.DeleteContent)){
        //    //            Debug.Log("Deleting");
        //    //        }
        //    //    }


        //    //    //  NameField of Selected Selector, Qualifier or Action
        //    //    elementName = InspectorUtility.NameField("Test Name");
        //    //    //  Description of Selector.
        //    //    elementDescription = InspectorUtility.DescriptionField("", 2);


        //    //    //  Custom Attribute Fields.
        //    //    EditorGUILayout.LabelField(" <Custom Attributes> ");
        //    //    EditorGUILayout.IntField("First Test Field", 1);
        //    //    EditorGUILayout.IntField("Second Test Field", 2);



        //    //    //  Header for list of items.
        //    //    EditorGUILayout.Space();
        //    //    using (new EditorGUILayout.HorizontalScope())
        //    //    {
        //    //        elementDisplayName = "Qualifiers";
        //    //        EditorGUILayout.LabelField(new GUIContent(elementDisplayName));

        //    //        if (InspectorUtility.OptionsPopupButton(InspectorUtility.AddContent))
        //    //        {
        //    //            ShowOptionsWindow<AddOptionsWindow>(typeof(QualifierBase));
        //    //            activeClient.rootSelector.qualifiers.Add(new CompositeScoreQualifier());
        //    //            aiAssets[0].ApplyModifiedProperties();
        //    //        }
        //    //    }

        //    //    //  List of items.
        //    //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        //    //    {
        //    //        if (rootSelector.qualifiers.Count != 0){
        //    //            list = new ReorderableList(rootSelector.qualifiers, typeof(IQualifier), true, false, false, false);
        //    //            list.showDefaultBackground = false;
        //    //            HandleReorderableList(list);
        //    //            list.DoLayoutList();
        //    //        }
        //    //        EditorGUILayout.LabelField(new GUIContent(rootSelector.defaultQualifier.GetType().Name));
        //    //    }


        //    //}
        //    #endregion


        //    #region Debug Editor

        //    GUILayout.Space(18);

        //    debugEditorFoldout = EditorGUILayout.Foldout(debugEditorFoldout, "< Debug >");
        //    if (debugEditorFoldout)
        //    {
        //        using (new EditorGUILayout.HorizontalScope())
        //        {
        //            if (GUILayout.Button("Actions", EditorStyles.miniButton))
        //            {
        //                InspectorUtility.ShowOptionsWindow<AddOptionsWindow>(taskNetwork, typeof(ActionBase));
        //            }
        //            if (GUILayout.Button("Scorers", EditorStyles.miniButton))
        //            {
        //                InspectorUtility.ShowOptionsWindow<AddOptionsWindow>(taskNetwork, typeof(ScorerBase));
        //            }
        //        }

        //        //  -- Debug
        //        if (GUILayout.Button(" Print Root Selector ", EditorStyles.miniButton, GUILayout.Height(18)))
        //        {
        //            Debug.Log(activeClient.configuration.rootSelector + "\n" + activeClient.configuration.rootSelector.qualifiers.Count);
        //            //Debug.Log(activeClient.configuration.selector + "\n" + activeClient.configuration.selector.qualifiers.Count);
        //            Debug.Log(activeClientSO.FindProperty("configuration.selector").type);
        //            Debug.Log(taskNetworkSO.FindProperty("clients.Array.data[0].ai"));
        //        }
        //        EditorGUILayout.Space();
        //        //selectorConfigInfo = activeClient != null ? DebugEditorUtilities.SelectorConfig(activeClient.configuration.selector) : "No Selected Selector";
        //        selectorConfigInfo = activeClient != null ? DebugEditorUtilities.SelectorConfig(activeClient.configuration.rootSelector) : "No Selected Selector";
        //        EditorGUILayout.HelpBox(selectorConfigInfo, MessageType.Info);
        //    }


        //    #endregion
        //}





        #endregion


        #region LastTab

        ///// <summary>
        ///// Draws the construct client inspector.
        ///// </summary>
        //protected virtual void DrawPreferenceInspector()
        //{
        //    EditorGUILayout.Space();
        //    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        //    {
        //        EditorGUILayout.LabelField("Debug AI Clients", EditorStyles.boldLabel);


        //        if (GUILayout.Button("Debug AI RootSelector Properties"))
        //        {
        //            Debug.Log(activeClient.configuration.rootSelector.GetType());

        //            var entity = activeClient.configuration.rootSelector;
        //            var obj = TaskNetworkUtilities.GetAllProperties(entity);
        //            foreach (PropertyInfo info in obj)
        //            {
        //                Debug.Log(info.GetValue(entity));
        //            }
        //        }

        //        //if (GUILayout.Button("Debug Serialized AI"))
        //        //{
        //        //    if (aiAssets.Count == 0)
        //        //        Debug.Log("AI Asset Count is: " + aiAssets.Count);

        //        //    if(aiAssets.Count > 0){
        //        //        SerializedProperty selector = aiAssets[0].FindProperty("selector");
        //        //        Debug.Log(selector);
        //        //        Debug.Log(selector.propertyPath);
        //        //        Debug.Log(selector.propertyType);
        //        //    }
        //        //}

        //    }  // The group is now ended
        //}

        #endregion




    }






}