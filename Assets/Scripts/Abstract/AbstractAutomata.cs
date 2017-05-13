using UnityEngine;

public abstract class AbstractAutomata : IAutomata {

	protected int width, height;

	protected float[,] env, nextEnv;

	public TrainingBatch TrainingBatch { get; private set; }

    public void MirrorEnvironment(float[,] toMirror)
    {
        InitializeEnvironments(toMirror.GetLength(0), toMirror.GetLength(1));
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                env[x, y] = toMirror[x, y];
            }
        }
    }

	protected void InitializeEnvironments(int width, int height) {
		this.width = width;
		this.height = height;

		env = new float[width, height];
		nextEnv = new float[width, height];

		ResetEnvironment();

		TrainingBatch = new TrainingBatch(1500);
	}

	protected abstract float NextState(int x, int y);
	public abstract System.Action<int, int> onToggle { get; }

	public void ResetEnvironment() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				env[x, y] = 0.5f;
			}
		}
	}

	public void Step() {
		float[] sampleInputCache = new float[9];
		// string tmp = "";
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				nextEnv[x, y] = NextState(x, y);

				FillSampleInputArray(x, y, sampleInputCache);
				TrainingBatch.RecordSample(sampleInputCache, nextEnv[x, y]);
				// tmp += nextEnv[x, y].ToString("F2") + " ";
			}
			// tmp += "\n";
		}
		// Debug.Log(tmp);

		float[,] tmpEnv = env;
		env = nextEnv;
		nextEnv = tmpEnv;
	}

	public float this[int x, int y] {
		get {
			return env[GetToroidalX(x), GetToroidalY(y)];
		}
		set {
			env[GetToroidalX(x), GetToroidalY(y)] = value;
		}
	}

	protected void FillSampleInputArray(int x, int y, float[] inputArrayNoBias) {
		int left = GetToroidalX(x - 1);
		int right = GetToroidalX(x + 1);
		int bottom = GetToroidalY(y - 1);
		int top = GetToroidalY(y + 1);

		// corner neighbors
		inputArrayNoBias[0] = env[right, top];
		inputArrayNoBias[1] = env[left,  top];
		inputArrayNoBias[2] = env[left,  bottom];
		inputArrayNoBias[3] = env[right, bottom];

		// axial neighbors
		inputArrayNoBias[4] = env[right, y];
		inputArrayNoBias[5] = env[x,     top];
		inputArrayNoBias[6] = env[left,  y];
		inputArrayNoBias[7] = env[x,     bottom];

		// self
		inputArrayNoBias[8] = env[x, y];
	}

	protected int GetToroidalX(int x) {
		return (x + width) % width;
	}

	protected int GetToroidalY(int y) {
		return (y + height) % height;
	}
}
