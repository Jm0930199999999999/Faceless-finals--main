using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip BGMusic;

    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BGMusic != null)
        {
            PlayBackgrounMusic(false, BGMusic);
        }
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
    }

    public static void SetVolume(float volume)
    {
        Instance.audioSource.volume = volume;
    }

    public void PlayBackgrounMusic(bool resetMusic, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }

        if (audioSource.clip != null)
        {
            if (resetMusic)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }
}