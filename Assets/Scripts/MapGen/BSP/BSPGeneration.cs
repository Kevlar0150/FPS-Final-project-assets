using System.Collections.Generic;
using UnityEngine;

// All code has been produced following the tutorial by Sunny Valley Studio, 2019 (https://www.youtube.com/watch?v=VnqN0v95jtU&list=PLcRSafycjWFfEPbSSjGMNY-goOZTuBPMW&index=1)
internal class BSPGeneration
{
    // Determines the size of the dungeon

    private int dungeonWidth;
    private int dungeonLength;

    List<RoomNode> allSpaceNodes = new List<RoomNode>();

    public BSPGeneration(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    public List<Node> CalculateRooms(int maxIteration, int roomWidthMin, int roomLengthMin)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIteration, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIteration, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces);
        return new List<Node>(allSpaceNodes);
    }
}