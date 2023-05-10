using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelArea : MonoBehaviour
{
    private Plane groundPlane;
    private Camera myCamera;

    [SerializeField] private Mesh debugMesh;
    [SerializeField] private Material debugMaterial;
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {        
        groundPlane = new Plane(Vector3.zero, Vector3.forward, Vector3.right);
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterSingleton.Instance.GameManager.State != GameManager.GameState.edit) return;
        if (myCamera == null) myCamera = Camera.main;

        Ray mouseRay = myCamera.ScreenPointToRay(Input.mousePosition);
        bool hasFoundPos = false;
        Vector3 foundPos = Vector3.zero;

        if (Physics.Raycast(mouseRay, out RaycastHit hit, 500))
        {
            hasFoundPos = true;
            foundPos = hit.point + hit.normal * 0.5f;
            foundPos = Vector3Int.RoundToInt(foundPos);
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                GameObject.Destroy(hit.collider.gameObject);
            }
        }

        else if (groundPlane.Raycast(mouseRay, out float planeHit))
        {
            hasFoundPos = true;
            foundPos = Vector3Int.RoundToInt(mouseRay.GetPoint(planeHit));
        }

        if (hasFoundPos)
        {
            Graphics.DrawMesh(debugMesh, Matrix4x4.TRS(foundPos, Quaternion.identity, Vector3.one), debugMaterial, 0);

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Instantiate(ChooseTile(MasterSingleton.Instance.UIManager.chosenTile), foundPos, Quaternion.identity);
            }
        }
    }

    public GameObject ChooseTile(string blockName)
    {
        GameObject foundTile = tiles.FirstOrDefault(t => t.name == blockName);
        return foundTile;
    }
}
