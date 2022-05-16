using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // _pailouComps[col / x][row / y]
    public List<List<PComponent>> _pailouComps;

    public GameObject choiceViewOb;
    public GameObject componentUITreeRoot;
    public GameObject _pailou3DManager;

    public int UIBoxOffsetX;
    public int UIBoxOffsetY;
    public int UIBoxWidth;
    public int UIBoxHeight;
    public int UIBoxSpaceX;
    private int _offsetX;
    private int _offsetY;

    public PComponent _nowComp;

    public GameObject ClippedRoof;
    public GameObject EavesRoof;
    public GameObject FlowerBoard;
    public GameObject Lintel;
    public GameObject MiddleToukung;
    public GameObject Pillar;
    public GameObject PillarBase;
    public GameObject Queti;
    public GameObject SideToukung;
    public GameObject Yundan;

    public GameObject UIClippedRoof;
    public GameObject UIEavesRoof;
    public GameObject UIFlowerBoard;
    public GameObject UILintel;
    public GameObject UIMiddleToukung;
    public GameObject UIPillar;
    public GameObject UIPillarBase;
    public GameObject UIQueti;
    public GameObject UISideToukung;
    public GameObject UIYundan;

    // Start is called before the first frame update
    void Start()
    {
        PailouUtils._gameManager = this;
        _offsetX = UIBoxOffsetX + UIBoxWidth;
        _offsetY = UIBoxOffsetY + UIBoxHeight;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BuildUITree()
    {
        int sideOffsetCount = 0;
        for (int i = 0; i < componentUITreeRoot.transform.childCount; i++)
        {
            GameObject child = componentUITreeRoot.transform.GetChild(i).gameObject;
            if (child.GetComponent<Text>() != null)
                continue;
            RectTransform childTransf = PailouUtils.GetRectTransform(child);
            childTransf.localPosition = new Vector3(UIBoxWidth / 2 + UIBoxOffsetX + sideOffsetCount++ * _offsetX, -UIBoxHeight / 2 - UIBoxOffsetY, 0);
            sideOffsetCount += Mathf.Max(TraceUITree(child) - 1, 0);
        }    
    }

    public int TraceUITree(GameObject parent)
    {
        int sideOffsetCount = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            if (child.GetComponent<Text>() != null)
                continue;
            RectTransform childTransf = PailouUtils.GetRectTransform(child);
            childTransf.localPosition = new Vector3(sideOffsetCount++ * _offsetX, -_offsetY, 0);
            sideOffsetCount += Mathf.Max(TraceUITree(child) - 1, 0);
        }
        return sideOffsetCount;
    }

    public void UpdateChoiceView(ComponentUI componentUI)
    {
        ClearChoices();
        // Set choice view with relation matrix.
        int offsetCount = 0;
        int blockOffsetCount = 0;
        // Top candidate comps
        for (int i = 0; i < componentUI.topCandidateComps.Count; i++)
        {
            GameObject compUIOb = Instantiate(GetUIPrefabObFromPailouComp(componentUI.topCandidateComps[i]));
            compUIOb.transform.SetParent(choiceViewOb.transform);
            ComponentUI compUI = compUIOb.GetComponent<ComponentUI>();
            compUI._parentComponent = componentUI._myComponent;
            compUI.SetInstallDir(Direction.UP);
            RectTransform compUITransf = PailouUtils.GetRectTransform(compUIOb);
            compUITransf.transform.localPosition = new Vector3(60 + offsetCount++ * _offsetX + blockOffsetCount * 20, -60, 0);
            compUITransf.transform.localScale = Vector3.one;
        }
        blockOffsetCount++;
        // Side candidate comps
        for (int i = 0; i < componentUI.sideCandidateComps.Count; i++)
        {
            GameObject compUIOb = Instantiate(GetUIPrefabObFromPailouComp(componentUI.sideCandidateComps[i]));
            compUIOb.transform.SetParent(choiceViewOb.transform);
            ComponentUI compUI = compUIOb.GetComponent<ComponentUI>();
            compUI._parentComponent = componentUI._myComponent;
            // Component install from side is always on the left side.
            compUI.SetInstallDir(Direction.LEFT);
            RectTransform compUITransf = PailouUtils.GetRectTransform(compUIOb);
            compUITransf.transform.localPosition = new Vector3(60 + offsetCount++ * _offsetX + blockOffsetCount * 20, -60, 0);
            compUITransf.transform.localScale = Vector3.one;
        }
        blockOffsetCount++;
        for (int i = 0; i < componentUI.sideCandidateComps.Count; i++)
        {
            GameObject compUIOb = Instantiate(GetUIPrefabObFromPailouComp(componentUI.sideCandidateComps[i]));
            compUIOb.transform.SetParent(choiceViewOb.transform);
            ComponentUI compUI = compUIOb.GetComponent<ComponentUI>();
            compUI._parentComponent = componentUI._myComponent;
            // Component install from side is always on the left side.
            compUI.SetInstallDir(Direction.RIGHT);
            RectTransform compUITransf = PailouUtils.GetRectTransform(compUIOb);
            compUITransf.transform.localPosition = new Vector3(60 + offsetCount++ * _offsetX + blockOffsetCount * 20, -60, 0);
            compUITransf.transform.localScale = Vector3.one;
        }
        blockOffsetCount++;
        // Top candidate comps
        for (int i = 0; i < componentUI.bottomCandidateComps.Count; i++)
        {
            GameObject compUIOb = Instantiate(GetUIPrefabObFromPailouComp(componentUI.bottomCandidateComps[i]));
            compUIOb.transform.SetParent(choiceViewOb.transform);
            ComponentUI compUI = compUIOb.GetComponent<ComponentUI>();
            compUI._parentComponent = componentUI._myComponent;
            compUI.SetInstallDir(Direction.DOWN);
            RectTransform compUITransf = PailouUtils.GetRectTransform(compUIOb);
            compUITransf.transform.localPosition = new Vector3(60 + offsetCount++ * _offsetX + blockOffsetCount * 20, -60, 0);
            compUITransf.transform.localScale = Vector3.one;
        }
        blockOffsetCount++;
    }

    public GameObject GetPrefabObFromPailouComp(PailouComponent pailouComp)
    {
        switch (pailouComp)
        {
            case PailouComponent.ROOF:
                return ClippedRoof;
            case PailouComponent.ROOF_EDGE:
                return EavesRoof;
            case PailouComponent.MIDDLE_TOUKUNG:
                return MiddleToukung;
            case PailouComponent.SIDE_TOUKUNG:
                return SideToukung;
            case PailouComponent.LINTEL:
                return Lintel;
            case PailouComponent.FLOWER_BOARD:
                return FlowerBoard;
            case PailouComponent.QUETI:
                return Queti;
            case PailouComponent.YUNDAN:
                return Yundan;
            case PailouComponent.PILLAR:
                return Pillar;
            case PailouComponent.PILLAR_BASE:
                return PillarBase;

        }
        return null;
    }

    public GameObject GetUIPrefabObFromPailouComp(PailouComponent pailouComp)
    {
        switch (pailouComp)
        {
            case PailouComponent.ROOF:
                return UIClippedRoof;
            case PailouComponent.ROOF_EDGE:
                return UIEavesRoof;
            case PailouComponent.MIDDLE_TOUKUNG:
                return UIMiddleToukung;
            case PailouComponent.SIDE_TOUKUNG:
                return UISideToukung;
            case PailouComponent.LINTEL:
                return UILintel;
            case PailouComponent.FLOWER_BOARD:
                return UIFlowerBoard;
            case PailouComponent.QUETI:
                return UIQueti;
            case PailouComponent.YUNDAN:
                return UIYundan;
            case PailouComponent.PILLAR:
                return UIPillar;
            case PailouComponent.PILLAR_BASE:
                return UIPillarBase;

        }
        return null;
    }

    public void ClearChoices()
    {
        // Clear the choice view
        for (int i = 0; i < choiceViewOb.transform.childCount; i++)
        {
            GameObject child = choiceViewOb.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    public void ClearScene(GameObject root)
    {
        PComponent pComponent = root.GetComponent<PComponent>();
        pComponent._compsLeft.Clear();
        pComponent._compsRight.Clear();
        pComponent._compsUnder.Clear();
        pComponent._compsUpper.Clear();
        pComponent._procedureContainer.Clear();
        for (int i = 0; i < root.transform.childCount; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            ClearComponent(child);
        }
        for (int i = 0; i < componentUITreeRoot.transform.childCount; i++)
        {
            GameObject child = componentUITreeRoot.transform.GetChild(i).gameObject;
            ClearUIComponent(child);
        }
        ClearChoices();
        GameObject compUIOb = Instantiate(UILintel, componentUITreeRoot.transform);
        compUIOb.transform.localPosition = new Vector3(55, -55, 0);
        ComponentUI compUI = compUIOb.GetComponent<ComponentUI>();
        compUI._isOnScene = true;
        compUI._myComponent = pComponent;
        compUI._parentComponent = _pailou3DManager.transform.GetComponent<PComponent>();
        compUI.SetInstallDir(Direction.UP);
        pComponent._myButton = compUIOb;
    }

    private void ClearComponent(GameObject root)
    {
        for (int i = 0; i < root.transform.childCount; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            ClearComponent(child);
        }
        Destroy(root);
    }
    
    private void ClearUIComponent(GameObject root)
    {
        for (int i = 0; i < root.transform.childCount; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            ClearUIComponent(child);
        }
        Destroy(root);
    }
}    
