using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeniusPuzzle : MonoBehaviour
{
    private bool allowTry = false;

    [Header("Tempo de mostrar resposta")]
    public float timer = 1f;

    [Header("Ordem dos Botoes")]
    public GameObject[] buttons;

    private List<GameObject> buttonsPressed = new List<GameObject>();

    [Header("Porta")]
    public GameObject portaSaida;
    public GameObject portaEntrada;

    [Header("Punição")]
    public GameObject punicao;

    IEnumerator PlayAnswer()
    {
        allowTry = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SoundManager.instance.PlaySound3D("UI", transform.position);
            buttons[i].GetComponent<Animator>().SetBool("isOn", true);
            yield return new WaitForSeconds(timer);
            buttons[i].GetComponent<Animator>().SetBool("isOn", false);
        }

        allowTry = true;
    }

    public void ShowAnswer() => StartCoroutine(PlayAnswer());

    public void AddButtonToList(GameObject button)
    {
        if (buttonsPressed.Contains(button) || !allowTry) return;

        buttonsPressed.Add(button);
        button.GetComponent<Animator>().SetBool("isPressed", true); ;

        if (buttonsPressed.Count == buttons.Length) 
        {
            if (!VerifyAnswer())
            {
                foreach (GameObject i in buttonsPressed)
                {
                    i.GetComponent<Animator>().SetBool("isPressed", false);
                }

                buttonsPressed.Clear();
                StartCoroutine(PlayAnswer());
                
                Instantiate(punicao, transform.position, transform.rotation);
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
