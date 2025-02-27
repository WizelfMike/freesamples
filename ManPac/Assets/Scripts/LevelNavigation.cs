using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelNavigation
{
    public static AsyncOperation LoadScene(int bundleIndex, LoadSceneParameters loadParameters = new LoadSceneParameters())
    {
        return SceneManager.LoadSceneAsync(bundleIndex, loadParameters);
    }

    public static AsyncOperation LoadScene(string sceneName, LoadSceneParameters loadParameters = new LoadSceneParameters())
    {
        return SceneManager.LoadSceneAsync(sceneName, loadParameters);
    }
}
