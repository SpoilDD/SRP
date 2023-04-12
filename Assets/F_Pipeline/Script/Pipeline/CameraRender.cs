using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

abstract public class CameraRender : IDisposable
{
    virtual public void Dispose() { }
    abstract public void Render(ScriptableRenderContext context);


}
