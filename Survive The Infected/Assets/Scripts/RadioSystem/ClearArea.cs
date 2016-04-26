using UnityEngine;
using System.Collections;

/// <summary>
/// Script for finding clear area and scripting narration (PUBLISH-SUBSCRIBE MODEL)
/// </summary>

public class ClearArea : MonoBehaviour {

	public float timeSinceLastTrigger = 0f;

	private bool foundClearArea = false;

	void Update () {
		timeSinceLastTrigger += Time.deltaTime;

		if (timeSinceLastTrigger > 1f && Time.realtimeSinceStartup > 10f && !foundClearArea) {
			SendMessageUpwards ("OnFindClearArea");
			foundClearArea = true;
		}
	}

	void OnTriggerStay (Collider collider) {
		if (collider.tag != "Player") {
			timeSinceLastTrigger = 0f;
		}
	}
}
