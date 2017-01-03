using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{

    public Camera cam;
    public LayerMask PlayerUnitsLayerMask;
    public LayerMask TerrainLayerMask;
    public GameObject SelectionCubePrefab;

    private List<Unit> SelectedUnits;
    private Vector3 SelectionCubeHeight;
    private bool isDragging;
    private RaycastHit startDragRayHit;
    private RaycastHit endDragRayHit;
    private GameObject SelectionCube;
    public float MinDragDistance;

    public bool IsDragging
    {
        get
        {
            return isDragging;
        }

        set
        {
            isDragging = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        SelectedUnits = new List<Unit>();
        SelectionCubeHeight = SelectionCubePrefab.transform.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        // while in drag
        if (IsDragging)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out endDragRayHit, 100, TerrainLayerMask))
                {
                    onDragEnd();
                }
                else
                {
                    if (!SelectionCube && (endDragRayHit.point - startDragRayHit.point).magnitude > MinDragDistance)
                    {
                        SelectionCube = GameObject.Instantiate<GameObject>(SelectionCubePrefab);
                        SelectionCube.transform.position = startDragRayHit.point;
                    }
                    if (SelectionCube)
                    {
                        SelectionCube.transform.localScale = SelectionCubeHeight + endDragRayHit.point - startDragRayHit.point;
                        SelectionCube.transform.position = startDragRayHit.point + SelectionCube.transform.localScale / 2;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                onDragEnd();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out startDragRayHit, 100, TerrainLayerMask))
                {
                    IsDragging = true;
                }
            }
        }
    }

    void onDragEnd()
    {
        IsDragging = false;
        if (SelectionCube)
        {
            Vector3 SelectionCubeScale = new Vector3(Mathf.Abs(SelectionCube.transform.localScale.x), SelectionCube.transform.localScale.y, Mathf.Abs(SelectionCube.transform.localScale.z));
            Collider[] SelectedUnitsCollider = Physics.OverlapBox(SelectionCube.transform.position, SelectionCubeScale / 2, SelectionCube.transform.rotation, PlayerUnitsLayerMask);
            SelectedUnits.Clear();
            for (int i = SelectedUnitsCollider.Length - 1; i > 0; i--)
            {
                Unit unit = SelectedUnitsCollider[i].GetComponentInParent<Unit>();
                if (SelectedUnits.IndexOf(unit) == -1)
                {
                    SelectedUnits.Add(unit);
                    //                print(unit);
                }
            }
            GameObject.Destroy(SelectionCube);
            SelectionCube = null;
        }
        else
        {
            OnClick();
        }
    }

    private void OnClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out endDragRayHit);
        if(endDragRayHit.collider.gameObject.layer==Mathf.Log(TerrainLayerMask.value,2))//reverse operation of layerMask 
        {
            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                GoToTask goToTask= SelectedUnits[i].ChangeTask<GoToTask>();
                goToTask.SetDestination(startDragRayHit.point);
            }
        }
    }
}
