using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkGameManager_Camera : NetworkManager
{
	[SerializeField] private GameObject sceneCamera;

	public override void OnStartClient( NetworkClient client ) 
	{
		HideSceneCamera();
	}

	public override void OnStartHost() 
	{
		HideSceneCamera();
	}

	public override void OnStopClient () 
	{
		ShowSceneCamera();
	}

	public override void OnStopHost () 
	{
		ShowSceneCamera();
	}

	private void HideSceneCamera()
	{		
		if(sceneCamera)
			sceneCamera.SetActive(false);
	}

	private void ShowSceneCamera()
	{		
		if(sceneCamera)
			sceneCamera.SetActive(true);
	}
}
