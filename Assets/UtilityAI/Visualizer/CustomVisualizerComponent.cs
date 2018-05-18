namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;

    using System.Collections.Generic;

    /// <summary>
    /// Context gizmo GUIV isualizer component.
    /// </summary>
    /// <typeparam name="T">The context type.</typeparam>
    public abstract class ContextGizmoGUIVisualizerComponent : MonoBehaviour
    {
        public bool drawGUI = true;
        public bool drawGizmo = true;

        //[SerializeField]
        [SerializeField, HideInInspector]
        protected AIContextProvider contextProvider;



        protected void Awake()
		{
            if (contextProvider == null)
                contextProvider = gameObject.GetComponent<TaskNetworkComponent>().contextProvider;
		}

        protected void OnEnable()
        {
            if (contextProvider == null)
                contextProvider = gameObject.GetComponent<TaskNetworkComponent>().contextProvider;
        }

        protected void OnDisable()
		{
			
		}



        protected virtual void OnGUI()
        {
            if (contextProvider != null && drawGUI == true && Application.isEditor){
                if (Camera.current == Camera.main || Camera.current == SceneView.lastActiveSceneView.camera){
                    DrawGUI(contextProvider.GetContext());
                }
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (contextProvider != null && drawGizmo == true && Application.isEditor){
                if (Camera.current == Camera.main || Camera.current == SceneView.lastActiveSceneView.camera){
                    DrawGizmos(contextProvider.GetContext());
                }
            }
        }



        protected abstract void DrawGUI(IAIContext context);

        protected abstract void DrawGizmos(IAIContext context);




    }


}

