namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;



    public class StageContainer : StageItem
    {
        [SerializeField]
        private StageItem[] elements;  //  Qualifiers
        [SerializeField]
        private StageItem defaultElement;  //  DefaultQualifier



        public override void Init(string name)
        {
            displayName = name;

        }

        public void Init(string name, bool isTesting)
        {
            if (isTesting == true){
                if(elements == null){
                    elements = new StageItem[]{
                        CreateInstance<StageElement>(),
                        CreateInstance<StageElement>(),
                        CreateInstance<StageElement>()
                    };
                    for (int i = 0; i < elements.Length; i++)
                        elements[i].Init("Element" + i);
                }
                if(defaultElement == null){
                    defaultElement = CreateInstance<StageElement>();
                    defaultElement.Init("DefaultElement");
                }
            }

            Init(name);

        }


        public virtual void Add(StageItem item)
        {
            //  Set the parent.

            //  Add the item
            StageItem[] array = new StageItem[elements.Length + 1];
            Array.Copy(this.elements, 0, array, 0, this.elements.Length);
            array[this.elements.Length] = item;
            this.elements = array;
        }

        /// <summary>
        /// Gets all child items.
        /// </summary>
        public StageItem[] Items()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all child elements.
        /// </summary>
        public StageItem[] Elements()
        {
            //throw new NotImplementedException();
            var obj = new List<StageItem>(elements);
            obj.Add(defaultElement);
            return obj.ToArray();
        }


        /// <summary>
        /// Returns all descendant items
        /// </summary>
        public StageItem[] Descendants()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all descendants of type.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public StageItem[] Descendants<T>()
        {
            throw new NotImplementedException();
        }



	}






}

