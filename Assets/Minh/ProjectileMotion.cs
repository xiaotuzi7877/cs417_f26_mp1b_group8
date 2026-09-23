using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public Vector3 velocity;
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}