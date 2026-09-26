using UnityEngine;

/// Editor sticky note. Select the GameObject in the Hierarchy and read
/// the note in the Inspector. Does nothing at runtime.
class DevNote : MonoBehaviour
{
    [TextArea(4, 12)]
    public string note;
}
