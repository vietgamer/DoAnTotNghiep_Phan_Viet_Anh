using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameController : MonoBehaviour
{
    public AudioSource playerVoice;
    public AudioClip winClip;
    public AudioClip loseClip;
    public CanvasGroup fadeScreen;
    public MeshRenderer fadeOverlay; // Tấm màn đen trước mắt người chơi

    private bool isEnding = false;

    void Update()
    {
        if (isEnding) return;

        // Lấy thời gian thực tế từ đồng hồ đeo tay
        float gameTime = dongho.timeRemaining;

        // 1. Kiểm tra thắng: Cắm đủ cờ
        if (BoardManager.Instance.CorrectFlagsCount >= BoardManager.Instance.mineCount)
        {
            StartCoroutine(EndRoutine(true));
        }

        // 2. Kiểm tra thua: Đồng hồ vượt quá giới hạn
        if (gameTime <= 0)
        {
            StartCoroutine(EndRoutine(false));
        }
    }

    IEnumerator EndRoutine(bool isWin)
    {
        isEnding = true;

        // Dừng đồng hồ đeo tay lại ngay lập tức
        FindObjectOfType<dongho>().timerIsRunning = false;

        Time.timeScale = 0f;

        if (playerVoice != null)
            playerVoice.PlayOneShot(isWin ? winClip : loseClip);

        yield return new WaitForSecondsRealtime(3f);

        // Hiệu ứng đen màn hình
        float fadeDuration = 2f;
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.unscaledDeltaTime;
            if (fadeScreen != null) fadeScreen.alpha = fadeTimer / fadeDuration;
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(isWin ? "Good Ending" : "Bad Ending");
    }
}