using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;

    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        soundDictionary = new Dictionary<string, AudioClip>();

        if (damageSound != null)
        {
            soundDictionary.Add("Damage", damageSound);
        }
    }

    private void OnEnable()
    {
        PlayerStateMachine.OnPlayerStateChanged += OnPlayerStateChanged;
        EnemyStateMachine.OnEnemyStateChanged += OnEnemyStateChanged;
    }

    private void OnDisable()
    {
        PlayerStateMachine.OnPlayerStateChanged -= OnPlayerStateChanged;
        EnemyStateMachine.OnEnemyStateChanged -= OnEnemyStateChanged;
    }

    private void OnPlayerStateChanged(string stateName)
    {
        Debug.Log("AUDIOMANAGER OBSERVER - Player changed to: " + stateName);

        if (stateName == "Attacking")
        {
            PlaySFX("Damage");
        }
    }

    private void OnEnemyStateChanged(string stateName)
    {
        Debug.Log("AUDIOMANAGER OBSERVER - Enemy changed to: " + stateName);
    }

    public void PlaySFX(string soundName)
    {
        if (soundDictionary != null &&
            soundDictionary.ContainsKey(soundName) &&
            audioSource != null)
        {
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
        else
        {
            Debug.LogWarning("Sound not found or AudioSource missing: " + soundName);
        }
    }
}