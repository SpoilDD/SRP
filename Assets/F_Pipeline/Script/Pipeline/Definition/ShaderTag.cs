using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class ShaderTag
{

    #region ShaderTagIds


    public static readonly ShaderTagId NONE = ShaderTagId.none;
    public static readonly ShaderTagId FORWARD = new ShaderTagId("FForwardPass");

    #endregion

    #region Lighting
    
    public static readonly ShaderTagId MAIN_LIGHT = new ShaderTagId("_MainLight");
    
    #endregion
}
