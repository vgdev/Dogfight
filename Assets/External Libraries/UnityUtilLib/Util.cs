using UnityEngine;
using System.Collections;

namespace UnityUtilLib
{
	/// <summary>
	/// Util.
	/// </summary>
	public static class Util {

		/// <summary>
		/// The degree2 RAD.
		/// </summary>
		public const float Degree2Rad = Mathf.PI / 180f;

		/// <summary>
		/// The rad2 degree.
		/// </summary>
		public const float Rad2Degree = 180f / Mathf.PI;

		
		private static Mesh quadMesh;
		private static Material standardSpriteMat;

		public static Vector2 SpriteScale(Sprite sprite, Vector2 scale) {
			float width = sprite.textureRect.width;
			float height = sprite.textureRect.height;
			Vector2 scaled = Util.ComponentProduct2(scale, new Vector2(width, height) / sprite.pixelsPerUnit);
			return scaled;
		}

		public static MaterialPropertyBlock SpriteToMPB(Sprite sprite, Color color, MaterialPropertyBlock mpb = null) {
			Debug.Log (mpb);
			if(mpb == null) {
				Debug.Log("Hi!");
				mpb = new MaterialPropertyBlock();
			}
			mpb.AddTexture("_MainTex", sprite.texture);
			mpb.AddColor("_Color", color);
			return mpb;
		}
		
		public static void DrawSprite(Sprite sprite,
		                              Vector3 position, 
		                              Quaternion rotation, 
		                              Vector3 scale, 
		                              Color color = default(Color), 
		                              Material material = null,
		                              MaterialPropertyBlock materialProperties = null,
		                              Camera camera = null,
		                              int layer = 0) {
			Matrix4x4 transform = Matrix4x4.TRS(position, rotation, Util.SpriteScale(sprite, scale));
			DrawSpriteUnscaled(sprite, transform, color, material, materialProperties, camera, layer);
		}

		public static void DrawSpriteUnscaled(Sprite sprite, 
				                              Matrix4x4 transform, 
				                              Color color = default(Color), 
				                              Material material = null, 
				                              MaterialPropertyBlock materialProperties = null,
				                              Camera camera = null,
				                              int layer = 0) {
			Debug.Log (color);
			if(material == null) {
				if(standardSpriteMat == null) {
					standardSpriteMat = new Material(Shader.Find("Sprites/Default"));
				}
				material = standardSpriteMat;
			}
			materialProperties = SpriteToMPB (sprite, color, materialProperties);
			DrawSpriteUnscaled (transform, material, materialProperties, camera, layer);
		}
		
		public static void DrawSpriteUnscaled(Matrix4x4 transform, 
				                              Material material, 
				                              MaterialPropertyBlock materialProperties,
		                                      Camera camera = null,
		                                      int layer = 0) {
			if (quadMesh == null) {
				quadMesh = CreateQuad();
			}
			Debug.Log (quadMesh.vertexCount);
			Graphics.DrawMesh(quadMesh, transform, material, layer, camera, 0, materialProperties, false, false);
		}
		
		private static Mesh CreateQuad() {
			Mesh mesh = new Mesh
			{
				vertices = new[]
				{
					new Vector3(-.5f, -.5f, 0),
					new Vector3(-.5f, +.5f, 0),
					new Vector3(+.5f, +.5f, 0),
					new Vector3(+.5f, -.5f, 0),
				},
				
				normals = new[]
				{
					Vector3.forward,
					Vector3.forward,
					Vector3.forward,
					Vector3.forward,
				},
				
				triangles = new[] { 0, 1, 2, 2, 3, 0 },
				
				uv = new[]
				{
					new Vector2(0, 0),
					new Vector2(0, 1),
					new Vector2(1, 1),
					new Vector2(1, 0),
				}
			};
			return mesh;
		}

		/// <summary>
		/// Sign the specified e.
		/// </summary>
		/// <param name="e">E.</param>
		public static float Sign(float e) {
			return (e == 0f) ? 0f : Mathf.Sign (e);
		}

		/// <summary>
		/// To2s the d.
		/// </summary>
		/// <returns>The d.</returns>
		/// <param name="v">V.</param>
		public static Vector2 To2D(Vector3 v) {
			return new Vector2(v.x, v.y);
		}

