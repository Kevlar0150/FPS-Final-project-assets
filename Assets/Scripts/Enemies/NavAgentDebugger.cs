using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Entire code taken from youtube video https://www.youtube.com/watch?v=nrRfqS6u_zg posted by Jason Weimann, 2017.
// Not used in the full project and was only used for debugging.
public class NavAgentDebugger : MonoBehaviour
{
    public NavMeshAgent agentToDebug;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (agentToDebug.hasPath)
        {
            lineRenderer.positionCount = agentToDebug.path.corners.Length;
            lineRenderer.SetPositions(agentToDebug.path.corners);
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }     
    }
}
