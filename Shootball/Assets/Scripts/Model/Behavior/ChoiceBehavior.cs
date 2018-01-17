namespace Shootball.Model.Behavior
{
    public class ChoiceBehavior : ComplexBehavior
    {
        private int _lastVisitedNode;

        public ChoiceBehavior() : base()
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
            else if (ChildrenBehavior[_lastVisitedNode].State == BehaviorState.Complete)
            {
                nodeToVisit = 0;
            }
            else if (ChildrenBehavior[_lastVisitedNode].State == BehaviorState.Failed)
            {
                nodeToVisit = (_lastVisitedNode + 1) % ChildrenBehavior.Count;
            }

            var result = ChildrenBehavior[nodeToVisit].Run();
            if (result == BehaviorState.Complete || result == BehaviorState.Running)
            {
                State = result;
            }
            else if (result == BehaviorState.Failed)
            {
                if (nodeToVisit == ChildrenBehavior.Count - 1) { State = BehaviorState.Failed; }
                else { State = BehaviorState.Running; }
            }
            return State;
        }
    }
}