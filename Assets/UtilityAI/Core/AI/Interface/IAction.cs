namespace UtilityAI
{
    
    public interface IAction
    {
        UtilityAIComponent utilityAIComponent {get; set;}
        void Execute(IContext context);
    }
}