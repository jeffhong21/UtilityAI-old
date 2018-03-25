namespace UtilityAI
{

    public interface IDefaultQualifier
    {

        IAction action { get; set; }
        float Score(IContext _context);
    }
}