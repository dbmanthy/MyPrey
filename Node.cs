using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    //need to figure out how to implement the other states???? othe bools?
    //public enum moveState {walkable, climbable, notWalkable}
    //public moveState traverseState;
    //public Vector3 worldPostion;

    //public Node (moveState _traverseState, Vector3 _worldPosition) {
    //    traverseState = _traverseState;
    //    worldPostion = _worldPosition;
    //}

    public bool walkable;
    public Vector3 worldPostion;
    public int gridX, gridY, gridZ;
    public int gCost, hCost;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, int _gridZ)
    {
        walkable = _walkable;
        worldPostion = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        gridZ = _gridZ;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
