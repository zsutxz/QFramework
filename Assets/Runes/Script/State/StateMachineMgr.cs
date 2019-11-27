using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineMgr<T> {

    private Dictionary<T, BaseState<T>> _StateArray = new Dictionary<T, BaseState<T>> ();

    private BaseState<T> _PreState;
    private BaseState<T> _CurrentState;

    public void Register (BaseState<T> state) {
        _StateArray.Add (state.getStateName (), state);
    }

    public void init () {

    }

    public void update (float dt) {
        if (_CurrentState != null) {
            _CurrentState.StateUpdate (dt);
        }
    }

    public void changeState (T state) {
        BaseState<T> c = this._StateArray[state];
        if (c == null) {
            Debug.LogError ("no this state, Register first!");
            return;
        }

        if (_CurrentState != null)
            _CurrentState.ExitState ();

        _PreState = _CurrentState;

        _CurrentState = c;

        _CurrentState.EnterState ();

    }

    public T GetCurrentState () {
        return _CurrentState.getStateName ();

    }

}