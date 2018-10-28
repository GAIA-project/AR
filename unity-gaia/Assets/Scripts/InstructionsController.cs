using UnityEngine;
using System.Collections;
using TMPro;

public class InstructionsController : MonoBehaviour
{
    public GameObject next;
    public GameObject previous;
    public TextMeshPro text;

    public string[] instructions;
    public int current = 0;

    private bool nextActive;
    private bool previousActive;

	// Use this for initialization
	void Start()
	{
        next = transform.Find("Next").gameObject;
        previous = transform.Find("Previous").gameObject;
        text = transform.Find("HintBackground").GetComponentInChildren<TextMeshPro>();

        Debug.Log(instructions);

        UpdateInstruction();
	}

    public void ShowPrevious()
    {
        if (!previousActive) return;

        Debug.Log("Showing next instructions...");
        current--;
        if (current < 0) current = 0;
        UpdateInstruction();
    }

    public void ShowNext()
    {
        if (!nextActive) return;
        
        Debug.Log("Showing previous instructions...");
        current++;
        if (current == instructions.Length) current = instructions.Length - 1;
        UpdateInstruction();
    }

    private void UpdateInstruction()
    {
        Debug.Log("New index: " + current + " with insctruction: " + instructions[current]);
        text.text = instructions[current];
        UpdateButtonsState();
    }

    public void UpdateButtonsState()
    {
        Color previousColor = previous.GetComponent<SpriteRenderer>().color;
        Color nextColor = next.GetComponent<SpriteRenderer>().color;

        if (current == 0)
        {
            previousColor.a = 0.5f;
            nextColor.a = 1f;
        }
        else if (current == instructions.Length - 1)
        {
            nextColor.a = 0.5f;
            previousColor.a = 1f;
        }
        else {
            nextColor.a = 1f;
            previousColor.a = 1f;
        }

        previous.GetComponent<SpriteRenderer>().color = previousColor;
        next.GetComponent<SpriteRenderer>().color = nextColor;

        previousActive = previousColor.a > 0.9f ? true : false;
        nextActive = nextColor.a > 0.9f ? true : false;
    }
}
