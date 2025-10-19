using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
                // Completar texto instantáneamente
                StopCoroutine(typingCoroutine);
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
            // Mostrar nombre
            if (characterNames.Length > currentIndex && characterNames[currentIndex] != "")
            {
                nameText.text = characterNames[currentIndex];
                nameText.gameObject.SetActive(true);
            }
            else
            {
                nameText.gameObject.SetActive(false);
            }

            // Iniciar efecto de máquina de escribir
            if (continueIndicator != null)
                continueIndicator.SetActive(false);

            typingCoroutine = StartCoroutine(TypeText(dialogues[currentIndex]));
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        canSkip = false;
        dialogueText.text = "";

        // Pequeño delay antes de permitir saltar
        yield return new WaitForSeconds(0.2f);
        canSkip = true;

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;

            // Reproducir sonido (opcional)
            if (typingSound != null && audioSource != null && letter != ' ')
            {
                audioSource.PlayOneShot(typingSound, 0.1f);
            }

            yield return new WaitForSeconds(typingSpeed);
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
        Debug.Log("Diálogo terminado");
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