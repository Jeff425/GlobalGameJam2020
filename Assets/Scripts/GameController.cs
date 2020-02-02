using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField]
    Tilemap black;
    [SerializeField]
    Tilemap yellow;
    [SerializeField]
    Tilemap orange;
    [SerializeField]
    Tilemap blue;

    Collider2D blackCollider;
    Collider2D yellowCollider;
    Collider2D orangeCollider;
    Collider2D blueCollider;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        Reset();
        blackCollider = black.GetComponent<Collider2D>();
        yellowCollider = yellow.GetComponent<Collider2D>();
        orangeCollider = orange.GetComponent<Collider2D>();
        blueCollider = blue.GetComponent<Collider2D>();
    }

    void RotateTiles(Tilemap map, Direction direction) {
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        switch (direction) {
            case Direction.down:
                rot = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.left:
                rot = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.right:
                rot = Quaternion.Euler(0, 0, 270);
                break;
        }
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        BoundsInt bounds = map.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++) {
            for  (int y = bounds.yMin; y < bounds.yMax; y++) {
                map.SetTransformMatrix(new Vector3Int(x, y, 0), matrix);
            }
        }
    }

    public void RotateTilesOfColor(Colors color) {
        if (color == Colors.black) {
            RotateTiles(black, GameVariables.Black);
        } else if (color == Colors.yellow) {
            RotateTiles(yellow, GameVariables.Yellow);
        } else if (color == Colors.orange) {
            RotateTiles(orange, GameVariables.Orange);
        } else if (color == Colors.blue) {
            RotateTiles(blue, GameVariables.Blue);
        }
    }

    public Direction GetGravityDirection(Tilemap map) {
        if (map == black) {
            return GameVariables.Black;
        }
        if (map == yellow) {
            return GameVariables.Yellow;
        }
        if (map == blue) {
            return GameVariables.Blue;
        }
        if (map == orange) {
            return GameVariables.Orange;
        }
        return Direction.notset;
    }

    public Direction GetDirectionFromPoint(Vector2 point) {
        if (blackCollider.OverlapPoint(point)) {
            return GameVariables.Black;
        }
        if (yellowCollider.OverlapPoint(point)) {
            return GameVariables.Yellow;
        }
        if (blueCollider.OverlapPoint(point)) {
            return GameVariables.Blue;
        }
        if (orangeCollider.OverlapPoint(point)) {
            return GameVariables.Orange;
        }
        return Direction.notset;
    }

    public void Reset() {
        GameVariables.Black = Direction.up;
        GameVariables.Yellow = Direction.right;
        GameVariables.Orange = Direction.left;
        GameVariables.Blue = Direction.down;
        RotateTiles(black, GameVariables.Black);
        RotateTiles(yellow, GameVariables.Yellow);
        RotateTiles(orange, GameVariables.Orange);
        RotateTiles(blue, GameVariables.Blue);
    }
}
