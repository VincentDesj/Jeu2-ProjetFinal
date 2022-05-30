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

    public float lastTimeListening;
    public float listeningDuration;
    public float startTimeListening;
    public bool isListening;


    public void Awake()
    {
        terrainBounds = terrain.gameObject.GetComponent<Collider>().bounds;
        nextPosition = new Vector3();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        GetNextPosition();

        players = GameObject.FindGameObjectsWithTag("Player");

        lastTimeListening = 0; //Time.deltaTime;
        listeningDuration = 3f;
        isListening = false;
    }


    // Update is called once per frame
    public void Update()
    {
        if (!navMeshAgent.hasPath)
        {
            if (!isListening)
            {
                startTimeListening = Time.time;
                isListening = true;
            }
            else
            {
                if ((Time.time - startTimeListening) > listeningDuration)
                {
                    GetNextPosition();
                    navMeshAgent.SetDestination(nextPosition);
                    isListening = false;
                }
            }

            /*
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    GetNextPosition();
                    navMeshAgent.SetDestination(nextPosition);
                }
            }
            */
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("joueur détecté");
        if (isListening)
        {
            if (other.transform.CompareTag("Player"))
            {
                if (other.transform.gameObject.GetComponent<src_CharacterController>().isNoisy)
                {
                    Vector3 playerPosition = other.transform.position;
                    nextPosition.Set(playerPosition.x, transform.position.y, playerPosition.z);
                    navMeshAgent.SetDestination(nextPosition);
                    isListening = false;
                }
            }
        }
    }

    /*
    public void OnCollisionEnter(Collision collision)
    {
        if (isListening)
        {
            if (collision.transform.CompareTag("Player"))
            {
                Debug.Log("joueur détecté");
                Vector3 playerPosition = collision.transform.position;
                nextPosition.Set(playerPosition.x, transform.position.y, playerPosition.z);
                navMeshAgent.SetDestination(nextPosition);
                isListening = false;
            }
        }
    }
    */

    private void GetNextPosition()
    {
        nextPosition.Set(UnityEngine.Random.Range(terrainBounds.min.x, terrainBounds.max.x),
                        transform.position.y,
                        UnityEngine.Random.Range(terrainBounds.min.z, terrainBounds.max.z));
    }


}
