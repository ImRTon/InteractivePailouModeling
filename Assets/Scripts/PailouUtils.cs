using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PailouUtils
{
    public static GameManager _gameManager;
    public static Component _nowFocusComp;
    public static RectTransform GetRectTransform(GameObject gameObject)
    {
        return gameObject.GetComponent<RectTransform>();
    }
    
    public static Direction Opposite(Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                return Direction.DOWN;
            case Direction.DOWN:
                return Direction.UP;
            case Direction.LEFT:
                return Direction.RIGHT;
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.FRONT:
                return Direction.BACK;
            case Direction.BACK:
                return Direction.FRONT;
        }
        return Direction.UP;
    }

    public static void SetLocalX(Transform transform, float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    public static void SetLocalY(Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }

    public static void SetLocalZ(Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }
}    
public enum PailouComponent
{
    UNDEFINED,
    ROOF,
    ROOF_EDGE,
    MIDDLE_TOUKUNG,
    SIDE_TOUKUNG,
    LINTEL,
    FLOWER_BOARD,
    QUETI,
    YUNDAN,
    PILLAR,
    PILLAR_BASE,
    // For all component in live 3d view.
    // http://graphics.csie.ntust.edu.tw/pub/PailouModeling/images/Overview.jpg
}

public enum Margin
{
    // Horizontal
    LEFT,
    CENTER,
    RIGHT,
    
    // Vertical
    TOP,
    MIDDLE,
    BOTTOM,
}

public enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    FRONT,
    BACK
}