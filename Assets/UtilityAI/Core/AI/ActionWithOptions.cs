namespace UtilityAI
{
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Collections.Generic;



    /// <summary>
    /// action with options.
    /// </summary>
    public abstract class ActionWithOptions<TOption> : IAction, IEquatable<TOption>
    {
        public string name;
        public TaskNetworkComponent utilityAIComponent { get; set; }
        public ActionStatus actionStatus { get; protected set; }

        //  All the OptionScorers attached to this action.
        public List<IOptionScorer<TOption>> _scorers;
        public List<IOptionScorer<TOption>> scorers
        {
            get { return _scorers; }
            protected set { _scorers = value; }
        }
        //  All the TOptions and its score.
        public List<ScoredOption<TOption>> scoredOptions { get; protected set;}


        protected abstract void Execute(IAIContext context);


        public void EndAction(){
            if (actionStatus != ActionStatus.Running)
                return;
            actionStatus = ActionStatus.Success;
        }


        /// <summary>
        /// "Gets the best option, i.e. the option that was given the highest combined score by thr ActionWithOptions<TOption>.scorers"
        /// Gets the TOption tha has the highest score after being scored by all the scorers.  
        /// </summary>
        /// <returns> Return a type option for the action to execute on..</returns>
        /// <param name="context">Context.</param>
        /// <param name="options">Options.</param>
        public TOption GetBest(IAIContext context, List<TOption> options)
        {
            scoredOptions.Clear();

            if (options.Count == 0)
                return default(TOption);

            TOption best = options[0];

            //  Loop through every TOption and tallys all scorers.  (e.g loop through all Vector3 position points and than calculate all scores of that position.)
            for (int i = 0; i < options.Count; i++)
            {
                var option = options[i];
                float score = 0f;
                //  Loop through each scorer options.
                for (int index = 0; index < scorers.Count; index++){
                    score += scorers[index].Score(context, option);
                }
                //  ScoredOptions would contain all the scores and TOption to return to the ActionWithOptions.
                scoredOptions.Add(new ScoredOption<TOption>(option, score));
            }

            //for (int index = 0; index < scoredOptions.Count; index++)
            //{
            //    //  Compare al the ScoredOptions.
            //}
            scoredOptions.Sort();
            scoredOptions.Reverse();
            best = scoredOptions[0].option;

            return scoredOptions[0].score == 0 ? default(TOption) : scoredOptions[0].option;

            //string scoredOptionInfo = "";
            //foreach(ScoredOption<TOption> scoredOption in scoredOptions){
            //    scoredOptionInfo += string.Format("OptionType:  {0}  | Score: {1}\n", scoredOption.option, scoredOption.score);
            //}
            //Debug.Log(scoredOptionInfo);

            //return best;
        }


        /// <summary>
        /// "Gets all options with the score they received from the ActionWithOptions<TOption>.scorers."
        /// Instead of scoring with default method, which returns the TOption with the highest score, 
        /// you can get a list of all the options with their scores and you can select them yourself with another logic.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="options">Options.</param>
        /// <param name="optionsBuffer">The buffer which is populated with the scored options.</param>
        public List<ScoredOption<TOption>> GetAllScorers(IAIContext context, List<TOption> options, List<ScoredOption<TOption>> optionsBuffer)
        {
            optionsBuffer.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                var option = options[i];
                float score = 0f;

                for (int index = 0; index < scorers.Count; index++){
                    score += scorers[index].Score(context, option);
                }

                //  ScoredOptions would contain all the scores and TOption to return to the ActionWithOptions.
                optionsBuffer.Add(new ScoredOption<TOption>(option, score));
            }

            return optionsBuffer;
        }


        /// <summary>
        /// "Clones or transfers settings from the other entity to itself"
        /// Used if you want to switch this ActionWithOptions to another.
        /// From video demo.
        /// </summary>
        /// <param name="other">Other.</param>
        public void CloneFrom(ActionWithOptions<TOption> other){
            utilityAIComponent = other.utilityAIComponent;
            scorers = other.scorers;
            scoredOptions = other.scoredOptions;
        }

        //  From video demo.
        public bool Equals(TOption other){
            throw new NotImplementedException();
        }


        public void ExecuteAction(IAIContext context)
        {
            //  TODO: Maybe add some check to see if action can be executed.
            actionStatus = ActionStatus.Running;
            Execute(context);
        }


        public ActionWithOptions(){
            scoredOptions = new List<ScoredOption<TOption>>();
        }

        public ActionWithOptions(ActionWithOptions<TOption> other){
            scoredOptions = other.scoredOptions;
            scorers = other.scorers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:UtilityAI.ActionWithOptions`1"/> class.
        /// </summary>
        /// <param name="objects">Objects.</param>
        public ActionWithOptions(params IOptionScorer<TOption>[] objects)
        {
            scoredOptions = new List<ScoredOption<TOption>>();
            scorers = new List<IOptionScorer<TOption>>(objects.ToList() );

        }

    }





}