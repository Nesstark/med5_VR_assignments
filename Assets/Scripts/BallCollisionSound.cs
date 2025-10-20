using UnityEngine;

public class BallCollisionSound : MonoBehaviour
{
    public AudioClip rimHitSound;
    public AudioClip backboardHitSound;
    public AudioClip courtBounceSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Choose clip based on hit surface
        if (collision.gameObject.CompareTag("Backboard"))
            PlayClip(backboardHitSound, collision.relativeVelocity.magnitude);
        else if (collision.gameObject.CompareTag("Rim"))
            PlayClip(rimHitSound, collision.relativeVelocity.magnitude);
        else if (collision.gameObject.CompareTag("Court"))
            PlayClip(courtBounceSound, collision.relativeVelocity.magnitude);
    }

    void PlayClip(AudioClip clip, float intensity)
    {
        if (clip != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.volume = Mathf.Clamp(intensity / 6f, 0.2f, 1f);
            audioSource.PlayOneShot(clip);
        }
    }
}
