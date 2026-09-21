using System;
using UnityEngine;

public class CometOrbit : MonoBehaviour
{
    public double gravity = 0.2;

    [SerializeField] private Transform attractor;

    private Vector3 velocity = new Vector3(0f, 0f, 0.2f);

    void Update()
    {
        if (attractor == null)
        {
            return;
        }

        Vector3 position = transform.position;
        Vector3 attractorPosition = attractor.position;

        double distance = Math.Sqrt(
            Math.Pow(position.x - attractorPosition.x, 2) +
            Math.Pow(position.y - attractorPosition.y, 2) +
            Math.Pow(position.z - attractorPosition.z, 2)
        );
        distance = Math.Max(distance, 0.5);

        float ax = (float)(-gravity * (position.x - attractorPosition.x) / Math.Pow(distance, 3));
        float ay = (float)(-gravity * (position.y - attractorPosition.y) / Math.Pow(distance, 3));
        float az = (float)(-gravity * (position.z - attractorPosition.z) / Math.Pow(distance, 3));

        velocity.x = velocity.x + ax * Time.deltaTime;
        velocity.y = velocity.y + ay * Time.deltaTime;
        velocity.z = velocity.z + az * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
    }
}
