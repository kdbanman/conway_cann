using System;
using UnityEngine;

public class ConwaysAutomata : AbstractAutomata {

	public ConwaysAutomata(int width, int height) {
		InitializeEnvironments(width, height);
	}

    public override Action<int, int> onToggle {
		get { return (x, y) => env[x, y] = env[x, y] == 1 ? 0.5f : 1; }
	}

    protected override float NextState(int x, int y) {
		int liveNbrs = GetLiveNbrs(x, y);
		if (env[x, y] == 1) {
			if (liveNbrs == 2 || liveNbrs == 3) {
				return 1;
			} else {
				return 0.5f;
			}
		} else {
			if (liveNbrs == 3) {
				return 1;
			} else {
				return 0.5f;
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

		return IsLive(rightIdx, y) +
			   IsLive(rightIdx, topIdx) +
			   IsLive(x,        topIdx) +
			   IsLive(leftIdx,  topIdx) +
			   IsLive(leftIdx,  y) +
			   IsLive(leftIdx,  bottomIdx) +
			   IsLive(x,        bottomIdx) +
			   IsLive(rightIdx, bottomIdx);
	}

	private int IsLive(int x, int y) {
		return env[x, y] == 1 ? 1 : 0;
	}
}
