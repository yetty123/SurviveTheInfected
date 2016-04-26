using UnityEngine;
using System.Collections;

/// <summary>
/// Script for inner voice of the player and scripting narration (FSM using PUBLISH-SUBSCRIBE MODEL)
/// </summary>

public class InnerVoice : MonoBehaviour {

	public AudioClip whatHappened;
	public AudioClip goodLandingArea;

	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = whatHappened;
		audioSource.Play ();
	}

	// when clear area is found
	void OnFindClearArea () {
		print (name + " OnFindClearArea");
		audioSource.clip = goodLandingArea;
		audioSource.Play ();

		Invoke ("CallHeli", goodLandingArea.length + 1f);
	}

	// when helicopter is called
	void CallHeli () {
		SendMessageUpwards ("OnMakeInitialHeliCall");
	}

}