using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    public List<GameObject> inimigosArea = new List<GameObject>();
    private List<GameObject> inimigosMortos = new List<GameObject>();
    public GameObject portaEntrada;
    public GameObject portaSaida;

    private void FixedUpdate()
    {
        CheckEnemiesAreDead();
    }

    public void CheckEnemiesAreDead()
    {
        if (inimigosMortos.Count != inimigosArea.Count)
        {
            foreach (GameObject enemy in inimigosArea)
            {
                if (enemy.IsDestroyed() && !inimigosMortos.Contains(enemy))
                {
                    inimigosMortos.Add(enemy);
                }
            }
        }
        else
        {
            portaEntrada.SetActive(false);
            portaSaida.SetActive(false);
        }
    }
}
