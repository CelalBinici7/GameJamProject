using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Hedef Portal")]
    public Transform hedefPortal;

    [Header("I��nlama Bekleme S�resi")]
    public float teleportCooldown = 0.5f;

    private bool teleportEdildi = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !teleportEdildi)
        {
            // I��nla
            other.transform.position = hedefPortal.position;

            // Geri ���nlanmay� engellemek i�in hedef portala "cooldown" ba�lat
            Portal hedefPortalScript = hedefPortal.GetComponent<Portal>();
            if (hedefPortalScript != null)
            {
                hedefPortalScript.BeklemeBaslat();
            }
        }
    }

    public void BeklemeBaslat()
    {
        teleportEdildi = true;
        Invoke(nameof(BeklemeBitir), teleportCooldown);
    }

    private void BeklemeBitir()
    {
        teleportEdildi = false;
    }
}
