using UnityEngine;
using System.Collections;

public static class glob_const {
#if UNITY_EDITOR
    public const bool DEBUG = true;
    public const bool LOG = true;
#else
    public const bool DEBUG = false;
    public const bool LOG = false;
#endif
}
