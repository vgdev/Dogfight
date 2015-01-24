using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class ScreenBoundary : BaseLib.CachedObject {

	public enum Edge { Top = 0, Bottom = 1, Left = 2, Right = 3}
	private static Vector2[] fixedPoints = new Vector2[] {
				new Vector2 (0.5f, 1f),
				new Vector2 (0.5f, 0f),
				new Vector2 (0f, 0.5f),
				new Vector2 (1f, 0.5f)
		};
	
	public Camera referenceCamera;
	public Edge location;
	public float cameraSizeBufferRatio;
	private BoxCollider2D boundary;

	public override void Awake ()
	{
		base.Awake ();
		boundary = GetComponent<BoxCollider2D> ();
	}

	// Update is called once per frame
	void Update () 
	{
		Vector2 fixedPoint = fixedPoints [(int)location];
		Vector3 viewportPoint = new Vector3 (fixedPoint.x, fixedPoint.y, 0f);
		Vector2 screenSize = new Vector2 (1f, (float)((double)Screen.width / (double)Screen.height)) * referenceCamera.orthographicSize * 2.0f;
		Vector3 newPosition = referenceCamera.ViewportToWorldPoint (viewportPoint);

		Vector2 area = boundary.size;
		switch(location)
		{
			case Edge.Top:
			case Edge.Bottom:
				area.y = cameraSizeBufferRatio * referenceCamera.orthographicSize;
				area.x = screenSize.x;
				break;
			case Edge.Left:
			case Edge.Right:
				area.x = cameraSizeBufferRatio * referenceCamera.orthographicSize;
				area.y = screenSize.y;
				break;
		}
		boundary.size = area;

		Bounds oldBounds = boundary.bounds;
		switch(location)
		{
			case Edge.Top:
				newPosition.y += oldBounds.extents.y;
				break;
			case Edge.Bottom:
				newPosition.y -= oldBounds.extents.y;
				break;
			case Edge.Left:
				newPosition.x -= oldBounds.extents.x;
				break;
			case Edge.Right:
				newPosition.x += oldBounds.extents.x;
				break;
		}

		newPosition.z = Transform.position.z;
		Transform.position = newPosition;
	}
}
