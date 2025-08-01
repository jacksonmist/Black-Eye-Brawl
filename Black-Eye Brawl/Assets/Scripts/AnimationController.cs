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
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Right Hook", true);
    }
    public void LeftHook()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Left Hook", true);
    }
    public void Cross()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Cross", true);
    }
    public void Uppercut()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetBool("Uppercut", true);
    }
    public void Hammer()
    {
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

    public void RightHookPlayer()
    {
        print("hook");
        playerAnimator.SetBool("Right Hook", true);
    }
    public void LeftHookPlayer()
    {
        playerAnimator.SetBool("Left Hook", true);
    }
    public void CrossPlayer()
    {
        playerAnimator.SetBool("Cross", true);
    }
    public void UppercutPlayer()
    {
        playerAnimator.SetBool("Uppercut", true);
    }
    public void HammerPlayer()
    {
        playerAnimator.SetBool("Hammer", true);
    }
    public void EndAnimationPlayer()
    {
        playerAnimator.SetBool("Right Hook", false);
        playerAnimator.SetBool("Left Hook", false);
        playerAnimator.SetBool("Cross", false);
        playerAnimator.SetBool("Uppercut", false);
        playerAnimator.SetBool("Hammer", false);
    }
}
