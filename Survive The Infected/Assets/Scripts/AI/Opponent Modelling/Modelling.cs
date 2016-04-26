using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Script to handle opponent modelling by registering frequently visited areas and spawning zombies in the region
/// </summary>

public class Modelling : MonoBehaviour {

	private int value = 0; // counter of how many times dictionary has been checked
	private int addCount = 0; // counter of how many times dictionary has been updated
	private Dictionary<Vector2, int> d = new Dictionary<Vector2, int>(); // a Dictionary of keys which save frequency of regions
	private GameObject player; // the player around which opponent modelling has to be done

	void Start () {
		// initialize the player by finding it with the tag
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {

		// increment the count of dictionary checked
		value++;

		// store player's position
		var position = player.transform.position;

		// transpose player coordinates to a plane from (0,0) to (1000,1000) for ease of dividing into regions
		var x = position.x + 500;
		var y = position.z + 500;

		// approxmite the region to which current x-coordinate lies in
		x = x / 100;
		x = (float) ((int)x);
		x *= 100;

		// approxmite the region to which current y-coordinate lies in
		y = y / 100;
		y = (float)((int)y);
		y *= 100;

		// temporary 2D vector to store a region 
		Vector2 v = new Vector2 (x, y);

		// boolean logic which adds new regions to dictionary if it isn't already added
		if (d.Keys.ToList ().Any (k => k.x == v.x && k.y == v.y)) {
			var key = d.Keys.First (k => k.x == v.x && k.y == v.y);
			d [key]++;
		} 
		else {
			d.Add (v, 1);
			addCount++;
		}

		// spawn zombies if a modelling has been updated for 250 refreshes OR after 5 regions are added
		if (value >= 250 || addCount >= 5) {
			
			// reset modelling data
			value = 0;
			addCount = 0;

			// temporary variables for finding maximum region
			int max = -1;
			Vector2 maxValue = new Vector2();

			// fInd the Vector with max count from dictionary
			for (int i = 0; i < d.Keys.Count; i++) {
				if (d [d.Keys.ToList () [i]] > max) {
					maxValue = d.Keys.ToList () [i];
					max = d [d.Keys.ToList () [i]];
				}
			}

			// new location at which zombie will be spawned based on opponent modelling by transposing back
			Vector3 newLoc = new Vector3 (maxValue.x - 500, 50, maxValue.y - 500);

			// use opponent modelling to spawn zombies in most frequently visited regions
			Instantiate (PrefabManager.Instance.zombiePrefab, newLoc, player.transform.rotation);
			print ("Zombie spawned!");
		}

		// print frequency list (for testing)
		d.Keys.ToList ().ForEach (k => print (k + " - " + d [k]));

	}
}
