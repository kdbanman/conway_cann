using System;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetAutomata : AbstractAutomata {
	private const int INPUT_DIM_INCLUDING_BIAS = 10;
	private const float BIAS_ACTIVATION = 1f;

    private int rngSeed;
	private float initializationMax;

	private float[,] inputToHiddenWeights;
	private float[] hiddenToOutputWeights;

	// Previous states tracked for backpropagation
	private float[] previousForwardInput,
					previousForwardHiddenState;
	private float previousForwardOutput;

    // Batch weight corrections instantiated once for performance
    private float[] hiddenToOutputCorrections;
    private float[,] inputToHiddenCorrections;

    // public variables for setting in the unity editor
	public int HiddenDimIncludingBias { get; private set; }

    // public properties for rendering in the scene
    public float PreviousMinBatchCost { get; private set; }
    public float PreviousMaxBatchCost { get; private set; }
    public float PreviousAvgBatchCost { get; private set; }

	public NeuralNetAutomata(int width, int height, int hiddenUnits = 3, int rngSeed = 7878, float initializationMax = 0.1f)
    {
        InitializeEnvironments(width, height);

        HiddenDimIncludingBias = hiddenUnits + 1;

        inputToHiddenWeights = new float[HiddenDimIncludingBias, INPUT_DIM_INCLUDING_BIAS];
        hiddenToOutputWeights = new float[HiddenDimIncludingBias];

        previousForwardInput = new float[INPUT_DIM_INCLUDING_BIAS];
        previousForwardHiddenState = new float[HiddenDimIncludingBias];

        hiddenToOutputCorrections = new float[HiddenDimIncludingBias];
        inputToHiddenCorrections = new float[HiddenDimIncludingBias, INPUT_DIM_INCLUDING_BIAS];

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

    public override System.Action<int, int> onToggle {
		get {
			return (x, y) => env[x, y] = env[x, y] < 0.75f ? 1f : 0.5f;
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

		// setting every call is technically redundant but explicit.
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
		
		FileLogger.WriteLine("Batch size: " + trainingBatch.Size + "\n");
		FileLogger.WriteLine("Input --> Known Output,   Prediction,   Cost\n");

        for (int k = 0; k < HiddenDimIncludingBias; k++) {
            hiddenToOutputCorrections[k] = 0;
            for (int i = 0; i < INPUT_DIM_INCLUDING_BIAS; i++) {
                inputToHiddenCorrections[k, i] = 0;
            }
        }
        // This is stored here because after the batch is consumed, its size will be zero.
        int trainingBatchSize = trainingBatch.Size;

		trainingBatch.ConsumeBatch((inputSample, knownOutput, sampleNumber) =>
        {
            LogSample(inputSample, knownOutput);

            SetInputTmp(inputSample);
            float predictedOutput = ComputeOutputFromInputField();

            float cost = (predictedOutput - knownOutput) * (predictedOutput - knownOutput);

            PreviousMinBatchCost = Mathf.Min(PreviousMinBatchCost, cost);
            PreviousMaxBatchCost = Mathf.Max(PreviousMaxBatchCost, cost);
            PreviousAvgBatchCost += cost;

            FileLogger.Write(predictedOutput.ToString("F1") + ",   ");
            FileLogger.Write(cost.ToString("F2"));

            // BEGIN tHE DERIVATIVES.  BYE READABILITY. 👋
            float yp = predictedOutput;
            float yt = knownOutput;

            float di_yp = 2f * (yp - yt);
            float di_zy = di_yp * yp * (1f - yp);

            float[] di_wh = new float[HiddenDimIncludingBias];
            float[] di_h = new float[HiddenDimIncludingBias];
            float[] di_zh = new float[HiddenDimIncludingBias];
            for (int k = 0; k < HiddenDimIncludingBias; k++)
            {
                float hk = previousForwardHiddenState[k];
                float wk = hiddenToOutputWeights[k];

                di_wh[k] = di_zy * hk;
                di_h[k] = di_zy * wk;
                di_zh[k] = di_h[k] * hk * (1f - hk);

                // gradient descent on hidden layer weights
                hiddenToOutputWeights[k] += -learningRate * di_wh[k];
                // hiddenToOutputCorrections[k] += di_wh[k];
            }

            float[,] di_w = new float[HiddenDimIncludingBias, INPUT_DIM_INCLUDING_BIAS];
            for (int k = 0; k < HiddenDimIncludingBias; k++)
            {
                for (int i = 0; i < INPUT_DIM_INCLUDING_BIAS; i++)
                {
                    float xi = previousForwardInput[i];
                    di_w[k, i] = di_zh[k] * xi;

                    // gradient descent on input layer weights
                    inputToHiddenWeights[k, i] += -learningRate * di_w[k, i];
                    // inputToHiddenCorrections[k, i] += di_w[k, i];
                }
            }

			FileLogger.Write("\n");
        });

        // for (int k = 0; k < HiddenDimIncludingBias; k++) {
        //     hiddenToOutputWeights[k] += -learningRate * hiddenToOutputCorrections[k] / trainingBatchSize;
        //     for (int i = 0; i < INPUT_DIM_INCLUDING_BIAS; i++) {
        //         inputToHiddenWeights[k, i] += -learningRate * inputToHiddenCorrections[k, i] / trainingBatchSize;
        //     }
        // }

        PreviousAvgBatchCost /= trainingBatchSize;
    }

    private static void LogSample(float[] inputSample, float knownOutput)
    {
        for (int i = 0; i < inputSample.Length; i++)
        {
			if (inputSample[i] > 0.5f) {
				FileLogger.Write(" ");
			}
			else {
				FileLogger.Write("X");				
			}
        }
        FileLogger.Write("--> ");

		if (knownOutput > 0.5f) {
			FileLogger.Write(" ,   ");
		}
		else {
			FileLogger.Write("X,   ");				
		}
    }
}
