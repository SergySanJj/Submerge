using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public static GameLoader instance;
    public GameObject loadingScreen;
    public Animator transition;

    void Awake()
    {
        if (instance==null)
            instance = this;
    }

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadScene(int sceneId)
    {
        StartCoroutine(TransitedLoading(sceneId));
    }

    private IEnumerator TransitedLoading(int sceneId)
    {
        transition.SetTrigger("ShowLoadingScreen");
        float waitingTime = 2.0f;
        yield return new WaitForSeconds(waitingTime);
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneId));
        StartCoroutine(GetScenesProgress());
    }

    public IEnumerator GetScenesProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }

        scenesLoading.Clear();
        //transition.SetTrigger("FinishLoading");
    }
}
