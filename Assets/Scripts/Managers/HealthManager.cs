using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public List<GameObject> healthBar = new List<GameObject>();

    public void TakeDamage(float playerHp) => healthBar[(int)playerHp-1].SetActive(false);

    public void Heal(float playerHp) => healthBar[(int)playerHp-1].SetActive(true);
}
