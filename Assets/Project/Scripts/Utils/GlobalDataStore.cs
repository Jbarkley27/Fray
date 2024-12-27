using UnityEngine;

public class GlobalDataStore : MonoBehaviour 
{
    public static GlobalDataStore instance { get; private set; }

    [Header("Player")]
    public Transform player;
    public Transform projectileSource;


    [Header("Parents")]
    public Transform skillParent;

    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an GlobalDataReference object, destroying new one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}