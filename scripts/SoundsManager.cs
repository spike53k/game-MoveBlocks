using UnityEngine;

public class SoundsManager : MonoBehaviour 
{
    public static SoundsManager instance;
    public AudioSource musicSource;

    void Awake()
    {
        // Если менеджер уже существует - уничтожаем дубликат
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        // Сохраняем себя как единственный экземпляр
        instance = this;
        DontDestroyOnLoad(gameObject); // Не уничтожаем между сценами
        
        // Запускаем музыку если она не играет
        if (!musicSource.isPlaying)
            musicSource.Play();
    }
}