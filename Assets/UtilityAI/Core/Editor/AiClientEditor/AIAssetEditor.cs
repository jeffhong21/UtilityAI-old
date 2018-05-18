namespace UtilityAI
{
    using System;
    using System.IO;
    using System.Collections;
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



        void AIReflection()
        {

            //if (GUILayout.Button("Debug AI RootSelector Properties")){
            //    var utilityAI = currentClient.configuration.rootSelector;
            //    var obj = TaskNetworkUtilities.GetAllProperties(utilityAI);
            //    foreach (PropertyInfo info in obj){
            //        Debug.Log(info.GetValue(utilityAI));
            //    }
            //}
            if (currentClient == null) return;

            var utilityAI = currentClient.configuration.rootSelector;


            PropertyInfo[] properties = TaskNetworkUtilities.GetAllProperties(utilityAI);
            foreach (PropertyInfo property in properties)
            {
                string displayInfo = "";


                //  Name of the property.
                string propertyName = property.Name;
                //  Property type.
                Type propertyType = property.PropertyType;
                //  What class it was declared in.
                Type declaringType = property.DeclaringType;
                //IEnumerable<CustomAttributeData> customAttributes = property.CustomAttributes;
                //  If it is a property, field or method.
                MemberTypes memberType = property.MemberType;
                object getValue = property.GetValue(utilityAI, null);
                ICollection collection = getValue as ICollection;

                int? count;
                string countString = "";
                string collectionItems = "";
                string itemInfoString = "";

                if (collection != null){
                    count = collection.Count;
                    countString = "(Count: " + count + ")";
                    if(getValue is IEnumerable)
                    {
                        collectionItems += "  Collection Items\n";
                        IList list = getValue as IList;
                        int index = 0;
                        foreach(object obj in list)
                        {
                            itemInfoString += "    " + obj.GetType() + "(" + index + ")\n";
                            PropertyInfo[] itemInfo = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            foreach (PropertyInfo item in itemInfo){
                                itemInfoString += string.Format("      Property Name:     {0}\n" +
                                                               "      Property Type:     {1}\n" +
                                                               "      Property GetValue: {2}\n\n", 
                                                               item.Name, item.PropertyType, item.GetValue(obj, null));
                            }
                            //FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            //foreach (FieldInfo item in fields)
                            //{
                            //    itemInfoString += string.Format("      Field Name:     {0}\n" +
                            //                                    "      Field Type:     {1}\n" +
                            //                                    "      Field GetValue: {2}\n\n",
                            //                                    item.Name, item.FieldType, item.GetValue(obj));
                            //}
                            index++;
                            collectionItems += itemInfoString + "\n";
                        }
                    }

                    //for (int i = 0; i < count; i ++){
                    //    object item = property.GetValue(utilityAI, new object[] { i });
                    //    collectionItems += "    " + item.GetType().Name + "\n";
                    //}

                }
                else{
                    count = null;
                    countString = "<None>";
                }


                displayInfo = string.Format("Property Name:     {0}\n" +
                                            "Property Type:     {1}\n" +
                                            "Declaring Type:    {2}\n" +
                                            "Custom Attributes: {3}\n" +
                                            "Property GetValue: {4} ({7})| {5}\n" +
                                            "{6}\n", 
                                            propertyName, propertyType, declaringType, "<None>", getValue, countString, collectionItems, getValue.GetType());
                
                GUILayout.Label(displayInfo);
            }



        }








		void OnGUI()
        {
            menuBarRect = new Rect(0, 2, position.width, menuBarHeight);
            DrawMenuBar(menuBarRect);

            Rect rect = new Rect(0, menuBarRect.y + menuBarHeight + 2 ,position.width, position.height );
            GUILayout.BeginArea(rect);
            DrawHeader();
            GUILayout.Space(5);

            AIReflection();

            GUILayout.Space(5);
            //DrawDebug();
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
                    CreateNewClientWindow window = new CreateNewClientWindow();
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

        private void DrawHeader()
        {
            //  --Header
            using (new EditorGUILayout.HorizontalScope())
            {
                var activeClientName = currentClient == null ? "<None>" : currentClient.name;
                GUILayout.Label("Active Client: " + activeClientName);
            }

            ////  -- Container
            //using (new EditorGUILayout.VerticalScope())
            //{
            //    if (currentClient != null)
            //    {
            //        //var count = currentClient.configuration.selector.qualifiers.Count;
            //        var count = currentClient.configuration.rootSelector.qualifiers.Count;
            //        GUILayout.Label("Number of Qualifiers:  " + count.ToString());
            //    }

            //    //ContainerNode container = new ContainerNode();
            //    //container.DrawNode(new Vector2(rect.width / 2 - 100, rect.y + 24));
            //}


            //using (new EditorGUILayout.HorizontalScope())
            //{
            //    GUILayout.Label("Client Editor Inspector");
            //}
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
                        //Debug.Log(currentClient.configuration.selector + "\n" + currentClient.configuration.selector.qualifiers.Count);
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
                            //Debug.Log(info.GetValue(entity).GetType());
                            //Debug.Log(info.GetValue(entity));
                        }
                    }

                }

                EditorGUILayout.Space();
                //string selectorConfigInfo = currentClient != null ? DebugEditorUtilities.SelectorConfig(currentClient.configuration.selector) : "No Selected Selector";
                string selectorConfigInfo = currentClient != null ? DebugEditorUtilities.SelectorConfig(currentClient.configuration.rootSelector) : "No Selected Selector";
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