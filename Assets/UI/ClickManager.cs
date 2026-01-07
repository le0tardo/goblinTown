using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask groundLayer;
    Vector2 dragStartPos;
    Vector2 dragCurrentPos;
    bool isDragging;
    float dragThreshold = 10f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            dragCurrentPos = dragStartPos;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            dragCurrentPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            if (Vector2.Distance(dragStartPos, dragCurrentPos) < dragThreshold)
            {
                HandleClick();
            }
            else
            {
                HandleBoxSelection(dragStartPos, dragCurrentPos);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            UnitManager.inst.ClearSelection();
            BuildingManager.inst.DeselectBuilding();
        }
    }


    Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        Vector2 bottomLeft = Vector2.Min(start, end);
        Vector2 topRight = Vector2.Max(start, end);
        return new Rect(bottomLeft, topRight - bottomLeft);
    }
    Rect GetGuiRect(Vector2 start, Vector2 end)
    {
        Vector2 bottomLeft = Vector2.Min(start, end);
        Vector2 topRight = Vector2.Max(start, end);

        return new Rect(
            bottomLeft.x,
            Screen.height - topRight.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y
        );
    }
    void HandleClick()
    {
        // Ignore clicks while placing buildings
        if (BuildingManager.inst != null && BuildingManager.inst.isPlacingBuilding)
            return;

        // 0. UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 1. Raycast
        if (!Physics.Raycast(ray, out hit))
            return;

        // 2. Selectable objects
        ISelectable selectable = hit.collider.GetComponentInParent<ISelectable>();
        if (selectable != null)
        {
            Unit unit = selectable as Unit;
            if (unit != null)
            {
                if (UnitManager.inst.IsSelected(unit))
                {
                    UnitManager.inst.DeselectUnit(unit);
                    UnitManager.inst.ClearSelection();
                }
                else
                {
                    UnitManager.inst.ClearSelection();
                    UnitManager.inst.SelectUnit(unit);
                }
                BuildingManager.inst.DeselectBuilding();
            }

            return;
        }

        //2b buildings
        IBuilding building=hit.collider.GetComponentInParent<IBuilding>();
        if (building != null)
        {
            if (UnitManager.inst.selectedUnits.Count <= 0)
            {
                if (BuildingManager.inst.selectedBuilding == null)
                {
                    BuildingManager.inst.SelectBuilding(building.Bbh);
                }
                else if (BuildingManager.inst.selectedBuilding == building.Bbh)
                {
                    BuildingManager.inst.DeselectBuilding();
                }
                else
                {
                    BuildingManager.inst.DeselectBuilding();
                    BuildingManager.inst.SelectBuilding(building.Bbh);
                }
            }
            else
            {
                BuildingManager.inst.DeselectBuilding();
                float radius = 1.5f; // spacing from center
                int count = UnitManager.inst.selectedUnits.Count;

                for (int i = 0; i < count; i++)
                {
                    Unit unit = UnitManager.inst.selectedUnits[i];

                    float angle = (2 * Mathf.PI / count) * i;

                    Vector3 offset = new Vector3(
                        Mathf.Cos(angle),
                        0,
                        Mathf.Sin(angle)
                    ) * radius;

                    Vector3 destination = hit.point + offset;

                    unit.MoveTo(destination);
                    unit.endAction = Unit.EndAction.None;
                    unit.ReleaseSlot();
                }
            }
        }
        else
        {
            BuildingManager.inst.DeselectBuilding();
        }

        //3 forageable objects
        IForageable forageable = hit.collider.GetComponentInParent<IForageable>();
        if (forageable != null)
        {
            ISlotProvider slotProvider = hit.collider.GetComponentInParent<ISlotProvider>();
            foreach (Unit unit in UnitManager.inst.selectedUnits)
            {
                unit.forageTarget = forageable;
                unit.endAction = Unit.EndAction.Forage;

                if (slotProvider != null)
                {
                    unit.ReleaseSlot();
                    Vector3 slot = slotProvider.RequestSlot(unit);
                    unit.currentSlotProvider = slotProvider;
                    unit.slotProviderName = slotProvider.ToString(); // debug only
                    unit.MoveTo(slot);
                }
                else
                {
                    unit.MoveTo(forageable.Position);
                }
            }

            return;
        }

        // 4 drop off points
        IDepositable deposit = hit.collider.GetComponentInParent<IDepositable>();
        if (deposit != null)
        {
            ISlotProvider slotProvider = hit.collider.GetComponentInParent<ISlotProvider>();
            foreach (Unit unit in UnitManager.inst.selectedUnits)
            {
                unit.depositTarget = deposit;
                unit.endAction = Unit.EndAction.Deposit;

                if (slotProvider != null)
                {
                    unit.ReleaseSlot();
                    Vector3 slot = slotProvider.RequestSlot(unit);
                    unit.currentSlotProvider = slotProvider;
                    unit.slotProviderName = slotProvider.ToString(); // debug only
                    unit.MoveTo(slot);
                }
                else
                {
                    unit.MoveTo(deposit.Position);
                }
            }
         return;
        }


        // 5. Ground, walk to position here
        if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
        {

            if (UnitManager.inst.selectedUnits.Count > 0)
            {
                if (UnitManager.inst.selectedUnits.Count < 2)
                {
                    //UnitManager.inst.selectedUnits[0].MoveTo(hit.point); //move one unit to the click point
                    Unit unit=UnitManager.inst.selectedUnits[0];
                    unit.endAction=Unit.EndAction.None;
                    unit.MoveTo(hit.point);
                    unit.ReleaseSlot();
                }
                else
                {
                    float radius = 1.5f; // spacing from center
                    int count = UnitManager.inst.selectedUnits.Count;

                    for (int i = 0; i < count; i++)
                    {
                        Unit unit = UnitManager.inst.selectedUnits[i];

                        float angle = (2 * Mathf.PI / count) * i;

                        Vector3 offset = new Vector3(
                            Mathf.Cos(angle),
                            0,
                            Mathf.Sin(angle)
                        ) * radius;

                        Vector3 destination = hit.point + offset;

                        unit.MoveTo(destination);
                        unit.endAction = Unit.EndAction.None;
                        unit.ReleaseSlot();
                    }
                }
                foreach (Unit unit in UnitManager.inst.selectedUnits) //needed now? resets in Unit...
                {
                    unit.forageTarget = null;
                    unit.depositTarget = null;
                    unit.currentSlotProvider = null;
                    unit.ReleaseSlot();
                }

            }
            return;
        }
    }
    void HandleBoxSelection(Vector2 start, Vector2 end)
    {
        Rect selectionRect = GetScreenRect(start, end);

        UnitManager.inst.ClearSelection();
        BuildingManager.inst.DeselectBuilding();

        foreach (Unit unit in UnitManager.inst.units)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (screenPos.z < 0)
                continue; // behind camera

            if (selectionRect.Contains(screenPos))
            {
                UnitManager.inst.SelectUnit(unit);
            }
        }
    }
    void OnGUI()
    {
        if (!isDragging)
            return;

        Rect rect = GetGuiRect(dragStartPos, dragCurrentPos);

        // Filled box
        GUI.color = new Color(0, 1, 0, 0.15f);
        GUI.DrawTexture(rect, Texture2D.whiteTexture);

        // Border
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, 1, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, 1, rect.height), Texture2D.whiteTexture);

        GUI.color = Color.white;
    }

}

