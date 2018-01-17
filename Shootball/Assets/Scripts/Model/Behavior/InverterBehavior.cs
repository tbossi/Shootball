using System;

namespace Shootball.Model.Behavior
{
    public class InverterBehavior : ComplexBehavior
    {
        public InverterBehavior() : base()
        { }

        public override void AddBehavior(AIBehavior behavior)
        {
            if (ChildrenBehavior.Count != 0)
                throw new InvalidOperationException();
            base.AddBehavior(behavior);
        }

        public override BehaviorState Run()
        {
            var result = ChildrenBehavior[0].Run();
            if (result == BehaviorState.Running) { State = BehaviorState.Running; }
            else if (result == BehaviorState.Failed) { State = BehaviorState.Complete; }
            else if (result == BehaviorState.Complete) { State = BehaviorState.Failed; }
            return State;
        }
    }
}