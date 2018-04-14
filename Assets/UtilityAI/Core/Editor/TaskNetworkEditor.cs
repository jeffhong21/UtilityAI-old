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
        UtilityAI activeClient;
        List<SerializedObject> aiAssets = new List<SerializedObject>();


        public static bool debugClients, debugClientFoldout;
        public static bool showDefaultInspector, showDeleteAssetOption;

        public static int currentTab;
        public static int selectedClient;


        void OnEnable()
        {
            taskNetwork = target as TaskNetworkComponent;
            taskNetworkSO = new SerializedObject(taskNetwork);

        }


        public virtual void ShowOptionsWindow<T>(Type windowType, Type optionType = null) where T : TaskNetworkOptionsWindow, new()
        {
            T window = new T();

            if (optionType == null)
                window.Init(this);
            else
                window.Init(this, optionType);
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
                    ShowOptionsWindow<AddClientWindow>(typeof(AddClientWindow));
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
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (client != null){
                            //  For Client Options
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.ToggleLeft(GUIContent.none, true);  // GUILayout.Width(Screen.width * 0.6f)

                                if (GUILayout.Button("Debug", EditorStyles.miniButton, GUILayout.Width(48f))){  //  GUILayout.Width(Screen.width * 0.15f)
                                    Debug.Log(client.ai.name);
                                }

                                if (GUILayout.Button(" - ", EditorStyles.miniButton, GUILayout.Width(28f))){
                                    RemoveUtilityAIAsset(i);
                                }
                            }


                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(new GUIContent("AI: "), GUILayout.Width(Screen.width * 0.33f));
                                EditorGUILayout.LabelField(new GUIContent(client.ai.name));
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
                                EditorGUILayout.LabelField("Start Delay: ", GUILayout.Width(Screen.width * 0.33f));
                                client.startDelayMin = EditorGUILayout.FloatField(client.startDelayMin, GUILayout.Width(35f));
                                EditorGUILayout.LabelField("to ", GUILayout.Width(20f));
                                client.startDelayMax = EditorGUILayout.FloatField(client.startDelayMax, GUILayout.Width(35f));
                            }
                            EditorGUILayout.Space();
                        }
                    }
                }

            }  // The group is now ended


        }


        public void AddUtilityAIAsset(UtilityAIAsset aiAsset)
        {
            UtilityAI ai = aiAsset.configuration;
            UtilityAIClient client = new UtilityAIClient(ai);
            //  Add to Lists
            taskNetwork.clients.Add(client);
            aiAssets.Add(new SerializedObject(aiAsset));
            //  Update Editor.
            EditorUtility.SetDirty(target);
            Repaint();
        }

        public void RemoveUtilityAIAsset(int index)
        {
            taskNetwork.clients.RemoveAt(index);
            aiAssets.RemoveAt(index);
            //  Update Editor.
            EditorUtility.SetDirty(target);
            Repaint();
        }


        #endregion


        #region AI Client Editor Inspector

        /// <summary>
        /// Draws the task network client inspector.
        /// </summary>
        protected virtual void DrawTaskNetworkClientInspector()
        {
            //  -- Header
            using (new EditorGUILayout.HorizontalScope()){
                EditorGUILayout.LabelField("Client Editor", EditorStyles.boldLabel);

                GUIContent buttonLabel = activeClient != null ? new GUIContent("Selected Client:  " + activeClient.name) : new GUIContent("No Selected Client");

                if (GUILayout.Button(buttonLabel, EditorStyles.popup, GUILayout.Height(24))){
                    var clients = new GenericMenu();
                    foreach (UtilityAIClient client in taskNetwork.clients){
                        clients.AddItem(new GUIContent(client.ai.name), false, SetActiveClient, client.ai);
                    }
                    clients.ShowAsContext();
                }
            }

            //ContainerNode containerNode = new ContainerNode("TestNode");
            //SerializedObject node = new SerializedObject(containerNode);




            //  -- Inspector 
            EditorGUILayout.Space();
            if (activeClient != null)
            {
                ReorderableList list;
                Selector rootSelector = activeClient.rootSelector;


                EditorGUILayout.LabelField(activeClient.rootSelector.GetType().ToString(), EditorStyles.boldLabel);
                EditorGUILayout.Space();

                //var attr = activeClient.rootSelector.GetType().GetProperty("FriendlyNameAttribute").GetCustomAttribute(typeof(FriendlyNameAttribute));
                //var friendlyName = attr as FriendlyNameAttribute;
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.ToggleLeft(GUIContent.none, true, GUILayout.Width(16f));
                    //  Name of Selected Selector, Qualifier or Action Type
                    EditorGUILayout.LabelField(new GUIContent(rootSelector.GetType().Name + " | TASKNETWORK AI"));  // rootSelector.GetType().Name

                    //  Change Selector
                    if (GUILayout.Button("C", EditorStyles.miniButton, GUILayout.Width(28))){
                        ShowOptionsWindow<AddOptionsWindow>(typeof(AddOptionsWindow), typeof(Selector));
                    }
                    //  Delete Selector
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(28))){
                        Debug.Log("Deleting");
                    }
                }


                //  NameField of Selected Selector, Qualifier or Action
                EditorGUILayout.DelayedTextField("Name: ", "");
                //  Description of Selector.
                EditorGUILayout.LabelField("Description");
                EditorGUILayout.TextArea("", GUILayout.Height(EditorGUIUtility.singleLineHeight * 4));
                EditorGUILayout.Space();


                //  Custom Attribute Fields.
                EditorGUILayout.IntField("First Test Field", 1);
                EditorGUILayout.IntField("Second Test Field", 2);


                //  List of items.
                EditorGUILayout.Space();
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(new GUIContent("Qualifiers"));

                    if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus").image), GUIStyle.none, GUILayout.Width(28))){
                        ShowOptionsWindow<AddOptionsWindow>(typeof(AddOptionsWindow), typeof(QualifierBase));
                    }
                }

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    if (rootSelector.qualifiers.Count != 0){
                        list = new ReorderableList(rootSelector.qualifiers, typeof(IQualifier), true, false, false, false);
                        list.showDefaultBackground = false;
                        HandleReorderableList(list);
                        list.DoLayoutList();
                    }
                    EditorGUILayout.LabelField(new GUIContent(rootSelector.defaultQualifier.GetType().Name));
                }



                #region Debug Actions/Scorers Buttons
                //  -- Debug
                GUILayout.Space(18);
                using (new EditorGUILayout.HorizontalScope()){
                    if (GUILayout.Button("Actions", EditorStyles.miniButton))
                        ShowOptionsWindow<AddOptionsWindow>(typeof(AddOptionsWindow), typeof(ActionBase));
                    if (GUILayout.Button("Scorers", EditorStyles.miniButton))
                        ShowOptionsWindow<AddOptionsWindow>(typeof(AddOptionsWindow), typeof(ScorerBase));
                }

                #endregion
            }




            #region Debug Editor
            //  -- Debug
            GUILayout.Space(8);
            EditorGUILayout.LabelField("Debug");
            if (GUILayout.Button(" Print Root Selector ", EditorStyles.miniButton, GUILayout.Height(18))){
                Debug.Log(activeClient.rootSelector);
                Debug.Log(taskNetworkSO.FindProperty("clients.Array.data[0].ai"));
            }
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(activeClient != null ? DebugEditorUtilities.DebugSelectorInfo(activeClient) : "No Selected Selector", MessageType.Info);

            #endregion
        }



        private void HandleReorderableList(ReorderableList list)
        {

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.list[index];
                EditorGUI.LabelField(rect, new GUIContent(element.GetType().Name));

                if (GUI.Button(new Rect(rect.x + rect.width - 18, rect.y, 18, EditorGUIUtility.singleLineHeight), new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus").image), GUIStyle.none)){
                    list.list.RemoveAt(index);
                    EditorUtility.SetDirty(target);
                    Repaint();
                }
            };

            list.onSelectCallback = (ReorderableList l) =>
            {

            };

        }


        protected virtual void SetActiveClient(object client){
            activeClient = client as UtilityAI;
        }


        #endregion




        #region LastTab
        /// <summary>
        /// Draws the construct client inspector.
        /// </summary>
        protected virtual void DrawPreferenceInspector()
        {
            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Debug AI Clients", EditorStyles.boldLabel);


                if (GUILayout.Button("Debug AI")){
                    Debug.Log(activeClient.rootSelector.GetType());

                    var entity = activeClient.rootSelector;
                    var obj = TaskNetworkEditorUtilities.GetAllProperties(entity);
                    foreach(PropertyInfo info in obj){
                        Debug.Log(info.GetValue(entity));
                    }
                }

            }  // The group is now ended

        }


        #endregion




        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                showDefaultInspector = EditorGUILayout.ToggleLeft("Show Default Inspector", showDefaultInspector);

                if (GUILayout.Button("Clear", EditorStyles.miniButton, GUILayout.Width(65f))){
                    taskNetwork.clients.Clear();
                    aiAssets.Clear();
                    activeClient = null;
                    //Repaint();
                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                showDeleteAssetOption = EditorGUILayout.ToggleLeft("Show Delete Asset Btn", showDeleteAssetOption);
                if(showDeleteAssetOption){
                    if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(65f))){
                        activeClient = null;
                        taskNetwork.clients.Clear();
                        aiAssets.Clear();
                        var results = AssetDatabase.FindAssets("t:UtilityAIAsset", new string[]{AiManager.StorageFolder} );
                        foreach(string guid in results){
                            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
                        }
                        //Repaint();
                    }
                }
            }


            currentTab = GUILayout.Toolbar(currentTab, new string[] { "Clients", "Client Editor", "Preferences" });

            serializedObject.Update();

            switch (currentTab)
            {
                case 0:
                    if(showDefaultInspector) DrawDefaultInspector();
                    DrawTaskNetworkInspector();
                    break;
                case 1:
                    DrawTaskNetworkClientInspector();
                    break;
                case 2:
                    DrawPreferenceInspector();
                    break;
            }

        }









    }






}