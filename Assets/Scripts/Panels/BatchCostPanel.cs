using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BatchCostPanel : MonoBehaviour {

	public int historySize, costBins;

	public Color costColor, backgroundColor;

	private readonly Queue<float> costHistory = new Queue<float>();

	private Texture2D graphTexture;

	void Start() {
		historySize = historySize == 0 ? 200 : historySize;
		costBins = costBins == 0 ? 100 : costBins;
		
		for (int i = 0; i < historySize; i++) {
			costHistory.Enqueue(0f);
		}

		graphTexture = new Texture2D(historySize, costBins);
		graphTexture.filterMode = FilterMode.Point;
		GetComponent<Renderer>().material.mainTexture = graphTexture;
	}

	public void Add(float batchCost) {
		costHistory.Enqueue(batchCost);
		costHistory.Dequeue();
	}

	void Update() {
		RenderGraph();
	}

	public void RenderGraph() {
		float maxCost = costHistory.Max();

		int x = historySize - 1;
		foreach (float cost in costHistory) {
			int barHeight = Mathf.CeilToInt(costBins * cost / maxCost);

			for (int y = 0; y < costBins; y++) {
				if (y < costBins - barHeight) {
					graphTexture.SetPixel(x, y, backgroundColor);
				}
				else {
					graphTexture.SetPixel(x, y, costColor);
				}
			}
			x--;
		}

		graphTexture.Apply();
	}
}
