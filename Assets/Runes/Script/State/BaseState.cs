using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseState<T> {

    protected T _StateName;
    public BaseState(T name)
    {
        _StateName = name;
    }

    public T getStateName()
    {
        return _StateName;
    }

    public virtual void EnterState()
    {

    }

    public virtual void ExitState()
    {
        Debug.Log("exitState " + this._StateName);
    }

    public virtual void StateUpdate(float dt)
    {

    }
}
