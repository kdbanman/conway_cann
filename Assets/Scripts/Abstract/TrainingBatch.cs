public class TrainingBatch {

	private int INPUT_DIM_NO_BIAS = 9;

	// each row (major index) is a sample
	private float[][] inputTensor;

	private float[] outputList;

	private int nextSampleIndex;

	public bool Empty { get { return nextSampleIndex == 0; } }

	public bool Full { get { return nextSampleIndex >= MaxSize; } }

	public int Size { get { return nextSampleIndex; } }

	public int MaxSize { get; private set; }

	public TrainingBatch(int maxBatchSize = 100) {
		inputTensor = new float[maxBatchSize][];
		for (int i = 0; i < maxBatchSize; i++) {
			inputTensor[i] = new float[INPUT_DIM_NO_BIAS];
		}

		outputList = new float[maxBatchSize];

		MaxSize = maxBatchSize;
	}

	public void RecordSample(float[] input, float output) {
		if (Full) {
			return;
		}

		for (int i = 0; i < INPUT_DIM_NO_BIAS; i++) {
			inputTensor[nextSampleIndex][i] = input[i];
		}
		outputList[nextSampleIndex] = output;

		nextSampleIndex++;
	}

	public void ConsumeBatch(System.Action<float[], float, int> processSample) {
		int sampleNumber = 0;
		while (!Empty) {
			nextSampleIndex--;
			processSample(inputTensor[nextSampleIndex], outputList[nextSampleIndex], sampleNumber);
		}
	}
}
