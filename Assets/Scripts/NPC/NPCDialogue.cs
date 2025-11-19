using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public float dialogueRange;
    public LayerMask playerLayer;

    public string speakerName;
    public Sprite speakerSprite;
    public string[] sentences;

    private PlayerController player;
    private bool playerHit;
    public GameObject indicator;


    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (playerHit && player.interact)
        {
            if (!DialogueController.instance.isShowing)
            {
                DialogueController.instance.Speak(speakerName, speakerSprite, sentences);
            }
            else
            {
                DialogueController.instance.NextSentence();
            }
        }
    }

    void FixedUpdate()
    {
        ShowDialogue();
    }

    void ShowDialogue()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, dialogueRange, playerLayer);

        if (hit != null)
        {
            playerHit = true;
            indicator.SetActive(true);
        }
        else
        {
            playerHit = false;
            indicator.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, dialogueRange);
    }
}
