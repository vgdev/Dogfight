using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ProjectileBoundary : MonoBehaviour 
{
	[SerializeField]
	private string tagFilter;

	void Awake()
	{
		if(tagFilter == null)
			tagFilter = "";
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Entered");
		if(other.CompareTag(tagFilter))
		{
			Projectile proj = other.GetComponent<Projectile>();
			if(proj != null)
			{
				proj.Active = false;
			}
		}
	}
}
