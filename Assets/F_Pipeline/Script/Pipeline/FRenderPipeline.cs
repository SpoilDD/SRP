using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class FRenderPipeline : RenderPipeline
{
    FPipelineAssetSetting m_FPipelineAssetSetting;

    Dictionary<Camera, GameCameraRender> m_CameraRenderDic = new Dictionary<Camera, GameCameraRender>();

    public FRenderPipeline(FPipelineAssetSetting setting)
    {
        m_FPipelineAssetSetting = setting;
        GraphicsSettings.useScriptableRenderPipelineBatching = setting.SRPBatchingSwitch;
    }

    CameraRender CreateCameraRender(Camera camera)
    {
        return new GameCameraRender(camera);
    }

    CameraRender GetCameraRenderCls(Camera camera)
    {
        if (m_CameraRenderDic.ContainsKey(camera))
        {
            return m_CameraRenderDic[camera];
        }
        m_CameraRenderDic[camera] = CreateCameraRender(camera) as GameCameraRender;
        return m_CameraRenderDic[camera];
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        BeginFrameRendering(context, cameras);
        for (int i = 0; i < cameras.Length; i++)
        {
            Camera camera = cameras[i];

            CameraRender cameraRender = GetCameraRenderCls(camera);

            BeginCameraRendering(context, camera);

            cameraRender.Render(context);

            EndCameraRendering(context, camera);
        }

        EndFrameRendering(context, cameras);
        context.Submit();
    }

}
