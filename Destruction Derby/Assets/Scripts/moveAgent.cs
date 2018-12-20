using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class moveAgent : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject player;
    private EnemyHPController hp;
    public GameObject destination;

    public enum State
    {
        PATROL,
        CHASE,
        VUNERABLE
    }

    public State state;
    private bool alive;

    public GameObject[] waypoints;
    public GameObject[] healthSpots;
    private int waypointInd;
    private int healthInd;
    public float patrolSpeed = 0.5f;

    public float chaseSpeed = 1f;
    public GameObject target;

    bool knockBack;
    public Vector3 direction;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hp = player.GetComponent<EnemyHPController>();
        agent.updatePosition = true;
        agent.updateRotation = true;

        state = moveAgent.State.PATROL;

        alive = true;

        StartCoroutine("FSM");
        waypointInd = Random.Range(0, waypoints.Length - 1);
        healthInd = Random.Range(0, healthSpots.Length - 1);
        target = null;
    }
    void FixedUpdate()
    {
        if (knockBack)
        {
            agent.velocity = direction * 10;
        }
    }

    IEnumerator KnockBack()
    {
        knockBack = true;
        agent.speed = 2;
        agent.acceleration = 10;

        yield return new WaitForSeconds(0.2f);

        knockBack = false;
        agent.speed = 25;
        agent.angularSpeed = 300;
        agent.acceleration = 30;
        target = null;
        if (hp.currentHealth < 50)
        {
            state = moveAgent.State.VUNERABLE;
        }
        else
        {
            state = moveAgent.State.PATROL;
        }
    }



    IEnumerator FSM()
    {

        while (alive)
        {
            switch (state)
            {
                case State.PATROL:
                    Patrol();
                    break;
                case State.CHASE:
                    Chase();
                    break;
                case State.VUNERABLE:
                    Vulnerable();
                    break;
            }
            if(hp.currentHealth == 0){
                alive = false;
            }
            yield return null;
        }
        agent.enabled = false;
    }

    void Patrol()
    {
        
        agent.speed = patrolSpeed;
        if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2f)
        {
            agent.SetDestination(waypoints[waypointInd].transform.position);
            destination = waypoints[waypointInd];
        }
        else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2f)
        {

            waypointInd = Random.Range(0, waypoints.Length - 1);
            if (waypointInd > waypoints.Length - 1)
            {
                waypointInd = 0;
            }
        }
        else
        {

        }

    }
    void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(target.transform.position);
    }
    void Vulnerable()
    {
        agent.speed = 15;
		if (Vector3.Distance(this.transform.position, healthSpots[healthInd].transform.position) >= 2f)
		{
			agent.SetDestination(healthSpots[healthInd].transform.position);
			destination = healthSpots[healthInd];
		}
        else if (Vector3.Distance(this.transform.position, healthSpots[healthInd].transform.position) <= 2f)
		{

			healthInd = Random.Range(0, healthSpots.Length - 1);
            if (healthInd > healthSpots.Length - 1)
			{
                healthInd = 0;
			}
		}
		else
		{

		}

    }

    void OnTriggerEnter (Collider coll)
    {
        if (state != moveAgent.State.VUNERABLE)
        {
            if (coll.tag == "Enemy" || coll.tag == "Player")
            {
                state = moveAgent.State.CHASE;
                target = coll.gameObject;
            }

        }
    }

	void OnCollisionEnter(Collision col)
	{
        
		if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Player")
		{
            direction = col.transform.forward;
            StartCoroutine(KnockBack());
            waypointInd = Random.Range(0, waypoints.Length - 1);
		}
        if(col.gameObject.tag == "Post")
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(0, 100, 0);
            StartCoroutine(DestoryObject(col.gameObject));
        }
       

	}

    IEnumerator DestoryObject(GameObject item)
        {
            // Wait 5secs then remove the object from play
            yield return new WaitForSeconds(5);
            Destroy(item);
        }
}


