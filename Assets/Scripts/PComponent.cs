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

    public void AttachWith(PComponent other, Direction dir, Margin margin, float marginDis)
    {
        // Dont scale with parent
        //other.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
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
                other._stage = _stage + 1;
                break;
        }

        // Before the position is set.
        
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
                        }
                        else if (dir == Direction.UP)
                        {
                            marginDis = -GetBounding().x * 0.2f;
                            other.transform.localScale = new Vector3(other.transform.localScale.x, other.transform.localScale.y * 0.5f, other.transform.localScale.z);
                        }
                    }
                    else if (other._componentType == PailouComponent.ROOF)
                    {
                        margin = Margin.CENTER;

                    }
                    else if (other._componentType == PailouComponent.MIDDLE_TOUKUNG)
                    {
                        margin = Margin.CENTER;

                    }
                }
                break;
            case PailouComponent.PILLAR:
                {



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

                    }
                    else if (dir == Direction.LEFT)
                    {
                        other.SetCompWidth(GetObDisHorizontal(_procedureContainer[0], true));
                        margin = Margin.TOP;
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
                        if (_procedureContainer.Count <= 0 || _procedureContainer.Count > 1)
                        {
                            Debug.LogError("Pillar should not store more or less than 1 mirrored pillar");
                            break;
                        }
                        float x = (_procedureContainer[0].transform.position.x + transform.position.x) / 2f;
                        other.transform.position = new Vector3(x, other.transform.position.y, other.transform.position.z);

                    }
                }
                break;
            case PailouComponent.MIDDLE_TOUKUNG:
                {
                    if (other._componentType == PailouComponent.SIDE_TOUKUNG)
                    {
                        float width = GetBounding().x;
                        float obWidth = other.GetBounding().x;
                        PComponent parrent = transform.parent.GetComponent<PComponent>();
                        float parentWidth = parrent.GetBounding().x;
                        int obCount = (int)(parentWidth / width);
                        float offset = parentWidth / (float)obCount;
                        if (obCount % 2 == 0)
                            obCount = obCount / 2 - 1;
                        else
                            obCount = obCount / 2;
                        float centerBias = (other.transform.GetComponent<Renderer>().bounds.center.x - other.transform.GetComponent<Collider>().bounds.center.x);
                        other.SetLocalPosX(offset * obCount + 0);
                    }
                }
                break;
        }

        switch (margin)
        {
            case Margin.TOP:
                other.SetLocalPosY(marginDis + GetOffsetOf(Direction.UP).y - other.GetOffsetOf(Direction.UP).y, true);
                break;
            case Margin.BOTTOM:
                other.SetLocalPosY(marginDis, true);
                break;             
            case Margin.RIGHT:
                other.SetLocalPosX(GetOffsetOf(Direction.RIGHT).x + other.GetOffsetOf(Direction.LEFT).x + marginDis, true);
                break;             
            case Margin.LEFT:
                other.SetLocalPosX(GetOffsetOf(Direction.LEFT).x + other.GetOffsetOf(Direction.RIGHT).x+ marginDis, true);
                break;
        }
        
        switch (other._procedureType)
        {
            case ProcedureType.MIRROR:
                {
                    if (_stage >= 1 || other._componentType == PailouComponent.PILLAR)
                    {
                        GameObject mirroredOb = Instantiate(other.gameObject);
                        mirroredOb.transform.SetParent(transform);
                        Vector3 otherPos = other.transform.localPosition;
                        mirroredOb.transform.localPosition = new Vector3(-otherPos.x, otherPos.y, otherPos.z);
                        mirroredOb.transform.localScale = other.transform.localScale;
                        mirroredOb.transform.localRotation = new Quaternion(other.transform.localRotation.x, 
                            -other.transform.localRotation.y, other.transform.localRotation.z, other.transform.localRotation.w);
                        other._procedureContainer.Add(mirroredOb.GetComponent<PComponent>());
                    }
                }
                break;
            case ProcedureType.ADAPTIVE_HORIZONTAL:
                {
                    float mid_x = other.transform.localPosition.x;
                    float width = GetBounding().x;
                    float obWidth = other.GetBounding().x;
                    int obCount = (int)(width / obWidth);
                    float offset = width / (float)obCount;
                    if (obCount % 2 == 0)
                    {
                        // 雙數個咚咚
                        List<PComponent> pComponents = new List<PComponent>();
                        other.SetLocalPosX(mid_x + offset / 2);
                        pComponents.Add(other);
                        for (int i = 1; i < obCount / 2; i++)
                        {
                            GameObject arrayedOb = Instantiate(other.gameObject);
                            PComponent arrayedPComp = arrayedOb.GetComponent<PComponent>();
                            arrayedOb.transform.SetParent(transform);
                            arrayedOb.transform.localPosition = other.transform.localPosition;
                            arrayedOb.transform.localScale = other.transform.localScale;
                            arrayedPComp.SetLocalPosX(mid_x + offset * (i + 0.5f));
                            other._procedureContainer.Add(arrayedOb.GetComponent<PComponent>());
                            pComponents.Add(arrayedOb.GetComponent<PComponent>());
                        }
                        for (int i = 0; i < pComponents.Count; i++)
                        {
                            GameObject mirroredOb = Instantiate(pComponents[i].gameObject);
                            mirroredOb.transform.SetParent(transform);
                            Vector3 otherPos = pComponents[i].transform.localPosition;
                            mirroredOb.transform.localPosition = new Vector3(-otherPos.x, otherPos.y, otherPos.z);
                            mirroredOb.transform.localScale = other.transform.localScale;
                            other._procedureContainer.Add(mirroredOb.GetComponent<PComponent>());
                        }
                    }
                    else
                    {
                        // 單數個咚咚
                        List<PComponent> pComponents = new List<PComponent>();
                        for (int i = 1; i <= (int)obCount / 2; i++)
                        {
                            GameObject arrayedOb = Instantiate(other.gameObject);
                            PComponent arrayedPComp = arrayedOb.GetComponent<PComponent>();
                            arrayedOb.transform.SetParent(transform);
                            arrayedOb.transform.localPosition = other.transform.localPosition;
                            arrayedOb.transform.localScale = other.transform.localScale;
                            arrayedPComp.SetLocalPosX(mid_x + offset * (i));
                            other._procedureContainer.Add(arrayedOb.GetComponent<PComponent>());
                            pComponents.Add(arrayedOb.GetComponent<PComponent>());
                        }
                        for (int i = 0; i < pComponents.Count; i++)
                        {
                            GameObject mirroredOb = Instantiate(pComponents[i].gameObject);
                            mirroredOb.transform.SetParent(transform);
                            Vector3 otherPos = pComponents[i].transform.localPosition;
                            mirroredOb.transform.localPosition = new Vector3(-otherPos.x, otherPos.y, otherPos.z);
                            mirroredOb.transform.localScale = other.transform.localScale;
                            other._procedureContainer.Add(mirroredOb.GetComponent<PComponent>());
                        }
                    }
                }
                break;
        }
    }
}
