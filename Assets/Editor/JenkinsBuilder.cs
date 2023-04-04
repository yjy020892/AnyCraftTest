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
        /// <summary>
        /// 젠킨스 CommandLine으로 Argument 넘겨줄수 있게
        /// </summary>
        public static void BuildJenkins()
        {
            string pathName = GetArgument("-pathName"); // 예시로 그냥 넣음

            JenkinsBuild_AOS(pathName);
        }

        private static void JenkinsBuild_AOS(string pathName = "")
        {
            string[] SCENES = FindEnabledEditorScenes();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = SCENES;

            if (string.IsNullOrWhiteSpace(pathName))
            {
                buildPlayerOptions.locationPathName = $"Build/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}_{DateTime.Now.ToString("yyyyMMdd_hhmm")}.apk";
            }
            else
            {
                buildPlayerOptions.locationPathName = $"Build/{pathName}_{PlayerSettings.bundleVersion}_{DateTime.Now.ToString("yyyyMMdd_hhmm")}.apk";
            }
            
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

        private static string GetArgument(string value)
        {
            var arguments = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == value && arguments.Length > i + 1)
                    return arguments[i + 1];
            }

            return null;
        }
    }
}