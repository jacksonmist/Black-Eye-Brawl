using UnityEngine;

public class CalculateBrachium : MonoBehaviour
{
    public Transform rightBrachium;
    public Transform leftBrachium;

    public Transform rightShoulder;
    public Transform leftShoulder;

    public Transform rightElbow;
    public Transform leftElbow;

    Vector3 rightMidpoint;
    Vector3 leftMidpoint;

    Vector3 rightVector;
    Vector3 leftVector;


    void Start()
    {

    }

    void Update()
    {
        MoveBrachium();
    }

    void CalculateMidpoints()
    {
        rightMidpoint = (rightShoulder.position + rightElbow.position) / 2;
        leftMidpoint = (leftShoulder.position + leftElbow.position) / 2;
    }
    void CalculateDirections()
    {
        rightVector = (rightElbow.position - rightShoulder.position).normalized;
        leftVector = (leftElbow.position - leftShoulder.position).normalized;   
    }
    void MoveBrachium()
    {
        CalculateMidpoints();
        CalculateDirections();

        rightBrachium.position = rightMidpoint;
        leftBrachium.position = leftMidpoint;

        rightBrachium.up = rightVector;
        leftBrachium.up = leftVector;
    }
}
