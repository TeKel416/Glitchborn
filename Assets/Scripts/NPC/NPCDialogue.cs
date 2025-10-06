using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public float dialogueRange;
    public LayerMask playerLayer;

    public DialogueSettings dialogue;
    private List<string> sentences = new List<string>();

    private PlayerController player;
    private bool playerHit;


    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        GetNPCInfo();
    }

    private void Update()
    {
        if (playerHit && player.interact)
        {
            DialogueController.instance.Speak(sentences.ToArray());
        }
    }

    void FixedUpdate()
    {
        ShowDialogue();
    }

    void GetNPCInfo()
    {
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
        }
        else
        {
            playerHit = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, dialogueRange);
    }
}
