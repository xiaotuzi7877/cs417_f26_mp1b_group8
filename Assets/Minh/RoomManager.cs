using UnityEngine;

public class Room2Manager : MonoBehaviour
{
    [Header("Sockets")]
    public ClockLock clock1;
    public ClockLock clock2;
    public ClockLock clock3;

    [Header("Exit Door / Reveal")]
    public GameObject doorToRoom3; 
    public AudioSource successSound; 

    private bool roomCleared = false;

    void Update()
    {
        if (roomCleared) return;
        if (clock1.isSolved && clock2.isSolved && clock3.isSolved)
        {
            ClearRoom();
        }
    }

    void ClearRoom()
    {
        roomCleared = true;
        Debug.Log("Room 2 Cleared!");

        if (successSound != null) successSound.Play();
        if (doorToRoom3 != null)
        {
            doorToRoom3.SetActive(false); 
        }
    }
}