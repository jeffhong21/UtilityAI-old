namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// UtilityAI creates the rootSelector and initializes the tree.
    /// </summary>
    public interface IUtilityAI : ISerializationCallbackReceiver
    {
        void AddSelector(Selector s);
        void RemoveSelector(Selector s);
        bool ReplaceSelector(Selector current, Selector replacement);
        Selector FindSelector(Selector s);
        IAction Select(IAIContext context);
        void RegenerateIds();
        byte[] PrepareForSerialize();
        void InitializeAfterDeserialize(byte[] data);
        Guid id { get; }
        string name { get; set; }
        Selector rootSelector { get; set; }
        int selectorCount { get; }
        Selector Item { get; }
    }


    [Serializable]
    public class UtilityAI : IUtilityAI
    {
        #region Properties

        public Guid id { get; private set; }
        public string name { get; set; }
        public Selector rootSelector { get; set; }
        public ScoreSelector selector;

        public int selectorCount { get; private set; }
        //  Get Selector with specific index.
        public Selector Item { get; private set; }

        #endregion


        public UtilityAI()
        {
            rootSelector = new ScoreSelector();
            selector = new ScoreSelector();
            RegenerateIds();
        }

        public UtilityAI(string aiName)
        {
            rootSelector = new ScoreSelector();
            selector = new ScoreSelector();
            name = aiName;
            RegenerateIds();
        }

        public void AddSelector(Selector s)
        {
            throw new NotImplementedException();
        }

        public Selector FindSelector(Selector s)
        {
            throw new NotImplementedException();
        }

        public void RemoveSelector(Selector s)
        {
            throw new NotImplementedException();
        }

        public bool ReplaceSelector(Selector current, Selector replacement)
        {
            throw new NotImplementedException();
        }

        public void RegenerateIds(){
            id = Guid.NewGuid();
            //Debug.Log(id);
        }


        /// <summary>
        /// Selects the action for execution.
        /// </summary>
        /// <returns>The select.</returns>
        /// <param name="context">Context.</param>
        public IAction Select(IAIContext context)
        {
            List<IQualifier> qualifiers = rootSelector.qualifiers;
            IDefaultQualifier defaultQualifier = rootSelector.defaultQualifier;
            IQualifier winner = rootSelector.Select(context, qualifiers, defaultQualifier);

            CompositeQualifier cq = winner as CompositeQualifier;
            // TODO:  What if there are no scoreres?
            //float score = cq.Score(context, cq.scorers);
            IAction action = winner.action;

            return action;
        }






        [SerializeField] [HideInInspector]
        private byte[] data;

        public byte[] PrepareForSerialize()
        {
            //throw new NotImplementedException();
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            //  Serialize utilityAI to memoryStream
            binaryFormatter.Serialize(memoryStream, selector);

            //data = memoryStream.ToArray();

            memoryStream.Close();
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Part of ISerializationCallbackReceiver.
        /// </summary>
        public void OnBeforeSerialize()
        {
            data = PrepareForSerialize();

            //throw new NotImplementedException();
        }


        public void InitializeAfterDeserialize(byte[] data)
        {
            int count = 0;

            if(data != null){
                object obj = new BinaryFormatter().Deserialize(new MemoryStream(data));

                if (obj is Selector)
                {
                    Selector root = obj as Selector;
                    rootSelector = root;
                    selector = root as ScoreSelector;

                    if (count < 1)
                    {
                        Debug.Log("Succesfully Serialized");
                        count++;
                    }
                }
                else throw new ApplicationException("Unable to deserialize type " + obj.GetType());
            }

            if(count < 1){
                Debug.Log("Did not Deserialize");
                count++;
            }

            //throw new NotImplementedException();
        }


        /// <summary>
        /// Part of ISerializationCallbackReceiver.
        /// </summary>
        public void OnAfterDeserialize()
        {
            InitializeAfterDeserialize(data);

            // InitializeAfterDeserialize(object rootObject);
            //throw new NotImplementedException();
        }
    }
}

