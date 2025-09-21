using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour,IInteractable
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] GameObject dialoguePanel;

    [SerializeField] DialogueLine[] dialogueLines;
    [SerializeField] float lineDelay = 1f; // her replik için bekleme süresi

    int currentIndex = 0;
    bool isActive = false;
    public void Interact()
    {
        StartDialogue();
    }
    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        isActive = true;
        currentIndex = 0;
        StartCoroutine(DialogueRoutine());
    }

    IEnumerator DialogueRoutine()
    {
        while (currentIndex < dialogueLines.Length)
        {
            DialogueLine line = dialogueLines[currentIndex];
            nameText.text = line.speakerName;
            dialogueText.text = line.text;

            yield return new WaitForSeconds(lineDelay);
            currentIndex++;
        }

        EndDialogue();
    }

    void EndDialogue()
    {
        isActive = false;
        dialoguePanel.SetActive(false);
    }
}
