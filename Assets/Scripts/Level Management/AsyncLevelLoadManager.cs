using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLevelLoadManager : MonoBehaviour
{
    public int[] scenes;
    private void Awake()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
        }
    }
}