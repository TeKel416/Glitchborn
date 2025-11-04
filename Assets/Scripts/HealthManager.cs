using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    private float sectionSize = 0.125f; // tamanho de cada barra da barra de vida
    private float healthAmount = 1; // fill da imagem

    public void TakeDamage(float damage)
    {
        if (healthAmount > 0)
        {
            healthAmount -= damage * sectionSize;
            healthBar.fillAmount = healthAmount;
        }

        if (healthAmount < 0)
        {
            healthAmount = 0;
        }
    }

    public void Heal(float heal)
    {
        if (healthAmount < 1)
        {
            healthAmount += heal * sectionSize;
            healthBar.fillAmount = healthAmount;
        }
    }
}
