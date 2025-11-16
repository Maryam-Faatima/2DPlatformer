

using UnityEngine;

public class soundManagement : MonoBehaviour
{
    public static soundManagement instance; // Optional singleton access
    public AudioSource audioSource;


    public AudioClip jumpSound;
    public AudioClip fireSound;
    public AudioClip explosionSound;

    void Awake()
    {
        // Optional singleton pattern (easy access from anywhere)
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            Debug.LogError("[soundManagement] Missing AudioSource!");
    }

    public void playJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
        // PlayClip(jumpSound, "jumpSound");
    }

    public void playFireSound()
    {
        audioSource.PlayOneShot(fireSound);
        //PlayClip(fireSound, "fireSound");
    }

    public void playExplosionSound()
    {
        audioSource.PlayOneShot(explosionSound);
        //PlayClip(explosionSound, "explosionSound");
    }


}

