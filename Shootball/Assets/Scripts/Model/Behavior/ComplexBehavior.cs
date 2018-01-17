using System.Collections.Generic;

namespace Shootball.Model.Behavior
{
    public abstract class ComplexBehavior : AIBehavior
    {
        protected readonly List<AIBehavior> ChildrenBehavior;

        public ComplexBehavior() : base()
        {
            ChildrenBehavior = new List<AIBehavior>();
        }

        public virtual void AddBehavior(AIBehavior behavior)
        {
            ChildrenBehavior.Add(behavior);
        }
    }
}