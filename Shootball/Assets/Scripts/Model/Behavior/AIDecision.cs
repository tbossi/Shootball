namespace Shootball.Model.Behavior
{
    public class AIDecision
    {
        private readonly AIBehavior _behavior;
        private float _choiceValue;
        public float ChoiceValue
        {
            get { return _choiceValue; }
            set
            {
                if (value < 0) { _choiceValue = 0; }
                else if (value > 1) { _choiceValue = 1; }
                else { _choiceValue = value; }
            }
        }

        public AIDecision(AIBehavior behavior, float choiceValue)
        {
            _behavior = behavior;
            ChoiceValue = choiceValue;
        }

        public bool Run()
        {
            var state = _behavior.Run();
            return state == BehaviorState.Running;
        }

        public override string ToString()
        {
            return "state: " + _behavior.State + "\t| choiceValue: " + ChoiceValue;
        }
    }
}