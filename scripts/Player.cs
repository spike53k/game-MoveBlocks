using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] KeyCode keyOne;
    [SerializeField] KeyCode keyTwo;
    [SerializeField] Vector3 moveDirection;
    
    // Добавленные переменные для перехода в меню
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private KeyCode menuKey = KeyCode.E;
    [SerializeField] private bool enableMenuKey = true;
    
    private Rigidbody rb;
    private bool isResetting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Кэшируем Rigidbody
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(keyOne))
        {
            rb.linearVelocity += moveDirection;
        }
        if (Input.GetKey(keyTwo))
        {
            rb.linearVelocity -= moveDirection;
        }
    }
    
    private void Update()
    {
        // ПЕРЕНОСИМ в Update и используем GetKeyDown
        if (Input.GetKeyDown(KeyCode.R) && !isResetting)
        {
            ResetGame();
        }
        
        // Добавлен переход в меню по нажатию E
        if (enableMenuKey && Input.GetKeyDown(menuKey))
        {
            ReturnToMenu();
        }
    }
    
    private void ResetGame()
    {
        if (isResetting) return;
        
        isResetting = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    // Добавленный метод для перехода в меню
    private void ReturnToMenu()
    {
        // Проверяем, существует ли сцена меню
        if (string.IsNullOrEmpty(menuSceneName))
        {
            Debug.LogWarning("Menu scene name is not set!");
            return;
        }
        
        // Проверяем, не находимся ли уже в меню
        if (SceneManager.GetActiveScene().name == menuSceneName)
        {
            Debug.Log("Already in menu scene");
            return;
        }
        
        // Проверяем, существует ли такая сцена
        if (!SceneExists(menuSceneName))
        {
            Debug.LogError($"Scene '{menuSceneName}' not found in Build Settings!");
            return;
        }
        
        Debug.Log($"Loading menu: {menuSceneName}");
        SceneManager.LoadScene(menuSceneName);
    }
    
    // Метод для проверки существования сцены
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string scene = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (scene == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Player") && other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if(this.CompareTag("Cube") && other.CompareTag("Cube"))
        {
            // ОПТИМИЗИРУЕМ медленный FindObjectsOfType
            foreach(Activator button in FindObjectsByType<Activator>(FindObjectsSortMode.None))
            {
                button.CanPush = false;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(this.CompareTag("Cube") && other.CompareTag("Cube"))
        {
            foreach(Activator button in FindObjectsByType<Activator>(FindObjectsSortMode.None))
            {
                button.CanPush = true;
            }
        }
    }
}