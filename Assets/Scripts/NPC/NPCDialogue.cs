using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public float dialogueRange;
    public LayerMask playerLayer;

    public DialogueSettings dialogue;
    private List<string> sentences = new List<string>();

    private PlayerController player;
    private bool playerHit;
    public GameObject indicator;


    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        GetNPCInfo();
    }

    private void Update()
    {
        if (playerHit && player.interact)
        {
            if (!DialogueController.instance.isShowing)
            {
                DialogueController.instance.Speak(sentences.ToArray());
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

    void GetNPCInfo()
    {
        DialogueController.instance.profileSprite.sprite = dialogue.speakerSprite;
        DialogueController.instance.actorNameText.text = dialogue.dialogues[0].actorName;

        for (int i = 0; i < dialogue.dialogues.Count; i++)
        {
            switch (DialogueController.instance.languague)
            {
                case DialogueController.idiom.en:
                    sentences.Add(dialogue.dialogues[i].sentence.english);
                    break;

                default:
                    sentences.Add(dialogue.dialogues[i].sentence.portuguese);
                    break;
            }
        }
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
