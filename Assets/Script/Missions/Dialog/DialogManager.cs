using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Script.Missions.Dialog;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public Canvas canvas;
    public float maxLineLength = 70f;
    public float minDelay = 1.5f;

    private Queue<DialogLine> dialogQueue = new();
    private Coroutine currentCoroutine;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    public void ShowDialog(List<DialogLine> lines)
    {
        dialogQueue.Clear();
        foreach (var line in lines)
        {
            canvas.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(line.text))
                continue;

            var splitLines = BreakLongLines(line.text);
            foreach (string split in splitLines)
            {
                dialogQueue.Enqueue(new DialogLine { text = split, timePerCharacter = line.timePerCharacter });
            }
        }

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(RunDialog());
    }

    private IEnumerator RunDialog()
    {
        while (dialogQueue.Count > 0)
        {
            DialogLine line = dialogQueue.Dequeue();
            dialogText.text = line.text;

            float delay = Mathf.Max(minDelay, line.text.Length * line.timePerCharacter);
            yield return new WaitForSeconds(delay);
        }

        dialogText.text = "";
        canvas.gameObject.SetActive(false);
    }

    private List<string> BreakLongLines(string input)
    {
        List<string> result = new();
        string[] words = input.Split(' ');

        string currentLine = "";
        foreach (string word in words)
        {
            if ((currentLine + " " + word).Length > maxLineLength)
            {
                result.Add(currentLine.Trim());
                currentLine = word;
            }
            else
            {
                currentLine += " " + word;
            }
        }

        if (!string.IsNullOrWhiteSpace(currentLine))
            result.Add(currentLine.Trim());

        return result;
    }
}