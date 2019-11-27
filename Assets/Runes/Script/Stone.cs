using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;

public enum StonType {
	e1,
	e2,
	e3,
	e4,
	e5,
	e6,
	e7,
	ed1,
	ed2,
	ed3,
	ed4,
}


public class Stone : MonoBehaviour {

	
	public StonType type;
	public int size = 1;
	private bool moving = false;

	public GridPos PositionGrid { get; set; }
	
	public void RevisePositionImmediately()
	{
		this.transform.localPosition = Battlefield.C2RelativePos(PositionGrid);
	}

	public void DropDow()
	{
		float y = Battlefield.C2RelativePos(PositionGrid).y;
		moving = true;
		this.transform.DOLocalMoveY(y,0.5f).SetEase(Ease.OutBounce).onComplete = DownFinished;
	}
	
	public bool IsMoving()
	{	
		return moving;
	}

	void DownFinished()
	{
		moving = false;
	}
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Eliminate()
	{
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.4f).SetEase(Ease.OutElastic).onComplete = () =>
        {

            Destroy(gameObject);
        };
    }

}