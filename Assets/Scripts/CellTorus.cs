using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTorus : MonoBehaviour {

	public GameObject cellPrefab;
	public IAutomata automata;

	public int planeWidth, planeHeight;

	public float cellSize, cellStride, torusRadius, sliceRadius;

	private GameObject[,] cells;

	void Start () {
		if (automata == null) {
			Debug.LogError("CellTorus automata is null.  Set it from its parent");
		}
		
		cells = new GameObject[planeWidth, planeHeight];

		for (int x = 0; x < planeWidth; x ++) {
			for (int y = 0; y < planeHeight; y++) {
				GameObject cell = (GameObject)Instantiate(cellPrefab);
				cells[x, y] = cell;
				cell.GetComponent<StateCell>().x = x;
				cell.GetComponent<StateCell>().y = y;
				cell.transform.parent = this.transform;

				cell.transform.localScale = new Vector3(cellSize, cellSize, cellSize / 3);
				cell.transform.localPosition = new Vector3(x * cellStride, y * cellStride);
			}
		}
	}

	void Update() {
		for (int x = 0; x < planeWidth; x ++) {
			for (int y = 0; y < planeHeight; y++) {
				GameObject cell = cells[x, y];
				StateCell stateCell = cell.GetComponent<StateCell>();
				stateCell.SetState(automata[x, y]);
			}
		}
	}
}
