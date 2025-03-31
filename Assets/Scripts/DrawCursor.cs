using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Linq;

public class DrawCursor : MonoBehaviour
{
    // The following code was COMMENTED by ChatGPT

    [Header("Drawing Settings")]
    public float threshold = 0.5f;              // Distance threshold to start drawing
    public GameObject spriteRenderer;           // Visual follower for the user's touch
    public DrawLetter letter;                   // Reference to the DrawLetter controller

    private DrawLetterPart currentPart;         // The current part of the letter being drawn
    private Camera mainCam;                     // Cached reference to the main camera
    private bool isTouching = false;            // Whether the user is currently drawing
    private int touchIndex = -1;                // Track the correct touch when multi-touching

    void Start()
    {
        mainCam = Camera.main;
        currentPart = letter.letters[0]; // Start with the first letter part
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector3 touchPos = mainCam.ScreenToWorldPoint(touch.position);
                touchPos.z = 0;

                if (touch.phase == TouchPhase.Began)
                {
                    var knots = currentPart.path.Spline.Knots.ToList();

                    // Check if touch started close enough to the first point of the spline
                    if (Vector3.Distance(touchPos, knots.First().Position) < threshold * 5)
                    {
                        // Only start drawing if we are at the beginning of the path
                        if (Completion(touchPos) < threshold)
                        {
                            touchIndex = touch.fingerId;
                            spriteRenderer.SetActive(true);
                            spriteRenderer.transform.position = NearestPoint(touchPos);
                            isTouching = true;
                        }
                    }
                }

                if (isTouching && touch.fingerId == touchIndex)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        CheckCompletion(touchPos);
                        spriteRenderer.transform.position = NearestPoint(touchPos);
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        ResetPart();
                    }
                }
            }
        }
    }

    // Check if drawing has completed or update progress
    void CheckCompletion(Vector3 touchPos)
    {
        float t = Completion(touchPos);

        if (t > 1 - threshold)
        {
            FinishPart(); // Finished drawing this part
        }
        else
        {
            if (Vector3.Distance(touchPos, NearestPoint(touchPos)) > threshold * 5) ResetPart();
            else UpdateRenderer(t); // Update current progress
        }
    }

    // Called when a letter part is finished
    void FinishPart()
    {
        var nextPart = letter.AdvanceLetter();

        if (nextPart == null)
        {
            isTouching = false;
            touchIndex = -1;
            spriteRenderer.SetActive(false);
            return;
        }

        currentPart = nextPart;
        touchIndex = -1;
        spriteRenderer.SetActive(false);
        isTouching = false;
    }

    // Gets the closest point on the spline and time (0-1 along the path)
    void GetCompletion(Vector3 touchPos, out Vector3 distance, out float time)
    {
        Spline spline = currentPart.path.Spline;
        float3 nearestPoint;
        float t;
        SplineUtility.GetNearestPoint(spline, touchPos, out nearestPoint, out t);

        distance = (Vector3)nearestPoint;
        time = t;
    }

    // Returns the nearest position on the path
    Vector3 NearestPoint(Vector3 touchPos)
    {
        GetCompletion(touchPos, out Vector3 nearest, out _);
        return nearest;
    }

    // Returns how far along the spline the touch is (0-1)
    float Completion(Vector3 touchPos)
    {
        GetCompletion(touchPos, out _, out float t);
        return t;
    }

    // Updates the visible drawn line based on progress
    void UpdateRenderer(float t)
    {
        currentPart.extruder.Range = new Vector2(0, Mathf.Clamp(t, 0, 1));
        currentPart.extruder.Rebuild();
    }

    // Resets the visual progress for the current part
    void ResetPart()
    {
        currentPart.extruder.Range = new Vector2(0, 0);
        currentPart.extruder.Rebuild();

        touchIndex = -1;
        spriteRenderer.SetActive(false);
    }
}
