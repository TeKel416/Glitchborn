using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public PlayerController player;

    [Header("Elementos da Caixa de Diálogo")]
    public GameObject dialogueObj; // janela do dialogo
    public Image profileSprite; // sprite do perfil
    public Text speechText; // texto da fala
    public Text actorNameText; // nome do npc

    [Header("Settings")]
    public float typingSpeed; // tempo para escrever as letras da fala

    #region Variaveis de Controle
    public bool isShowing; // visibilidade da janela
    public int index; // index das falas
    public string[] sentences;
    #endregion

    public static DialogueController instance;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    IEnumerator TypeSentence()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // chamar a fala
    public void Speak(string speakerName, Sprite speakerSprite, string[] txt)
    {
        if (!isShowing)
        {
            SceneLoader.ShowMouseCursor(true);
            dialogueObj.SetActive(true);
            sentences = txt;
            actorNameText.text = speakerName;
            profileSprite.sprite = speakerSprite;
            StartCoroutine(TypeSentence());
            isShowing = true;

            // bloqueia o player
            player.locked = true;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("IsWalking", false);
        }
    }

    // pular pra proxima frase/fala
    public void NextSentence()
    {
        if (speechText.text == sentences[index]) // impedir de pular a frase antes do npc terminar de falar
        {
            if (index < sentences.Length - 1) // verifica se ainda nao acabaram todas as falas
            {
                index++;
                speechText.text = "";
                StartCoroutine(TypeSentence());
            }
            else // quando terminam as falas
            {
                speechText.text = "";
                index = 0;
                dialogueObj.SetActive(false);
                sentences = null;
                isShowing = false;
                SceneLoader.ShowMouseCursor(false);

                // desbloqueia o player
                player.locked = false;
                player.interact = false;
            }
        }
    }
}
