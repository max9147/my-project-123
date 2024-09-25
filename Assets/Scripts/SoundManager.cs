using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _correctSound;
    [SerializeField] private AudioSource _failSound;
    [SerializeField] private AudioSource _whooshSound;
    [SerializeField] private AudioSource _wrongSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlayCorrectSound()
    {
        _correctSound.Play();
    }

    public void PlayFailSound()
    {
        _failSound.Play();
    }

    public void PlayWhooshSound()
    {
        _whooshSound.Play();
    }

    public void PlayWrongSound()
    {
        _wrongSound.Play();
    }
}