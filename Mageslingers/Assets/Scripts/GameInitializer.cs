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
       
        if(AutoHostOnPlay && GameManager.instance)
        {
            GameObject g = Instantiate(NetworkObjectPrefab);
            //g.GetComponent<GameNetworkManager>().onlineScene = SceneManager.GetActiveScene().name;
            //SceneManager.LoadScene(OnlineScene, LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(OnlineScene));
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
