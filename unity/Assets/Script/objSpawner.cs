using UnityEngine;

public class objSpawner : MonoBehaviour
{
    public pileManager pilePrefab;
    public Transform spawnPoint;
    public BoardEditorManager boardEditor;

    public void SpawnPile()
    {
        pileManager newPile = Instantiate(pilePrefab, spawnPoint.position, Quaternion.identity);

        BoardObjectDraggable drag = newPile.GetComponent<BoardObjectDraggable>();
        if (drag != null)
        {
            drag.boardEditor = boardEditor;
        }

        SelectablePile selectable = newPile.GetComponent<SelectablePile>();
        if (selectable != null)
        {
            selectable.boardEditor = boardEditor;
        }
    }
}