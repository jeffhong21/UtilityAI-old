namespace UtilityAI
{
    using System;
    using UnityEngine;
    using Bang;


    [RequireComponent(typeof(NpcController))]
    public class ContextProvider : MonoBehaviour, IContextProvider
    {


        [SerializeField]
        private AIContext _context;
        public AIContext context { get {return _context;} set {_context = value;}}

        [SerializeField]
        private PerceptionModule _perception;
        public PerceptionModule perception { get { return _perception; } set { _perception = value; } }





        public IAIContext GetContext(){
            return _context as IAIContext;
        }


        // void Awake()
        // {
        //     npc = GetComponent<NpcController>();
        //     context = new AIContext(npc);
        // }
    }
}
