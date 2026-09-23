using UnityEngine;

public class Touchable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Touchable"))
        {
            AudioSource source = other.GetComponent<AudioSource>();
            if (source != null && !source.isPlaying)
            {
                source.Play();
            }
        }
    }
}