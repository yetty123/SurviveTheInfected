using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using UnityEngine.Networking;

public class ObjectMoverRB_Net :  NetworkBehaviour
{
	private float moveHorizontal;			//Variables to construct the movement and rotation values
	private float moveVertical;

	private Rigidbody localRigidBody;		//Cache the reference to the rigidbody

	public float translationSpeed = 5.0f;	//The ammount of unity the object will move at

	public float rotationSpeed = 45.0f;		//In euler angles, the speed of the roation

	//Network syncvar

	[SyncVar]
	public Color color;


	void Start()
	{
		if (isLocalPlayer) 
		{
			//Get the reference to the object's rigidbody since we will be using it a lot
			localRigidBody = GetComponent<Rigidbody> ();
		}


		//if this isn't the localPlayer, remove all of the interactive bits. Non-local players are
		//effectively "dumb players" and they get all of their data from the network. They won't need 
		//to do any local calculations


		if( !isLocalPlayer )
		{
			Transform playerCameraTransform = transform.FindChild("Camera");
			Camera playerCamera = playerCameraTransform.GetComponent<Camera>();
			AudioListener playerAudioListener = playerCameraTransform.GetComponent<AudioListener>();
			if(playerCamera)
			{
				playerCamera.enabled = false;
			}
			if(playerAudioListener)
			{
				playerAudioListener.enabled = false;
			}
		}

	


		//Set the color of the tank. The color has a hook to the script LobbyHookCarl because the color is synchvariable

	
		Renderer[] rends = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in rends)
		r.material.color = color;
	

	}

	void FixedUpdate()
	{

		if (isLocalPlayer) {
			//Get the horizontal and vertical input. We do that differently depending on the platform
			#if UNITY_IOS || UNITY_ANDROID || UNITY_WSA_10_0
			moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
			moveVertical = CrossPlatformInputManager.GetAxis ("Vertical"); 
			#else
		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");
			#endif

			//Calculate and apply the new translation
			Vector3 deltaTranslation = transform.position + transform.forward * translationSpeed * moveVertical * Time.deltaTime;
			localRigidBody.MovePosition (deltaTranslation);

			//Calculate and apply the new rotation
			Quaternion deltaRotation = Quaternion.Euler (rotationSpeed * new Vector3 (0, moveHorizontal, 0) * Time.deltaTime);
			localRigidBody.MoveRotation (localRigidBody.rotation * deltaRotation);
		}
	}
}
