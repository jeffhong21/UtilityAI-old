namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;


    /// <summary>
    /// Utility for creating various UI controls
    /// </summary>
    public static class InspectorUtility
    {
        public static GUIContent DeleteContent;
        public static GUIContent ChangeContent;
        public static GUIContent AddContent;

        static readonly float miniBtnWidth;
        static readonly float miniBtnHeight;


        static InspectorUtility()
        {
            DeleteContent = new GUIContent(Icons.DeleteIcon, "<Tooltip> Remove option content");
            ChangeContent = new GUIContent(Icons.ChangeIcon, "<Tooltip> Change options content");
            AddContent = new GUIContent(Icons.AddIcon, "<Tooltip> Add new options content");

            miniBtnWidth = 28f;
            miniBtnHeight = EditorGUIUtility.singleLineHeight;
        }



        /// <summary>
        /// Small square mini button with icon
        /// </summary>
        /// <returns><c>true</c>, if popup button was optionsed, <c>false</c> otherwise.</returns>
        /// <param name="content">Content.</param>
        public static bool OptionsPopupButton(GUIContent content)
        {
            bool clicked;
            clicked = GUILayout.Button(content, EditorStyles.miniButton, GUILayout.Width(miniBtnWidth), GUILayout.Height(miniBtnHeight) );
            //clicked = GUILayout.Button(content, new GUIStyle(GUI.skin.button), GUILayout.Width(miniBtnWidth), GUILayout.Height(miniBtnHeight));
            return clicked;
        }

        /// <summary>
        /// Field for writing descriptions
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="description">Description.</param>
        /// <param name="lines">Lines.</param>
        public static string DescriptionField(string description, int lines = 3)
        {
            EditorGUILayout.LabelField("Description");
            description = EditorGUILayout.TextArea(description, GUILayout.Height((EditorGUIUtility.singleLineHeight + 2) * lines));
            EditorGUILayout.Space();

            return description;
        }

        /// <summary>
        /// Field for inputing name
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="name">Name.</param>
        public static string NameField(string name){
            name = EditorGUILayout.DelayedTextField("Name: ", name);
            return name;
        }


        public static float[] MinMaxInputField(ref float min, ref float max, GUIContent label, float fieldWidth = 35f)
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(Screen.width * 0.33f));
            min = EditorGUILayout.FloatField(min, GUILayout.Width(fieldWidth));
            EditorGUILayout.LabelField("to ", GUILayout.Width(20f));
            max = EditorGUILayout.FloatField(max, GUILayout.Width(fieldWidth));
            return new float[] { min, max };
        }





        public static void CreateClientDrawer(string aiName, TaskNetworkComponent taskNetwork, bool isDemoAI = false)
        {
            int windowSize = 250;
            string defaultAiID = "NewUtilityAI";
            string defaultAiName = "New Utility AI";
            string defaultDemoAiID = "NewDemoMockAI";
            string defaultDemoAiName = "NewDemoMockAI";

            string _defaultAiID = isDemoAI == false ? defaultDemoAiID : defaultAiID;
            string _defaultAiName = isDemoAI == false ? defaultDemoAiName : defaultAiName;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("New AI Name", Styles.TextCenterStyle);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Name: ", GUILayout.Width(windowSize * 0.18f));
                    aiName = GUILayout.TextField(aiName);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Ok"))
                    {
                        var utilityAIAsset = new UtilityAIAsset();
                        //var aiAsset = utilityAIAsset.CreateAsset(String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? _defaultAiID : aiName,
                                                                 //String.IsNullOrEmpty(aiName) || String.IsNullOrWhiteSpace(aiName) ? _defaultAiName : aiName,
                                                                 //taskNetwork.selectAiAssetOnCreate);

                        //editor.AddUtilityAIAsset(aiAsset);
                        //CloseWindow();
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        //CloseWindow();
                    }
                }
            }
        }


        public static void HandleReorderableList(ReorderableList list)
        {
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = list.list[index];
                EditorGUI.LabelField(rect, new GUIContent(element.GetType().Name));

                if (GUI.Button(new Rect(rect.x + rect.width - 18, rect.y, 18, EditorGUIUtility.singleLineHeight), new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus").image), GUIStyle.none))
                {
                    list.list.RemoveAt(index);
                }
            };

            list.onSelectCallback = (ReorderableList l) =>
            {

            };
        }








        /// <summary>
        /// Shows the options window.
        /// </summary>
        /// <param name="editor">Editor.</param>
        /// <param name="optionType">Option type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ShowOptionsWindow<T>(TaskNetworkComponent taskNetwork, Type optionType = null) where T : OptionsWindow<T>, new()
        {
            T window = new T();
            if (optionType == null)
                window.Init(window, taskNetwork);
            else
                window.Init(window, taskNetwork, optionType);
        }


        public static UtilityAIAsset[] GetAllClients()
        {
            string filterType = "t:UtilityAIAsset";
            var aiAssets = new List<UtilityAIAsset>();

            foreach (var guid in AssetDatabase.FindAssets(filterType)){
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UtilityAIAsset aiAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as UtilityAIAsset;
                aiAssets.Add(aiAsset);
            }
            return aiAssets.ToArray();
        }



    }



    public static class Icons
    {
        public static Texture2D DeleteIcon;
        public static Texture2D ChangeIcon;
        public static Texture2D AddIcon;

        static Icons()
        {
            DeleteIcon = EditorGUIUtility.IconContent("Toolbar Minus").image as Texture2D;
            ChangeIcon = EditorGUIUtility.IconContent("preAudioLoopOff").image as Texture2D;
            AddIcon = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D;
        }
    }



    public static class Styles
    {
        //  Node Styles.
        public static GUIStyle defaultNodeStyle;

        //  Text Styles.
        public static GUIStyle TextCenterStyle;
        public static GUIStyle DebugTextStyle;
        //  Node Style
        public static GUIStyle nodeStyle;
        public static GUIStyle selectedNodeStyle;

        static Styles()
        {

            defaultNodeStyle = new GUIStyle{
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                contentOffset = new Vector2(25, -3)
            };


            TextCenterStyle = new GUIStyle{
                alignment = TextAnchor.MiddleCenter
            };

            DebugTextStyle = new GUIStyle
            {
                richText = true
            };


            nodeStyle = new GUIStyle("TL SelectionButton PreDropGlow")
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
            };

            selectedNodeStyle = new GUIStyle("TL SelectionButton PreDropGlow")
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
            };


        }
    }


}