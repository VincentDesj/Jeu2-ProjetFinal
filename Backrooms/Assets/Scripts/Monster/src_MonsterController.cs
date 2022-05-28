using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class src_MonsterController : MonoBehaviour
{
    public GameObject terrain;
    public Bounds terrainBounds;
    public NavMeshAgent navMeshAgent;
    //private SphereCollider collider;

    public bool playerHearable;

    private GameObject[] players;

    public Vector3 personalLastSighting;
    public Vector3 previousSighting;

    public Vector3 nextPosition;

    public GameObject visualMonster;
    public float verticalSpeed;
    public float amplitude;
    public float verticalOffset;
    public Vector3 tempPosition;

    public void Awake()
    {
        verticalSpeed = 6f;
        amplitude = 0.2f;
        verticalOffset = 2.5f;
        tempPosition = transform.position;

        terrainBounds = terrain.gameObject.GetComponent<Collider>().bounds;
        nextPosition = new Vector3();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        GetNextPosition();

        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        tempPosition.y = (Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude) + verticalOffset;
        visualMonster.transform.position = new Vector3(transform.position.x, tempPosition.y, transform.position.z);

        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    GetNextPosition();
                    navMeshAgent.SetDestination(nextPosition);
                }
            }
        }
    }

    private void GetNextPosition()
    {
        nextPosition.Set(UnityEngine.Random.Range(terrainBounds.min.x, terrainBounds.max.x),
                        tempPosition.y,
                        UnityEngine.Random.Range(terrainBounds.min.z, terrainBounds.max.z));
    }
}
