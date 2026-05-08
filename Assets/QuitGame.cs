using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ExitProject()
    {

        // Thoát khi chơi bản Build chính thức (Quest 2, PC)
        Application.Quit();

    }
}
