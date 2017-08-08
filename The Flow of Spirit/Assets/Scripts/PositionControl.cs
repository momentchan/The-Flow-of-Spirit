using UnityEngine;
using System.Collections;

public class PositionControl : MonoBehaviour {
	private float distance;

	// Use this for initialization
	void Start () {
		Vector3 toObjectVector = transform.position - Camera.main.transform.position;
		Vector3 linearDistanceVector = Vector3.Project (toObjectVector, Camera.main.transform.forward); 
		distance = linearDistanceVector.magnitude;
	}


	// Update is called once per frame
	void Update () {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = distance;
		transform.position = Camera.main.ScreenToWorldPoint (mousePosition);
	}
}
