namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Custom gizmo GUIV isualizer component.
    /// </summary>
    /// <typeparam name="T">The type that this visualizer visualizes.</typeparam>
    /// <typeparam name = "TData" > The type of the data to be visualized.</typeparam>
    public abstract class CustomGizmoGUIVisualizer<T, TData> : MonoBehaviour
    {
        public bool drawGUI = true;
        public bool drawGizmo = true;
        protected T data;

        //[SerializeField, HideInInspector]
        [SerializeField]
        protected AIContextProvider contextProvider;


        private void OnEnable(){
            if (contextProvider == null)
                contextProvider = gameObject.GetComponent<TaskNetworkComponent>().contextProvider;
        }

		private void Start(){
            OnEnable();
		}


		protected virtual void OnGUI(){
            DrawGUI(data);
		}

        protected virtual void OnDrawGizmos(){
            DrawGizmos(data);
		}


        protected abstract void DrawGUI(T data);

        protected abstract void DrawGizmos(T data);


    }



    /// <summary>
    /// Action with options visualizer component.
    /// </summary>
    /// <typeparam name="T">The concrete ActionWithOptions type</typeparam>
    /// <typeparam name = "TOption" > The type of the options.</typeparam>
    public class ActionWithOptionsVisualizerComponent <T, TOption> : CustomGizmoGUIVisualizer<List<ScoredOption<TOption>>, TOption> 
        where T : ActionWithOptions<TOption>
    {

        protected virtual List<TOption> GetOptions(AIContext context){
            return new List<TOption>();
        }

        /// <summary>
        /// Called after an entity of the type associated with this visualizer has been executed in the AI, e.g. an <see cref="T:Apex.AI.IAction" />.
        /// </summary>
        /// <returns>The data for visualization.</returns>
        /// <param name="aiEntity">Ai entity.</param>
        /// <param name="context">Context.</param>
        /// <param name="aiID">Ai identifier.</param>
        protected List<ScoredOption<TOption>> GetDataForVisualization(T aiEntity, AIContext context, string aiID)
        {
            List<ScoredOption<TOption>> _data = aiEntity.GetAllScorers(context, GetOptions(context), aiEntity.scoredOptions);
            data = aiEntity.scoredOptions;
            return data;
        }


        protected override void OnGUI(){
            if (data != null && contextProvider != null && drawGUI == true && Application.isEditor){
                if (Camera.current == Camera.main || Camera.current == SceneView.lastActiveSceneView.camera){
                    DrawGUI(data);
                }
            }
        }

        protected override void OnDrawGizmos(){
            if (data != null && contextProvider != null && drawGUI == true && Application.isEditor){
                if (Camera.current == Camera.main || Camera.current == SceneView.lastActiveSceneView.camera){
                    DrawGizmos(data);
                }
            }
        }



        protected override void DrawGUI(List<ScoredOption<TOption>> data)
        {
           
        }

        protected override void DrawGizmos(List<ScoredOption<TOption>> data)
        {

        }


    }






}
