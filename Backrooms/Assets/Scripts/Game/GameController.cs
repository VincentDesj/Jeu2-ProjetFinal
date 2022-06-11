using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using Random = UnityEngine.Random;

public class GameController : NetworkBehaviour
{
    private bool isOpenedExit = false;

    private List<Transform> listOfGoalSpawns = new List<Transform>();
    private List<GameObject> listOfGoals = new List<GameObject>();
    public List<GameObject> listOfPlayers = new List<GameObject>();

    public GameObject exit;
    public GameObject spawnee;


    // Start is called before the first frame update
    void Start()
    {
        listOfGoalSpawns.AddRange(GameObject.FindGameObjectsWithTag("GoalSpawnPoint").Select(item => item.transform).ToList());
        SetGoalSpawnPoint();
        listOfGoals.AddRange(GameObject.FindGameObjectsWithTag("Goal"));
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerState();
        if (!isOpenedExit)
            checkGoalActivation();
        checkVictoryCondition();
    }

    private void checkVictoryCondition()
    {
        if (listOfPlayers.All(x => x.GetComponent<HealthController>().isDead))
        {
            for (int i = 0; i < listOfPlayers.Count; i++)
            {
                listOfPlayers[i].GetComponent<HealthController>().UpdateFinalMessage("Vous etes mort! ECHEC TOTAL");
            }
        }
        else if (listOfPlayers.All(x => x.GetComponent<HealthController>().hasEscaped))
        {
            for (int i = 0; i < listOfPlayers.Count; i++)
            {
                listOfPlayers[i].GetComponent<HealthController>().UpdateFinalMessage("Vous etes sorti! VICTOIRE TOTALE");
            }
        }
        else if (!listOfPlayers.Any(x => x.GetComponent<HealthController>().isDead || x.GetComponent<HealthController>().hasEscaped))
        {
            for (int i = 0; i < listOfPlayers.Count; i++)
            {
                if (listOfPlayers[i].GetComponent<HealthController>().isDead)
                    listOfPlayers[i].GetComponent<HealthController>().UpdateFinalMessage("Vous etes mort! ECHEC PARTIEL");
                if (listOfPlayers[i].GetComponent<HealthController>().hasEscaped)
                    listOfPlayers[i].GetComponent<HealthController>().UpdateFinalMessage("Vous etes mort! VICTOIRE PARTIELLE");
            }
        }
    }

    private void checkPlayerState()
    {
        for (int i = 0; i < listOfPlayers.Count; i++) {
            if (listOfPlayers[i].GetComponent<HealthController>().isDead)
            {
                listOfPlayers[i].GetComponent<HealthController>().UpdateFinalMessage("Vous etes mort!");
                listOfPlayers[i].GetComponent<src_CharacterController>().enabled = false;
            }

            if (listOfPlayers[i].GetComponent<HealthController>().hasEscaped)
            {
                listOfPlayers[i].GetComponent<HealthController>().UpdateFinalMessage("Vous etes sorti!");
                listOfPlayers[i].GetComponent<src_CharacterController>().enabled = false;
            }
        }
    }

    private void SetGoalSpawnPoint()
    {
        List<int> randomListInt = new List<int>();

        while (randomListInt.Count < 3)
        {
            int number = Random.Range(0, listOfGoalSpawns.Count);
            if (!randomListInt.Contains(number))
            {
                randomListInt.Add(number);
            }
        }

        foreach (int number in randomListInt)
        {
            Instantiate(spawnee, listOfGoalSpawns[number]);
        }
    }

    private void checkGoalActivation()
    {
        if (!listOfGoals.Any(x => !x.GetComponent<GoalController>().goalActive))
        {
            if (exit.transform.position.y >= -12)
            {
                exit.transform.Translate(Vector3.down * Time.deltaTime);
            }
            else {
                isOpenedExit = true;
            }
        }
    }
}
