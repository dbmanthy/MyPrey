using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest> ();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    PathFinding pathfinding;

    bool isProcessingPath;

    struct PathRequest {
        public Vector3 pathStart, pathFin;
        public
        Action<Vector3[], bool> callBack;

        public PathRequest(Vector3 _pathStart, Vector3 _pathFin, Action<Vector3[], bool> _callBack)
        {
            pathStart = _pathStart;
            pathFin = _pathFin; 
            callBack = _callBack;

        }
    }

    void Awake () {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }

    public static void RequestPath (Vector3 pathStart, Vector3 pathFin, Action<Vector3[], bool> callBack) {
        PathRequest newRequest = new PathRequest(pathStart, pathFin, callBack);
        instance.pathRequestsQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext () {
        if (!isProcessingPath && pathRequestsQueue.Count > 0) {
            currentPathRequest = pathRequestsQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathFin);
        }
    }

    public void FinishedProcessingPath (Vector3[] path, bool success) {
        currentPathRequest.callBack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
}
