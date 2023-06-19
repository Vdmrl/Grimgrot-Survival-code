using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunScene : MonoBehaviour
{
    public string sceneName;
    public void Scene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
