namespace Shootball.Model.Behavior
{
    public abstract class AIBehavior
    {
        public BehaviorState State { get; protected set; }

        public AIBehavior()
        {
            State = BehaviorState.Running;
        }

        public abstract BehaviorState Run();
    }
}