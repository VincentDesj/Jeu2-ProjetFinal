using UnityEngine;

public class MonsterHoverAnimation : MonoBehaviour
{

    public GameObject visualMonster;
    public float verticalSpeed;
    public float amplitude;
    public float verticalOffset;
    public Vector3 tempPosition;

    public void Start()
    {
        verticalSpeed = 6f;
        amplitude = 0.2f;
        verticalOffset = 2.5f;
        tempPosition = transform.position;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        // Animation "hover"
        tempPosition.y = (Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude) + verticalOffset;
        visualMonster.transform.position = new Vector3(transform.position.x, tempPosition.y, transform.position.z);
    }
}
