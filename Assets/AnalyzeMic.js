#pragma strict

public var SAMPLES = 512;
public var FREQS = 1024;
public var SAMPLE_RATE = 44100;
public var source : AudioSource;
public var micInput : String = null;
public var outputFreq : float;

private var sampleData : float[];
private var freqData : float[];
private var freqDataRaw : float[];

function Start () {
	LogInfo();
	var duration = 1.0 / 30.0;
	sampleData = new Array(SAMPLES);
	freqData = new Array(FREQS);
	freqDataRaw = new Array(FREQS);
	source.clip = Microphone.Start(micInput, true, 1, SAMPLE_RATE);
	while (Microphone.GetPosition(micInput) <= 0) {
		WaitForSeconds(0.01);
		// wait...
	}
	source.Play();
	source.loop = true;
}

function LogInfo() {
	var minFreq : int;
	var maxFreq : int;
	Microphone.GetDeviceCaps(micInput, minFreq, maxFreq);
	//Debug.Log("min/max freq: " + minFreq + " " + maxFreq);
	// Debug.Log("camera pixel width " + GetComponent.<Camera>().pixelWidth);

}

function Update () {
	outputFreq = GetFundamentalFreqEstimate();
	transform.localPosition = Vector3(outputFreq, 0, 0);
}

function BinToHz(binNum : int) {
	return parseFloat(binNum) * SAMPLE_RATE / FREQS;
}

function HzToBin(hz : float) {
	return hz * FREQS / SAMPLE_RATE;
}

function HzToMidi(hz : float) {
	return hz == 0 ? 0 : 12 * Mathf.Log(hz / 440, 2) + 69;
}

function CopyData(data : float[]) {
	var weight = 1.0;
	for (var bin=0; bin < freqData.Length / 2; bin++) {
		freqData[bin] *= (weight - 1);
		freqData[bin] += weight * data[bin];
	}
}

function DoFFT() {
	source.GetSpectrumData(freqDataRaw, 0, FFTWindow.Hamming);
}

function GetFundamentalFreqEstimate () {
	// CopyData(freqDataRaw);  // unnecessary for now
	DoFFT();
	var bin : int;
	var maxBin : float = -1.0;
	var maxVal = 0.0;
	var foundFirstPeak = false;
	for (bin=0; bin < freqDataRaw.Length / 2; bin++) {
		var dampening : float = 1 / Mathf.Log(bin, 2);
		var d = Mathf.Log(freqDataRaw[bin] / 0.001, 2) * dampening;
		if (d > maxVal) {
			maxBin = parseFloat(bin);
			maxVal = d;
		}
		freqData[bin] = d;
	}

	for (bin=0; bin < freqDataRaw.Length / 2; bin++) {

	}

	var x = 0.0;
	for (bin=0; bin < freqData.Length / 2; bin++) {
		var y = 1 * freqData[bin];
		var color = (bin == maxBin) ? Color.green : Color.white;
		color.a = 0.1;
		Debug.DrawLine(Vector3(x, 0, 0), Vector3(x, y, 0), color);
		x += 0.01;
	}
	return maxBin < 0 ? 0 : BinToHz(maxBin);
}
