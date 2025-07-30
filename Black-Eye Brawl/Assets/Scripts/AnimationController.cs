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

        opponentAnimator.SetTrigger("Right Hook");
    }
    public void LeftHook()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetTrigger("Left Hook");
    }
    public void Cross()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetTrigger("Cross");
    }
    public void Uppercut()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetTrigger("Uppercut");
    }
    public void Hammer()
    {
        opponentAnimator.SetBool("isWalking", false);
        opponentAnimator.SetBool("isIdle", true);

        opponentAnimator.SetTrigger("Hammer");
    }
}
