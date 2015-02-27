using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	
	public abstract class AbstractGameController : SingletonBehavior<AbstractGameController> {

		private static float oldTimeScale;
		private static bool gamePaused;
		public static bool IsGamePaused {
			get {
				return gamePaused;
			}
		}

		public static void Pause() {
			PausableGameObject[] pauseableObjects = FindObjectsOfType<PausableGameObject> ();
			for(int i = 0; i < pauseableObjects.Length; i++) {
				pauseableObjects[i].Pause();
			}
			gamePaused = true;
			oldTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}

		public static void Unpause() {
			PausableGameObject[] pauseableObjects = FindObjectsOfType<PausableGameObject> ();
			for(int i = 0; i < pauseableObjects.Length; i++) {
				pauseableObjects[i].Unpause();
			}
			gamePaused = false;
			Time.timeScale = oldTimeScale;
		}
	}
}