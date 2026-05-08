using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlagBag : MonoBehaviour
{
    public GameObject flagPrefab; // Kéo Prefab cái cờ vào đây
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    protected void OnEnable()
    {
        // Lắng nghe sự kiện khi có vật thể rời khỏi hoặc đi vào túi
        socket.selectExited.AddListener(OnFlagRemoved);
        socket.selectEntered.AddListener(OnFlagReturned);
    }

    protected void OnDisable()
    {
        socket.selectExited.RemoveListener(OnFlagRemoved);
        socket.selectEntered.RemoveListener(OnFlagReturned);
    }

    // Khi bạn lấy cờ ra khỏi túi
    private void OnFlagRemoved(SelectExitEventArgs args)
    {
        // Tạo lại một cái cờ mới nằm sẵn trong túi để lần sau lấy tiếp
        CreateFlagInBag();
    }

    // Khi bạn bỏ cờ thừa vào túi
    private void OnFlagReturned(SelectEnterEventArgs args)
    {
        // Hủy cái cờ vừa bỏ vào để túi luôn gọn gàng (không bị chồng chất cờ)
        GameObject returnedFlag = args.interactableObject.transform.gameObject;
        Destroy(returnedFlag, 0.1f);
    }

    public void CreateFlagInBag()
    {
        Instantiate(flagPrefab, transform.position, transform.rotation);
    }
}