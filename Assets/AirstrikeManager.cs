using UnityEngine;
using System.Collections;

public class AirstrikeManager : MonoBehaviour
{
    public GameObject airplane;
    public Transform startPoint;
    public Transform endPoint;
    public AudioSource radioSource;
    public AudioClip WarningClip;
    public AudioClip FallingClip;
    public GameObject bombPrefab;
    public Transform playerTransform;
    public float thoigianroi;

    public bool isPlayerHidden = false;
    private bool isAirstrikeActive = false;

    void Start()
    {
        // Cứ sau 120s (2 phút) sẽ gọi máy bay một lần
        InvokeRepeating("CallAirstrike", 60f, 120f);
    }

    public void CallAirstrike()
    {
        if (!isAirstrikeActive)
            StartCoroutine(AirstrikeRoutine());
    }

    IEnumerator AirstrikeRoutine()
    {
        isAirstrikeActive = true;

        // 1. Cảnh báo 5s
        if (radioSource != null) radioSource.PlayOneShot(WarningClip);
        yield return new WaitForSeconds(5f);

        // 2. Máy bay bắt đầu bay
        airplane.SetActive(true);
        float duration = 20f;
        float elapsed = 0f;
        bool hasDroppedBomb = false; // Biến kiểm soát để chỉ thả 1 lần hoặc theo đợt

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            airplane.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, percent);

            // 3. Logic thả bom khi máy bay bay đến giữa đường (hoặc khi gần người chơi)
            // Ví dụ: Thả bom khi máy bay bay được 10 giây (giữa map) và người chơi không trốn
            if (elapsed >= thoigianroi && !hasDroppedBomb)
            {
                if (!isPlayerHidden)
                {
                    DropBombFromPlane();
                }
                hasDroppedBomb = true;
            }

            yield return null;
        }

        airplane.SetActive(false);
        isAirstrikeActive = false;
    }

    void DropBombFromPlane()
    {
        radioSource.PlayOneShot(FallingClip);
        Debug.Log("Máy bay đang xả bom!");
        // Tạo bom tại chính vị trí hiện tại của máy bay
        GameObject bomb = Instantiate(bombPrefab, airplane.transform.position, Quaternion.identity);

        // Nếu quả bom có Rigidbody, nó sẽ tự rơi xuống do trọng lực
        // Bạn có thể thêm một chút lực tiến để bom rơi theo đường chéo cho thật
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Lấy vận tốc hướng bay của máy bay gán cho bom
            Vector3 flyDirection = (endPoint.position - startPoint.position).normalized;
            rb.linearVelocity = flyDirection * 10f;
        }
    }
}