using UnityEngine;
using System.Collections;

/// <summary>
/// Basic level manager, used here as a basic manager for loading or quiting screens
/// </summary>

public class LevelManager : MonoBehaviour {

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		Application.LoadLevel (name);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

}
