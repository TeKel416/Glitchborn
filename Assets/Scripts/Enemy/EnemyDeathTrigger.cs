using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    public List<GameObject> inimigosArea = new List<GameObject>();
    public GameObject portaEntrada;
    public GameObject portaSaida;

    private void FixedUpdate()
    {
        CheckEnemiesAreDead();
    }

    public void CheckEnemiesAreDead()
    {
        if (inimigosArea.Count > 0)
        {
            foreach (GameObject enemy in inimigosArea)
            {
                if (enemy.IsDestroyed())
                {
                    inimigosArea.Remove(enemy);
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
