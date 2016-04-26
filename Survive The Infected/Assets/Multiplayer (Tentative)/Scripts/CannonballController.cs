using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CannonballController : NetworkBehaviour
{
	[SerializeField] float ballLifetime = 4f;	//How long does the ball live
	[SerializeField] bool canKill = false;		//Can the ball damage the players?

	private float age;							//How long has the ball been alive


	void Start ()
	{
		age = 0.0f;	
	}


	//Balls are updated by the server
	[ServerCallback]
	void Update () 
	{	
		//If the ball has been alive too long...
		age += Time.deltaTime;
		if( age > ballLifetime )
		{	
			//...Destroy it on the network
			NetworkServer.Destroy(gameObject);
		}
	}

	//When the cannonball hits something
	void OnCollisionEnter(Collision other)
	{
		//If the cannonball isn't lethal, leave
		if(!canKill)
			return;

		//If this isn't the server, leave. This prevents someone from cheating with a hacked client
		if (!isServer)
			return;

		//If the ball didn't hit a player, leave. We only care about impacts with players
		if (other.gameObject.tag != "Player")
			return;

		//Get a reference to the hit object's Tank Health script and tell it to take a point of damage
		TankHealth health = other.gameObject.GetComponent<TankHealth> ();

		if(health != null)
			health.TakeDamage (1);

		//Destroy this ball on the network so it cannot hit the player multiple times
		NetworkServer.Destroy (gameObject);
	}
}
