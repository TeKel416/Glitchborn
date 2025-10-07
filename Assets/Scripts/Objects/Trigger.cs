using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] string tagFilter;
    [SerializeField] UnityEvent onTriggerEnter2D;
    [SerializeField] UnityEvent onTriggerExit2D;
    [SerializeField] bool destroyOnTriggerEnter2D;
    [SerializeField] bool destroyOnTriggerExit2D;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // verifica se o que entrou na area de colisao possui uma tag especifica
        if (!string.IsNullOrEmpty(tagFilter) && !collision.gameObject.CompareTag(tagFilter)) return;

        onTriggerEnter2D.Invoke();

        // destroi a colisao apos o trigger ter sido acionado
        if (destroyOnTriggerEnter2D)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !collision.gameObject.CompareTag(tagFilter)) return;

        onTriggerExit2D.Invoke();

        // destroi a colisao apos o trigger ter sido acionado
        if (destroyOnTriggerExit2D)
        {
            Destroy(gameObject);
        }
    }
}