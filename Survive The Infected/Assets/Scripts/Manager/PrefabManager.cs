using UnityEngine;
 
/// <summary>
/// Prefab manager for associating instances of newly spawned zombies, which are otherwise lost
/// </summary>

 public class PrefabManager : MonoBehaviour
 {
    // Assign the prefab in the inspector
    public GameObject zombiePrefab;
    //Singleton
    private static PrefabManager m_Instance = null;
    public static PrefabManager Instance
    {
       get
       {
          if (m_Instance == null)
          {
             m_Instance = (PrefabManager)FindObjectOfType(typeof(PrefabManager));
          }
          return m_Instance;
       }
    }
 }