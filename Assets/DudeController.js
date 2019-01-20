#pragma strict

var analyzeScript : AnalyzeMic;

private var height : float = 0;

function Start () {

}

function Update () {
	var hz:float = analyzeScript.outputFreq;
	var note = analyzeScript.HzToMidi(hz);
	var newHeight = (note - 40) / 10;
	height = newHeight;
	Debug.Log(hz + ' ' + note);
	transform.position.y = height;
}