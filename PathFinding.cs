using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour {

    PathRequestManager requestManager; 
    Grid grid;

    void Awake () {
        grid = GetComponent<Grid>();//must have both scripts attacked to an object in order for this to work
        requestManager = GetComponent<PathRequestManager>();
    }

    IEnumerator FindPath (Vector3 startPos, Vector3 targetPos) {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSucess = false;

        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node targetNode = grid.GetNodeFromWorldPosition(targetPos);


        if (startNode.walkable && targetNode.walkable) { 
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize );
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode); 

                if(currentNode == targetNode){
                    sw.Stop();
                    print("Path found in: " + sw.ElapsedMilliseconds + " ms");
                    pathSucess = true;
                    break;
                }

                foreach (Node neighbourNode in grid.GetNeighbours(currentNode)) {
                    if (!neighbourNode.walkable || closedSet.Contains(neighbourNode)) {
                        continue;
                    }

                    int newMovmentCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourNode);
                    if (newMovmentCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode)) {
                        neighbourNode.gCost = newMovmentCostToNeighbour;
                        neighbourNode.hCost = GetDistance(neighbourNode, targetNode);
                        neighbourNode.parent = currentNode;

                        if (!openSet.Contains(neighbourNode)) {
                            openSet.Add(neighbourNode);
                        }
                        else{
                            openSet.UpdateItem(neighbourNode);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSucess) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSucess);
    }

    Vector3[] RetracePath (Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode); 
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 dirOfOld = Vector3.zero;

        for(int i = 1; i < path.Count; i++) {
            Vector3 dirOfNew = new Vector3(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY, path[i - 1].gridZ - path[i].gridZ);
            if(dirOfNew != dirOfOld) {
                waypoints.Add(path[i].worldPostion);
            }
            dirOfOld = dirOfNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance (Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        //return Mathf.Sqrt()

        //NEED TO OPTIMIZE https://www.youtube.com/watch?v=mZfyt03LDH4 @ 15:00
        if (dstX < dstY)
            return 14 * dstX + 10 * (dstY - dstX) + dstZ;
        return 14 * dstY + 10 * (dstX - dstY) + dstZ;
    }

    public void StartFindPath (Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));  
    }
}
