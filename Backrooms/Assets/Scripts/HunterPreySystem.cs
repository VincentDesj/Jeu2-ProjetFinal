using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterPreySystem : MonoBehaviour
{
    public GameObject preys;
    public GameObject hunter;

    private List<GameObject> listOfPreys;


    void Start()
    {
        int nbrOfPreys = preys.transform.childCount;

        for (int i = 0; i < nbrOfPreys; i++) {
            listOfPreys.Add(preys.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
