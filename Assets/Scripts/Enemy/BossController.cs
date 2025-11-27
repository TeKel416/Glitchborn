using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject bossArmPrefab;
    public GameObject bracos;
    public GameObject endScreen;
    public Animator anim;
    public Transform[] tpPoints;
    private int nextTpPoint = 0;
    public Transform roomCenter;

    public float getHitDelay = 0.2f;
    public float hp = 12;
    public float knockbackForce = 25f;

    protected PlayerController player;
    public bool locked = false;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    public void EnterGetHit(float dealtDamage)
    {
        locked = true;
        hp -= dealtDamage;
        VerifyHp();
        anim.SetBool("IsHurting", true);
        SoundManager.instance.PlaySound3D("Glitch", transform.position);

        CancelInvoke("Teleport");
        Invoke("Teleport", getHitDelay);

        CancelInvoke("Unlock");
        Invoke("Unlock", getHitDelay);
    }

    private void VerifyHp()
    {
        if (hp >= 5)
        {
            SpawnArm();

        } 
        else if (hp < 5)
        {
            SpawnArm();
            Invoke("SpawnArm", 0.5f);
        }
        else if (hp <= 0)
        {
            SoundManager.instance.PlaySound2D("Death");
            Destroy(bracos);
            Destroy(gameObject);
            endScreen.SetActive(true);
        }
    }

    private void SpawnArm()
    {
        GameObject bossArm = Instantiate(bossArmPrefab, roomCenter.position, roomCenter.rotation, bracos.transform);
    }

    private void Teleport()
    {
        transform.position = tpPoints[nextTpPoint].position;
        nextTpPoint++;
        if (nextTpPoint >= tpPoints.Length) nextTpPoint = 0;
    }

    void Unlock()
    {
        anim.SetBool("IsHurting", false);
        locked = false;
    }
}
