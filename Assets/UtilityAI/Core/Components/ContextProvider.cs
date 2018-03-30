namespace UtilityAI
{
    using System;
    using UnityEngine;
    using Bang;

    public class ContextProvider : MonoBehaviour, IContextProvider
    {
        private NpcController npc;

        [SerializeField]
        private AIContext _context;
        public AIContext context { get {return _context;} set {_context = value;}}

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
