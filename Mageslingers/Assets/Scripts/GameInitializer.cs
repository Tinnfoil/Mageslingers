using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    public bool AutoHostOnPlay = true;
    public string OnlineScene = "OnlineScene";
    public GameObject NetworkObjectPrefab;

    private void Awake()
    {
       
        if(AutoHostOnPlay && GameManager.instance == null)
        {
            Instantiate(NetworkObjectPrefab);
            SceneManager.LoadScene(OnlineScene, LoadSceneMode.Additive);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
