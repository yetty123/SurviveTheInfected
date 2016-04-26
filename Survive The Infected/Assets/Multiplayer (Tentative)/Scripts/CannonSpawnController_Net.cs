using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CannonSpawnController_Net : NetworkBehaviour
{
	[SerializeField] float power = 800f;		//How hard to shoot the cannonballs
	[SerializeField] GameObject cannonBall;


	private Transform playerCanon;				//Where should the cannonballs shoot from


	void Start ()
	{
		//Get the location of the cannonball spawingpoint
		playerCanon = transform.FindChild("CannonBallSpawnPoint");
	}
		

	void Update ()
	{
		//If this isn't the local player, leave. Since tanks are local authorities they 
		//should be responsible for updating themselves
		if (!isLocalPlayer)
			return;
		
		//If we click the mouse or hit the spacebar...
		if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
		{
			//Run the server Command to spawn a cannonball
			CmdSpawnCannonball();
		}

	}

	//This Command is called from the localPlayer and run on the server. Note that Commands must begin
	//with Cmd
	[Command]
	void CmdSpawnCannonball ()
	{
		//we instantiate a cannonball from Resources
		GameObject instance = Instantiate (cannonBall) as GameObject; 
		//Let's name it
		instance.name = "Cannonball";
		//Let's position it at the player
		instance.transform.position = playerCanon.position;
		//Let's send it forward
		instance.GetComponent<Rigidbody> ().AddForce (playerCanon.forward * power);

		//Finally, let's instantiate this object on the network for all players to see
		NetworkServer.Spawn (instance);
	}
}