using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    AudioSource mainThemeSource;

    public AudioClip mainTheme;

    public AudioClip fightSound;
    public AudioClip hitSound;
    public AudioClip groanSound;
    public AudioClip ringSound;
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
        audioSource.PlayOneShot(mainTheme);
    }

    void Update()
    {
        
    }

    public void PlayFightSound()
    {
        audioSource.PlayOneShot(fightSound);
        audioSource.PlayOneShot(ringSound);
    }
    public void PlayerHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }
    public void PlayerGroanSound()
    {
        audioSource.PlayOneShot(groanSound);
    }

}
