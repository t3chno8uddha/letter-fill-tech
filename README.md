# Letter Drawing System (Unity)

This is a basic Unity system for drawing letters using touch input. Each letter is made up of parts (strokes), and the user draws along a path to complete each part.

## How It Works

- The user touches near the start of a spline.
- As they move their finger, the system checks if they are following the path.
- If the user reaches the end of the spline, the next part is activated.
- If they move too far off the path, it resets.

## Scripts

### DrawLetter
- Manages all parts of a letter.
- Use `AdvanceLetter()` to move to the next stroke.

### DrawLetterPart
- Holds the spline and visual mesh.
- Use `Enable()` to start drawing.
- Use `Disable()` to show the full part (used when moving to the next).

### DrawCursor
- Handles touch input.
- Tracks user's finger on the spline.
- Moves a visual cursor and updates the drawn mesh.
- Resets progress if the user goes off track.

## Setup Instructions

1. **Create Letter Parts**
   - Add a `DrawLetterPart` component to each part.
   - Assign the `SplineContainer`, `SplineExtrude`, and `MeshRenderer`.

2. **Create the Main Letter**
   - Add a `DrawLetter` component.
   - Assign all the `DrawLetterPart` objects in the correct order.

3. **Set Up the Cursor**
   - Add a `DrawCursor` component to a GameObject.
   - Assign:
     - `spriteRenderer`: visual object that follows the finger
     - `letter`: the `DrawLetter` object
     - `threshold`: how close the user must stay to the spline

## Notes

- Designed for mobile touch.
- Requires Unity's Spline package.

