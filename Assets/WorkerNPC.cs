using UnityEngine;
using UnityEngine.AI;

public class WorkerNPC : MonoBehaviour
{
    public enum NPCState { Working, GoingToHide, Hiding }
    public NPCState currentState = NPCState.Working;

    public Transform pointA, pointB, pointC;
    public Animator anim;
    public NavMeshAgent agent;
    public AudioSource voiceSource;
    public AudioClip[] randomVoices;

    public AirstrikeManager airstrikeManager;

    private bool carryingItem = false;
    private float voiceTimer = 0f;

    // Biến đếm thời gian núp
    private float hidingTimer = 0f;
    private const float TIME_TO_HIDE_AFTER_AIRSTRIKE = 10f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToPointA();
    }

    void Update()
    {
        // Kiểm tra tiếng báo động từ AirstrikeManager
        bool isWarning = airstrikeManager.radioSource.isPlaying;

        // QUAN TRỌNG: Nếu đang làm việc mà thấy báo động -> Đi trốn ngay (Áp dụng cho mọi lần)
        if (isWarning && currentState == NPCState.Working)
        {
            GoToHide();
            return; // Thoát nhanh để thực hiện trạng thái mới
        }

        // Quản lý các trạng thái cụ thể
        switch (currentState)
        {
            case NPCState.Working:
                DoWorkLogic();
                break;

            case NPCState.GoingToHide:
                // Khi đang chạy đi trốn, nếu đến nơi thì bắt đầu Núp
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    StartHiding();
                }
                break;

            case NPCState.Hiding:
                // Nếu máy bay vẫn đang báo động thì reset timer liên tục
                if (isWarning)
                {
                    hidingTimer = 0f;
                }
                else
                {
                    // Chỉ khi HẾT báo động mới bắt đầu đếm 10s
                    hidingTimer += Time.deltaTime;
                    if (hidingTimer >= TIME_TO_HIDE_AFTER_AIRSTRIKE)
                    {
                        ResumeWork();
                        hidingTimer = 0f;
                    }
                }
                break;
        }

        // Logic âm thanh ngẫu nhiên
        if (currentState == NPCState.Working)
        {
            voiceTimer += Time.deltaTime;
            if (voiceTimer > Random.Range(10, 20))
            {
                PlayRandomVoice();
                voiceTimer = 0;
            }
        }
    }

    void DoWorkLogic()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (carryingItem) GoToPointA(); else GoToPointB();
        }
    }

    void GoToPointA()
    {
        carryingItem = false;
        agent.destination = pointA.position;
        agent.speed = 2f;
        anim.SetBool("isCarrying", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isHiding", false);
    }

    void GoToPointB()
    {
        carryingItem = true;
        agent.destination = pointB.position;
        agent.speed = 2f;
        anim.SetBool("isCarrying", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isHiding", false);
    }

    void GoToHide()
    {
        currentState = NPCState.GoingToHide;
        agent.destination = pointC.position;
        agent.speed = 5f; // Chạy nhanh
        anim.SetBool("isRunning", true);
        anim.SetBool("isCarrying", false);
        anim.SetBool("isHiding", false);
    }

    void StartHiding()
    {
        currentState = NPCState.Hiding;
        anim.SetBool("isHiding", true); // Bật Animation núp
        anim.SetBool("isRunning", false);
    }

    void ResumeWork()
    {
        currentState = NPCState.Working;
        GoToPointA();
    }

    void PlayRandomVoice()
    {
        if (randomVoices.Length > 0 && !voiceSource.isPlaying)
            voiceSource.PlayOneShot(randomVoices[Random.Range(0, randomVoices.Length)]);
    }
}