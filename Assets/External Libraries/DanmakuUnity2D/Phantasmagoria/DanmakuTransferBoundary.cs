using UnityEngine;
using System.Collections;

namespace Danmaku2D.Phantasmagoria {
	public class DanmakuTransferBoundary : DanmakuBoundary {

		[SerializeField]
		private DanmakuField field;
		public DanmakuField Field {
			get {
				return field;
			}
			set {
				field = value;
			}
		}

		protected override void ProcessDanmaku (Danmaku proj) {
			if (field != null) {
				PhantasmagoriaGameController.Transfer(proj);
			}
		}
	}
}