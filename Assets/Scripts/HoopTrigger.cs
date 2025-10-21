using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HoopTrigger : MonoBehaviour
{
    [Header("Team Setup")]
    [Tooltip("If true, this hoop belongs to the Home team (so the Away team scores).")]
    public bool isHomeHoop = true;

    [Header("Scoring Settings")]
    [Tooltip("Empty transform at the exact rim center.")]
    public Transform hoopCenter;

    [Tooltip("Points for a normal field goal.")]
    public int twoPointer = 2;

    [Tooltip("Extra point added if beyond threePointRadius (making it a 3-pointer).")]
    public int extraForThree = 1;

    [Tooltip("Radius (in meters) for 3-pointer line. NBA ~7.24m, FIBA ~6.75m")]
    public float threePointRadius = 6.75f;

    [Header("Audio")]
    [Tooltip("Sound played when a basket is scored.")]
    public AudioClip scoreClip;

    [Header("Validation")]
    [Tooltip("Minimum downward velocity (y) to count as a valid 'through from above' basket.")]
    public float minDownwardVelocity = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && scoreClip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Basketball")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && rb.linearVelocity.y > -minDownwardVelocity) return;

        Vector3 ballPos = other.transform.position;
        Vector3 centerPos = hoopCenter ? hoopCenter.position : transform.position;

        // Calculate horizontal distance (ignore Y difference)
        Vector2 ballXZ = new Vector2(ballPos.x, ballPos.z);
        Vector2 centerXZ = new Vector2(centerPos.x, centerPos.z);
        float horizontalDistance = Vector2.Distance(ballXZ, centerXZ);

        int points = twoPointer;
        if (horizontalDistance >= threePointRadius)
            points += extraForThree; // becomes 3-pointer

        // Award points to the opposite team
        if (isHomeHoop)
            ScoreManager.Instance?.AddAwayPoints(points);
        else
            ScoreManager.Instance?.AddHomePoints(points);

        if (scoreClip != null && audioSource != null)
            audioSource.PlayOneShot(scoreClip);

        Debug.Log($"Basket scored! {(isHomeHoop ? "Away" : "Home")} +{points} points (distance {horizontalDistance:F2}m)");
    }
}
