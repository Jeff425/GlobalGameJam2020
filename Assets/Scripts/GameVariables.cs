using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVariables {
    private static Direction black = Direction.notset, blue = Direction.notset, yellow = Direction.notset, orange = Direction.notset;

    public static Direction Black {
        get {
            return black;
        }
        set {
            black = value;
        }
    }

    public static Direction Blue {
        get {
            return blue;
        }
        set {
            blue = value;
        }
    }

    public static Direction Yellow {
        get {
            return yellow;
        }
        set {
            yellow = value;
        }
    }

    public static Direction Orange {
        get {
            return orange;
        }
        set {
            orange = value;
        }
    }

    public static void SetColor(Colors color, Direction direction) {
        switch (color) {
            case Colors.black:
                Black = direction;
                break;
            case Colors.blue:
                Blue = direction;
                break;
            case Colors.orange:
                Orange = direction;
                break;
            case Colors.yellow:
                Yellow = direction;
                break;
        }
    }

    public static Direction GetColor(Colors color) {
        switch (color) {
            case Colors.black:
                return Black;
            case Colors.blue:
                return Blue;
            case Colors.orange:
                return Orange;
            case Colors.yellow:
                return Yellow;
        }
        return Direction.notset;
    }
}

public enum Direction {
    up, down, left, right, notset
}

public enum Colors {
    black, yellow, orange, blue
}
