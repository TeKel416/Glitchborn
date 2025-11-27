using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 10;
    public Direction direction = Direction.Right;

    private Collider2D col;
    void Start() => col = GetComponent<Collider2D>();

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound2D("ConveyorBelt");
            hit.GetComponent<PlayerController>().EnterConveyorBelt(direction, col, speed);
        }
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            SoundManager.instance.StopSound();
        }
    }
}

public enum Direction { Up, Down, Left, Right }