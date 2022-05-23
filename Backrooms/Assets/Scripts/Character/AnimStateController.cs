using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateController : MonoBehaviour
{
    Animator animator;

    int verticalVelocityHash;
    int horizontalVelocityHash;

    private float verticalMovementInput= 0;
    private float horizontalMovementInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        verticalVelocityHash = Animator.StringToHash("verticalVelocity");
        horizontalVelocityHash = Animator.StringToHash("horizontalVelocity");
    }

    public void setVelocities(float verticalMovementInput, float horizontalMovementInput)
    {
        animator.SetFloat(verticalVelocityHash, verticalMovementInput);
        animator.SetFloat(horizontalVelocityHash, horizontalMovementInput);
    }
}
