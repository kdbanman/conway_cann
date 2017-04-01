using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conways {

	private int width, height;

	private int[,] env, nextEnv;

	public Conways(int width, int height) {
		this.width = width;
		this.height = height;

		env = new int[width, height];
		nextEnv = new int[width, height];
	}

	public void Step() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				int liveNbrs = GetLiveNbrs(x, y);
				if (env[x, y] == 0) {
					if (liveNbrs == 3) {
						nextEnv[x, y] = 1;
					} else {
						nextEnv[x, y] = 0;
					}
				} else {
					if (liveNbrs == 2 || liveNbrs == 3) {
						nextEnv[x, y] = 1;
					} else {
						nextEnv[x, y] = 0;
					}
				}
			}
		}

		int[,] tmpEnv = env;
		env = nextEnv;
		nextEnv = tmpEnv;
	}

	public int this[int x, int y] {
		get {
			return env[x, y];
		}
		set {
			env[x, y] = value;
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

		return env[rightIdx, y] +
		       env[rightIdx, topIdx] +
			   env[x,        topIdx] +
			   env[leftIdx,  topIdx] +
			   env[leftIdx,  y] +
			   env[leftIdx,  bottomIdx] +
			   env[x,        bottomIdx] +
			   env[rightIdx, bottomIdx];
	}
}
