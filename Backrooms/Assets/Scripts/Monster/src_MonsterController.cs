using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class src_MonsterController : MonoBehaviour
{
    public GameObject[] terrains;
    public NavMeshAgent navMeshAgent;
    public ParticleSystem particles;

    public bool playerHearable;

    public GameObject gameController;
    private List<GameObject> players;
    private GameObject nearestPlayer;
    public float nearestPlayerDistance;


    public Vector3 nextPosition;

    public float restingDuration;
    public float startTimeResting;
    public float lastTimeResting;

    public MonsterHoverAnimation hoverAnim;
    public bool isResting;

    public void Awake()
    {
        nextPosition = new Vector3();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        players = gameController.GetComponent<GameController>().listOfPlayers;
        restingDuration = 5f;
        isResting = false;
    }

    // Update is called once per frame
    public void Update()
    {
        /*
        if (navMeshAgent.remainingDistance < 1)
        {
            navMeshAgent.ResetPath();
            StartCoroutine(Rest());
        }
        */
        // Debug.Log("isResting ? " + isResting);

        if(navMeshAgent.hasPath)
        {
            if (players.Count == 0)
            {
                // N'essaie pas de poursuivre un joueur s'il n'y en a pas.
            }
            else
            {
                if (players.Count == 1)
                {
                    nearestPlayer = players[0];
                    nearestPlayerDistance = Vector3.Distance(transform.position, nearestPlayer.transform.position);
                }
                else
                {
                    SetNearestPlayer();
                }

                bool isNoisy = nearestPlayer.GetComponent<src_CharacterController>().isNoisy;
                if (nearestPlayerDistance < 5)
                {
                    Debug.Log("Monstre has path mais Joueur détecté car assez près");
                    SetDestinationTargetPlayer();
                    isResting = false;
                }
                else if (nearestPlayerDistance < 10 && isNoisy)
                {
                    Debug.Log("Monstre has path mais joueur assez loin a été détecté car il fait du bruit");
                    SetDestinationTargetPlayer();
                    isResting = false;
                }
            }
        }


        else if (!navMeshAgent.hasPath)
        {
            {
                if (players.Count == 0)
                {
                    // N'essaie pas de poursuivre un joueur s'il n'y en a pas.
                }
                else
                {
                    if (players.Count == 1)
                    {
                        nearestPlayer = players[0];
                        nearestPlayerDistance = Vector3.Distance(transform.position, nearestPlayer.transform.position);
                    }
                    else
                    {
                        SetNearestPlayer();
                    }

                    bool isNoisy = nearestPlayer.GetComponent<src_CharacterController>().isNoisy;
                    if (nearestPlayerDistance < 5)
                    {
                        Debug.Log("Joueur détecté car assez près");
                        SetDestinationTargetPlayer();
                        isResting = false;
                    }
                    else if (nearestPlayerDistance < 10 && isNoisy)
                    {
                        Debug.Log("Un joueur assez loin a été détecté car il fait du bruit");
                        SetDestinationTargetPlayer();
                        isResting = false;
                    }
                    else
                    {
                        // Debug.Log("Pas de joueur à pourchasser");
                        SetDestinationRandom();
                    }
                }
            }
        }

    }

    /*
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Joueur repéré");
            StartCoroutine(Attack());
            // DoYourThing();
        }

    }
    */

    private void SetDestinationRandom()
    {
        isResting = false;
        GameObject terrainRandom = terrains[UnityEngine.Random.Range(0, terrains.Length - 1)];
        Bounds bounds = terrainRandom.GetComponent<Collider>().bounds;

        nextPosition.Set(UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                        transform.position.y,
                        UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
        navMeshAgent.SetDestination(nextPosition);
    }

    private void SetDestinationTargetPlayer()
    {
        isResting = false;
        Vector3 playerPosition = nearestPlayer.transform.position;
        nextPosition.Set(playerPosition.x, transform.position.y, playerPosition.z);
        navMeshAgent.SetDestination(nextPosition);
    }

    private void Attack()
    {
        Debug.Log("Attaque !!!");
        particles.Play();
    }
    /*
    private IEnumerator Attack()
    {
        Debug.Log("Attaque !!!");
        particles.Play();
        //isResting = true;
        yield return new WaitForSeconds(restingDuration);
        //isResting = false;
    }
    */

    private IEnumerator Rest()
    {
        Debug.Log("Repos...");
        isResting = true;
        yield return new WaitForSeconds(restingDuration);
        isResting = false;
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


