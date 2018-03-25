namespace UtilityAI
{
    using System;

    /// <summary>
    /// Interface for Qualifier.
    /// Can be used as a base class in which the Qualifier generates a score via the Score method.
    /// </summary>
    public interface IQualifier : IComparable<IQualifier>
    {
        bool isDisabled { get; set; }
        IAction action { get; set; }
        float Score(IContext context);
        float _score { get;  }
    }
}