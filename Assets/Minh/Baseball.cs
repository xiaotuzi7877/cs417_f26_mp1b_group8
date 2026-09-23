using UnityEngine;

public class BaseballOrbit : MonoBehaviour
{
    public Transform attractor;
    public double gravity = 0.2;
    private Vector3 velocity;

    public Vector3 roomMin = new Vector3(-7.4f, 0.2f, -7.4f);
    public Vector3 roomMax = new Vector3(7.4f, 14.8f, 7.4f);

    void Start()
    {
        Vector3 relativePos = transform.position - attractor.position;
        double distance = relativePos.magnitude;
        float speed = (float)System.Math.Sqrt(gravity / distance);
        velocity = Vector3.Cross(relativePos.normalized, Vector3.up) * speed;
    }

    void Update()
    {
        Vector3 relativePos = transform.position - attractor.position;
        double distance = relativePos.magnitude;

        double ax = -gravity * relativePos.x / System.Math.Pow(distance, 3);
        double ay = -gravity * relativePos.y / System.Math.Pow(distance, 3);
        double az = -gravity * relativePos.z / System.Math.Pow(distance, 3);

        velocity.x += (float)(ax * Time.deltaTime);
        velocity.y += (float)(ay * Time.deltaTime);
        velocity.z += (float)(az * Time.deltaTime);

        Vector3 nextPosition = transform.position + velocity * Time.deltaTime;

        if (nextPosition.x < roomMin.x || nextPosition.x > roomMax.x)
        {
            velocity.x = -velocity.x;
        }
        if (nextPosition.y < roomMin.y || nextPosition.y > roomMax.y)
        {
            velocity.y = -velocity.y;
        }
        if (nextPosition.z < roomMin.z || nextPosition.z > roomMax.z)
        {
            velocity.z = -velocity.z;
        }

        nextPosition = transform.position + velocity * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(nextPosition.x, roomMin.x, roomMax.x),
            Mathf.Clamp(nextPosition.y, roomMin.y, roomMax.y),
            Mathf.Clamp(nextPosition.z, roomMin.z, roomMax.z)
        );
    }
}