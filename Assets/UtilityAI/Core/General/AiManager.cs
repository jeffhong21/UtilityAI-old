namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.IO;


    public static class AiManager
    {
        public static string StorageFolder = Path.GetDirectoryName("Assets/UtilityAI/Resources/AIStorage/");
        public static event Action GetAIClient;

        /// <summary>
        /// Gets all registered clients.
        /// </summary>
        /// <value>All clients.</value>
        //public static HashSet<UtilityAIClient> allClients { get; private set; } = new HashSet<UtilityAIClient>();


        /// <summary>
        /// Gets the list of clients for a given AI.
        /// </summary>
        /// <param name="aiID">Ai identifier.</param>
        ///<returns> The list of clients for the specified AI. </returns>
        public static List<UtilityAIClient> GetAllClients(string aiID){
            List<UtilityAIClient> clients = new List<UtilityAIClient>();

            return clients;
        }


        ///<summary>
        ///Gets an AI by ID.
        ///</summary>
        ///<param name = "id" > The ID.</param>
        ///<returns> The AI with the specified ID, or null if no match is found.</returns>
        public static TaskNetworkComponent GetAI(string id){
            TaskNetworkComponent ai = null;

            return ai;
        }


        //<summary>
        //Executes the specified AI once.
        //</summary>
        //<param name = "id" > The AI ID.</param>
        //<param name = "context" > The context.</param>
        //<returns><c>true</c> if the AI was found and executed; otherwise<c>false</c>.</returns>
        public static bool ExecuteAI(string id, IAIContext context){
            return true;
        }


        //public static void Register(UtilityAIClient client){
        //    allClients.Add(client);
        //    //GetAIClient += client.Stop;
        //}


        //public static void UnRegister(UtilityAIClient client){
        //    if(allClients.Contains(client)){
        //        allClients.Remove(client);
        //        //GetAIClient -= client.Start;
        //    }
        //}



    }



}