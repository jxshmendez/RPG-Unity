using UnityEngine;

public class Cursors : MonoBehaviour
{
    public GameObject CursorObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        CursorObject.transform.position = Input.mousePosition;
    }
}
