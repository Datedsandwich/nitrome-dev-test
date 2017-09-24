using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Emulate the Optional object from Java 8
public class Optional {
    public static string ToStringOrElseNull(float num) {
        return num == 0 ? null : MathRound(num, 2).ToString();
    }

    public static string ToStringOrElseOne(float num) {
        return num == 1 ? null : MathRound(num, 2).ToString();
    }

    public static float ToFloatOrElseNull(string value) {
        return value == null ? 0 : float.Parse(value);
    }
    public static float ToFloatOrElseOne(string value) {
        return value == null ? 1 : float.Parse(value);
    }

    private static float MathRound(float round, int decimals) {
        return Mathf.Round(round * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);
    }
}
