using UnityEngine;
using System.Collections;

public class TestTrig : MonoBehaviour {

	public float resolution;
	public int testCount;

	private Vector2[] cache;

	// Use this for initialization
	void Start () {
		int count = Mathf.CeilToInt(360f / resolution);
		cache = new Vector2[count];
		float rad;
		for(int i = 0; i < count; i++) {
			rad = Mathf.Deg2Rad * i * resolution;
			cache[i] = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
		}

		Vector2 initial = Vector2.zero, test;
		float errorSum = 0, degree = 0, initialTime = 0, endTime = 0;
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();
		for (int i = 0; i < testCount; i++) {
			degree = Random.Range(0f, 360f);
			initial = Normal(degree);
			test = Cached (degree);
			errorSum += (test - initial).sqrMagnitude;
		}
		sw.Start();
		for(int i = 0; i < testCount; i++) {
			initial = Normal(degree);
		}
		print ("Init: " + sw.ElapsedTicks);
		sw.Reset ();
		for(int i = 0; i < testCount; i++) {
			initial = Cached(degree);
		}
		print ("Cached: " + sw.ElapsedTicks);
		print (Mathf.Sqrt(errorSum) / testCount);
	}

//	void Update() {
//		Vector2 initial, test;
//		float errorSum = 0, degree, rad; 
//		for (int i = 0; i < testCount; i++) {
//			degree = Random.Range(0f, 360f);
//			initial = Normal(degree);
//			test = Cached (degree);
//			errorSum += (test - initial).sqrMagnitude;
//		}
//	}

	Vector2 Normal(float degree) {
		float rad = Mathf.Deg2Rad * degree;
		return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
	}

	Vector2 Cached(float degree) {
		return cache[Mathf.FloorToInt(degree / resolution)];
	}
}
