using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float walkSpeed = 2.0f;
    private float translation;

    public GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float vitesse = 0;

        if (Input.GetKeyDown("w")) {
            vitesse = walkSpeed;
            translation = vitesse * Time.deltaTime;
            transform.Translate(translation, 0, 0);
            Debug.Log("!");
        }
    }
}
