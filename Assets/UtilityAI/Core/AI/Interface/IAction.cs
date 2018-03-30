namespace UtilityAI
{
    
    public interface IAction
    {
        UtilityAIComponent utilityAIComponent {get; set;}
        ActionStatus actionStatus { get;  }
        void EndAction();
        void ExecuteAction(IContext context);
    }
}