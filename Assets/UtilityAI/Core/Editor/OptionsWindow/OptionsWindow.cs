namespace UtilityAI
{
    using System;
    using UnityEditor;


    public abstract class OptionsWindow<T> : EditorWindow
    {
        //protected T window;
        protected int windowMinSize = 250;
        protected int windowMaxSize = 350;


        //  Need to know so we can add UtilityAIAssets
        protected TaskNetworkComponent taskNetwork { get; set; }


        public virtual void Init(T window, TaskNetworkComponent taskNetwork)
        {
            
        }
        public virtual void Init(T window, TaskNetworkComponent taskNetwork, Type type)
        {
            Init(window, taskNetwork);
        }


        protected abstract void CloseWindow();
        protected abstract void DrawWindowContents();


        protected virtual void OnGUI(){
            DrawWindowContents();
        }

    
    }


}