using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{
    GameDataSO gameDataSO;
    [SerializeField] Ease jumpEase;
    [SerializeField] private float moveSpeed = 1f;
    private Animator animator;
    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;
    private Vector3 playerLastPosition;
    private bool isDead = true;
    private bool isActingMovement;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckSphereRadius = 0.3f;
    [SerializeField] private Vector3 groundCheckSphereOffset;
    [SerializeField] private LayerMask groundCheckLayerMask;

    // private CharacterController characterController;

    [SerializeField] List<float> xPositions;
    int currentXPositionIndex = 1;



    // [SerializeField] private AnimationCurve fallDownCurve;
    // private float fallDownTime = 0;
    [SerializeField] private float jumpPower = 3f;
    // [SerializeField] private float curveValue;

    void OnEnable()
    {
        EventManager.getPlayer += GetPlayer;
        EventManager.getPosition += () => transform.position;
        EventManager.increasePlayerSpeed += () => moveSpeed += 0.1f;
        EventManager.startGame += StartGame;
    }

    void OnDisable()
    {
        EventManager.getPlayer -= GetPlayer;
        EventManager.getPosition -= () => transform.position;
        EventManager.increasePlayerSpeed -= () => moveSpeed += 0.1f;
        EventManager.startGame -= StartGame;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        gameDataSO = EventManager.getGameDataSO?.Invoke();

        InvokeRepeating(nameof(IncreasePlayerSpeed), 5f, 5f);
    }

    void Update()
    {
        PlayerMovement();
        if (!isActingMovement)
        {
            transform.position = new Vector3(xPositions[currentXPositionIndex], transform.position.y, transform.position.z);
        }
    }

    void IncreasePlayerSpeed()
    {
        if (transform.position.z > playerLastPosition.z + 50f && moveSpeed < 25f)
        {
            moveSpeed += 0.3f;
            playerLastPosition = transform.position;
        }
    }

    private void PlayerMovement()
    {
        if (isDead) return;

        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y, moveSpeed * Time.deltaTime * 100);

        if (!Physics.CheckSphere(transform.position + groundCheckSphereOffset, groundCheckSphereRadius, groundCheckLayerMask))
        {
            isGrounded = false;
            // fallDownTime += Time.deltaTime;
            // curveValue = fallDownCurve.Evaluate(fallDownTime);
            //pos = Vector3.down * (moveSpeed * curveValue) * Time.deltaTime;
            //characterController.Move(pos);

        }
        else
        {
            isGrounded = true;
            //fallDownTime = 0;
            //characterController.Move(pos);
        }



        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentXPositionIndex > 0 && !isActingMovement)
        {
            isActingMovement = true;
            Vector3 currentPos = transform.position;
            currentPos.x = xPositions[--currentXPositionIndex];

            transform.DOMoveX(currentPos.x, .3f).SetId("Movement").OnComplete(() => isActingMovement = false);
            transform.DORotate(new Vector3(0, -55, 0), .2f).OnComplete(() => transform.DORotate(new Vector3(0, 0, 0), .2f));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentXPositionIndex < 3 && !isActingMovement)
        {
            isActingMovement = true;
            Vector3 currentPos = transform.position;
            currentPos.x = xPositions[++currentXPositionIndex];

            transform.DOMoveX(currentPos.x, .3f).SetId("Movement").OnComplete(() => isActingMovement = false);
            //transform.DOLookAt(new Vector3(currentPos.x * moveSpeed, transform.position.y, transform.position.z), .2f);
            transform.DORotate(new Vector3(0, 55, 0), .2f).OnComplete(() => transform.DORotate(new Vector3(0, 0, 0), .2f));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !isActingMovement && isGrounded && !animator.GetBool("isSliding"))
        {
            isActingMovement = true;
            playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            animator.SetBool("isJumping", true);


        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isActingMovement && isGrounded)
        {
            animator.SetBool("isSliding", true);
            DOTween.To(() => playerCollider.height, x => playerCollider.height = x, 0.6f, .5f).OnStart(() =>
            {
                DOTween.To(() => playerCollider.center, x => playerCollider.center = x, new Vector3(0, 0.3f, 0), .5f);
            }).OnComplete(() =>
            {
                DOTween.To(() => playerCollider.height, x => playerCollider.height = x, 1.52f, 0.5f);
                DOTween.To(() => playerCollider.center, x => playerCollider.center = x, new Vector3(0, 0.7f, 0), 0.5f);
            });

        }
    }

    private void StartGame()
    {
        animator.SetBool("isRunning", true);
        isDead = false;
    }
    private PlayerManager GetPlayer()
    {
        return this;
    }

    public void PlayerDeath()
    {
        if (isDead) return;
        animator.CrossFade("Death", 0.1f);
        isDead = true;
        gameObject.tag = "Untagged";
        var cam = EventManager.getCinemachine?.Invoke();
        cam.transform.DORotate(new Vector3(40, 0, 0), 2f);
        DOTween.Kill("Movement");

        EventManager.showGameOverPanel?.Invoke();

    }

    public void SetAnimationToRunning()
    {
        //yield return new WaitForSeconds(.003f);
        animator.SetBool("isJumping", false);
        animator.SetBool("isSliding", false);
        animator.SetBool("isRunning", true);
        isActingMovement = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckSphereOffset, groundCheckSphereRadius);
    }

}

