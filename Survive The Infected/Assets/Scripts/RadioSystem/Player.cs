using UnityEngine;
using System.Collections;

/// <summary>
/// Program to implement the First Person Controller of the player
/// </summary>

public class Player : MonoBehaviour {

	public Transform playerSpawnPoints; // the parent of the spawn points
	public GameObject landingAreaPrefab; // the landing area prefab needed for instantiating it

	private bool reSpawn = false; // the boolean which tells if spawn was done 
	private Transform[] spawnPoints;
	private bool lastRespawnToggle = false; // toggle for respawning

	// initialize spawnpoints and call respawn to spawn at a random spawn point
	void Start () {
		spawnPoints = playerSpawnPoints.GetComponentsInChildren<Transform> ();
		Respawn ();
	}
	
	// player who has access to project files can use Inspector and click on the toggle to see this work
	void Update () {
		if (lastRespawnToggle != reSpawn) {
			Respawn ();
			reSpawn = false;
		} else {
			lastRespawnToggle = reSpawn;
		}
	}

	// respawn logic which uses randomizer to respawn at one of the spawn points
	private void Respawn() {
		int i = Random.Range (1, spawnPoints.Length);
		transform.position = spawnPoints [i].transform.position;
	}

	// invokes the flare function when clear area is found for the helicopter
	void OnFindClearArea () {
		Invoke ("DropFlare", 3f);
	}

	// instantiates a landing area with the flare for the helicopter
	void DropFlare () {
		Instantiate (landingAreaPrefab, transform.position, transform.rotation);
	}

}