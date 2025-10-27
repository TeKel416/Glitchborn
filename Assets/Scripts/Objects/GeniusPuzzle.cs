using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GeniusPuzzle : MonoBehaviour
{
    private bool allowTry = false;

    [Header("Tempo de mostrar resposta")]
    public float timer = 1f;

    [Header("Ordem dos Botoes")]
    public GameObject[] buttons;

    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private List<GameObject> buttonsPressed = new List<GameObject>();

    [Header("Porta")]
    public GameObject portaSaida;
    public GameObject portaEntrada;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            sprites.Add(buttons[i].GetComponent<SpriteRenderer>());
        }
    }

    IEnumerator PlayAnswer()
    {
        allowTry = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            TurnWhite(sprites[i]);
            yield return new WaitForSeconds(timer);
            TurnBlack(sprites[i]);
        }

        allowTry = true;
    }

    public void ShowAnswer() => StartCoroutine(PlayAnswer());

    private void TurnWhite(SpriteRenderer sprite)
    {
        sprite.color = Color.white;
    }

    private void TurnBlack(SpriteRenderer sprite)
    {
        sprite.color = Color.black;
    }

    public void AddButtonToList(GameObject button)
    {
        if (buttonsPressed.Contains(button) || !allowTry) return;

        buttonsPressed.Add(button);
        TurnWhite(button.GetComponent<SpriteRenderer>());

        if (buttonsPressed.Count == buttons.Length) 
        {
            if (!VerifyAnswer())
            {
                foreach (GameObject i in buttonsPressed)
                {
                    TurnBlack(i.GetComponent<SpriteRenderer>());
                }
                buttonsPressed.Clear();
                StartCoroutine(PlayAnswer());
            }
            else
            {
                portaSaida.SetActive(false);
                if (portaEntrada != null) portaEntrada.SetActive(false);
            }
        }
    }

    private bool VerifyAnswer()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != buttonsPressed[i]) return false;
        }

        return true;
    }
}
