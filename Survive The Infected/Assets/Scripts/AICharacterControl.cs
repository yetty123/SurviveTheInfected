using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target; // target to aim for
		public List<AICharacterControl> OtherZombies;
		public GameObject landingZone;



        // Use this for initialization
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
			OtherZombies = GameObject.FindObjectsOfType<AICharacterControl> ().ToList ().Where (e => e != this).ToList ();


	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        // Update is called once per frame
        private void Update() {
			var zombiesClosetoMe = OtherZombies.Where (z => {
				var d = dist(z.transform.position.x, z.transform.position.z, this.transform.position.x, this.transform.position.z);
				var d2 = dist (target.position.x, target.position.z, z.transform.position.x, z.transform.position.z);
				return (d <= 50 && d2 > 25);
			});

			if (target != null)
            {
				var d = dist (target.position.x, target.position.z, character.transform.position.x, character.transform.position.z);

				// When zombie catches the player
				if (d <= 2) {
					WhenLose ();
				} 

				// When zombie sees the player and chases and lets other zombies in the vicinity converge and swarm
				else if (d <= 25) {
					agent.SetDestination (target.position);
					// use the values to move the character
					character.Move (agent.desiredVelocity, false, false);
					foreach (var z in zombiesClosetoMe) {
						z.agent.SetDestination(target.position);
						z.character.Move(agent.desiredVelocity, false, false);
					}


				} else {

					landingZone = GameObject.FindGameObjectWithTag ("PrefabTag");

					if (landingZone == null) {
						character.Move (Vector3.zero, false, false);
					} else {
						agent.SetDestination (landingZone.transform.position);

						var newVel = agent.desiredVelocity;


						newVel.Set (newVel.x / 2, newVel.y / 2, newVel.z / 2);
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

		void WhenLose() {
			Application.LoadLevel ("Lose Screen");
		}

		double dist(double x, double z, double x2, double z2) {

			return Math.Sqrt ((x - x2) * (x - x2) + (z - z2) * (z - z2));
		}


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
