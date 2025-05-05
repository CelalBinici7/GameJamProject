using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Takip Ayarları")]
    [Tooltip("Takip edilecek nesne (Player)")]
    public Transform target;

    [Tooltip("Takip yumuşaklığı (Ne kadar büyükse o kadar yavaş takip eder)")]
    [Range(0.1f, 10f)]
    public float smoothTime = 0.5f;

    [Tooltip("Kamera ofseti (X,Y)")]
    public Vector2 offset = new Vector2(0f, 1f);

    [Header("Sınırlar")]
    [Tooltip("Kamera sınırlarını aktif et")]
    public bool useBounds = false;

    [Tooltip("Minimum kamera pozisyonu")]
    public Vector2 minBounds;

    [Tooltip("Maksimum kamera pozisyonu")]
    public Vector2 maxBounds;

    [Header("Gelişmiş Ayarlar")]
    [Tooltip("Pixel perfect kamera (Pixel art oyunlar için)")]
    public bool pixelPerfect = false;

    [Tooltip("Pixel perfect için birim başına pixel sayısı")]
 
    public int pixelsPerUnit = 32;

    private Vector3 _currentVelocity;
    private Camera _camera;
    private float _initialZ;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _initialZ = transform.position.z;

        // 2D oyun için gerekli kamera ayarları
        _camera.orthographic = true;

        if (pixelPerfect)
        {
            SetPixelPerfect();
        }

        // FPS sınırı (isteğe bağlı)
        Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Kamera takip hedefi atanmamış!");
            return;
        }

        FollowTarget();
    }

    private void FollowTarget()
    {
       
        Vector3 targetPosition = (Vector3)(offset) + target.position;
        targetPosition.z = _initialZ;

       
        Vector3 newPosition = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            smoothTime);
  
        if (useBounds)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = newPosition;
    }

    private void SetPixelPerfect()
    {
        if (_camera.orthographic)
        {
            float screenRatio = (float)Screen.width / Screen.height;
            float targetRatio = Screen.width / (float)Screen.height;

            if (screenRatio >= targetRatio)
            {
                _camera.orthographicSize = Screen.height / (2f * pixelsPerUnit);
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                _camera.orthographicSize = Screen.height / (2f * pixelsPerUnit) * differenceInSize;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (pixelPerfect)
        {
            SetPixelPerfect();
        }
    }
#endif

    private void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.green;

            Vector3 center = new Vector3(
                (minBounds.x + maxBounds.x) * 0.5f,
                (minBounds.y + maxBounds.y) * 0.5f,
                0);

            Vector3 size = new Vector3(
                maxBounds.x - minBounds.x,
                maxBounds.y - minBounds.y,
                1);

            Gizmos.DrawWireCube(center, size);
        }
    }

}