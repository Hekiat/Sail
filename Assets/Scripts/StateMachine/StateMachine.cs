using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class StateMachine : MonoBehaviour
    {
        protected bool _isTransiting;

        protected State _CurrentState;
        public virtual State CurrentState
        {
            get { return _CurrentState; }
            set { Transition(value); }
        }

        public virtual T GetState<T>() where T : State
        {
            T target = GetComponent<T>();
            if (target == null)
            {
                target = gameObject.AddComponent<T>();
            }

            return target;
        }

        public virtual void ChangeToState<T>() where T : State
        {
            CurrentState = GetState<T>();
        }

        protected virtual void Transition(State value)
        {
            if (_CurrentState == value || _isTransiting)
            {
                return;
            }

            _isTransiting = true;

            if (_CurrentState != null)
            {
                _CurrentState.Exit();
            }

            _CurrentState = value;

            if (_CurrentState != null)
            {
                _CurrentState.Enter();
            }

            _isTransiting = false;
        }
    }
}
