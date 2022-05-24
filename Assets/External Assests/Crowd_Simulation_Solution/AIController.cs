using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {

    
    public GameObject goal;
    NavMeshAgent agent;
    Animator anim;

    public Health health;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start() {
        
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(goal.transform.position);

        goal = GameObject.FindGameObjectWithTag("Goal");
        anim.SetBool("isRunning",true);

        SetSpeedValueToRandom();
    }

    void Update()
    {
        DisableDestinationonDeath();
    }

    void SetSpeedValueToRandom()
    {
        agent.speed = Random.Range(6,8.2f);
    }


    void DisableDestinationonDeath()
    {

        if (health != null)
        {
            if (health.isDead)
            {
                agent.enabled = false;

            }
        }
    }
}
