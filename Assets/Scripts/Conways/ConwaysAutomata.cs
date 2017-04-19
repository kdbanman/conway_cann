using System;
using UnityEngine;

public class ConwaysAutomata : AbstractAutomata {

	public ConwaysAutomata(int width, int height) {
		InitializeEnvironments(width, height);
	}

    public override Action<int, int> onToggle {
		get { return (x, y) => env[x, y] = env[x, y] == 1 ? 0 : 1; }
	}

    protected override float NextState(int x, int y) {
		int liveNbrs = GetLiveNbrs(x, y);
		if (env[x, y] == 0) {
			if (liveNbrs == 3) {
				return 1;
			} else {
				return 0;
			}
		} else {
			if (liveNbrs == 2 || liveNbrs == 3) {
				return 1;
			} else {
				return 0;
			}
		}
	}

	private int GetLiveNbrs(int x, int y) {
		int leftIdx = x - 1;
		if (x == 0) {
			leftIdx = width - 1;
		}

		int rightIdx = x + 1;
		if (x == width - 1) {
			rightIdx = 0;
		}

		int topIdx = y - 1;
		if (y == 0) {
			topIdx = height - 1;
		}
		
		int bottomIdx = y + 1;
		if (y == height - 1) {
			bottomIdx = 0;
		}

		return Mathf.RoundToInt(
			env[rightIdx, y] +
			env[rightIdx, topIdx] +
			env[x,        topIdx] +
			env[leftIdx,  topIdx] +
			env[leftIdx,  y] +
			env[leftIdx,  bottomIdx] +
			env[x,        bottomIdx] +
			env[rightIdx, bottomIdx]
		);
	}
}
