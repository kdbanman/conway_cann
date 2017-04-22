using System;
using System.Collections.Generic;
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

    public override System.Action<int, int> onToggle {
		get {
			return (x, y) => env[x, y] = env[x, y] < 0.5f ? 1f : 0f;
		}
	}

    public int rngSeed;
	public float initializationMax;

	public NeuralNetAutomata(int width, int height, int hiddenUnits = 3, int rngSeed = 7878, float initializationMax = 0.1f)
    {
        InitializeEnvironments(width, height);

        HiddenDimIncludingBias = hiddenUnits + 1;

        inputToHiddenWeights = new float[HiddenDimIncludingBias, INPUT_DIM_INCLUDING_BIAS];
        hiddenToOutputWeights = new float[HiddenDimIncludingBias];

        previousForwardInput = new float[INPUT_DIM_INCLUDING_BIAS];
        previousForwardHiddenState = new float[HiddenDimIncludingBias];

        if (this.rngSeed == 0)
        {
            this.rngSeed = rngSeed;
        }

        if (this.initializationMax == 0f)
        {
            this.initializationMax = initializationMax;
        }

        RandomizeWeights(rngSeed, initializationMax);
    }

    public void RandomizeWeights(int rngSeed, float initializationMax) {
		UnityEngine.Random.InitState(rngSeed);

		int rows = inputToHiddenWeights.GetLength(0);
		int cols = inputToHiddenWeights.GetLength(1);
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				inputToHiddenWeights[row, col] = UnityEngine.Random.Range(0f, initializationMax);
			}
			hiddenToOutputWeights[row] = UnityEngine.Random.Range(0f, initializationMax);
		}
	}

	protected override float NextState(int x, int y) {
		SetInputTmp(x, y);

		return ComputeOutputFromInputField();
	}

    private float ComputeOutputFromInputField()
    {
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
        FillSampleInputArray(x, y, previousForwardInput);

		// bias.  setting every call is technically redundant but explicit.
		previousForwardInput[INPUT_DIM_INCLUDING_BIAS - 1] = BIAS_ACTIVATION;
    }

	private void SetInputTmp(float[] fixedInput) {
		for (int i = 0; i < INPUT_DIM_INCLUDING_BIAS - 1; i++) {
			previousForwardInput[i] = fixedInput[i];
		}

		previousForwardInput[INPUT_DIM_INCLUDING_BIAS - 1] = BIAS_ACTIVATION;
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

	// ///////////
    //
	// Training
    //
	// ///////////

    internal void TrainFrom(TrainingBatch trainingBatch, float learningRate)
    {
		if (trainingBatch.Empty) {
			return;
		}
		
		FileLogger.Write("Batch size: " + trainingBatch.Size + "\n");
        FileLogger.Write("Costs: ");

		trainingBatch.ConsumeBatch((inputSample, knownOutput) => {
			SetInputTmp(inputSample);

			float predictedOutput = ComputeOutputFromInputField();

			float cost = (predictedOutput - knownOutput) * (predictedOutput - knownOutput);
			FileLogger.Write(cost.ToString("F2") + ", ");
			
			// BEGIN tHE DERIVATIVES.  BYE READABILITY. 👋
			float yp = predictedOutput;
			float yt = knownOutput;

			float di_yp = 2f * (yp - yt);
			float di_zy = di_yp * yp * (1f - yp);

			float[] di_wh = new float[HiddenDimIncludingBias];
			float[] di_h = new float[HiddenDimIncludingBias];
			float[] di_zh = new float[HiddenDimIncludingBias];
			for (int k = 0; k < HiddenDimIncludingBias; k++) {
				float hk = previousForwardHiddenState[k];
				float wk = hiddenToOutputWeights[k];

				di_wh[k] = di_zy * hk;
				di_h[k] = di_zy * wk;
				di_zh[k] = di_h[k] * hk * (1f - hk);

				// gradient descent on hidden layer weights
				hiddenToOutputWeights[k] = -learningRate * di_wh[k] * wk;
			}

			float[,] di_w = new float[HiddenDimIncludingBias, INPUT_DIM_INCLUDING_BIAS];
			for (int k = 0; k < HiddenDimIncludingBias; k++) {
				for (int i = 0; i < INPUT_DIM_INCLUDING_BIAS; i++) {
					float xi = previousForwardInput[i];
					di_w[k, i] = di_zh[k] * xi;

					// gradient descent on input layer weights
					inputToHiddenWeights[k, i] = -learningRate * di_w[k, i] * inputToHiddenWeights[k, i];
				}
			}
		});
		FileLogger.Write("\n");
    }
}
