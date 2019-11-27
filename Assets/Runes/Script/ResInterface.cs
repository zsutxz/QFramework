using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResInterface : MonoBehaviour {

	public static ResInterface Instance { get; private set; }
	public Stone[] stones_1;
	public Stone[] stones_2;
	// Use this for initialization

	public Stone GetRandomStone (int size) {
		Stone s = null;
		if (size == 1)
			s = Instantiate<Stone> (stones_1[Random.Range (0, stones_1.Length)]);
		else if (size == 2)
			s = Instantiate<Stone> (stones_2[Random.Range (0, stones_2.Length)]);
		s.transform.SetParent(transform,false);
		return s;
	}
	void Awake () {
		Instance = this;
	}
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}