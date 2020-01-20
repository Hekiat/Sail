using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HexCoord
{
    public int q { get; set; }
    public int r { get; set; }
    public int s { get; set; }
}

public struct SquareCoord
{
    public int x { get; set; }
    public int y { get; set; }
}

public struct TileCoord
{
    public TileCoord(int x, int y)
        : this()
    {
        _Square.x = x;
        _Square.y = y;

        updateHex();
    }

    private HexCoord _Hex;
    public HexCoord Hex { get { return _Hex; } set { _Hex = value; updateSquare(); } }

    private SquareCoord _Square;
    public SquareCoord Square { get { return _Square; } set { _Square = value; updateHex(); } }

    void updateHex()
    {
        var q = _Square.x - (_Square.y - (_Square.y & 1)) / 2;
        var s = _Square.y;
        var r = -q - s;

        _Hex.q = q;
        _Hex.r = r;
        _Hex.s = s;
    }

    void updateSquare()
    {
        var x = _Hex.q + (_Hex.s - (_Hex.s & 1)) / 2;
        var y = _Hex.s;

        _Square.x = x;
        _Square.y = y;
    }

    public void setX(int x) { _Square.x = x; updateHex(); }
    public void setY(int y) { _Square.y = y; updateHex(); }

    public void setQ(int q) { _Hex.q = q; updateSquare(); }
    public void setR(int r) { _Hex.r = r; updateSquare(); }
    public void setS(int s) { _Hex.s = s; updateSquare(); }

    public bool isValid()
    {
        var x = Square.x;
        var y = Square.y;
        var w = GlobalManagers.mapManager.Width;
        var h = GlobalManagers.mapManager.Height;

        return x >= 0 && x < w && y >= 0 && y < h;
    }
}
