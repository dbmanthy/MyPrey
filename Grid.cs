using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    //fields
    public Transform player;

    //grid properties
    public LayerMask unwalkableMask, walkableMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    Node[,,] grid;

    //grid calculations
    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;


    void Start () {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    void Update () {
    }

    void CreateGrid () {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2 - Vector3.forward * gridWorldSize.z / 2;

        for (int x=0; x<gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                for (int z = 0; z< gridSizeZ; z++) {
                    //is its smarter to create each of these variable once rather than a dozen times????????
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)) && (Physics.CheckSphere(worldPoint, nodeRadius, walkableMask));
                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z);
                }
            }
        }
    }

    public List<Node> GetNeighbours (Node  node) {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                for (int z = -1; z <= 1; z++) {
                    if ( x == 0 && y == 0 && z == 0) {
                        continue;
                    }
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ) {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }
        return neighbours;
    }


    public Node GetNodeFromWorldPosition (Vector3 worldPosition) {
        float pecentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float pecentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        float pecentZ = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;
        pecentX = Mathf.Clamp01(pecentX);
        pecentY = Mathf.Clamp01(pecentY);
        pecentZ = Mathf.Clamp01(pecentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * pecentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * pecentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * pecentZ);

        return grid[x, y, z];
    }


    public List<Node> path;
    private void OnDrawGizmos (){
        Gizmos.DrawWireCube(transform.position, gridWorldSize);
        if (grid != null) {
            Node playerNode = GetNodeFromWorldPosition(player.position);
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable) ? Color.white : Color.clear;
                if (playerNode == n) {
                    Gizmos.color = Color.magenta;
                }
                if (path != null){
                    if(path.Contains(n)){
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(n.worldPostion, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
