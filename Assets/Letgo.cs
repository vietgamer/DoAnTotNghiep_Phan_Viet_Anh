using UnityEngine;

public class Letgo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartSelectedLevel()
    {
        if (!string.IsNullOrEmpty(LevelPoint.selectedScene))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(LevelPoint.selectedScene);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
