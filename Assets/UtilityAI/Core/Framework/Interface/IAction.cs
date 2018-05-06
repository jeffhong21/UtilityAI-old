namespace UtilityAI
{
    
    public interface IAction
    {
        //TaskNetworkComponent utilityAIComponent {get; set;}
        //ActionStatus actionStatus { get;  }
        //void EndAction();
        void ExecuteAction(IAIContext context);
    }
}