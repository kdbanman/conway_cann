using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTorus : MonoBehaviour {

	public GameObject cellPrefab;
	public IAutomata automata;

	public float cellSize, torusRadius, sliceRadius;

	public bool debugRepositionEveryFrame;

	private GameObject[,] cells;
	private int planeWidth, planeHeight;

	private bool cellsRotated;

	public void SetPlaneSize(int width, int height) {
		if (cells != null) {
			DestroyCells();
		}
		planeWidth = width;
		planeHeight = height;
		InitCells();
	}

	void Start () {
	}

	void Update() {
		if (automata == null) {
			Debug.LogError("CellTorus automata is null.  Set it.");
		}

		for (int x = 0; x < planeWidth; x ++) {
			for (int y = 0; y < planeHeight; y++) {
				cells[x, y].GetComponent<StateCell>().SetState(automata[x, y]);

				if (debugRepositionEveryFrame) {
					DestroyCells();
					InitCells();
				}
			}
		}
	}

	private void InitCells() {
		cells = new GameObject[planeWidth, planeHeight];

		for (int x = 0; x < planeWidth; x ++) {
			for (int y = 0; y < planeHeight; y++) {
				GameObject cell = (GameObject)Instantiate(cellPrefab);
				cells[x, y] = cell;
				cell.transform.parent = this.transform;

				PositionCell(cell, x, y, rotate: true);

                StateCell stateCell = cell.GetComponent<StateCell>();
                stateCell.x = x;
                stateCell.y = y;
				stateCell.onToggle = (cellX, cellY) => automata[cellX, cellY] = automata[cellX, cellY] == 1 ? 0 : 1;
			}
		}
	}

	private void DestroyCells() {
		for (int x = 0; x < planeWidth; x ++) {
			for (int y = 0; y < planeHeight; y++) {
				Destroy(cells[x, y]);
			}
		}
	}

	private void PositionCell(GameObject cell, int x, int y, bool rotate=false) {
		// Assume cells are provided with transform such that they face correctly outward in
		// direction of (0, 0, 1), and are rotated that way already.

		cell.transform.localScale = new Vector3(cellSize, cellSize, cellSize / 3);
		float tau = Mathf.PI * 2;

		float phi = tau * x / planeWidth;
		Vector3 centerToSlice = new Vector3(Mathf.Cos(phi), 0, Mathf.Sin(phi)) * torusRadius;
		Vector3 sliceCenter = transform.localPosition + centerToSlice;

		float theta = tau * y / planeHeight;
		Vector3 sliceCenterToCell = new Vector3(
			Mathf.Cos(theta) * Mathf.Cos(phi),
			Mathf.Sin(theta),
			Mathf.Cos(theta) * Mathf.Sin(phi)
		) * sliceRadius; 
		Vector3 cellPosition = sliceCenter + sliceCenterToCell;

		// Outermost is theta == 0
		Vector3 sliceCenterToOuterMostCell = new Vector3(
			Mathf.Cos(phi),
			0,
			Mathf.Sin(phi)
		);
		Quaternion toPosition = Quaternion.FromToRotation(sliceCenterToOuterMostCell, sliceCenterToCell);
		Quaternion yToZCorrection = Quaternion.FromToRotation(new Vector3(1, 0, 0), new Vector3(Mathf.Sin(-phi), 0, Mathf.Cos(-phi)));

		cell.transform.localPosition = cellPosition;

		if (rotate) {
			cell.transform.localRotation *= toPosition;
			cell.transform.localRotation *= yToZCorrection;
		}
	}
}
