using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // _pailouComps[col / x][row / y]
    public List<List<Component>> _pailouComps;

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

    public Component _nowComp;

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
        TraceUITree(componentUITreeRoot);
    }

    public int TraceUITree(GameObject parent)
    {
        int sideOffsetCount = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = componentUITreeRoot.transform.GetChild(i).gameObject;
            RectTransform childTransf = PailouUtils.GetRectTransform(child);
            childTransf.localPosition = new Vector3(sideOffsetCount++ * _offsetX, -_offsetY, 0);
            sideOffsetCount += TraceUITree(child);
        }
        return sideOffsetCount;
    }

    public void UpdateChoiceView(ComponentUI componentUI)
    {
        // Clear the choice view
        for (int i = 0; i < choiceViewOb.transform.childCount; i++)
        {
            GameObject child = choiceViewOb.transform.GetChild(i).gameObject;
            Destroy(child);
        }
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
            RectTransform compUITransf = PailouUtils.GetRectTransform(compUIOb);
            compUITransf.transform.localPosition = new Vector3(60 + offsetCount++ * _offsetX + blockOffsetCount * 20, -60, 0);
            compUITransf.transform.localScale = Vector3.one;
        }
        blockOffsetCount++;
        // Side candidate comps
        for (int i = 0; i < componentUI.sideCandidateComps.Count; i++)
        {
            GameObject compUIOb = Instantiate(GetUIPrefabObFromPailouComp(componentUI.topCandidateComps[i]));
            compUIOb.transform.SetParent(choiceViewOb.transform);
            ComponentUI compUI = compUIOb.GetComponent<ComponentUI>();
            compUI._parentComponent = componentUI._myComponent;
            RectTransform compUITransf = PailouUtils.GetRectTransform(compUIOb);
            compUITransf.transform.localPosition = new Vector3(60 + offsetCount++ * _offsetX + blockOffsetCount * 20, -60, 0);
            compUITransf.transform.localScale = Vector3.one;
        }
        blockOffsetCount++;
        // Top candidate comps
        for (int i = 0; i < componentUI.bottomCandidateComps.Count; i++)
        {
            GameObject compUIOb = Instantiate(GetUIPrefabObFromPailouComp(componentUI.topCandidateComps[i]));
            compUIOb.transform.SetParent(choiceViewOb.transform);
            ComponentUI compUI = compUIOb.GetComponent<ComponentUI>();
            compUI._parentComponent = componentUI._myComponent;
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
}    