using UnityEngine;
using System.Collections;
using UnityUtilLib;

public class TestApproximateDistance : TestScript {
	
	// Update is called once per frame
	void Update () {
		int size = 10000;
		Vector2[] testArray = new Vector2[size];
		for(int i = 0; i < size; i++) {
			testArray[i] = Random.insideUnitCircle.normalized;
		}
		float[] approx = TestApproximate (testArray);
		float[] exact = TestMagnitude (testArray);
		float error = 0;
		Debug.Log(approx[0] + " " + exact[0]);
		for (int i = 0; i < size; i++) {
			error += Mathf.Abs(approx[i] - exact[i]);
		}
		Debug.Log (error / (float) size );
	}

	float[] TestApproximate(Vector2[] v) {
		float[] temp = new float[v.Length];
		for(int i = 0; i < v.Length; i++) {
			temp[i] = v[i].ManhattanMagnitude();
		}
		return temp;
	}

	float[] TestMagnitude(Vector2[] v) {
		float[] temp = new float[v.Length];
		for(int i = 0; i < v.Length; i++) {
			temp[i] = v[i].magnitude;
		}
		return temp;
	}
}
