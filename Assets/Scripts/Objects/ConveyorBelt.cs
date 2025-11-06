using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 10;
    public Direction direction = Direction.Right;
    public Transform center;
    public Transform end;

    void OnTriggerEnter2D(Collider2D hit)
    {
        if(hit.CompareTag("Player"))
        {
            hit.GetComponent<PlayerController>().StartCoroutine(
                hit.GetComponent<PlayerController>().EnterConveyorBelt(direction, center.position, end.position, speed));
        }
    }
}

public enum Direction { Up, Down, Left, Right }