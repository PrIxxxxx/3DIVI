using UnityEngine;
using UnityEngine.AI;
public class enemyAiPatrole : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet && agent.isOnNavMesh)
            agent.SetDestination(destPoint);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
            walkPointSet = false;

        if (agent.velocity.magnitude < 0.05f && walkPointSet && !agent.pathPending)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = transform.position + new Vector3(
                Random.Range(-range, range),
                0,
                Random.Range(-range, range)
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();

                if (agent.CalculatePath(hit.position, path) &&
                    path.status == NavMeshPathStatus.PathComplete)
                {
                    destPoint = hit.position;
                    walkPointSet = true;
                    return;
                }
            }
        }
    }
}
