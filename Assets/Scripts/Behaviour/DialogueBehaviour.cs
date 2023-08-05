using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBehaviour : MonoBehaviour
{
    [SerializeField] private Text DialogueText;
    [SerializeField] private CharacterMovement Character;
    public string[] DialogueString;

    public float TextTime = 0.05f;
    private bool Delay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartDialogue(bool IsCollect)
    {
        for (int i = 0; i < DialogueString.Length; i++)
        {
            string StartDialogueString = "";
            int stringPos = 0;
            float elapsedTime = 0;

            StartCoroutine(Delayfor(0.25f));
            if (IsCollect) { AudioManager.Instance.PlaySFX("Collect"); }

            while (StartDialogueString.Length < DialogueString[i].Length)
            {
                if (Input.GetKey(KeyCode.Space) && !Delay)
                {
                    StartDialogueString = DialogueString[i];
                    DialogueText.text = StartDialogueString;
                    StartCoroutine(Delayfor(0.25f));
                    break;
                }
                if (elapsedTime >= TextTime)
                {
                    StartDialogueString += DialogueString[i][stringPos];
                    DialogueText.text = StartDialogueString;
                    elapsedTime = 0;
                    stringPos++;
                    AudioManager.Instance.PlaySFX("Text");
                }
                else { elapsedTime += Time.deltaTime; yield return null; }
            }

            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !Delay)
                {
                    AudioManager.Instance.PlaySFX("draw");
                    break;
                }
                else { yield return null; }
            }
        }

        Character.Continue();
    }

    public void Reset()
    {
        DialogueString = new string[1];
        DialogueString[0] = "";
        DialogueText.text = "";
    }

    private IEnumerator Delayfor(float DelayTime)
    {
        float elapsedTime = 0;
        Delay = true;
        while (true)
        {
            if (elapsedTime >= DelayTime)
            {
                Delay = false;
                elapsedTime = 0;
                break;
            }
            else { elapsedTime += Time.deltaTime; yield return null; }
        }
    }
}
