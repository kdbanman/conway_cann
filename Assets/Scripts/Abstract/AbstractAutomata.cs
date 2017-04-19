public abstract class AbstractAutomata : IAutomata {

	protected int width, height;

	protected float[,] env, nextEnv;

	protected void InitializeEnvironments(int width, int height) {
		this.width = width;
		this.height = height;

		env = new float[width, height];
		nextEnv = new float[width, height];
	}

	protected abstract float NextState(int x, int y);
	public abstract System.Action<int, int> onToggle { get; }

	public void Step() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y++) {
				nextEnv[x, y] = NextState(x, y);
			}
		}

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

	protected int GetToroidalX(int x) {
		return (x + width) % width;
	}

	protected int GetToroidalY(int y) {
		return (y + height) % height;
	}
}
