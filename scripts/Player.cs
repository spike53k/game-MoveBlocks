using UnityEngine;                                   // Импорт основного пространства имён
using UnityEngine.SceneManagement;                   // Для управления сценами

public class Player : MonoBehaviour                  // Основной класс игрока
{
    [SerializeField] KeyCode keyOne, keyTwo,         // Клавиши для движения
                      menuKey = KeyCode.E;           // Клавиша меню (по умолчанию E)
    [SerializeField] Vector3 moveDirection;          // Направление движения куба
    [SerializeField] string menuSceneName = "Menu";  // Имя сцены меню
    [SerializeField] bool enableMenuKey = true;      // Включить меню по клавише?
    
    private Rigidbody _rb;                           // Физический компонент
    private bool _isResetting;                       // Защита от двойного рестарта
    private Activator[] _buttons;                    // Кэш кнопок на уровне

    void Start() => _rb = GetComponent<Rigidbody>(); // Получаем Rigidbody при старте

    void FixedUpdate()                               // Физика (50 раз в секунду)
    {
        if (Input.GetKey(keyOne)) _rb.linearVelocity += moveDirection; // Двигаем вперёд
        if (Input.GetKey(keyTwo)) _rb.linearVelocity -= moveDirection; // Двигаем назад
    }
    
    void Update()                                    // Логика (каждый кадр)
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isResetting) ResetGame(); // Рестарт по R
        if (enableMenuKey && Input.GetKeyDown(menuKey)) ReturnToMenu(); // В меню по E
    }
    
    void ResetGame()                                 // Перезагрузка уровня
    {
        _isResetting = true;                         // Блокируем повторное нажатие
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Загружаем сцену заново
    }
    
    void ReturnToMenu()                              // Возврат в главное меню
    {
        if (string.IsNullOrEmpty(menuSceneName)) return; // Проверяем имя сцены
        SceneManager.LoadScene(menuSceneName);       // Загружаем меню
    }

    void OnTriggerEnter(Collider other)              // При входе в триггер
    {
        if (CompareTag("Player") && other.CompareTag("Finish")) LoadNextLevelOrMenu(); // Финиш
        if (CompareTag("Cube") && other.CompareTag("Cube")) SetButtons(false); // Куб+куб = блокировка
    }void OnTriggerExit(Collider other)               // При выходе из триггера
    {
        if (CompareTag("Cube") && other.CompareTag("Cube")) SetButtons(true); // Разблокировка
    }
    
    void SetButtons(bool state)                      // Управление всеми кнопками
    {
        if (_buttons == null) _buttons = FindObjectsByType<Activator>(FindObjectsSortMode.None); // Кэшируем
        foreach (var button in _buttons) button.CanPush = state; // Вкл/выкл каждую кнопку
    }

    void LoadNextLevelOrMenu()                       // Переход на след. уровень или в меню
    {
        int current = SceneManager.GetActiveScene().buildIndex; // Текущий индекс сцены
        if (current + 1 < SceneManager.sceneCountInBuildSettings) // Если есть след. уровень
            SceneManager.LoadScene(current + 1);     // Загружаем его
        else if (!string.IsNullOrEmpty(menuSceneName)) // Иначе если задано меню
            SceneManager.LoadScene(menuSceneName);   // Идём в меню
        else
            ResetGame();                             // Или рестартим уровень
    }
}
