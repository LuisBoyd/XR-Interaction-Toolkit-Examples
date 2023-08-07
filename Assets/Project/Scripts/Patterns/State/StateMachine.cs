using System;
using System.Collections.Generic;
using System.Linq;
using QuikGraph;
using QuikGraph.Collections;
using UnityEngine;

namespace Project.Scripts.Patterns.State
{
    public abstract class StateMachine : MonoBehaviour
    {
        //Be able to switch what the current state is
        //be able to update the current state.
        protected BidirectionalGraph<State, StateTransition> graph;
        protected State _CurrentState;
        private bool _waitAFrame = false;

        protected virtual void Awake()
        {
            graph = new BidirectionalGraph<State, StateTransition>();
        }

        protected virtual void Start()
        {
            _CurrentState = graph.Vertices.First();
            _CurrentState.OnStateStart();
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void Update()
        {
            _waitAFrame = false;
            //Get all Edges going out of the curren state
            var outgoingTransitions = graph.OutEdges(_CurrentState);
            foreach (var transition in outgoingTransitions)
            {
                if (transition.Condition())
                {
                    _CurrentState.OnStateExit();
                    _CurrentState = transition.Target;
                    _CurrentState.OnStateStart();
                    _waitAFrame = true;
                }
            }
            if (!_waitAFrame)
            {
                _CurrentState.OnStateUpdate();
            }
        }
        protected virtual void OnDisable()
        {
            _CurrentState.OnStateExit();
        }

        protected virtual void SwitchState(State newState)
        {
            _CurrentState.OnStateExit();
            _CurrentState = newState;
            _CurrentState.OnStateStart();
        }
    }
}