using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction attackAction;
    public InputAction blockAction;

    public Direction blockDirection;

    public GameManager manager;

    public float moveVal;
    float attackVal;
    float blockVal;

    public bool isAttacking;
    public bool isBlocking;
    public bool isBusy;

    public float playerSpeed;

    public float leftBound = -2.75f;
    public float rightBound = 2.75f;

    public float crossBoundary = 200f;

    public Vector3 moveVector;

    Vector2 screenCenter = new Vector2(557, 273);

    float startingY;
    float startingZ;

    [Header("Attack Damages")]
    public float crossDamage = 10;
    public float jabDamage = 5;
    public float rightHookDamage = 20;
    public float leftHookDamage = 15;
    public float uppercutDamage = 20;
    public float hammerDamage = 25;

    [Header("Attack Speeds")]
    public float crossSpeed = 0.5f;
    public float jabSpeed = 0.25f;
    public float hookSpeed = 0.75f;
    public float uppercutSpeed = 1f;
    public float hammerSpeed = 1.5f;

    public float playerHealth = 100;
    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        attackAction = playerInput.actions.FindAction("Attack");
        blockAction = playerInput.actions.FindAction("Block");

        startingY = transform.position.y;
        startingZ = transform.position.z;

        LockCursor();
    }

    void Update()
    {
        ReadActions();
    }

    void ReadActions()
    {
        moveVal = moveAction.ReadValue<Vector2>().x;
        attackVal = attackAction.ReadValue<float>();
        blockVal = blockAction.ReadValue<float>();

        if (attackVal == 1 || blockVal == 1)
            isBusy = true;
    
        if (!isBusy)
        {
            LockCursor();
            Move();
        }
        else
        {
            UnlockCursor();
            if (attackVal == 1 || isAttacking)
                Attack();
            else if (blockVal == 1 || isBlocking)
                Block();
        }
    }

    void Move()
    {
        moveVector.x = -moveVal * playerSpeed * Time.deltaTime;

        transform.position += moveVector;
        if (transform.position.x > rightBound)
            transform.position = new Vector3(rightBound, startingY, startingZ);
        else if (transform.position.x < leftBound)
            transform.position = new Vector3(leftBound, startingY, startingZ);
    }

    void Attack()
    {
        isAttacking = true;
        if (attackVal == 0)
        {
            Vector2 mousePosition = GetMousePosition();
            float vectorMagnitude = mousePosition.magnitude;
            float angle = CalculateAngle(mousePosition);
            StartCoroutine(DecideAttack(vectorMagnitude, angle));
            isBusy = false;
        }
    }
    IEnumerator DecideAttack(float magnitude, float angle)
    {
        playerInput.DeactivateInput();
        if (magnitude < crossBoundary)
        {
            yield return new WaitForSeconds(crossSpeed);
            Cross();
        }
        else
        {
            if (angle > 45 && angle <= 135)
            {
                yield return new WaitForSeconds(uppercutSpeed);
                
                Uppercut();
            }               
            else if (angle > 135 && angle <= 225)
            {
                yield return new WaitForSeconds(hookSpeed);
                LeftHook();
            }                
            else if (angle > 225 && angle <= 315)
            {
                yield return new WaitForSeconds(hammerSpeed);
                Hammer();
            }               
            else if (angle > 315 || angle <= 45)
            {
                yield return new WaitForSeconds(hookSpeed);
                RightHook();
            }             
        }
    }
    void FinishAttack()
    {
        playerInput.ActivateInput();
        isBusy = false;
        isAttacking = false;
    }
    float CalculateAngle(Vector2 mouseVector)
    {
        float angleInDegrees;

        angleInDegrees = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        if (angleInDegrees < 0)
            angleInDegrees += 365;

        return angleInDegrees;
    }
    void SendHitPosition(float xValue, float damage, Direction direction)
    {
        manager.RecievePlayerHit(xValue, damage, direction);
    }
    void Cross()
    {
        float hitPosition = transform.position.x;
        print("Cross");
        SendHitPosition(hitPosition, crossDamage, Direction.Center);
        FinishAttack();
    }
    void RightHook()
    {
        print("Right Hook");
        float hitPosition = transform.position.x - 0.5f;
        SendHitPosition(hitPosition, rightHookDamage, Direction.Right);
        FinishAttack();
    }
    void LeftHook()
    {
        print("Left Hook");
        float hitPosition = transform.position.x + 0.5f;
        SendHitPosition(hitPosition, leftHookDamage, Direction.Left);
        FinishAttack();
    }
    void Uppercut()
    {
        print("Uppercut");
        float hitPosition = transform.position.x;
        SendHitPosition(hitPosition, uppercutDamage, Direction.Down);
        FinishAttack();
    }
    void Hammer()
    {
        print("Hammer");
        float hitPosition = transform.position.x;
        SendHitPosition(hitPosition, hammerDamage, Direction.Up);
        FinishAttack();
    }

    void Block()
    {
        isBlocking = true;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition -= screenCenter;
        return mousePosition;
    }
}

