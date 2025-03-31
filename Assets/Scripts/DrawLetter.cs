using UnityEngine;

public class DrawLetter : MonoBehaviour
{
    public DrawLetterPart[] letters;
    int partIndex = -1;

    public bool complete;

    void Start()
    {
        AdvanceLetter();
    }

    public DrawLetterPart AdvanceLetter()
    {
        if (complete) return null;

        if (partIndex >= 0 && partIndex < letters.Length)
        {
            letters[partIndex].Disable();
        }

        partIndex++;

        if (partIndex >= letters.Length)
        {
            Debug.Log("All letters have been displayed!");
            complete = true;
            return null;
        }

        letters[partIndex].Enable();
        return letters[partIndex];
    }

}