		/// <summary>
		/// To3s the d.
		/// </summary>
		/// <returns>The d.</returns>
		/// <param name="v">V.</param>
		/// <param name="z">The z coordinate.</param>
		public static Vector3 To3D(Vector2 v, float z = 0f) {
			return new Vector3 (v.x, v.y, z);
		}

		/// <summary>
		/// Randoms the vect2.
		/// </summary>
		/// <returns>The vect2.</returns>
		/// <param name="v">V.</param>
		public static Vector2 RandomVect2(Vector2 v) {
			return new Vector2 (Random.value * v.x, Random.value * v.y);
		}

		/// <summary>
		/// Randoms the vect3.
		/// </summary>
		/// <returns>The vect3.</returns>
		/// <param name="v">V.</param>
		public static Vector3 RandomVect3(Vector3 v) {
			return new Vector3 (Random.value * v.x, Random.value * v.y, Random.value * v.z);
		}

		/// <summary>
		/// Components the product2.
		/// </summary>
		/// <returns>The product2.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		public static Vector2 ComponentProduct2(Vector2 v1, Vector2 v2) {
			return new Vector2(v1.x * v2.x, v1.y * v2.y);
		}

		/// <summary>
		/// Components the product3.
		/// </summary>
		/// <returns>The product3.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		public static Vector3 ComponentProduct3(Vector3 v1, Vector3 v2) {
			return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
		}

		/// <summary>
		/// Maxs the component2.
		/// </summary>
		/// <returns>The component2.</returns>
		/// <param name="v">V.</param>
		public static float MaxComponent2(Vector2 v) {
			return (v.x > v.y) ? v.x : v.y;
		}

		/// <summary>
		/// Maxs the component3.
		/// </summary>
		/// <returns>The component3.</returns>
		/// <param name="v">V.</param>
		public static float MaxComponent3(Vector3 v) {
			if(v.x > v.y)
				return (v.z > v.y) ? v.z : v.y;
			else
				return (v.z > v.x) ? v.z : v.x;
		}

		/// <summary>
		/// Minimums the component2.
		/// </summary>
		/// <returns>The component2.</returns>
		/// <param name="v">V.</param>
		public static float MinComponent2(Vector2 v) {
			return (v.x < v.y) ? v.x : v.y;
		}

		/// <summary>
		/// Minimums the component3.
		/// </summary>
		/// <returns>The component3.</returns>
		/// <param name="v">V.</param>
		public static float MinComponent3(Vector3 v) {
			if(v.x < v.y)
				return (v.z < v.y) ? v.z : v.y;
			else
				return (v.z < v.x) ? v.z : v.x;
		}
		/// <summary>
		/// Berziers the curve vector lerp.
		/// </summary>
		/// <returns>The curve vector lerp.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="c1">C1.</param>
		/// <param name="c2">C2.</param>
		/// <param name="t">T.</param>
		public static Vector3 BerzierCurveVectorLerp(Vector3 start, Vector3 end, Vector3 c1, Vector3 c2, float t) {
			float u, uu, uuu, tt, ttt;
			Vector3 p, p0 = start, p1 = c1, p2 = c2, p3 = end;
			u = 1 - t;
			uu = u*u;
			uuu = uu * u;
			tt = t * t;
			ttt = tt * t;
			
			p = uuu * p0; //first term
			p += 3 * uu * t * p1; //second term
			p += 3 * u * tt * p2; //third term
			p += ttt * p3; //fourth term

			return p;
		}

		public static T FindClosest<T>(Vector3 position) where T : Component {
			T returnValue = default(T);
			T[] objects = GameObject.FindObjectsOfType<T> ();
			float minDist = float.MaxValue;
			for (int i = 0; i < objects.Length; i++) {
				float dist = (objects[i].transform.position - position).magnitude;
				if(dist < minDist) {
					returnValue = objects[i];
					minDist = dist;
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Angles the between 2d.
		/// </summary>
		/// <returns>The between2 d.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		public static float AngleBetween2D(Vector2 v1, Vector2 v2) {
			Vector2 diff = v2 - v1;
			return Mathf.Atan2 (diff.y, diff.x) * 180f / Mathf.PI - 90f; 
		}
		
		/// <summary>
		/// Rotations the between2 d.
		/// </summary>
		/// <returns>The between2 d.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		public static Quaternion RotationBetween2D(Vector2 v1, Vector2 v2) {
			return Quaternion.Euler (0f, 0f, AngleBetween2D (v1, v2));
		}
	}
}