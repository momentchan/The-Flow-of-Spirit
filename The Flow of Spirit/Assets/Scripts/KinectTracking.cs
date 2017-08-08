using UnityEngine;
using System.Collections;

public class KinectTracking : MonoBehaviour 
{

	//public GUITexture backgroundImage;
	public KinectWrapper.NuiSkeletonPositionIndex firstTrackId = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	public KinectWrapper.NuiSkeletonPositionIndex secondTrackId = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;

	public GameObject firstTrackObject;
	public GameObject secondTrackObject;
	public GameObject firstButterfly;
	public GameObject secondButterfly;

	public float smoothFactor = 5f;
	
	public GUIText debugText;

	private float distanceToCamera = 10f;
	private KinectManager manager;


	void Start()
	{
		if(firstTrackObject)
		{
			distanceToCamera = (firstTrackObject.transform.position - Camera.main.transform.position).magnitude;
		}
	}
	
	void Update() 
	{
		
		manager = KinectManager.Instance;
		
		if(manager && manager.IsInitialized())
		{
			if (manager.IsUserDetected ()) {
				uint userId = manager.GetPlayer1ID ();
				//firstButterfly.GetComponent<firstButterflyMove>().setTarget (firstTrackObject.transform.position);
				//firstButterfly.GetComponent<firstButterflyMove>().moveDistance = false;


				if (manager.IsJointTracked (userId, (int)firstTrackId)) {
					Vector3 posJoint = manager.GetRawSkeletonJointPos (userId, (int)firstTrackId);
					Vector3 firstMapPosition = getPosition (posJoint);

					firstButterfly.GetComponent<ButterflyMove>().setTarget (firstTrackObject.transform.position);
					firstButterfly.GetComponent<ButterflyMove>().roamAround = false;

					if(firstMapPosition!=Vector3.zero)
						firstTrackObject.transform.position = Vector3.Lerp (firstTrackObject.transform.position, firstMapPosition, smoothFactor * Time.deltaTime);
				}

				if (manager.IsJointTracked (userId, (int)secondTrackId)) {
					Vector3 posJoint = manager.GetRawSkeletonJointPos (userId, (int)secondTrackId);
					Vector3 secondMapPosition = getPosition (posJoint);

					secondButterfly.GetComponent<ButterflyMove>().setTarget (secondTrackObject.transform.position);
					secondButterfly.GetComponent<ButterflyMove>().roamAround = false;

					if(secondMapPosition!=Vector3.zero)
						secondTrackObject.transform.position = Vector3.Lerp (secondTrackObject.transform.position, secondMapPosition, smoothFactor * Time.deltaTime);
				}
					
			} else {
				firstButterfly.GetComponent<ButterflyMove>().roamAround = true;
				secondButterfly.GetComponent<ButterflyMove>().roamAround = true;
			}
			
		}
	}

	Vector3 getPosition(Vector3 posJoint){
		if (posJoint != Vector3.zero) {
			// 3d position to depth
			Vector2 posDepth = manager.GetDepthMapPosForJointPos (posJoint);

			// depth pos to color pos
			Vector2 posColor = manager.GetColorMapPosForDepthPos (posDepth);

			float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
			float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;

			Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint (new Vector3 (scaleX, scaleY, distanceToCamera));
			return vPosOverlay;
		}
		return Vector3.zero;
	}
}