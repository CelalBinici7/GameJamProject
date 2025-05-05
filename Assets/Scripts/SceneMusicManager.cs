using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicManager : MonoBehaviour
{
    public static SceneMusicManager Instance;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
        [Range(0f, 1f)] public float volume = 0.5f;
        public bool loop = true;
        public bool fadeTransition = true;
        public float fadeDuration = 1f;
    }

    [Header("Sahne Müzikleri")]
    public SceneMusic[] sceneMusics;

    private AudioSource audioSource;
    private AudioSource nextAudioSource; // Çapraz geçiþ için
    private float targetVolume;
    private bool isFading;

    private void Awake()
    {
        // Singleton pattern uyguluyoruz
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeAudioSources()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        nextAudioSource = gameObject.AddComponent<AudioSource>();
        nextAudioSource.playOnAwake = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    public void PlaySceneMusic(string sceneName)
    {
        SceneMusic sceneMusic = GetSceneMusic(sceneName);

        if (sceneMusic == null)
        {
            Debug.LogWarning($"{sceneName} sahnesi için müzik bulunamadý!");
            return;
        }

        if (audioSource.isPlaying && audioSource.clip == sceneMusic.musicClip)
            return;

        if (sceneMusic.fadeTransition && audioSource.isPlaying)
        {
            StartCoroutine(CrossFade(sceneMusic));
        }
        else
        {
            audioSource.clip = sceneMusic.musicClip;
            audioSource.volume = sceneMusic.volume;
            audioSource.loop = sceneMusic.loop;
            audioSource.Play();
        }
    }

    private IEnumerator CrossFade(SceneMusic newSceneMusic)
    {
        isFading = true;

        // Yeni müziði ikinci audio source'a yükle
        nextAudioSource.clip = newSceneMusic.musicClip;
        nextAudioSource.volume = 0f;
        nextAudioSource.loop = newSceneMusic.loop;
        nextAudioSource.Play();

        float timer = 0f;
        float fadeDuration = newSceneMusic.fadeDuration;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            // Eski müziði fade out
            audioSource.volume = Mathf.Lerp(newSceneMusic.volume, 0f, progress);

            // Yeni müziði fade in
            nextAudioSource.volume = Mathf.Lerp(0f, newSceneMusic.volume, progress);

            yield return null;
        }

        // AudioSource'larý swap et
        AudioSource temp = audioSource;
        audioSource = nextAudioSource;
        nextAudioSource = temp;

        nextAudioSource.Stop();
        isFading = false;
    }

    private SceneMusic GetSceneMusic(string sceneName)
    {
        foreach (SceneMusic sceneMusic in sceneMusics)
        {
            if (sceneMusic.sceneName == sceneName)
            {
                return sceneMusic;
            }
        }
        return null;
    }

    public void StopMusic()
    {
        audioSource.Stop();
        if (nextAudioSource.isPlaying)
            nextAudioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        if (!isFading)
        {
            audioSource.volume = volume;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
