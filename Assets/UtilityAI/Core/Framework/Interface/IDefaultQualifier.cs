namespace UtilityAI
{

    public interface IDefaultQualifier
    {

        IAction action { get; set; }
        float Score(IAIContext _context);
    }
}