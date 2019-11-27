using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotState : BaseState<BattleState> {
    private GridArray gridArray = null;
    KeyCode[] key = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
    public ShotState (BattleState name) : base (name) {

    }

    public override void EnterState () {
        Debug.Log ("EnterState " + this._StateName);
        gridArray = Battlefield.Instance.GetGridArray ();
    }

    public override void ExitState () {

    }

    public override void StateUpdate (float dt) {
        if (gridArray == null) return;

        bool elliminateflag = false;
        for (var i = 0; i < key.Length; i++) {
            if (Input.GetKeyDown (key[i])) {
                Stone s = gridArray.GetNearBottomStone (i);
                if (s != null) {
                    gridArray.ClearStonPosition (s);
                    s.Eliminate ();
                }

                elliminateflag = true;
            }
        }
        if (elliminateflag) {
            //Battlefield.Instance.ChangeState (BattleState.eEliminate);
           // Battlefield.Instance.ChangeState (BattleState.eSupplyAndDrop);
        }

    }
}