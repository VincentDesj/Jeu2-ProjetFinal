using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    private bool isOpenedExit = false;

    private List<Transform> listOfGoalSpawns = new List<Transform>();
    private List<GameObject> listOfGoals = new List<GameObject>();
    
    private GameObject exit;
    public GameObject spawnee;


    // Start is called before the first frame update
    void Start()
    {
        exit = GameObject.FindGameObjectWithTag("Exit");

        Debug.Log(GameObject.FindGameObjectsWithTag("GoalSpawnPoint").ToList().Count);
        listOfGoalSpawns.AddRange(GameObject.FindGameObjectsWithTag("GoalSpawnPoint").Select(item => item.transform).ToList());
        SetGoalSpawnPoint();
        listOfGoals.AddRange(GameObject.FindGameObjectsWithTag("Goal"));

    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpenedExit)
            checkGoalActivation();
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
            Debug.Log(exit.transform.position.y);
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
