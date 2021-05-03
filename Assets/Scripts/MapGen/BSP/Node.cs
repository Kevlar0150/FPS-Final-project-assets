using System;
using System.Collections.Generic;
using UnityEngine;

// All code has been produced following the tutorial by Sunny Valley Studio, 2019 (https://www.youtube.com/watch?v=VnqN0v95jtU&list=PLcRSafycjWFfEPbSSjGMNY-goOZTuBPMW&index=1)
public abstract class Node
{
    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList { get => childrenNodeList;}

    public bool Visited { get; set; }
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }

    public int TreeLayerIndex { get; set; }
    public Node Parent { get; set; }

    public Node(Node parentNode)
    {
        childrenNodeList = new List<Node>();
        this.Parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node);
    }
}