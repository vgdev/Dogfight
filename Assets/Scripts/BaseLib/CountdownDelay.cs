using UnityEngine;
using System.Collections;

namespace BaseLib {

	/// <summary>
	/// Countdown delay.
	/// </summary>
	[System.Serializable]
	public struct CountdownDelay {

		/// <summary>
		/// The max delay.
		/// </summary>
		[SerializeField]
		private float maxDelay;
		public float MaxDelay {
			get {
				return maxDelay;
			}
			set {
				maxDelay = value;
			}
		}	

		/// <summary>
		/// The current delay.
		/// </summary>
		[SerializeField]
		private float currentDelay;
		public float CurrentDelay {
			get {
				return currentDelay;
			}
			set {
				currentDelay = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseLib.CountdownDelay"/> struct.
		/// </summary>
		/// <param name="maxDelay">Max delay.</param>
		public CountdownDelay(float maxDelay) {
			this.maxDelay = maxDelay;
			currentDelay = maxDelay;
		}

		/// <summary>
		/// Ticks down.
		/// </summary>
		/// <returns>The down.</returns>
		/// <param name="dt">Dt.</param>
		public bool Tick(float dt, bool reset = true, float value = -1) {
			currentDelay -= dt;
			bool ready = currentDelay <= 0f;
			if(ready && reset)
				currentDelay = (value > 0) ? value : maxDelay;
			return ready;
		}

		/// <summary>
		/// Ready this instance.
		/// </summary>
		public bool Ready() {
			return currentDelay <= 0f;
		}

		/// <summary>
		/// Forces the ready.
		/// </summary>
		/// <returns><c>true</c>, if ready was forced, <c>false</c> otherwise.</returns>
		public void ForceReady() {
			currentDelay = 0f;
		}

		/// <summary>
		/// Reset the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Reset() {
			currentDelay = maxDelay;
		}
	}

	/// <summary>
	/// Counter.
	/// </summary>
	[System.Serializable]
	public struct Counter {

		[SerializeField]
		private int maxCount;

		[SerializeField]
		private int count;

		public Counter(int maxCount) {
			this.maxCount = maxCount;
			count = maxCount;
		}

		public bool Tick(bool reset = true) {
			count--;
			bool ready = count < 0;
			if(ready && reset)
				count = maxCount;
		}


	}
}
