using UnityEngine;
using System.Collections;
using BaseLib;

public class ProjectileData {
	private static Mesh quadMesh;
	private ProjectileGroup group; 	
	private Vector3 position = Vector3.zero;
	private Quaternion rotation = Quaternion.identity;
	private Vector3 scale = Vector3.zero;
	private float velocity = 0f;
	private Quaternion angularVelocity = Quaternion.identity;
	private ProjectileCollisionHandler collisionHandler;
	
	public static Mesh ProjectileMesh
	{
		get {
			if(quadMesh == null) {
				//Magic numbers woooooo!
				quadMesh = new Mesh();
				Vector3[] verts = new Vector3[]{
					new Vector3( 1, 0, 1),
					new Vector3( 1, 0, -1),
					new Vector3(-1, 0, 1),
					new Vector3(-1, 0, -1),
				};
				Vector2[] uv = new Vector2[] {
					new Vector2(1, 1),
					new Vector2(1, 0),
					new Vector2(0, 1),
					new Vector2(0, 0),
				};
				int[] tris = new int[]{0, 1, 2, 2, 1, 3};
				quadMesh.vertices = verts;
				quadMesh.uv = uv;
				quadMesh.triangles = tris;
			}
			return quadMesh;
		}
	}

	public Vector3 Position {
		get {
			return position;
		}
		set {
			position = value;
		}
	}

	public Quaternion Rotation {
		get {
			return rotation;
		}
		set {
			rotation = value;
		}
	}

	public Vector3 Scale {
		get {
			return position;
		}
		set {
			position = value;
		}
	}

	public float Velocity {
		get {
			return velocity;
		}
		set {
			velocity = value;
		}
	}

	public Quaternion AngularVelocity {
		get {
			return angularVelocity;
		}
		set {
			angularVelocity = value;
		}
	}

	public ProjectileCollisionHandler CollisionHandler {
		get {
			return collisionHandler;
		}
		set {
			collisionHandler = value;
		}
	}

	public ProjectileGroup Group {
		get {
			return group;
		}
		set {
			group = value;
		}
	}

	public void Draw()
	{
		Graphics.DrawMesh (ProjectileMesh, Matrix4x4.TRS (position, rotation, scale), group.ProjectileMaterial, group.Layer, group.TargetCamera);
	}

	public void Update(float dt)
	{
		Quaternion newRotation = Quaternion.Slerp (rotation, rotation * angularVelocity, dt);
		Vector3 movementVector = velocity * dt * (newRotation * Vector3.up);

		//TODO: add support for ProjectileControllers here

		RaycastHit2D[] hits = collisionHandler.CheckCollision (this, movementVector);

		rotation = newRotation;
		position += movementVector;
	}
}

public abstract class ProjectileCollisionHandler {
	public abstract RaycastHit2D[] CheckCollision(ProjectileData projectile, Vector3 movementVector);
}

public class BoxProjectileCollisionHandler : ProjectileCollisionHandler {
	private Vector2 boxOffset;
	private Vector2 boxSize;

	public BoxProjectileCollisionHandler(Vector2 offset, Vector2 size) {
		boxOffset = offset;
		boxSize = size;
	}

	public override RaycastHit2D[] CheckCollision (ProjectileData projectile, Vector3 movementVector) {
		Vector2 origin = Util.To2D (projectile.Position) + Util.ComponentProduct2 (projectile.Scale, boxOffset);
		Vector2 size = Util.ComponentProduct2 (projectile.Scale, boxSize);
		return Physics2D.BoxCastAll (origin, size, projectile.Rotation.eulerAngles.z, movementVector, movementVector.magnitude);
	}
}

public class CircleProjectileCollisionHandler : ProjectileCollisionHandler {
	private Vector2 circleOffset;
	private float circleRadius;

	public CircleProjectileCollisionHandler(Vector2 offset, float radius) {
		circleOffset = offset;
		circleRadius = radius;
	}

	private float MinAbsScale(params float[] values) {
		float min = float.MaxValue;
		for(int i = 0; i < values.Length; i++)
			if(values[i] < min)
				min = Mathf.Abs(values[i]);
		return min;
	}

	public override RaycastHit2D[] CheckCollision (ProjectileData projectile, Vector3 movementVector) {
		Vector2 origin = Util.To2D (projectile.Position) + Util.ComponentProduct2 (projectile.Scale, circleOffset);
		float size = circleRadius * MinAbsScale (projectile.Scale.x, projectile.Scale.y, projectile.Scale.z);
		return Physics2D.CircleCastAll(origin, size, movementVector, movementVector.magnitude, projectile.Group.Layer);
	}
}
