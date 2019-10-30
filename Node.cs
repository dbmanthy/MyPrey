using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    public bool walkable;
    public Vector3 worldPostion;
    public int gridX, gridY, gridZ;
    public int gCost, hCost;
    public Node parent;
    int heapIndex;

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

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set { 
            heapIndex = value;
        }
    }

    public int CompareTo (Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
