using UnityEngine;
using UnityEngine.AI;
public class enemyAiPatrole : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    Animator animator;

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    [SerializeField] float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;

    [SerializeField] float timeBetweenAttacks = 1.5f;
    bool alreadyAttacked;

    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = CanSeePlayer();

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        playerInAttackRange = distanceToPlayer <= attackRange;

        if (!playerInAttackRange && !playerInSightRange) Patrol();
        if (!playerInAttackRange && playerInSightRange) Chase();
        if (playerInAttackRange && playerInSightRange) Attack();
    }
    void Attack()
    {
        agent.SetDestination(transform.position);
        agent.velocity = Vector3.zero;

        LookAtPlayer();

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    void Chase()
    {
        agent.SetDestination(player.transform.position);
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

    void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 8f
        );
    }

    [SerializeField] LayerMask wallLayer;

    bool CanSeePlayer()
    {
        CharacterController playerController = player.GetComponent<CharacterController>();

        Vector3 rayStart = transform.position + Vector3.up * 1.6f;

        Vector3 rayEnd;

        if (playerController != null)
        {
            rayEnd = player.transform.position
                     + playerController.center
                     + Vector3.up * (playerController.height * 0.25f);
        }
        else
        {
            rayEnd = player.transform.position + Vector3.up * 1.5f;
        }

        float distanceToPlayer = Vector3.Distance(rayStart, rayEnd);

        if (distanceToPlayer > sightRange)
            return false;

        Vector3 directionToPlayer = (rayEnd - rayStart).normalized;

        if (Physics.Raycast(rayStart, directionToPlayer, out RaycastHit hit, distanceToPlayer))
        {
            Debug.DrawRay(rayStart, directionToPlayer * distanceToPlayer, Color.red);

            if (hit.transform.root.CompareTag("Player"))
                return true;

            return false;
        }

        return false;
    }
}
