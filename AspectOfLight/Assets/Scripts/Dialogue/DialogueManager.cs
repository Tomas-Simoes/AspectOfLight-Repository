using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
  [Header("Manager Information")]
  [SerializeField]
  private InputHandler inputHandler;

  [SerializeField]
  private UIDialogue UIDialogue;

  [SerializeField]
  private float minimumTimeInNode = 1;

  [Header("Dialogue Information")]
  private D_Dialogue currentDialogue;
  private D_Dialogue.DialogueNode currentNode;
  private int currentNodeIndex = 0;

  public bool inDialogueMode = false;

  [SerializeField]
  private float timeInNode;

  [Header("Stand-by Dialogue Information")]
  public bool hasDialogueInStandBy = false;
  private D_Dialogue dialogueInStandBy;
  private int nodeIndexInStandBy;

  void Update()
  {
    if (inDialogueMode)
      timeInNode += Time.deltaTime;
  }

  public void StartDialogueMode(D_Dialogue dialogue)
  {
    if (inDialogueMode)
      return;

    inDialogueMode = true;
    hasDialogueInStandBy = false;
    inputHandler.ChangePlayerMapState(false);

    currentDialogue = dialogue;
    currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

    UIDialogue.UpdateUI(
        dialogue.speakerName,
        currentNode.dialogueText,
        currentNode.dialogueChoises
    );
  }

  public void TryGoToNextNode()
  {
    if (timeInNode <= minimumTimeInNode)
      return;

    if (!UIDialogue.isReadyForNewNode)
    {
      if (!UIDialogue.nodeIsFinish)
        UIDialogue.FinishDisplayNode();
      return;
    }

    if (currentNodeIndex + 1 >= currentDialogue.dialogueNodes.Count)
    {
      if (currentNode.dialogueChoises.Count != 0)
        return;

      if (currentDialogue.followOnDialogue == null)
        ExitDialogueMode();
      else
        ChangeCurrentDialogue(
            currentDialogue.followOnDialogue,
            currentDialogue.startFollowOnDialogueIndex
        );

      return;
    }

    currentNodeIndex++;
    currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

    UIDialogue.UpdateUI(
        currentDialogue.speakerName,
        currentNode.dialogueText,
        currentNode.dialogueChoises
    );

    timeInNode = 0;
  }

  public void ExitDialogueMode()
  {
    inDialogueMode = false;
    currentNodeIndex = 0;
    inputHandler.ChangePlayerMapState(true);

    UIDialogue.ExitUIDialogue();
  }

  public void SelectChoise(int choiseIndex)
  {
    D_Dialogue.DialogueChoise _currentChoise = currentNode.dialogueChoises[choiseIndex];
    _currentChoise.choiseWasAlreadyChoosen = true;

    ChangeCurrentDialogue(_currentChoise.followOnDialogue);
  }

  public void ChangeCurrentDialogue(D_Dialogue newDialogue, int newDialogueStartIndex = 0)
  {
    currentDialogue = newDialogue;
    currentNodeIndex = newDialogueStartIndex;

    Debug.Log(currentNodeIndex);
    currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

    UIDialogue.UpdateUI(
        currentDialogue.speakerName,
        currentNode.dialogueText,
        currentNode.dialogueChoises
    );
  }

  public void TryToStandByCurrentDialogue()
  {
    if (hasDialogueInStandBy)
      return;

    if (timeInNode <= minimumTimeInNode)
      return;

    if (!UIDialogue.isReadyForNewNode)
    {
      UIDialogue.FinishDisplayNode();
      return;
    }

    hasDialogueInStandBy = true;

    dialogueInStandBy = currentDialogue;
    nodeIndexInStandBy = currentNodeIndex;

    ExitDialogueMode();
  }

  public void ResumeStandByDialogue()
  {
    currentNodeIndex = nodeIndexInStandBy + 1;
    timeInNode = 0;

    StartDialogueMode(dialogueInStandBy);
  }
}
