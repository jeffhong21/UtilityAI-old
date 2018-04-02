namespace UtilityAI
{
    using System;
    using UnityEngine;
    using Bang;

    public class ContextProvider : MonoBehaviour, IContextProvider
    {


        [SerializeField]
        private AIContext _context;
        public AIContext context { get {return _context;} set {_context = value;}}

        [SerializeField]
        private PerceptionModule _module;
        public PerceptionModule module { get { return _module; } set { _module = value; } }


        public IContext GetContext()
        {
            return _context as IContext;
        }


        // void Awake()
        // {
        //     npc = GetComponent<NpcController>();
        //     context = new AIContext(npc);
        // }
    }
}
