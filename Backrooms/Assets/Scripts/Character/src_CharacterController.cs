using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using static src_Models;

public class src_CharacterController : NetworkBehaviour
{
    public CharacterController characterController;
    private DefaultInput defaultInput;
    public AnimStateController animator;

    private int hp = 3;

    [SerializeField] TextMeshProUGUI hpAmount;
    public Image damage;

    [HideInInspector]
    public Vector2 inputMovement;
    [HideInInspector]
    public Vector2 inputView;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("References")]
    public Transform cameraHolder;
    private Camera cameraPlayer;
    private bool monsterIsSeen = false;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;

    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;

    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;


    [Header("State")]
    public PlayerState playerState;
    public float slowWalkSpeed = 0.7f;
    public float walkSpeed = 0.7f;
    public float runSpeed = 1.3f;
    public bool isNoisy = false;

    public bool isSlowWalk;
    public bool isSprinting;

    public GameObject[] flashlights;

    public Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;

    public float verticalMovementInput;
    public float horizontalMovementInput;

    [Header("Weapon")]
    public src_WeaponController currentWeapon;

    public GameObject characterModel;
    public AudioSource jumpScareSource;
    public AudioClip audioClip;
    private GameObject monsterModel;

    public void Awake()
    {

        defaultInput = new DefaultInput();

        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();
        defaultInput.Character.Sprint.performed += e => ToggleSprint();
        defaultInput.Character.SlowWalk.performed += e => ToggleSlowWalk();
        defaultInput.Character.ActivateFlashlight.performed += e => ToggleFlashlight();
        defaultInput.Character.Use.performed += e => CheckForGoal();

        defaultInput.Enable();

        animator = GetComponent<AnimStateController>();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        cameraPlayer = cameraHolder.GetComponentInChildren<Camera>();

        characterController = GetComponent<CharacterController>();
        monsterModel = GameObject.Find("Monster");


        UpdateHpUI();

        if (currentWeapon)
        {
            currentWeapon.Initialise(this);
        }
    }

    public void Update()
    {
        if (hasAuthority)
        { 
            EvaluateSpeed();
            CalculateView();
            CalculateMovement();
            CalculateJump();
            CheckForMonster();
        }
    }

    private void CheckForMonster()
    {
        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cameraPlayer), monsterModel.GetComponent<CapsuleCollider>().bounds) && monsterIsSeen==false)
        {
            var ray = new Ray(cameraPlayer.transform.position, cameraPlayer.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name == "Monster")
                    StartCoroutine(jumpScareMonster());
            }
        }
    }

    private IEnumerator jumpScareMonster()
    {
        jumpScareSource.PlayOneShot(audioClip, 0.25f);
        monsterIsSeen = true;
        yield return new WaitForSeconds(60f);
        monsterIsSeen = false;
    }

    private void EvaluateSpeed()
    {
        //if ()
    }

    private void CalculateMovement()
    {

        var verticalSpeed = playerSettings.walkingForwardSpeed;
        var horizontalSpeed = playerSettings.walkingStrafeSpeed;

        if (isSprinting)
        {
            if (inputMovement.y > 0)
            {
                verticalSpeed = playerSettings.runningForwardSpeed;
            }
            else
            {
                verticalSpeed = playerSettings.runningBackwardSpeed;
            }

            horizontalSpeed = playerSettings.runningStrafeSpeed;
        }

        if (isSlowWalk)
        {
            verticalSpeed = playerSettings.slowWalkForwardSpeed;
            horizontalSpeed = playerSettings.slowWalkStrafeSpeed;
        }

        if (characterController.isGrounded)
        {
            playerSettings.SpeedEffector = playerSettings.FallingSpeedEffector;
        }
        else
        {
            playerSettings.SpeedEffector = 1;
        }

        verticalSpeed *= playerSettings.SpeedEffector;
        horizontalSpeed *= playerSettings.SpeedEffector;

        verticalMovementInput = verticalSpeed * inputMovement.y * Time.deltaTime;
        horizontalMovementInput = horizontalSpeed * inputMovement.x * Time.deltaTime;

        animator.setVelocities(verticalMovementInput, horizontalMovementInput);

        if ((verticalMovementInput != 0 || horizontalMovementInput != 0) && !isSlowWalk)
        {
            isNoisy = true;
        }
        else 
        {
            isNoisy = false;
        }

        newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, new Vector3(horizontalSpeed * inputMovement.x * Time.deltaTime, 0, verticalSpeed * inputMovement.y * Time.deltaTime),
            ref newMovementSpeedVelocity, characterController.isGrounded ? playerSettings.movementSmoothing : playerSettings.fallingSmoothing);
        var movementSpeed = cameraHolder.TransformDirection(newMovementSpeed);

        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (playerGravity < -0.1f && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }

        movementSpeed.y += playerGravity;
        movementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(movementSpeed);
    }

    private void CalculateView()
    {
        newCameraRotation.y += playerSettings.viewXSensitivity * inputView.x * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        //Ajuste selon la sensibilité & le min + max de hauteur que la caméra peu monter
        newCameraRotation.x += playerSettings.viewYSensitivity * -inputView.y * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(new Vector3(newCameraRotation.x, cameraHolder.transform.rotation.y, characterModel.transform.rotation.z));
        characterModel.transform.rotation = Quaternion.Euler(new Vector3(characterModel.transform.rotation.x, newCameraRotation.y, characterModel.transform.rotation.z)) ;

        ///MOUVEMENT DES BRAS AVEC LA FLASHLIGHT PAS FAISABLE DANS UNE ANIM PAS FAISABLE SANS UMOTION
        //GameObject.Find("mixamorig:Spine1").transform.rotation = Quaternion.Euler(new Vector3(cameraHolder.localRotation.x, characterModel.transform.rotation.y, characterModel.transform.rotation.z));
        //GameObject.Find("mixamorig:Spine2").transform.rotation = Quaternion.Euler(new Vector3(cameraHolder.localRotation.x, characterModel.transform.rotation.y, characterModel.transform.rotation.z));
        //GameObject.Find("mixamorig:Neck").transform.rotation = Quaternion.Euler(new Vector3(cameraHolder.localRotation.x, characterModel.transform.rotation.y, characterModel.transform.rotation.z));
    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.jumpingFalloff);
    }

    private void Jump()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        //Jump
        jumpingForce = Vector3.up * playerSettings.jumpingHeight;
        playerGravity = 0;
    }

    private void ToggleSprint()
    {

        isSprinting = !isSprinting;
    }

    private void ToggleSlowWalk()
    {
        isSlowWalk = !isSlowWalk;
    }

    private void ToggleFlashlight()
    {
        int nbFlashlights = flashlights.Length;
        for (int i = 0; i < nbFlashlights; i++)
        {
            // flashlights[i].SetActive(!flashlights[i].active);

            // TODO : Valider que le comportement est toujours bon (isActiveInHierarchy remplace active qui est obsolète)
            flashlights[i].SetActive(!flashlights[i].activeInHierarchy);
        }
    }

    private void CheckForGoal()
    {
        //var ray = new Ray(cameraPlayer.transform.position, cameraPlayer.transform.forward);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(cameraPlayer.transform.position, cameraPlayer.transform.forward, 10f);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hits[i].transform.gameObject.CompareTag("Goal") && hit.distance <= 5f)
            {
                hits[i].transform.gameObject.GetComponent<GoalController>().ActivateGoal();
            }


        }

        /*RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.distance);
            Debug.Log(hit.transform.gameObject.tag);
            Debug.Log(hit.transform.gameObject.CompareTag("Computer") && hit.distance <= 5f);
            if (hit.transform.gameObject.CompareTag("Computer") && hit.distance <= 5f) {
                hit.transform.SendMessage("ActivateGoal");
                hit.transform.gameObject.GetComponent<GoalController>().ActivateGoal();
            }
        }*/
    }

    private void UpdateHpUI() 
    {
        char square = '\u25A0';
        string hpLeft = "";
        for (int i = 0; i < hp; i++)
        {
            hpLeft += square.ToString();
        }

        hpAmount.text = hpLeft;
    }

    private void AddToHp(int amount) 
    {
        hp += amount;
        UpdateHpUI();
    }

    private void SubstactToHp(int amount)
    {
        hp -= amount;
        UpdateHpUI();
        if (hp > 0) 
        {
            StartCoroutine(TakingDamage());
        }
        //If 0 is DEAD
    }

    private void TakingDamage()
    {
        yield return new WaitForSeconds(timeDelay);
    }
}
