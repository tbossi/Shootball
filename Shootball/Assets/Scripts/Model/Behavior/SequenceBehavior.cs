namespace Shootball.Model.Behavior
{
    public class SequenceBehavior : ComplexBehavior
    {
        private int _lastVisitedNode;

        public SequenceBehavior() : base()
        {
            _lastVisitedNode = 0;
        }

        public override BehaviorState Run()
        {
            int nodeToVisit = 0;
            if (ChildrenBehavior[_lastVisitedNode].State == BehaviorState.Running)
            {
                nodeToVisit = _lastVisitedNode;
            }
            else if (ChildrenBehavior[_lastVisitedNode].State == BehaviorState.Failed)
            {
                nodeToVisit = 0;
            }
            else if (ChildrenBehavior[_lastVisitedNode].State == BehaviorState.Complete)
            {
                nodeToVisit = (_lastVisitedNode + 1) % ChildrenBehavior.Count;
            }

            var result = ChildrenBehavior[nodeToVisit].Run();
            if (result == BehaviorState.Failed || result == BehaviorState.Running)
            {
                State = result;
            }
            else if (result == BehaviorState.Complete)
            {
                if (nodeToVisit == ChildrenBehavior.Count - 1) { State = BehaviorState.Complete; }
                else { State = BehaviorState.Running; }
            }
            return State;
        }
    }
}