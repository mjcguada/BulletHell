using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool loadOnAwake = false;
    [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Additive;
    [SerializeField] protected SceneCollection sceneCollection;

    private void Awake()
    {
        if (loadOnAwake) LoadScenes();
    }

    public virtual void LoadScenes()
    {
        List<string> loadedScenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedScenes.Add(SceneManager.GetSceneAt(i).name);
        }

        for (int i = 0; i < sceneCollection.scenes.Length; i++)
        {
            string currentScene = sceneCollection.scenes[i];
            if (!string.IsNullOrEmpty(currentScene) && !loadedScenes.Contains(currentScene)) ;
            {
                SceneManager.LoadScene(currentScene, loadMode);
                loadedScenes.Add(currentScene);
            }
        }
    }

    public void LoadScenesAsync()
    {
        List<string> loadedScenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedScenes.Add(SceneManager.GetSceneAt(i).name);
        }

        for (int i = 0; i < sceneCollection.scenes.Length; i++)
        {
            string currentScene = sceneCollection.scenes[i];
            if (!string.IsNullOrEmpty(currentScene) && !loadedScenes.Contains(currentScene)) ;
            {
                SceneManager.LoadSceneAsync(currentScene, loadMode);
                loadedScenes.Add(currentScene);
            }
        }
    }
}
