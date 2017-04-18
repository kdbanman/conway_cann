using UnityEngine;

public class NeuralNetAutomata : AbstractAutomata {

	// 8 neighbors, 1 self, and 1 bias
	private const int INPUT_DIM_INCLUDING_BIAS = 10;
	private const float BIAS_ACTIVATION = 1f;

	private float[,] inputToHiddenWeights;
	private float[] hiddenToOutputWeights;

	// Previous states tracked for backpropagation
	private float[] previousForwardInput,
					previousForwardHiddenState;
	private float previousForwardOutput;

	public int HiddenDimIncludingBias { get; private set; }

	public NeuralNetAutomata(int width, int height, int hiddenUnits = 3, int rngSeed = 7878) {
		InitializeEnvironments(width, height);

		HiddenDimIncludingBias = hiddenUnits + 1;

		inputToHiddenWeights = new float[HiddenDimIncludingBias, INPUT_DIM_INCLUDING_BIAS];
		hiddenToOutputWeights = new float[HiddenDimIncludingBias];

		previousForwardInput = new float[INPUT_DIM_INCLUDING_BIAS];
		previousForwardHiddenState = new float[HiddenDimIncludingBias];

		RandomizeWeights(rngSeed);
	}

	public void RandomizeWeights(int rngSeed) {
		Random.InitState(rngSeed);

		int rows = inputToHiddenWeights.GetLength(0);
		int cols = inputToHiddenWeights.GetLength(1);
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				inputToHiddenWeights[row, col] = Random.Range(0f, 0.1f);
			}
			hiddenToOutputWeights[row] = Random.Range(0f, 0.1f);
		}
	}

	protected override float NextState(int x, int y) {
		SetInputTmp(x, y);

		for (int hiddenUnit = 0; hiddenUnit < HiddenDimIncludingBias - 1; hiddenUnit++) {
			float hiddenLinearity = ComputeHiddenLinearity(hiddenUnit);
			previousForwardHiddenState[hiddenUnit] = Sigmoid(hiddenLinearity);
		}
		previousForwardHiddenState[HiddenDimIncludingBias - 1] = BIAS_ACTIVATION;

		float outputLinearity = 0;
		for (int hiddenUnit = 0; hiddenUnit < HiddenDimIncludingBias; hiddenUnit++) {
			outputLinearity += previousForwardHiddenState[hiddenUnit] * hiddenToOutputWeights[hiddenUnit];
		}
		
		previousForwardOutput = Sigmoid(outputLinearity);
		return previousForwardOutput;
	}

    private void SetInputTmp(int x, int y)
    {
        int left = GetToroidalX(x - 1);
		int right = GetToroidalX(x + 1);
		int bottom = GetToroidalY(y - 1);
		int top = GetToroidalY(y + 1);

		// corner neighbors
		previousForwardInput[0] = env[right, top];
		previousForwardInput[1] = env[left,  top];
		previousForwardInput[2] = env[left,  bottom];
		previousForwardInput[3] = env[right, bottom];

		// axial neighbors
		previousForwardInput[4] = env[right, y];
		previousForwardInput[5] = env[x,     top];
		previousForwardInput[6] = env[left,  y];
		previousForwardInput[7] = env[x,     bottom];

		// self
		previousForwardInput[8] = env[x, y];

		// bias.  setting every call is technically redundant but explicit.
		previousForwardInput[9] = BIAS_ACTIVATION;
    }

    private float ComputeHiddenLinearity(int hiddenUnit)
    {
		float linearity = 0;
        for (int inputIdx = 0; inputIdx < INPUT_DIM_INCLUDING_BIAS; inputIdx++) {
			linearity += previousForwardInput[inputIdx] * inputToHiddenWeights[hiddenUnit, inputIdx];
		}
		return linearity;
    }

    private float Sigmoid(float linearity)
    {
        return 1f / (1f + Mathf.Exp(-linearity));
    }
}
