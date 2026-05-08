using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VRBombExplosion : MonoBehaviour
{
    [Header("Settings")]
    public GameObject whiteSphere;
    public CanvasGroup fadeScreen;
    public float expandDuration = 5f;
    public float shakeMagnitude = 0.1f;
    public AudioSource explosource;
    public AudioClip exposound;

    [Header("References")]
    public Transform cameraOffset; // Kéo 'Camera Offset' của XR Origin vào đây

    private bool isExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra xem vật va chạm vào mặt đất có phải là Bom không
        if (collision.gameObject.CompareTag("boom"))
        {
            // Lấy vị trí va chạm để đặt quả cầu trắng
            Vector3 explosionPos = collision.contacts[0].point;

            // Chạy Coroutine hiệu ứng
            StartCoroutine(ExplosionSequence(explosionPos));

            // Xóa quả bom đi để tránh va chạm tiếp
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ExplosionSequence(Vector3 position)
    {
        isExploded = true;
        whiteSphere.transform.position = position + Vector3.up * 0.5f;
        whiteSphere.SetActive(true);

        // 1. Kích hoạt và đưa quả cầu lên trên mặt đất một chút
/*       whiteSphere.transform.position = transform.position + Vector3.up * 1.0f; */
 //       whiteSphere.transform.localScale = Vector3.one * 0.0001f;
        whiteSphere.SetActive(true);

        float elapsed = 0;
        Vector3 originalCameraPos = cameraOffset.localPosition;

        while (elapsed < expandDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / expandDuration;

            explosource.PlayOneShot(exposound);

            // 2. Rung Camera (Dịch chuyển nhẹ Camera Offset)
            if (t < 5f) // Rung mạnh lúc đầu, sau đó giảm dần
            {
                cameraOffset.localPosition = originalCameraPos + Random.insideUnitSphere * shakeMagnitude;
            }
            else
            {
                cameraOffset.localPosition = originalCameraPos;
            }

            // 3. Quả cầu to dần (Dùng animation curve hoặc Lerp)
 //           whiteSphere.transform.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one * 200f, t);

            // 4. Màn hình tối dần
            if (t > 0.6f)
            {
                fadeScreen.alpha = 1.0f;
            }

            yield return null;
        }

        cameraOffset.localPosition = originalCameraPos;
        SceneManager.LoadScene("GameOver");
    }
}