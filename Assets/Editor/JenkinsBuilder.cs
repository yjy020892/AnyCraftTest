using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Build.Reporting;

namespace JY.Jenkins
{
    /// <summary>
    /// 자동 빌드를 위한 스크립트 빌드
    /// </summary>
    public class JenkinsBuilder : MonoBehaviour
    {
        [MenuItem("Jenkins/Build AOS")]
        public static void JenkinsBuild_AOS()
        {
            string[] SCENES = FindEnabledEditorScenes();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = SCENES;
            buildPlayerOptions.locationPathName = $"Build/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}_{DateTime.Now.ToString("yyyyMMdd_hhmm")}.apk";
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if(summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build Succeeded : {summary.totalSize} bytes");
            }
            else if(summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build Failed");
            }
        }

        private static string[] FindEnabledEditorScenes()
        {
            List<string> editorScenes = new List<string>();

            foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)   
            {
                if (!scene.enabled)
                    continue;

                editorScenes.Add(scene.path);
            }

            return editorScenes.ToArray();
        }
    }
}