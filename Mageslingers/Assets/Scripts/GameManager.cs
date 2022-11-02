using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public Action OnSceneLoaded;
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;

        // TEMP
        scenesLoading.Add(SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        // Keep track of the current progress of the scene. Unity usually stops at .9 before jumping to 1
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                //Debug.Log(scenesLoading[i].progress);
                yield return null;
            }
        }
        scenesLoading.Clear();

        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        OnSceneLoaded?.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void SceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        SceneManager.SetActiveScene(scene);

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
