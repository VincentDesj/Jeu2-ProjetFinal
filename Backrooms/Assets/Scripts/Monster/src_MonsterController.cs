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
    public ParticleSystem particles;
    //private SphereCollider collider;

    public bool playerHearable;

    public GameObject gameController;
    private List<GameObject> players;
    private GameObject nearestPlayer;
    public float nearestPlayerDistance;

    // public Vector3 personalLastSighting;
    // public Vector3 previousSighting;

    public Vector3 nextPosition;

    //public float lastTimeResting;
    public float restingDuration;
    //public float startTimeResting;
    // public bool isListening;
    public bool isResting;

    public MonsterHoverAnimation hoverAnim;
    // public bool isTracking;


    /**
     * Si pas de destination
     *  Si joueur pas très loin && isNoisy --> destination le joueur
     *  Sinon, si joueur plus proche --> destination le joueur
     *  Sinon, destination aléatoire
     * 
     * Si joueur dans collider --> drop destination, destination le joueur
     * 
     * Si joueur très près --> attaque
     * 
     * Si joueur sort du collider --> drop destination
     */

    public void Awake()
    {
        terrainBounds = terrain.GetComponent<Collider>().bounds;
        nextPosition = new Vector3();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        SetNextPositionRandom();

        players = gameController.GetComponent<GameController>().listOfPlayers;
        restingDuration = 5f;
        isResting = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isResting)
        {
            SetNearestPlayer();
            Debug.Log("Nearest player : " + nearestPlayer.name);
            Debug.Log("src_CharacterController : " + nearestPlayer.GetComponent<src_CharacterController>());

            bool isNoisy = nearestPlayer.GetComponent<src_CharacterController>().isNoisy;
            if (nearestPlayerDistance < 10)
            {
                Debug.Log("Joueur détecté car assez près");
            }
            else if (nearestPlayerDistance < 20 && isNoisy)
            {
                Debug.Log("Un joueur assez loin a été détecté car il fait du bruit");
            }
            else
            {
                Debug.Log("Pas de joueur à pourchasser");
            }
        }

        /*
        // À revoir
        if (!navMeshAgent.hasPath)
        {
            if (!isListening)
            {
                startTimeResting = Time.time;
                isListening = true;
            }
            else
            {
                if ((Time.time - startTimeResting) > listeningDuration)
                {
                    GetNextPositionRandom();
                    navMeshAgent.SetDestination(nextPosition);
                    isListening = false;
                }
            }

        } */
    }

    /*
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("joueur détecté");
        {
            if (other.transform.CompareTag("Player"))
            {
                if (true) //other.transform.gameObject.GetComponent<src_CharacterController>().isNoisy)
                {
                    Vector3 playerPosition = other.transform.position;
                    nextPosition.Set(playerPosition.x, transform.position.y, playerPosition.z);
                    navMeshAgent.SetDestination(nextPosition);
                }
            }
        }
    }
    */

    /*
    public void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (Vector3.Distance(other.transform.position, transform.position) < 2)
            StartCoroutine(Attack());
        }
    }
    */

    private void SetNextPositionRandom()
    {
        nextPosition.Set(UnityEngine.Random.Range(terrainBounds.min.x, terrainBounds.max.x),
                        transform.position.y,
                        UnityEngine.Random.Range(terrainBounds.min.z, terrainBounds.max.z));
        navMeshAgent.SetDestination(nextPosition);
    }

    private void SetNextPositionTargetPlayer(GameObject player)
    {
        Vector3 playerPosition = player.transform.position;
        nextPosition.Set(playerPosition.x, transform.position.y, playerPosition.z);
        navMeshAgent.SetDestination(nextPosition);
    }

    private IEnumerator Attack()
    {
        particles.Play();
        isResting = true;
        yield return new WaitForSeconds(restingDuration);
    }

    private IEnumerable Rest()
    {
        isResting = true;
        yield return new WaitForSeconds(restingDuration);
    }

    private void SetNearestPlayer()
    {
        nearestPlayerDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // Un joueur qui n'est pas bruyant est considéré comme étant plus loin du monstre.
            if (!player.GetComponent<src_CharacterController>().isNoisy)
            {
                distance *= 1.5f;
            }

            if (distance < nearestPlayerDistance)
            {
                nearestPlayerDistance = distance;
                nearestPlayer = player;
            }
        }
    }
}


