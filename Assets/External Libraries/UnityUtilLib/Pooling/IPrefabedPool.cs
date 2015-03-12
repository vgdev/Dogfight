using UnityEngine;
using System.Collections;

namespace UnityUtilLib.Pooling {
	public interface IPrefabedPool<T, P> where T : IPooledObject, IPrefabed<P> {
		T Get(P prefab);
		void Return (T obj);
	}
}
