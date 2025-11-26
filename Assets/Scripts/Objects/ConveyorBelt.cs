using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 10;
    public Direction direction = Direction.Right;
    public Transform center;
    public Transform end;

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound2D("ConveyorBelt");
            hit.GetComponent<PlayerController>().StartCoroutine(
                hit.GetComponent<PlayerController>().EnterConveyorBelt(direction, center.position, end.position, speed));
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