using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour
{
	void OnCollisionStay(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
			// Visualize the contact point
			Debug.DrawRay(contact.point, contact.normal, Color.yellow);
			Debug.Log("MajkToHytnul");
		}
	}	








	/*
    public float force = 10f;
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			HandleInput();
		}
	}
	void HandleInput()
	{
		/*Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(inputRay, out hit))
		{
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
			if (deformer)
			{
				Vector3 point = hit.point;
				deformer.AddDeformingForce(point, force);
			}
		}

	}*/


}