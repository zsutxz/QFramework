using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class EliminateState : BaseState<BattleState> {
    public EliminateState (BattleState name) : base (name) {

    }
    private BattleState nextTo;
    float timeCt = 0;
    bool isEliminate = false;
    public override void EnterState () {
        timeCt = 0;
        isEliminate = false;
    }

    public override void ExitState () {
        Debug.Log ("exitState " + this._StateName);
    }

    void Eliminate () {
        GridArray gridArray = Battlefield.Instance.GetGridArray ();

        List<List<Stone>> lt = gridArray.CalculateArray ();
        bool hasEliminate = false;

        foreach (List<Stone> t in lt) {
            var type = t[0].type;
            var len = t.Count;
            
            //Debug.Log (type + " len: " + len);
            if (t[0].size == 1 && len <= 2) { continue; } //
            if (t[0].size == 2 && len <= 1) { continue; } //

            //
            foreach (Stone s in t) {
                gridArray.ClearStonPosition (s);
                s.Eliminate ();
                hasEliminate = true;
            }
        }

        if (hasEliminate) {
            nextTo = BattleState.eSupplyAndDrop;
        } else {
            nextTo = BattleState.eShot;
        }
    }
    public override void StateUpdate (float dt) {
        timeCt += dt;
        if (!isEliminate && timeCt > 1.0f) {
            Eliminate ();
            timeCt = 0.0f;
            isEliminate = true;
        }
        if (isEliminate && timeCt > 1.0f) {
           //Battlefield.Instance.ChangeState (nextTo);
        }
    }
}