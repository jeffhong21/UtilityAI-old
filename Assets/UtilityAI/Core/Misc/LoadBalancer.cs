namespace UtilityAI.LoadBalancing
{
    
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;


    public class LoadBalancer 
    {


    }


    public class LoadBalanceActionPool
    {
        
    }



    public class LoadBalanceQueue
    {
        
    }


    public class LoadBalancing
    {


        public bool repeat { get; private set; }

        //public float? ExecuteUpdate(float deltaTime, float nextInterval)
        //{

        //    return nextInterval;
        //}


        public IEnumerator ExecuteUpdate(float deltaTime, float nextInterval)
        {

            yield return nextInterval;
        }



    }
}

