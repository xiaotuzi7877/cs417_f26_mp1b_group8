using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public float rotationSpeed = 20f;

    void Update()
    {
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f
        );
    }
}