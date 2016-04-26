using UnityEngine;
using System.Collections;

/// <summary>
/// Script for not allowing manager to get deleted on changing the screen
/// </summary>

public class DontDestroyOnLoad : MonoBehaviour {

	public static DontDestroyOnLoad Instance;

	void Awake () {
		if (!Instance) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			DestroyImmediate (gameObject);
		}
	}

} 