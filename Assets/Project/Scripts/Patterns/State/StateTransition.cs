using System;
using QuikGraph;

namespace Project.Scripts.Patterns.State
{
    public class StateTransition : QuikGraph.IEdge<State>
    {
        public State Source
        {
            get => _source;
        }

        public State Target
        {
            get => _target;
        }

        public Func<bool> Condition
        {
            get => _condition;
        }

        private State _source;
        private State _target;
        private Func<bool> _condition;

        public StateTransition(State source, State target, Func<bool> condition)
        {
            _source = source;
            _target = target;
            _condition = condition;
        }
    }
}