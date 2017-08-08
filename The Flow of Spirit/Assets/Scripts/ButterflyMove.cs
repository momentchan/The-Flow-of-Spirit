using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyMove : MonoBehaviour {

	public GameObject flower;
	public bool roamAround = true;

	private Vector3 targetPosition;
	private float moveSpeed = 1f;
	private float moveDistance = 20f;
	private float reachThreshold = 1.0f;


	void Start () 
	{
		targetPosition = transform.position;	
	}
	
	void Update () {
		// Randomly move around
		if (roamAround) 
		{
			
			// Set visibility
			gameObject.layer = 0;
			flower.SetActive (false);


			// Generate new random target position for butterfly
			if (reachTarget ()) 
			{
				// Assure the butterfly with in the sight
				while (true) 
				{
					Vector3 displace = Random.insideUnitCircle * moveDistance;
					setTarget (new Vector3 (transform.position.x + displace.x, transform.position.y + displace.y, flower.transform.position.z + Random.Range(-2,2)));
					if (targetPosition.x < 40 && targetPosition.x > -40 && targetPosition.y < 15 && targetPosition.y > -15)
						break;
				}
			}
		} 
		else 
		{ 
			if (reachTarget ()) {
				print ("here");
				gameObject.layer = 1;
				flower.SetActive(true);
			}
		}

		transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
	}

	public bool reachTarget()
	{
		float distance = Vector3.Distance (targetPosition, transform.position);
		if (distance < reachThreshold)
			return true;
		
		return false;
	}

	public void setTarget(Vector3 target){
		targetPosition = target;
	}
}
