using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public Texture2D cursor;

    public void OnMouseEnter() => Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);

    public void OnMouseExit() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
}