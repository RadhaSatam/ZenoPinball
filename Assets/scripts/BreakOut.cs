﻿using UnityEngine;
using System.Collections;

public class BreakOut : MonoBehaviour, ITriggerable, IResetable {

	public GameObject MainGameMode;
	public Transform Block;

	public int SecondsToRun = 30;
	int _timeLeft;

	GameObject BreakoutMode;

	public int blocksHoriz = 8;
	public int blocksVert = 8;
	BreakOutBlock[] blocks;

	const float xPos = -3.5f;

	// Use this for initialization
	void Start () {
		Setup();	
	}
	
	// Update is called once per frame
	void Update () {
		// for debug
		if (Input.GetKeyUp(KeyCode.B))
			Trigger();
	}

	void Setup() {
		_timeLeft = SecondsToRun;

		BreakoutMode = new GameObject("Blocks");
		BreakoutMode.transform.parent = transform;
		BreakoutMode.transform.localPosition = Vector3.zero;
		BreakoutMode.transform.rotation = transform.rotation;

		blocks = new BreakOutBlock[blocksHoriz * blocksVert];

		for (int z = 0; z < blocksVert; ++z) {
			for (int x = 0; x < blocksHoriz; ++x) {
				Transform newBlock = (Transform)Instantiate(Block);
				newBlock.parent = BreakoutMode.transform;
				newBlock.localPosition = new Vector3(x * 1f, BreakoutMode.transform.position.y + 0.2f, z * 1f);
				newBlock.transform.rotation = BreakoutMode.transform.rotation;

				int index = z * blocksHoriz + x;
				blocks[index] = newBlock.GetComponent<BreakOutBlock>();
				// make the blocks at the back worth more than the ones at the front
				blocks[index].points *= (z + 1);
			}
		}
		transform.localPosition = new Vector3(xPos, transform.position.y, transform.position.z);

		BreakoutMode.SetActive(false);
	}

	public void Trigger() {
		if (!BreakoutMode.activeInHierarchy) {
			MainGameMode.SetActive(false);
			BreakoutMode.SetActive(true);
			InvokeRepeating("CountdownTimer", 1f, 1f);
		} else {
			Reset();
			MainGameMode.SetActive(true);
		}

	}

	public void Reset() {
		CancelInvoke("CountdownTimer");
		_timeLeft = SecondsToRun;
		foreach(BreakOutBlock block in blocks) {
			block.gameObject.SetActive(true);
		}
		BreakoutMode.SetActive(false);
	}

	void CountdownTimer() {
		if (--_timeLeft == 0) {
			Trigger();
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 0, 100, 50), "Bonus Time: " + _timeLeft);
	}
}
