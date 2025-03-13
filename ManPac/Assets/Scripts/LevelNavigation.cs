using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelNavigation
{
    private static Stack<int> _sceneStack = new Stack<int>();
    
    public static AsyncOperation LoadScene(int bundleIndex, LoadSceneParameters loadParameters = new LoadSceneParameters())
    {
        _sceneStack.Push(bundleIndex);
        return SceneManager.LoadSceneAsync(bundleIndex, loadParameters);
    }

    public static AsyncOperation LoadScene(string sceneName, LoadSceneParameters loadParameters = new LoadSceneParameters())
    {
        _sceneStack.Push(SceneUtility.GetBuildIndexByScenePath(sceneName));
        return SceneManager.LoadSceneAsync(sceneName, loadParameters);
    }
    
    public static Scene LoadSceneSync(int bundleIndex, LoadSceneParameters loadParameters = new LoadSceneParameters())
    {
        _sceneStack.Push(bundleIndex);
        return SceneManager.LoadScene(bundleIndex, loadParameters);
    }

    public static Scene LoadSceneSync(string sceneName, LoadSceneParameters loadParameters = new LoadSceneParameters())
    {
        _sceneStack.Push(SceneUtility.GetBuildIndexByScenePath(sceneName));
        return SceneManager.LoadScene(sceneName, loadParameters);
    }

    /// <summary>
    /// Pops the current index of the scene and returns it
    /// </summary>
    /// <returns></returns>
    public static int GetCurrentScene()
    {
        if (!_sceneStack.TryPop(out int index))
            return -1;

        return index;
    }

    public static int GetPreviousScene()
    {
        if (!_sceneStack.TryPop(out _))
            return -1;
        
        return GetCurrentScene();
    }
}
