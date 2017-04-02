using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceEnvironment : MonoBehaviour {

	private Conways conways;

	public int width, height;

	public float cellSize, cellStride;

	public GameObject cellPrefab;

	public bool autoRun;

	private GameObject[,] cells;

	void Start () {
		conways = new Conways(width, height);
		cells = new GameObject[width, height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				GameObject cell = (GameObject)Instantiate(cellPrefab);
				cells[x, y] = cell;
				cell.GetComponent<StateCell>().x = x;
				cell.GetComponent<StateCell>().y = y;
				cell.transform.parent = this.transform;

				cell.transform.localScale = new Vector3(cellSize, cellSize, cellSize / 3);
				cell.transform.localPosition = new Vector3(x * cellStride, y * cellStride);
			}
		}


		conways[12, 10] = 1;
		conways[12, 11] = 1;
		conways[12, 12] = 1;
		conways[11, 12] = 1;
		conways[10, 11] = 1;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				StateCell clickedCell = hit.transform.gameObject.GetComponent<StateCell>();
				if (clickedCell != null) {
					int x = clickedCell.x;
					int y = clickedCell.y;
					conways[x, y] = conways[x, y] == 0 ? 1 : 0;
				}
			}
		}

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				cells[x, y].GetComponent<StateCell>().SetState(conways[x, y]);
			}
		}

		if (autoRun || Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow)) {
			conways.Step();
		}
	}
}
