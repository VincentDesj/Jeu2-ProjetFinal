using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField] 
    public bool goalActive = false;
    private GameObject laptop;
    public Material laptopMaterialOn;

    private DefaultInput defaultInput;

    private void Awake()
    {
        laptop = this.transform.Find("Laptop_black_off").gameObject;

        defaultInput = new DefaultInput();
    }

    public void ActivateGoal() 
    {
        goalActive = true;
        laptop.GetComponent<Renderer>().material = laptopMaterialOn;
    }
}
