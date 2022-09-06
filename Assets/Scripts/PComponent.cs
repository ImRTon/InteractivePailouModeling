using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PComponent : MonoBehaviour
{
    public PailouComponent _componentType;

    public List<PComponent> _compsUnder = new List<PComponent>();
    public List<PComponent> _compsUpper = new List<PComponent>();
    public List<PComponent> _compsRight = new List<PComponent>();
    public List<PComponent> _compsLeft = new List<PComponent>();

    public List<PComponent> _procedureContainer = new List<PComponent>();

    public ProcedureType _procedureType;

    public GameObject _myButton;
    public int _stage;
    public Vector3 _centerPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetBounding()
    {
        Bounds bound = GetComponent<Renderer>().bounds;
        Collider compCollider = GetComponent<Collider>();
        if (compCollider != null)
        {
            bound = compCollider.bounds;
        }
        return bound.size;
    }

    public Vector3 GetCenterBias()
    {
        Bounds bound = GetComponent<Renderer>().bounds;
        Collider compCollider = GetComponent<Collider>();
        if (compCollider != null)
        {
            bound = compCollider.bounds;
        }
        Debug.Log("My center" + bound.center);
        return bound.center;
    }

    public Vector3 GetOffsetOf(Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                return new Vector3(0, GetBounding().y, 0);
            case Direction.DOWN:
                return new Vector3(0, 0, 0);
            case Direction.RIGHT:
                return new Vector3(GetBounding().x / 2, 0, 0);
            case Direction.LEFT:
                return new Vector3(-GetBounding().x / 2, 0, 0);
            case Direction.FRONT:
                return new Vector3(0, 0, GetBounding().z / 2);
            case Direction.BACK:
                return new Vector3(0, 0, -GetBounding().z / 2);
        }
        return Vector3.zero;
    }

    private float GetObDisHorizontal(PComponent comp, bool isInside)
    {
        Bounds myBound = GetComponent<Renderer>().bounds;
        Bounds otherBound = comp.GetComponent<Renderer>().bounds;
        float extent = myBound.extents.x + otherBound.extents.x;
        if (isInside)
        {
            extent = -extent;
        }
        if (otherBound.center.x > myBound.center.x)
        {
            Bounds tmp = myBound;
            myBound = otherBound;
            otherBound = tmp;
        }

        return myBound.center.x - otherBound.center.x + extent;
    }

    private float GetObDisVertical(PComponent comp, bool isInside)
    {
        Bounds myBound = GetComponent<Renderer>().bounds;
        Bounds otherBound = comp.GetComponent<Renderer>().bounds;
        float extent = myBound.extents.y + otherBound.extents.y;
        if (otherBound.center.y > myBound.center.y)
        {
            Bounds tmp = myBound;
            myBound = otherBound;
            otherBound = tmp;
        }
        
        return myBound.center.y - otherBound.center.y + extent;
    }

    public void SetCompWidth(float width) {
        Vector3 bound = GetBounding();
        transform.localScale = new Vector3(transform.localScale.x * width / bound.x, transform.localScale.y, transform.localScale.z);
    }

    public void SetCompHeight(float height)
    {
        Vector3 bound = GetBounding();
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * height / bound.y, transform.localScale.z);
    }

    public Vector3 SetLocalPos(Vector3 reletivePos, bool isReletive=false)
    {
        reletivePos.x *= transform.localScale.x / transform.lossyScale.x;
        reletivePos.y *= transform.localScale.y / transform.lossyScale.y;
        reletivePos.z *= transform.localScale.z / transform.lossyScale.z;
        if (isReletive)
        {
            transform.localPosition += reletivePos;
        }
        else
        {
            transform.localPosition = reletivePos;
        }
        return transform.localPosition;
    }

    public Vector3 SetLocalPosX(float reletivePos, bool isReletive = false)
    {
        reletivePos *= transform.localScale.x / transform.lossyScale.x;
        if (isReletive)
        {
            transform.localPosition += new Vector3(reletivePos, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(reletivePos, transform.localPosition.y, transform.localPosition.z);
        }
        return transform.localPosition;
    }

    public Vector3 SetLocalPosY(float reletivePos, bool isReletive = false)
    {
        reletivePos *= transform.localScale.y / transform.lossyScale.y;
        if (isReletive)
        {
            transform.localPosition += new Vector3(0, reletivePos, 0);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, reletivePos, transform.localPosition.z);
        }
        return transform.localPosition;
    }

    public float GetParentWidth(Direction dir)
    {
        PComponent parentPComp = transform.parent.GetComponent<PComponent>();
        if (parentPComp == null)
        {
            Debug.LogError("Parent does not exist a PComponent");
            return 0;
        }
        float width = 0;
        if (parentPComp._componentType == PailouComponent.FLOWER_BOARD || parentPComp._componentType == PailouComponent.MIDDLE_TOUKUNG ||
            parentPComp._componentType == PailouComponent.ROOF)
        {
            width = parentPComp.GetParentWidth(dir);
        }
        else if (parentPComp._componentType == PailouComponent.LINTEL)
        {
            width = parentPComp.GetWidth(dir);
        }
        else
        {
            Debug.LogError("Parent does not exist a lintel component!");
        }
        return width;
    }

    public float GetWidth(Direction dir)
    {
        float width = GetBounding().x;
        PComponent pillar0 = null;
        PComponent pillar1 = null;
        if (dir == Direction.DOWN)
        {
            for (int i = 0; i < _compsUnder.Count; i++)
            {
                if (_compsUnder[i]._componentType == PailouComponent.PILLAR)
                {
                    if (pillar0 == null)
                    {
                        pillar0 = _compsUnder[i];
                    }
                    else
                    {
                        pillar1 = _compsUnder[i];
                    }
                }
            }
        }
        else if (dir == Direction.UP)
        {
            for (int i = 0; i < _compsUpper.Count; i++)
            {
                if (_compsUpper[i]._componentType == PailouComponent.PILLAR)
                {
                    if (pillar0 == null)
                    {
                        pillar0 = _compsUpper[i];
                    }
                    else
                    {
                        pillar1 = _compsUpper[i];
                    }
                }
            }
        }
        if (pillar0 != null && pillar1 != null)
        {
            width = pillar0.GetObDisHorizontal(pillar1, true);
        }
        else if (pillar0 != null)
        {
            width -= pillar0.GetBounding().x * 1.1f;
        }
        return width;
    }

    public Vector3 SetLocalPosZ(float reletivePos, bool isReletive = false)
    {
        reletivePos *= transform.localScale.z / transform.lossyScale.z;
        if (isReletive)
        {
            transform.localPosition += new Vector3(0, 0, reletivePos);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, reletivePos);
        }
        return transform.localPosition;
    }

    public float GetCompCenterBias()
    {
        if (_componentType == PailouComponent.LINTEL)
        {
            return 0;
        }
        else
        {
            float bias = _centerPos.x * transform.lossyScale.x / transform.localScale.x;           
            return transform.parent.GetComponent<PComponent>().GetCompCenterBias() + bias;
        }
    }

    public void AttachWith(PComponent other, Direction dir, Margin margin=Margin.CENTER, float marginDis=0, Margin verMargin = Margin.CENTER)
    {
        // Dont scale with parent
        //other.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
        bool isMirror = false;
        switch (dir)
        {
            case Direction.UP:
                _compsUpper.Add(other);
                other._compsUnder.Add(this);
                other._stage = _stage;
                break;
            case Direction.DOWN:
                _compsUnder.Add(other);
                other._compsUpper.Add(this);
                other._stage = _stage;
                break;
            case Direction.LEFT:
                _compsLeft.Add(other);
                other._compsRight.Add(this);
                other._stage = _stage;
                break;
            case Direction.RIGHT:
                _compsRight.Add(other);
                other._compsLeft.Add(this);
                other._stage = _stage;
                if (other._componentType != PailouComponent.ROOF_EDGE && other._componentType != PailouComponent.SIDE_TOUKUNG &&
                    other._componentType != PailouComponent.QUETI && other._componentType != PailouComponent.YUNDAN)
                    other._stage += 1;
                break;
        }

        // Before the position is set.
        // Count length for dynamic length content
        PComponent parentPComp = null;
        other.transform.localScale = new Vector3(other.transform.localScale.x * PailouUtils._gameManager._widthSlider.value, 
            other.transform.localScale.y * PailouUtils._gameManager._heightSlider.value, other.transform.localScale.z * PailouUtils._gameManager._widthSlider.value);
        switch (_componentType)
        {
            case PailouComponent.LINTEL:
                {
                    if (other._componentType == PailouComponent.PILLAR)
                    {
                        margin = Margin.RIGHT;
                        if (dir == Direction.DOWN)
                        {
                            marginDis = -GetBounding().x * 0.05f;
                            if (_stage == 0)
                            {
                                isMirror = true;
                            }
                        }
                        else if (dir == Direction.UP)
                        {
                            isMirror = true;
                            marginDis = -GetBounding().x * 0.2f;
                            other.transform.localScale = new Vector3(other.transform.localScale.x, other.transform.localScale.y * 0.5f, other.transform.localScale.z);
                        } 
                        else if (dir == Direction.RIGHT)
                        {
                            margin = Margin.CENTER;
                        }
                    }
                    else if (other._componentType == PailouComponent.ROOF)
                    {
                        margin = Margin.CENTER;

                    }
                    else if (other._componentType == PailouComponent.MIDDLE_TOUKUNG)
                    {
                        margin = Margin.CENTER;
                        parentPComp = this;
                        
                    }
                    else if (other._componentType == PailouComponent.FLOWER_BOARD)
                    {
                        margin = Margin.CENTER;
                        parentPComp = this;
                    }
                    else if (other._componentType == PailouComponent.QUETI)
                    {
                        margin = Margin.RIGHT;
                        isMirror = true;
                    }
                }
                break;
            case PailouComponent.PILLAR:
                {
                    if (other._componentType == PailouComponent.PILLAR_BASE)
                    {
                        isMirror = true;
                    }

                    if (dir == Direction.UP)
                    {
                        if (other._componentType == PailouComponent.LINTEL)
                        {
                            other.SetCompWidth(GetObDisHorizontal(_procedureContainer[0], false) * 1.1f);
                            //other.transform.localScale = new Vector3(other.transform.localScale.x * 0.5f, other.transform.localScale.y, other.transform.localScale.z);
                        }
                    }
                    else if (dir == Direction.RIGHT)
                    {
                        verMargin = Margin.TOP;
                        //margin = Margin.OUT_LEFT;
                        
                    }
                    else if (dir == Direction.LEFT)
                    {
                        other.SetCompWidth(GetObDisHorizontal(_procedureContainer[0], true));
                        verMargin = Margin.TOP;
                    }
                }
                break;
            case PailouComponent.MIDDLE_TOUKUNG:
                {
                    if (other._componentType == PailouComponent.ROOF)
                    {
                        // Get lintel
                        parentPComp = transform.parent.GetComponent<PComponent>();
                    }
                    else if (other._componentType == PailouComponent.SIDE_TOUKUNG)
                    {
                        // Get lintel
                        isMirror = true;
                        parentPComp = transform.parent.GetComponent<PComponent>();
                    }
                }
                break;
            case PailouComponent.ROOF:
                {
                    if (other._componentType == PailouComponent.ROOF_EDGE)
                    {
                        isMirror = true;
                        // Get lintel
                        parentPComp = transform.parent.parent.GetComponent<PComponent>();
                    }
                }
                break;
            case PailouComponent.FLOWER_BOARD:
                {
                    if (other._componentType == PailouComponent.LINTEL)
                    {
                        // Get lintel
                        PComponent lintel = transform.parent.GetComponent<PComponent>();
                        other.SetCompWidth(GetParentWidth(dir));
                    }
                }
                break;
            case PailouComponent.QUETI:
                {
                    if (other._componentType == PailouComponent.YUNDAN)
                    {
                        margin = Margin.RIGHT;
                        isMirror = true;
                    }
                }
                break;
        }
        
        other.transform.parent = transform;
        other.transform.localPosition = Vector3.zero;
        // Debug.Log("Parent offset :" + GetOffsetOf(dir));
        // Debug.Log("Parent center bias :" + GetCenterBias());
        // Debug.Log("Parent size:" + GetBounding());
        // Debug.Log("Other offset :" + other.GetOffsetOf(PailouUtils.Opposite(dir)));
        // Debug.Log("Other center bias :" + other.GetCenterBias());
        // Debug.Log("Other size:" + other.GetBounding());
        other.SetLocalPos(GetOffsetOf(dir) - other.GetOffsetOf(PailouUtils.Opposite(dir)));
        
        // After the position is set.
        switch (_componentType)
        {
            case PailouComponent.LINTEL:
                {
                    if (other._componentType == PailouComponent.PILLAR)
                    {
                        if (dir == Direction.RIGHT)
                            verMargin = Margin.TOP;
                        if (dir == Direction.DOWN && _stage == 0)
                        {
                            PailouUtils.minY = other.transform.position.y - other.GetOffsetOf(dir).y;
                        }
                    }
                    else if (other._componentType == PailouComponent.ROOF)
                    {
                        
                    }
                }
                break;
            case PailouComponent.PILLAR:
                {
                    if (other._componentType == PailouComponent.LINTEL)
                    {
                        if (dir == Direction.UP || dir == Direction.DOWN)
                        {

                            if (_procedureContainer.Count <= 0 || _procedureContainer.Count > 1)
                            {
                                Debug.LogError("Pillar should not store more or less than 1 mirrored pillar");
                                break;
                            }
                            float x = (_procedureContainer[0].transform.position.x + transform.position.x) / 2f;
                            other.transform.position = new Vector3(x, other.transform.position.y, other.transform.position.z);
                        }
                        else if (dir == Direction.RIGHT)
                        {
                            
                        }
                    }
                }
                break;
            case PailouComponent.MIDDLE_TOUKUNG:
                {
                    if (other._componentType == PailouComponent.SIDE_TOUKUNG)
                    {
                        float width = GetBounding().x;
                        float obWidth = other.GetBounding().x;
                        float parentWidth = parentPComp.GetBounding().x;
                        int obCount = (int)(parentWidth / width);
                        if (parentPComp._componentType == PailouComponent.ROOF || 
                            parentPComp._componentType == PailouComponent.MIDDLE_TOUKUNG || parentPComp._componentType == PailouComponent.LINTEL)
                        {
                            obCount = (int)Mathf.Ceil(parentWidth / width);
                        }
                        float offset = parentWidth / (float)obCount;
                        if (obCount % 2 == 1)
                            obCount = obCount / 2;
                        else
                            obCount = obCount / 2 - 1;
                        float centerBias = (other.transform.GetComponent<Renderer>().bounds.center.x - other.transform.GetComponent<Collider>().bounds.center.x);
                        other.SetLocalPosX(offset * obCount + 0);
                    }
                }
                break;
            case PailouComponent.ROOF:
                {
                    if (other._componentType == PailouComponent.ROOF_EDGE)
                    {
                        float width = GetBounding().x;
                        float obWidth = other.GetBounding().x;
                        float parentWidth = parentPComp.GetBounding().x;
                        int obCount = (int)(parentWidth / width);
                        if (parentPComp._componentType == PailouComponent.ROOF || 
                            parentPComp._componentType == PailouComponent.MIDDLE_TOUKUNG || parentPComp._componentType == PailouComponent.LINTEL)
                        {
                            obCount = (int)Mathf.Ceil(parentWidth / width);
                        }
                        float offset = parentWidth / (float)obCount;
                        if (obCount % 2 == 1)
                            obCount = obCount / 2;
                        else
                            obCount = obCount / 2 - 1;
                        float centerBias = (other.transform.GetComponent<Renderer>().bounds.center.x - other.transform.GetComponent<Collider>().bounds.center.x);
                        float roofOffset = GetOffsetOf(dir).x + other.GetOffsetOf(PailouUtils.Opposite(dir)).x;
                        other.SetLocalPosX(offset * obCount + roofOffset);
                    }
                }
                break;
            case PailouComponent.FLOWER_BOARD:
                {
                    if (other._componentType == PailouComponent.LINTEL)
                    {
                        other.SetLocalPosX(-GetCompCenterBias());
                    }
                }
                break;
        }

        switch (margin)
        {   
            case Margin.RIGHT:
                other.SetLocalPosX(GetOffsetOf(Direction.RIGHT).x + other.GetOffsetOf(Direction.LEFT).x + marginDis, true);
                break;             
            case Margin.LEFT:
                other.SetLocalPosX(GetOffsetOf(Direction.LEFT).x + other.GetOffsetOf(Direction.RIGHT).x+ marginDis, true);
                break;
            case Margin.OUT_RIGHT:
                other.SetLocalPosX(GetOffsetOf(Direction.LEFT).x - other.GetOffsetOf(Direction.RIGHT).x + marginDis, true);
                break;
            case Margin.OUT_LEFT:
                other.SetLocalPosX(GetOffsetOf(Direction.RIGHT).x - other.GetOffsetOf(Direction.LEFT).x + marginDis, true);
                break;
        }
        
        switch (verMargin)
        {
            
            case Margin.TOP:
                other.SetLocalPosY(marginDis + GetOffsetOf(Direction.UP).y - other.GetOffsetOf(Direction.UP).y, true);
                break;
            case Margin.BOTTOM:
                other.SetLocalPosY(marginDis, true);
                break;
        }

        if (other._componentType == PailouComponent.PILLAR && (dir == Direction.DOWN || dir == Direction.RIGHT) && _stage != 0)
        {
            float top2MinY = other.transform.position.y + other.GetOffsetOf(Direction.UP).y - PailouUtils.minY;
            other.SetCompHeight(top2MinY);
            if (dir == Direction.DOWN)
            {
                other.SetLocalPosY(GetOffsetOf(Direction.DOWN).y - other.GetOffsetOf(Direction.UP).y, false);
            }
            else if (dir == Direction.RIGHT)
            {
                other.SetLocalPosY(GetOffsetOf(Direction.UP).y - other.GetOffsetOf(Direction.UP).y, false);
            }
        }
        
        List<PComponent> globalMirrorComps = new List<PComponent>();

        switch (other._procedureType)
        {
            case ProcedureType.MIRROR:
                {
                    if (isMirror)
                    {
                        float centerBias = 0;
                        GameObject mirroredOb = Instantiate(other.gameObject);
                        mirroredOb.transform.SetParent(transform);
                        Vector3 otherPos = other.transform.localPosition;
                        mirroredOb.transform.localScale = other.transform.localScale;
                        mirroredOb.transform.localRotation = new Quaternion(other.transform.localRotation.x,
                            -other.transform.localRotation.y + 180, other.transform.localRotation.z, other.transform.localRotation.w);
                        float parentScale = transform.parent.lossyScale.x / transform.localScale.x;
                        if (other._componentType == PailouComponent.ROOF_EDGE)
                            centerBias = transform.parent.GetComponent<PComponent>()._centerPos.x;
                        /*mirroredOb.transform.localPosition = new Vector3(-otherPos.x - 
                            (transform.localPosition.x + centerBias) * 2 * transform.lossyScale.x / transform.localScale.x, otherPos.y, otherPos.z);*/
                        mirroredOb.GetComponent<PComponent>().transform.localPosition = new Vector3(-otherPos.x, otherPos.y, otherPos.z);
                        mirroredOb.GetComponent<PComponent>().SetLocalPosX(-2 * GetCompCenterBias(), true);
                        other._procedureContainer.Add(mirroredOb.GetComponent<PComponent>());
                        // Special case
                        if (other._componentType == PailouComponent.QUETI)
                        {
                            other._centerPos = other.transform.localPosition;
                        }
                        if (other._stage >= 1)
                        {
                            globalMirrorComps.Add(other);
                            globalMirrorComps.Add(mirroredOb.GetComponent<PComponent>());
                        }
                        switch (dir)
                        {
                            case Direction.UP:
                                _compsUpper.Add(mirroredOb.GetComponent<PComponent>());
                                mirroredOb.GetComponent<PComponent>()._compsUnder.Add(this);
                                mirroredOb.GetComponent<PComponent>()._stage = _stage;
                                break;
                            case Direction.DOWN:
                                _compsUnder.Add(mirroredOb.GetComponent<PComponent>());
                                mirroredOb.GetComponent<PComponent>()._compsUpper.Add(this);
                                mirroredOb.GetComponent<PComponent>()._stage = _stage;
                                break;
                        }
                    }
                    else if (other._stage >= 1)
                    {
                        globalMirrorComps.Add(other);
                    }
                }
                break;
            case ProcedureType.ADAPTIVE_HORIZONTAL:
                {
                    //float mid_x = parentPComp.transform.localPosition.x;
                    float mid_x = 0;
                    if (other._componentType == PailouComponent.ROOF)
                    {
                        mid_x = -transform.localPosition.x / transform.lossyScale.x / transform.localScale.x;
                    }

                    if (_compsUnder.Count == 2 && other._componentType == PailouComponent.FLOWER_BOARD && _stage >= 1)
                    {
                        PComponent pillar = _compsUnder[0];
                        if (pillar._componentType != PailouComponent.PILLAR)
                            pillar = _compsUnder[1];
                        mid_x -= (transform.position.x + GetOffsetOf(Direction.RIGHT).x -
                            pillar.transform.position.x - pillar.GetOffsetOf(Direction.LEFT).x) / 2;
                    }
                    float width = other.GetParentWidth(dir);
                    if (_compsUnder.Count == 2 && other._componentType == PailouComponent.FLOWER_BOARD && _stage >= 1)
                    {
                        PComponent pillar = _compsUnder[0];
                        if (pillar._componentType != PailouComponent.PILLAR)
                            pillar = _compsUnder[1];
                        width -= (transform.position.x + GetOffsetOf(Direction.RIGHT).x -
                            pillar.transform.position.x - pillar.GetOffsetOf(Direction.LEFT).x) / 4f;
                    }
                    float obWidth = other.GetBounding().x;
                    int obCount = (int)(width / obWidth);
                    if (other._componentType == PailouComponent.ROOF || other._componentType == PailouComponent.MIDDLE_TOUKUNG)
                    {
                        obCount = (int)Mathf.Ceil(width / obWidth);
                    }
                    float offset = width / (float)obCount;
                    if (other._stage >= 1)
                    {
                        globalMirrorComps.Add(other);
                    }
                    if (obCount % 2 == 1)
                    {
                        // 單數個咚咚
                        List<PComponent> pComponents = new List<PComponent>();
                        other.SetLocalPosX(mid_x);
                        other._centerPos = other.transform.localPosition;
                        for (int i = 1; i <= obCount / 2; i++)
                        {
                            GameObject arrayedOb = Instantiate(other.gameObject);
                            PComponent arrayedPComp = arrayedOb.GetComponent<PComponent>();
                            arrayedOb.transform.SetParent(transform);
                            arrayedOb.transform.localPosition = other.transform.localPosition;
                            arrayedOb.transform.localScale = other.transform.localScale;
                            arrayedPComp.SetLocalPosX(mid_x + offset * (i));
                            other._procedureContainer.Add(arrayedOb.GetComponent<PComponent>());
                            pComponents.Add(arrayedOb.GetComponent<PComponent>());
                            if (other._stage >= 1)
                                globalMirrorComps.Add(arrayedOb.GetComponent<PComponent>());
                        }
                        for (int i = 0; i < pComponents.Count; i++)
                        {
                            GameObject mirroredOb = Instantiate(pComponents[i].gameObject);
                            mirroredOb.transform.SetParent(transform);
                            Vector3 otherPos = pComponents[i].transform.localPosition;
                            mirroredOb.transform.localPosition = new Vector3(-otherPos.x, otherPos.y, otherPos.z);
                            mirroredOb.GetComponent<PComponent>().SetLocalPosX(2 * mid_x, true);
                            mirroredOb.transform.localScale = other.transform.localScale;
                            other._procedureContainer.Add(mirroredOb.GetComponent<PComponent>());
                            if (other._stage >= 1)
                                globalMirrorComps.Add(mirroredOb.GetComponent<PComponent>());
                        }
                    }
                    else
                    {
                        // 雙數個咚咚
                        List<PComponent> pComponents = new List<PComponent>();
                        other.SetLocalPosX(mid_x + offset / 2);
                        pComponents.Add(other);
                        other._centerPos = other.transform.localPosition;
                        for (int i = 1; i < obCount / 2; i++)
                        {
                            GameObject arrayedOb = Instantiate(other.gameObject);
                            PComponent arrayedPComp = arrayedOb.GetComponent<PComponent>();
                            arrayedOb.transform.SetParent(transform);
                            arrayedOb.transform.localPosition = other.transform.localPosition;
                            arrayedOb.transform.localScale = other.transform.localScale;
                            arrayedPComp.SetLocalPosX(mid_x + offset * (i + 0.5f));
                            other._procedureContainer.Add(arrayedOb.GetComponent<PComponent>());
                            pComponents.Add(arrayedPComp);
                            if (other._stage >= 1)
                                globalMirrorComps.Add(arrayedOb.GetComponent<PComponent>());
                        }
                        for (int i = 0; i < pComponents.Count; i++)
                        {
                            GameObject mirroredOb = Instantiate(pComponents[i].gameObject);
                            mirroredOb.transform.SetParent(transform);
                            Vector3 otherPos = pComponents[i].transform.localPosition;
                            mirroredOb.transform.localPosition = new Vector3(-otherPos.x, otherPos.y, otherPos.z);
                            mirroredOb.GetComponent<PComponent>().SetLocalPosX(2 * mid_x, true);
                            mirroredOb.transform.localScale = other.transform.localScale;
                            other._procedureContainer.Add(mirroredOb.GetComponent<PComponent>());
                            if (other._stage >= 1)
                                globalMirrorComps.Add(mirroredOb.GetComponent<PComponent>());
                        }
                    }
                }
                break;
        }

        // Global mirror
        for (int i = 0; i < globalMirrorComps.Count; i++)
        {
            GameObject mirroredOb = Instantiate(globalMirrorComps[i].gameObject);
            mirroredOb.transform.SetParent(globalMirrorComps[i].transform.parent);
            mirroredOb.transform.localScale = globalMirrorComps[i].transform.localScale;
            Vector3 mirroredPos = globalMirrorComps[i].transform.position;
            mirroredPos.x = -mirroredPos.x;
            mirroredOb.transform.position = mirroredPos;
            Quaternion mirroredRot = globalMirrorComps[i].transform.rotation;
            //mirroredRot.SetFromToRotation(Vector3.right, Vector3.left);
            mirroredRot *= Quaternion.Euler(0, 180, 0);
            mirroredOb.transform.rotation = mirroredRot;
        }


    }
}
