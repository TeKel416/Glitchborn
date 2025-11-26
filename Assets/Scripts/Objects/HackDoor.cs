using UnityEngine;
using System.Collections.Generic;

public class HackDoor : MonoBehaviour
{
    private PlayerController player;
    public LayerMask playerLayer;
    private bool playerHit;
    public float hackRange = 2;
    public GameObject door;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        CheckHackRange();

        if (playerHit && player.hack)
        {
            SoundManager.instance.PlaySound2D("Door");
            Destroy(door);
        }
    }

    void CheckHackRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, hackRange, playerLayer);

        if (hit != null)
        {
            playerHit = true;
        }
        else
        {
            playerHit = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, hackRange);
    }
}
