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
}

public enum Direction {
    up, down, left, right, notset
}
