using System;

namespace UnityUtilLib {
	public interface IPrefabed<T> {
		T Prefab { get; set; }
		void MatchPrefab(T prefab);
	}
}

