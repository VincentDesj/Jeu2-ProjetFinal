using UnityEngine;

public class MonsterHoverAnimation : MonoBehaviour
{

    public GameObject visualMonster;
    public float verticalSpeed;
    public float amplitude;
    public float verticalOffset;
    public Vector3 tempPosition;
    //public bool isResting;
    //public src_MonsterController monsterController;

    public void Start()
    {
        verticalSpeed = 6f;
        amplitude = 0.2f;
        verticalOffset = 2.5f;
        tempPosition = transform.position;
        //isResting = monsterController.isResting;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        tempPosition.y = (Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude) + verticalOffset;
        visualMonster.transform.position = new Vector3(transform.position.x, tempPosition.y, transform.position.z);
        /*
        if (!isResting)
        {
            Hover();
        }
        */
        //Hover();
    }

    private void Hover()
    {
        tempPosition.y = (Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude) + verticalOffset;
        visualMonster.transform.position = new Vector3(transform.position.x, tempPosition.y, transform.position.z);
    }
}
