using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PComponent : MonoBehaviour
{
    public PailouComponent _componentType;

    public List<PComponent> _compsUnder = new List<PComponent>();
    public List<PComponent> _compsUpper = new List<PComponent>();
    public List<PComponent> _compsSide = new List<PComponent>();

    public GameObject _myButton;
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
        return GetComponent<Renderer>().bounds.size;
    }

    public Vector3 GetCenterBias()
    {
        Debug.Log("My center" + GetComponent<Renderer>().bounds.center);
        return GetComponent<Renderer>().bounds.center;
    }



    public Vector3 GetOffsetOf(Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                return new Vector3(0, GetBounding().y / 2, 0);
            case Direction.DOWN:
                return new Vector3(0, -GetBounding().y / 2, 0);
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

    public void AttachWith(PComponent other, Direction dir, Margin margin, float marginDis)
    {
        other.transform.parent = transform;
        other.transform.localPosition = GetOffsetOf(dir) - GetCenterBias() - other.GetOffsetOf(PailouUtils.Opposite(dir)) + other.GetCenterBias();
        switch (margin)
        {
            case Margin.TOP:
                PailouUtils.SetLocalY(other.transform, GetOffsetOf(Direction.UP).y - other.GetOffsetOf(Direction.UP).y - other.GetCenterBias().y + marginDis);
                break;
            case Margin.BOTTOM:
                PailouUtils.SetLocalY(other.transform, GetOffsetOf(Direction.DOWN).y - other.GetOffsetOf(Direction.DOWN).y - other.GetCenterBias().y + marginDis);
                break;
            case Margin.RIGHT:
                PailouUtils.SetLocalX(other.transform, GetOffsetOf(Direction.RIGHT).x - other.GetOffsetOf(Direction.RIGHT).x - other.GetCenterBias().x + marginDis);
                break;
            case Margin.LEFT:
                PailouUtils.SetLocalX(other.transform, GetOffsetOf(Direction.LEFT).x - other.GetOffsetOf(Direction.LEFT).x - other.GetCenterBias().x + marginDis);
                break;
        }
    }
}
