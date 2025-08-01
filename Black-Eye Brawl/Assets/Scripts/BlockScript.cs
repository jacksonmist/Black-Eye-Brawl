using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour
{
    bool isBlocked;
    bool isBlocking;

    public float armSpeed;

    public Vector3 rightTargetHomePosition;
    public Vector3 leftTargetHomePosition;

    public Transform rightArmTarget;
    public Transform leftArmTarget;

    public Transform rightFist;
    public Transform leftFist;

    [Header("Block Targets")]
    public Vector3 centerRight = new Vector3(0.1f, 0.2f, 6f);
    public Vector3 centerLeft = new Vector3(-0.1f, 0.2f, 6f);

    public Vector3 rightBlockRightArm = new Vector3(0.59f, 0.32f, 5.8f);

    public Vector3 leftBlockLeftArm = new Vector3(-0.75f, 0.3f, 7.4f);

    public Vector3 upBlockRightArm = new Vector3(0.17f, 0.46f, 4.7f);
    public Vector3 upBlockLeftArm = new Vector3(-0.1f, 0.48f, 5.89f);
    public Vector3 upRightRotationTarget = new Vector3(0f, 180f, 67f);
    public Vector3 upLeftRotationTarget = new Vector3(0f, 180f, -61f);

    public Vector3 downBlockRightArm = new Vector3(0.4f, 0.39f, 5.8f);
    public Vector3 downBlockLeftArm = new Vector3(-0.4f, 0.29f, 7.4f);
    public Vector3 downRightRotationTarget = new Vector3(0f, 180f, 85f);
    public Vector3 downLeftRotationTarget = new Vector3(0f, 180f, -85f);

    public Quaternion homeRotationTarget;

    public Quaternion rightRotationTarget;
    public Quaternion leftRotationTarget;
    public float rotationSpeed = 10;

    bool targetsEnabled = true;
    void Start()
    {
        rightTargetHomePosition = rightArmTarget.localPosition;
        leftTargetHomePosition = leftArmTarget.localPosition;

        rightArmTarget.localPosition = rightTargetHomePosition;
        leftArmTarget.localPosition = leftTargetHomePosition;

        homeRotationTarget = rightFist.rotation;
        targetsEnabled = true;
    }

    void Update()
    {
        if(targetsEnabled)
            ArmsToTarget();
    }
    void FinishBlock()
    {
        isBlocking = false;
        rightArmTarget.localPosition = rightTargetHomePosition;
        leftArmTarget.localPosition = leftTargetHomePosition;
        ResetArmRotationTarget();
    }
    void ResetArmRotationTarget()
    {
        rightRotationTarget = homeRotationTarget;
        leftRotationTarget = homeRotationTarget;
    }
    void ArmsToTarget()
    {
        rightFist.position = Vector3.Lerp(rightFist.position, rightArmTarget.position, armSpeed * Time.deltaTime);
        leftFist.position = Vector3.Lerp(leftFist.position, leftArmTarget.position, armSpeed * Time.deltaTime);

        rightFist.rotation = Quaternion.Slerp(rightFist.rotation, rightRotationTarget, rotationSpeed * Time.deltaTime);
        leftFist.rotation = Quaternion.Slerp(leftFist.rotation, leftRotationTarget, rotationSpeed * Time.deltaTime);

        float rightDistance = Vector3.Distance(rightFist.position, rightArmTarget.position);
        float leftDistance = Vector3.Distance(leftFist.position, leftArmTarget.position);

        if (rightDistance < 0.2f && leftDistance < 0.2f && isBlocking)
            isBlocked = true;
        else
            isBlocked = false;
    }
    public void DisableTargets()
    {
        targetsEnabled = false;
    }
    public void EnableTargets()
    {
        targetsEnabled = true;
    }
    public void BlockDirection(Direction direction)
    {
        StartCoroutine(BlockTimer());
        switch (direction)
        {
            case Direction.Up:
                UpBlock();
                break;
            case Direction.Down:
                DownBlock();
                break;
            case Direction.Left:
                RightBlock();
                break;
            case Direction.Right:
                LeftBlock();
                break;
            case Direction.Center:
                CenterBlock();
                break;
            case Direction.None:
                FinishBlock();
                break;
        }
    }
    IEnumerator BlockTimer()
    {
        yield return new WaitForSeconds(2);
        if(isBlocking)
        {
            FinishBlock();
        }
    }
    void CenterBlock()
    {
        rightArmTarget.localPosition = centerRight;
        leftArmTarget.localPosition = centerLeft;

        ResetArmRotationTarget();
    }
    void RightBlock()
    {
        rightArmTarget.localPosition = rightBlockRightArm;
        leftArmTarget.localPosition = leftTargetHomePosition;

        ResetArmRotationTarget();
    }
    void LeftBlock()
    {
        leftArmTarget.localPosition = leftBlockLeftArm;
        rightArmTarget.localPosition = rightTargetHomePosition;

        ResetArmRotationTarget();
    }
    void UpBlock()
    {
        rightArmTarget.localPosition = upBlockRightArm;
        leftArmTarget.localPosition = upBlockLeftArm;

        rightRotationTarget.eulerAngles = upRightRotationTarget;
        leftRotationTarget.eulerAngles = upLeftRotationTarget;
    }
    void DownBlock()
    {
        rightArmTarget.localPosition = downBlockRightArm;
        leftArmTarget.localPosition = downBlockLeftArm;

        rightRotationTarget.eulerAngles = downRightRotationTarget;
        leftRotationTarget.eulerAngles = downLeftRotationTarget;
    }
}
