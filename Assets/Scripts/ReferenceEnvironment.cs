using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceEnvironment : MonoBehaviour {

	private Conways conways;

	public int width, height;

	public float cellSize, cellStride;

	public GameObject cellPrefab, backPlane;

	public bool autoRun;

	private GameObject[,] cells;

	void Start () {
		conways = new Conways(width, height);
		cells = new GameObject[width, height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				GameObject cell = (GameObject)Instantiate(cellPrefab);
				cells[x, y] = cell;
				cell.transform.parent = this.transform;

				cell.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
				cell.transform.localPosition = new Vector3(x * cellStride, y * cellStride);
			}
		}

		backPlane.transform.parent = this.transform;

		backPlane.transform.localPosition = new Vector3((width - 1) * cellStride / 2, (height - 1) * cellStride / 2);
		backPlane.transform.localScale = new Vector3(width * cellStride / 10, 1, height * cellStride / 10);


		conways[12, 10] = 1;
		conways[12, 11] = 1;
		conways[12, 12] = 1;
		conways[11, 12] = 1;
		conways[10, 11] = 1;
	}

	void Update () {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				cells[x, y].GetComponent<StateCube>().SetState(conways[x, y]);
			}
		}

		if (autoRun || Input.GetKey(KeyCode.Space)) {
			conways.Step();
		}
	}
}
