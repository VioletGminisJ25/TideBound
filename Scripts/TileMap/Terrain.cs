using Godot;
using System;

public partial class Terrain : TileMapLayer
{
    AStarGrid2D astargrid;
    private int mainLayer = 0;
    private int mainSource = 7;
    private Vector2I pathTakenAtlasCoords = new Vector2I(4, 2);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        astargrid = new AStarGrid2D();
        setupGrid();
        showPath();
    }
    public void setupGrid()
    {
        astargrid.Region = new Rect2I(0, 0, 130, 130);
        astargrid.CellSize = new Vector2(16, 16);
        astargrid.Update();
    }

    public void showPath()
    {
        var path = astargrid.GetIdPath(new Vector2I(0, 0), new Vector2I(30, 30));

        SetCellsTerrainConnect(path, mainLayer, mainSource, true);

        

    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
