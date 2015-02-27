using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	public interface IPausable {
		bool Paused { get; }
		void Pause();
		void Unpause();
	}
}