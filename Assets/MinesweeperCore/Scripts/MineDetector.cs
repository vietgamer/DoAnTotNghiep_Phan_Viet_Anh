using UnityEngine;
using UnityEngine.InputSystem; // Bắt buộc phải có cái này

public class MineDetector : MonoBehaviour
{
    // Khai báo Action để nhận tín hiệu từ Controller
    public InputActionProperty triggerAction;

    // Trong MineDetector.cs
    private Tile currentTile;

    public void SetCurrentTile(Tile tile)
    {
        currentTile = tile;
    }

    public void ClearCurrentTile(Tile tile)
    {
        if (currentTile == tile) currentTile = null;
    }

    [Header("Gauge Settings")]
    public Transform needleTransform; // Kéo cái kim vào đây
    public float minAngle = 0f;       // Góc xoay khi có 0 bom
    public float maxAngle = 180f;     // Góc xoay khi có mức bom tối đa (ví dụ 8 bom)
    public int maxMinesToDisplay = 8; // Số bom tối đa trên mặt đồng hồ

    public float smoothSpeed = 5f;    // Độ mượt khi kim xoay
    private float targetAngle;

    [Header("Status Light")]
    public MeshRenderer lightRenderer; // Kéo cái Cube đèn vào đây
    public Color scanModeColor = Color.yellow; // Màu vàng cho chế độ quét
    public Color flagModeColor = Color.red;

    public static MineDetector Instance;
    void Awake() { Instance = this; }


    void Update()
    {
        if (triggerAction.action.WasPressedThisFrame())
        {
            if (currentTile != null)
            {
                currentTile.OnDetectorAction();
            }
            else
            {
                Debug.Log("Vẫn không trúng ô nào!");
            }
        }

        if (triggerAction.action.WasReleasedThisFrame())
        {
            if (currentTile != null)
            {
                currentTile.UnReveal();
            }
        }

        UpdateNeedle();
    }

    // Các hàm Trigger để xác định currentTile
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("detector")) // Hoặc kiểm tra component Tile
        {
            Tile t = other.GetComponent<Tile>();
            if (t != null) currentTile = t;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("detector"))
        {
            currentTile = null;
        }
    }

    void UpdateNeedle()
    {
        if (needleTransform == null) return;

        // Nếu đang đứng trên 1 ô và ô đó đã được lật (hoặc đang soi)
        if (currentTile != null && currentTile.isRevealed)
        {
            if (Tile.currentMode == Tile.InteractionMode.Reveal)
            {
                // Tính toán tỷ lệ góc dựa trên số bom
                float ratio = (float)currentTile.adjacentMines / maxMinesToDisplay;
                targetAngle = Mathf.Lerp(minAngle, maxAngle, ratio);
            }
            else
            {
                if (currentTile.isMine)
                {
                    // Giả sử số 1 trên đồng hồ tương ứng với 1/8 hành trình kim
                    float ratioForFlag = 1f / maxMinesToDisplay;
                    targetAngle = Mathf.Lerp(minAngle, maxAngle, ratioForFlag);
                }
                else
                {
                    targetAngle = minAngle;
                }
            }    

        }
        else
        {
            // Nếu không soi ô nào hoặc nhả cò, kim về 0
            targetAngle = minAngle;
        }

        // Xoay kim mượt mà (dùng trục Z hoặc trục tùy theo hướng model của bạn)
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        needleTransform.localRotation = Quaternion.Slerp(needleTransform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

        if (currentTile != null && currentTile.isMine)
        {
            targetAngle += Random.Range(-2f, 2f);
        }
    }

    public void UpdateStatusLight()
    {
        if (lightRenderer == null) return;

        // Kiểm tra chế độ từ class Tile
        if (Tile.currentMode == Tile.InteractionMode.Reveal)
        {
            lightRenderer.material.color = scanModeColor;
            // Nếu bạn dùng Emission để làm đèn phát sáng:
            lightRenderer.material.SetColor("_EmissionColor", scanModeColor * 2f);
        }
        else
        {
            lightRenderer.material.color = flagModeColor;
            lightRenderer.material.SetColor("_EmissionColor", flagModeColor * 2f);
        }
    }
}