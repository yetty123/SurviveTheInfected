using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Program to implement Finite State Machine behavior for zombies and the swarming algorithm. This is loosely couplied with Radio 
/// System and Opponent Modelling
/// </summary> 

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		private Transform target; // target to aim for

		private List<AICharacterControl> OtherZombies; // the other zombies that we will be giving information to
		private GameObject landingZone; // the landing zone where the flare will be thrown

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
			OtherZombies = GameObject.FindObjectsOfType<AICharacterControl> ().ToList ().Where (e => e != this).ToList ();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }
			
        private void Update() {

			// update the player's current transform for updating location of target
			target = GameObject.FindGameObjectWithTag ("Player").transform;

			// initialize zombies other than this one to give swarming information
			var zombiesClosetoMe = OtherZombies.Where (z => {
				var d1 = dist(z.transform.position.x, z.transform.position.z, this.transform.position.x, this.transform.position.z);
				var d2 = dist (target.position.x, target.position.z, z.transform.position.x, z.transform.position.z);
				return (d1 <= 50 && d2 > 25);
			});

			// basic if-else statements for FINITE STATE MACHINE for zombie behavior
			if (target != null)
            {
				// distance from this zombie to the player
				var d = dist (target.position.x, target.position.z, character.transform.position.x, character.transform.position.z);

				// When zombie catches the player
				if (d <= 1) {
					WhenLose ();
				} 

				// When zombie sees the player and chases and lets other zombies in the vicinity converge and implement SWARMING ALGORITHM
				else if (d <= 25) {
					agent.SetDestination (target.position);
					// use the values to move the character
					character.Move (agent.desiredVelocity, false, false);
					foreach (var z in zombiesClosetoMe) {
						z.agent.SetDestination(target.position);
						z.character.Move(agent.desiredVelocity, false, false);
					}


				} 

				// When zombie is not chasing the player
				else {

					// find the landing zone (and the flare) if it exists
					landingZone = GameObject.FindGameObjectWithTag ("LandingZone");

					// if flare is not found, then zombie will stay idle
					if (!landingZone) {
						character.Move (Vector3.zero, false, false);
					} 

					// if flare is found, the zombies will be alerted and move towards the flare

					else {
						agent.SetDestination (landingZone.transform.position);
						var newVel = new Vector3 (agent.desiredVelocity.x * (float) 0.75, agent.desiredVelocity.y * (float) 0.75, agent.desiredVelocity.z * (float) 0.75);
						character.Move (newVel, false, false);
					}
				}
                
            }
            else
            {
                // We still need to call the character's move function, but we send zeroed input as the move param.
                character.Move(Vector3.zero, false, false);
            }

        }

		// when the player loses, the lose screen is loaded
		void WhenLose() {
			Application.LoadLevel ("Lose Screen");
		}

		// distance formula between two points
		double dist(double x, double z, double x2, double z2) {

			return Math.Sqrt ((x - x2) * (x - x2) + (z - z2) * (z - z2));
		}

		// setting target to itself (for testing)
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}