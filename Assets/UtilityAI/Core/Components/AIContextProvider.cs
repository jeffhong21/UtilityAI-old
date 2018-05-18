namespace UtilityAI
{
    using System;
    using UnityEngine;


    public class AIContextProvider : MonoBehaviour, IContextProvider
    {


        [SerializeField]
        private AIContext _context;
        public AIContext context { get {return _context;} set {_context = value;}}


        void OnEnable()
        {
            _context = new AIContext(GetComponent<EntityAIController>());
        }


        public IAIContext GetContext(){
            return context as IAIContext;
        }

    }
}
