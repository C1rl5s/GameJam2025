using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialogueBox;
    public GameObject continueIndicator; // El triangulito de continuar

    [Header("Dialogue Settings")]
    public string[] dialogues;
    public string[] characterNames;

    [Header("Typewriter Effect")]
    public float typingSpeed = 0.05f; // Velocidad entre letras
    public AudioClip typingSound; // Sonido opcional
    private AudioSource audioSource;

    [Header("Scene Settings")]
    public bool changeSceneOnEnd = false; // Activar para cambiar de escena
    public string nextSceneName = ""; // Nombre de la escena
    public int nextSceneIndex = -1; // O usar índice (-1 = no usar)

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool canSkip = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        if (dialogues.Length > 0)
        {
            ShowDialogue();
        }
    }

    void Update()
    {
        // Detectar click o espacio con ambos sistemas de input
        bool clicked = false;

#if ENABLE_INPUT_SYSTEM
        clicked = UnityEngine.InputSystem.Mouse.current?.leftButton.wasPressedThisFrame == true ||
                 UnityEngine.InputSystem.Keyboard.current?.spaceKey.wasPressedThisFrame == true;
#else
            clicked = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
#endif

        if (clicked)
        {
            if (isTyping && canSkip)
            {
                // Completar ambos textos instantáneamente
                StopCoroutine(typingCoroutine);

                if (characterNames.Length > currentIndex && characterNames[currentIndex] != "")
                {
                    nameText.text = characterNames[currentIndex];
                }
                dialogueText.text = dialogues[currentIndex];

                isTyping = false;
                ShowContinueIndicator();
            }
            else if (!isTyping)
            {
                NextDialogue();
            }
        }
    }

    void ShowDialogue()
    {
        if (currentIndex < dialogues.Length)
        {
            // Ocultar indicador
            if (continueIndicator != null)
                continueIndicator.SetActive(false);

            // Aplicar efecto de máquina a AMBOS (nombre y diálogo)
            string currentName = "";
            if (characterNames.Length > currentIndex && characterNames[currentIndex] != "")
            {
                currentName = characterNames[currentIndex];
                nameText.gameObject.SetActive(true);
            }
            else
            {
                nameText.gameObject.SetActive(false);
            }

            typingCoroutine = StartCoroutine(TypeBothTexts(currentName, dialogues[currentIndex]));
        }
    }

    IEnumerator TypeBothTexts(string name, string dialogue)
    {
        isTyping = true;
        canSkip = false;
        nameText.text = "";
        dialogueText.text = "";


        // Pequeño delay antes de permitir saltar
        yield return new WaitForSeconds(0.2f);
        canSkip = true;

        // Luego escribir el diálogo
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;

            if (typingSound != null && audioSource != null && letter != ' ')
            {
                audioSource.PlayOneShot(typingSound, 0.1f);
            }

            yield return new WaitForSeconds(typingSpeed);
        }


        // Escribir el nombre primero
        if (name != "")
        {
            foreach (char letter in name.ToCharArray())
            {
                nameText.text += letter;

                if (typingSound != null && audioSource != null && letter != ' ')
                {
                    audioSource.PlayOneShot(typingSound, 0.1f);
                }

                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
        ShowContinueIndicator();
    }


    void ShowContinueIndicator()
    {
        if (continueIndicator != null)
            continueIndicator.SetActive(true);
    }

    void NextDialogue()
    {
        currentIndex++;

        if (currentIndex < dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);

        // Limpiar los textos
        dialogueText.text = "";
        nameText.text = "";

        // Ocultar indicador
        if (continueIndicator != null)
            continueIndicator.SetActive(false);
        Debug.Log("Diálogo terminado");
        if (changeSceneOnEnd)
        {
            if (nextSceneIndex >= 0)
            {
                // Usar índice si está configurado
                SceneManager.LoadScene(nextSceneIndex);
            }
            else if (nextSceneName != "")
            {
                // Usar nombre si está configurado
                SceneManager.LoadScene(nextSceneName);
            }

        }
    }

    public void StartNewDialogue(string[] newDialogues, string[] newNames = null)
    {
        dialogues = newDialogues;
        characterNames = newNames ?? new string[0];
        currentIndex = 0;
        dialogueBox.SetActive(true);
        ShowDialogue();
    }
}