using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    bool rotate;

    [SerializeField]
    Colors[] affectedColors;

    public void Trigger() {
        if (!rotate) {
            // Trade
            Direction colorA = GameVariables.GetColor(affectedColors[0]);
            Direction colorB = GameVariables.GetColor(affectedColors[1]);
            GameVariables.SetColor(affectedColors[0], colorB);
            GameVariables.SetColor(affectedColors[1], colorA);
        }
        foreach(Colors color in affectedColors) {
            GameVariables.SetColor(color, Rotate90(GameVariables.GetColor(color)));
            GameController.Instance.RotateTilesOfColor(color);
        }
    }

    Direction Rotate90(Direction inDirection) {
        switch (inDirection) {
            case Direction.up:
                return Direction.right;
            case Direction.right:
                return Direction.down;
            case Direction.down:
                return Direction.left;
            case Direction.left:
                return Direction.up;
        }
        return Direction.notset;
    }

    Direction Rotate180(Direction inDirection) {
        switch (inDirection) {
            case Direction.up:
                return Direction.down;
            case Direction.down:
                return Direction.up;
            case Direction.left:
                return Direction.right;
            case Direction.right:
                return Direction.left;
        }
        return Direction.notset;
    }
}


