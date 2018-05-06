namespace UtilityAI
{
    public interface IOptionScorer <TOption>
    {
        //IQualifier Qualifier { get; }

        //IQualifierCollection Collection { get;  }
        float Score(IAIContext context, TOption data);
    }
}