namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;


    [CustomEditor(typeof(UtilityAIComponent))]
    public class UtilityAIComponentEditor : Editor
    {
        
        private UtilityAIComponent utilityAIComponent;



        private void OnEnable()
        {
            utilityAIComponent = target as UtilityAIComponent;

        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); //base.OnInspectorGUI();

            if(utilityAIComponent.showCustomInspector)
            {
                if (GUILayout.Button("Create New AI"))
                {
                    OptionTypesWindow window = new OptionTypesWindow();
                    window.Init(window);
                }
                if (GUILayout.Button("Debug New AI"))
                {
                    if (utilityAIComponent.ai.Count > 0)
                    {
                        Debug.Log(utilityAIComponent.ai[0].description);
                        Debug.Log(utilityAIComponent.ai[0].configuration);
                        //Debug.Log(utilityAIComponent.ai[0].configuration.ai.rootSelector.qualifiers[0].action);
                    }

                }
            }

        }




    }


    public class OptionTypesWindow : EditorWindow // where T : class
    {
        OptionTypesWindow window;
        List<Type> availibleTypes = new List<Type>() ;


        public void Init (OptionTypesWindow window)
        {
            this.window = window;
            this.window.titleContent = new GUIContent(" AI Types ");
            this.window.minSize = this.window.maxSize = new Vector2(250, 350);


            //availibleTypes = UtilityAIManager.GetAllAvailibleOfType(type);
            //optionType = UtilityAIManager.GetOptionType(type);
            //this.property = property;

            this.window.ShowUtility();
        }


        void OnGUI()
        {
            
            if (GUILayout.Button(new GUIContent("MockMoveAI")))
            {
                var aiClient = new AIStorage();
                aiClient.Create("MockMoveAI", "MovementAI");

                this.window.Close();
            }
            if (GUILayout.Button(new GUIContent("MockScanAI")))
            {
                var aiClient = new AIStorage();
                aiClient.Create("MockScanningAI", "ScanningAI");
                this.window.Close();
            }

        }

    }


}