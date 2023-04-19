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
    
    public static readonly int MAIN_LIGHT = Shader.PropertyToID("MainLight");
    
    #endregion
    
    
    #region Lighting

    public static readonly int DIRECTIONAL_LIGHT_SHADOW_MAP = Shader.PropertyToID("DirectionalLightShadowMap");

    #endregion
}
