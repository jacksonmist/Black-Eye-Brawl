using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator opponentAnimator;
    public Animator playerAnimator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToWalk()
    {
        opponentAnimator.SetBool("isWalking", true);
        opponentAnimator.SetBool("isIdle", false);
    }
    public void SwitchToIdle()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);
    }
    public void RightHook()
    {
        print("RH");
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Right Hook", true);
    }
    public void LeftHook()
    {
        print("LH");
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Left Hook", true);
    }
    public void Cross()
    {
        print("C");
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Cross", true);
    }
    public void Uppercut()
    {
        print("U");
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Uppercut", true);
    }
    public void Hammer()
    {
        print("H");
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Hammer", true);
    }
    public void EndAnimation()
    {
        opponentAnimator.SetBool("Right Hook", false);
        opponentAnimator.SetBool("Left Hook", false);
        opponentAnimator.SetBool("Cross", false);
        opponentAnimator.SetBool("Uppercut", false);
        opponentAnimator.SetBool("Hammer", false);
    }
}
