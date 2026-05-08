using UnityEngine;
using UnityEngine.InputSystem; // Cần thư viện này

public class VRInputPauseMenu : MonoBehaviour
{
    public InputActionProperty primaryButtonAction; // Gán nút A vào đây trong Inspector
    public GameObject PauseUI;

    void Update()
    {
        // Kiểm tra nếu nút A vừa được nhấn xuống
        if (primaryButtonAction.action.WasPressedThisFrame())
        {
            Debug.Log("đã kích hoạt");
            PauseUI.SetActive(true);
            // Bạn có thể thêm âm thanh "tách" hoặc rung tay cầm ở đây để báo hiệu đã đổi mode
        }
    }

    void OnEnable()
    {
        primaryButtonAction.action.Enable();
    }

    void OnDisable()
    {
        primaryButtonAction.action.Disable();
    }
}