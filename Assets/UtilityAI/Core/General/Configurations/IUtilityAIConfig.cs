namespace UtilityAI
{
    using System.Collections.Generic;

    public interface IUtilityAIConfig
    {
        IAction a { get; set; }
        IScorer scorer { get; set; }
        List<IScorer> scorers { get; set; }
        IQualifier q { get; set; }
        Selector s { get; set; }

        List<IQualifier> qualifiers { get; set; }
        List<IScorer[]> allScorers { get; set; }
        List<IAction> actions { get; set; }
    }
}