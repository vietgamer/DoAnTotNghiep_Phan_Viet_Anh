using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Kéo Canvas Pause vào đây
    public InputActionProperty menuButton; // Nút để mở Menu (thường là nút Menu trên tay trái)

    // Tham chiếu đến tia Ray để bật/tắt khi Pause
    public GameObject leftRay;
    public GameObject rightRay;

    private bool isPaused = false;

    void Update()
    {
        // Kiểm tra nếu người chơi nhấn nút Menu
        if (menuButton.action.WasPressedThisFrame())
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void PauseButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed) KichhoatPause();
    }

    public void KichhoatPause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);

        // Dừng thời gian (Vật lý, NPC, Máy bay sẽ đứng yên)
        Time.timeScale = 0f;

        // Bật tia Ray để tương tác với Menu
        if (leftRay) leftRay.SetActive(true);
        if (rightRay) rightRay.SetActive(true);

        // (Tùy chọn) Tắt tiếng động cơ máy bay nếu đang bay qua
        AudioListener.pause = true;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);

        // Chạy lại thời gian
        Time.timeScale = 1f;

        // Tắt tia Ray nếu bạn muốn người chơi dùng tay cầm Grab thông thường
        // Hoặc giữ nguyên tùy vào thiết kế của bạn

        AudioListener.pause = false;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; // Phải reset lại timeScale trước khi đổi cảnh
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }    
}