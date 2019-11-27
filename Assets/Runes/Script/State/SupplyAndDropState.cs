using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyAndDropState : BaseState<BattleState> {

    public SupplyAndDropState (BattleState name) : base (name) {

    }

    public override void EnterState () {
        GridArray gridArray = Battlefield.Instance.GetGridArray ();

        gridArray.DropFillEmpty ();
        gridArray.FeedNewSton ();
       //Battlefield.Instance.ChangeState (BattleState.eEliminate);
    }

    public override void ExitState () {

    }
    public override void StateUpdate (float dt) {
        GridArray gridArray = Battlefield.Instance.GetGridArray ();
        //if (gridArray.AllStoneStatic ())
        //    Battlefield.Instance.ChangeState (BattleState.eEliminate);
    }

}