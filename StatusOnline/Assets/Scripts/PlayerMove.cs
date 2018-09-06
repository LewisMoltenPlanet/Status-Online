using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [Header("Fields")]
    [SerializeField] private string horizontalInputName, verticalInputName;
    [SerializeField] private float movementSpeed, sprintSpeed, jumpMultiplier, doubleJumpMultiplier, jumpCoolDown;
    [SerializeField] private KeyCode jumpKey, sprintKey;
    [SerializeField] private AnimationCurve jumpFalloff, doubleJumpFalloff;

    private CharacterController charController;

    public bool isJumping;
    public bool hasDoubleJumped;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement() //Calculates and initiates player movement.
    {
        float vertInput = Input.GetAxis(verticalInputName);
        float horizInput = Input.GetAxis(horizontalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        if (Input.GetKey(sprintKey))
        {
            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * sprintSpeed);
        }
        else
        {
            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);
        }

        JumpInput(); 
    }

    private void JumpInput() //Calculates and initiates jumping.
    {
        jumpCoolDown -= Time.deltaTime;

        if (!charController.isGrounded)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

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
}
