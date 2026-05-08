using UnityEngine;
using UnityEngine.AI;

public class PeopleAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform targetB;

    public void Setup(Transform target)
    {
        agent = GetComponent<NavMeshAgent>();
        targetB = target;
        agent.SetDestination(targetB.position);
    }

    void Update()
    {
        // Kiểm tra nếu đã đến gần khu vực B
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Thay vì Destroy, ta nên trả về Pool (nếu dùng Object Pooling)
            // Ở đây tôi dùng Destroy cho đơn giản lúc đầu
            Destroy(gameObject);
        }
    }
}