using System;
using Shootball.Model.Behavior;

namespace Shootball.Model.Player.AI
{
    public class RepeatedAction
    {
        private readonly Action _action;
        private readonly int _times;
        private int _executedTimes;

        public RepeatedAction(Action action, int times)
        {
            _action = action;
            _times = times;
            _executedTimes = 0;
        }

        public BehaviorState RunOnce()
        {
            if (_executedTimes >= _times) { return BehaviorState.Failed; }
            _action.Invoke();
            _executedTimes++;
            if (_executedTimes < _times - 1) { return BehaviorState.Running; }
            else { return BehaviorState.Complete; }
        }

        public override int GetHashCode()
        {
            return _action.GetHashCode() ^ _times.GetHashCode();
        }
    }
}