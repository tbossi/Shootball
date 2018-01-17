using System;
using System.Collections.Generic;

namespace Shootball.Model.Behavior
{
    public class BehaviorBuilder
    {
        private Stack<ComplexBehavior> _behaviorStack;
        private ComplexBehavior _currentBehavior;

        public BehaviorBuilder()
        {
            _behaviorStack = new Stack<ComplexBehavior>();
            _currentBehavior = null;
        }

        public BehaviorBuilder Do(Func<BehaviorState> action)
        {
            _behaviorStack.Peek().AddBehavior(new SimpleBehavior(action));

            if (_behaviorStack.Peek().GetType() == typeof(InverterBehavior))
            {
                return End();
            }
            return this;
        }

        public BehaviorBuilder Do(Action action)
        {
            return Do(() =>
            {
                action.Invoke();
                return BehaviorState.Complete;
            });
        }

        public BehaviorBuilder Join(AIBehavior behaviorTree)
        {
            _behaviorStack.Peek().AddBehavior(behaviorTree);
            if (_behaviorStack.Peek().GetType() == typeof(InverterBehavior))
            {
                return End();
            }
            return this;
        }

        public BehaviorBuilder Not()
        {
            StoreComplexBehavior(new InverterBehavior());
            return this;
        }

        public BehaviorBuilder Sequence()
        {
            StoreComplexBehavior(new SequenceBehavior());
            return this;
        }

        public BehaviorBuilder Choice()
        {
            StoreComplexBehavior(new ChoiceBehavior());
            return this;
        }

        private void StoreComplexBehavior(ComplexBehavior behavior)
        {
            if (_behaviorStack.Count > 0) { _behaviorStack.Peek().AddBehavior(behavior); }
            _behaviorStack.Push(behavior);
        }

        public BehaviorBuilder End()
        {
            do
            {
                _currentBehavior = _behaviorStack.Pop();
            }
            while (_behaviorStack.Count > 0 && _behaviorStack.Peek().GetType() == typeof(InverterBehavior));

            return this;
        }

        public AIBehavior Build()
        {
            return _currentBehavior;
        }
    }
}