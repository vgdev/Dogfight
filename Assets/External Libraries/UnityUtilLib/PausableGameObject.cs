using UnityEngine;
using System.Collections;

namespace UnityUtilLib { 
	public abstract class PausableGameObject : CachedObject, IPausable {

		private static WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
		private bool paused = false;
		public bool Paused {
			get {
				return paused;
			}
		}
		public void Pause () {
			paused = true;
		}
		public void Unpause () {
			paused = false;
		}

		public void Update() {
			AlwaysUpdate ();
			if(!paused)
				NormalUpdate();
		}

		public virtual void AlwaysUpdate() {
		}

		public virtual void NormalUpdate() {
		}

		protected YieldInstruction WaitForUnpause() {
			if(paused)
				return StartCoroutine (UnpauseWait ());
			else
				return wfeof;
		}

		private IEnumerator UnpauseWait() {
			while(paused)
				yield return wfeof;
		}
	}
}