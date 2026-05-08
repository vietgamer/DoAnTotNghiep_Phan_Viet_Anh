using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Tên của Scene bạn muốn chuyển đến
    public string sceneName;

    public void ChangeScene()
    {
        // Load scene mới
        SceneManager.LoadScene(sceneName);
    }
}