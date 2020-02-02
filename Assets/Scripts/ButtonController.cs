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
        foreach(Colors color in affectedColors) {
            switch(color) {
                case Colors.black:
                    GameVariables.Black = rotate ? Rotate90(GameVariables.Black) : Rotate180(GameVariables.Black);
                    break;
                case Colors.yellow:
                    GameVariables.Yellow = rotate ? Rotate90(GameVariables.Yellow) : Rotate180(GameVariables.Yellow);
                    break;
                case Colors.blue:
                    GameVariables.Blue = rotate ? Rotate90(GameVariables.Blue) : Rotate180(GameVariables.Blue);
                    break;
                case Colors.orange:
                    GameVariables.Orange = rotate ? Rotate90(GameVariables.Orange) : Rotate180(GameVariables.Orange);
                    break;
                    
            }
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


