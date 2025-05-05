using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject okPrefab; // Sahnedeki ok prefab'ı
    public float atisAraligi = 2f; // Ok atma aralığı (saniye)
    public float okHizi = 5f; // Okun hareket hızı

    private float zamanSayaci = 0f;

    void Update()
    {
        // Zaman sayacını güncelle
        zamanSayaci += Time.deltaTime;

        // Eğer 2 saniye geçtiyse
        if (zamanSayaci >= atisAraligi)
        {
            OkAt();
            zamanSayaci = 0f; // Sayacı sıfırla
        }
    }

    void OkAt()
    {
        // Yeni bir ok oluştur
        GameObject yeniOk = Instantiate(okPrefab, transform.position, Quaternion.identity);

        // Okun Rigidbody2D component'ini al
        Rigidbody2D okRigidbody = yeniOk.GetComponent<Rigidbody2D>();

        // Ok sola doğru hareket etsin
        okRigidbody.linearVelocity = Vector2.left * okHizi;

        // Ok belirli bir süre sonra yok olabilir (opsiyonel)
        Destroy(yeniOk, 5f);
    }
}
