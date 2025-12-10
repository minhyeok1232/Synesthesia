using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChangeScene(int _sceneNum)
    {
        SceneManager.LoadScene(_sceneNum);
    }
}
