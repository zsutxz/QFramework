using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public enum BattleState {
	eNone,
	eShot,
	eEliminate,
	eSupplyAndDrop,
}

public class Battlefield : MonoBehaviour {

	public static Battlefield Instance { get; private set; }
	// Use this for initialization
	private GridArray gridArray = new GridArray ();
	private StateMgr stageMgr = new StateMgr ();

    public GridArray GetGridArray () {
		return gridArray;
	}
	private void Awake () {
		Instance = this;
	}

    void Start () {
		gridArray.FeedNewSton ();
        stageMgr.init();
    }
	
	// Update is called once per frame
	void Update () {

        stageMgr.update (Time.deltaTime);
	}
	public void ChangeState(string state)
	{
		stageMgr.changeState(state);
	}

	private static float offsetX = -400;
	private static float offsetY = -200;
	private static float stonWidth = 110;
	public static Vector2 C2RelativePos (GridPos grid) {
		return new Vector2 (offsetX + grid.PosCol * stonWidth, offsetY + grid.PosRow * stonWidth);
	}
}