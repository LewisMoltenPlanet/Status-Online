using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private string horizontalInputName, verticalInputName;
    [SerializeField] private float movementSpeed, sprintSpeed, jumpMultiplier, doubleJumpMultiplier, jumpCoolDown, camFOV;
    [SerializeField] private KeyCode jumpKey, sprintKey;
    [SerializeField] private AnimationCurve jumpFalloff, doubleJumpFalloff;
    [SerializeField] private Animator anim, weaponAnim;
    [SerializeField] private ParticleSystem jetPack;

    public ParticleSystem muzzleFlash01;
    public ParticleSystem muzzleFlash02;
    public ParticleSystem muzzleFlash03;

    private Camera playerCam;
    private CharacterController charController;
    private float animCoolDown = 0.1f, startCamFOV;
    private bool isJumping;
    private bool hasDoubleJumped;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>();

        startCamFOV = playerCam.fieldOfView;
    }

    private void Update()
    {
        PlayerMovement();
        UseGun();

        if (anim == null)
            return;

        var X = Input.GetAxis("Horizontal");
        var Y = Input.GetAxis("Vertical");

        PlayerAnimation(X,Y);
    }

    private void PlayerMovement() //Calculates and initiates player movement.
    {
        float vertInput = Input.GetAxis(verticalInputName);
        float horizInput = Input.GetAxis(horizontalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        if (Input.GetKey(sprintKey) && Input.GetKey(KeyCode.W)) //Run logic
        {
            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * sprintSpeed);
            anim.SetBool("Run", true);
            weaponAnim.SetBool("Run", true);
            weaponAnim.SetBool("Walk", false);

            //playerCam.fieldOfView = camFOV;
        }
        else if (!Input.GetKey(sprintKey) && Input.GetButton("Horizontal") || Input.GetButton("Vertical")) //Walk logic
        {
            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);
            anim.SetBool("Run", false);
            weaponAnim.SetBool("Run", false);
            weaponAnim.SetBool("Walk", true);

            //playerCam.fieldOfView = startCamFOV;
        }
        else
        {
            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed); //Idle logic
            anim.SetBool("Run", false);
            weaponAnim.SetBool("Run", false);
            weaponAnim.SetBool("Walk", false);

            playerCam.fieldOfView = startCamFOV;
        }

        JumpInput();
    }

    private void JumpInput() //Calculates and initiates jumping.
    {
        jumpCoolDown -= Time.deltaTime;

        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            jumpCoolDown = 0.02f;
            isJumping = true;
            StartCoroutine(JumpEvent());
        }

        if (Input.GetKeyDown(jumpKey) && isJumping && !hasDoubleJumped && jumpCoolDown <= 0.0f)
        {
            hasDoubleJumped = true;
            StartCoroutine(DoubleJumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFalloff.Evaluate(timeInAir);

            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);

            timeInAir += Time.deltaTime;

            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }

    private IEnumerator DoubleJumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        jetPack.Play();

        do
        {
            float doubleJumpForce = doubleJumpFalloff.Evaluate(timeInAir);

            charController.Move(Vector3.up * doubleJumpForce * doubleJumpMultiplier * Time.deltaTime);

            timeInAir += Time.deltaTime;

            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        hasDoubleJumped = false;
    }

    private void PlayerAnimation(float x, float y)
    {
        animCoolDown -= Time.deltaTime;

        if (charController.isGrounded)
        {
            anim.SetFloat("VelX", x);
            anim.SetFloat("VelY", y);

            anim.SetBool("Move", true);
            anim.SetBool("InAir", false);

            animCoolDown = 0.25f;
        }
        else if (animCoolDown <= 0.0f)
        {
            anim.SetFloat("VelX", 0);
            anim.SetFloat("VelY", 0);

            anim.SetBool("Move", false);
            anim.SetBool("InAir", true);
            anim.SetBool("Run", false);

            weaponAnim.SetBool("Run", false);
            weaponAnim.SetBool("Walk", false);
        }
    }

    private void UseGun()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("Fire", true);
            anim.SetBool("Reload", false);

            muzzleFlash01.Play();
            muzzleFlash02.Play();
            muzzleFlash03.Play();
        }
        else
        {
            anim.SetBool("Fire", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetBool("Fire", false);
            anim.SetBool("Reload", true);
        }
        else
        {
            anim.SetBool("Reload", false);
        }
    } 
}