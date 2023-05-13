using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelArea : MonoBehaviour
{
    static public LevelArea Instance;
    private Plane groundPlane;
    private Camera myCamera;

    [SerializeField] private Mesh debugMesh;
    [SerializeField] private Material debugMaterial;
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();

    [SerializeField] private Light levelEditorLight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

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
                var lvlBlock = hit.collider.GetComponent<LevelBlock>();
                if (lvlBlock == null)
                {
                    lvlBlock = hit.collider.GetComponentInParent<LevelBlock>();
                }

                if (lvlBlock != null)
                {
                    Destroy(lvlBlock.gameObject);
                }
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

        if (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider == null) return;
            //First try to get tthe level block that we're targetting on the object itself
            var lvlBlock = hit.collider.GetComponent<LevelBlock>();
            //If It's null, try getting it on the parent.
            if (lvlBlock == null)
            {
                lvlBlock = hit.collider.GetComponentInParent<LevelBlock>();
            }
            //If it's not null
            if (lvlBlock != null)
            {
                //Call open menu on it, since the button overrides it, it'll call the connect button if it's a button, otherwise it won't do anything.
                if (lvlBlock.MiddleMouseEvent())
                {

                }
            }
        }
    }

    public GameObject ChooseTile(string blockName)
    {
        GameObject foundTile = tiles.FirstOrDefault(t => t.name == blockName);
        return foundTile;
    }

    public void ToggleEditorLight()
    {
        if (levelEditorLight.enabled)
        {
            levelEditorLight.enabled = false;
        }

        else
        {
            levelEditorLight.enabled = true;
        }
    }
}
