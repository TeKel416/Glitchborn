using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogueObj; // janela do dialogo
    public Image profileSprite; // sprite do perfil
    public Text speechText; // texto da fala
    public Text actorNameText; // nome do npc

    [Header("Settings")]
    public float typingSpeed; // tempo para escrever as letras da fala

    #region Variaveis de Controle

    private bool isShowing; // visibilidade da janela
    private int index; // index das falas
    private string[] sentences;

    #endregion

    public static DialogueController instance;


    public void Awake()
    {
        instance = this;
    }

    IEnumerator TypeSentence()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
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
            }
        }
    }

    // chamar a fala
    public void Speak(string[] txt)
    {
        if (!isShowing)
        {
            dialogueObj.SetActive(true);
            sentences = txt;
            StartCoroutine(TypeSentence());
            isShowing = true;
        }
    }
}
