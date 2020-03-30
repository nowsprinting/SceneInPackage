using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.TestTools;
using UnityEngine;

[assembly: TestPlayerBuildModifier(typeof(SceneInPackage.TestPlayerBuildModifier))]

namespace SceneInPackage
{
    /// <summary>
    /// Play Modeテストをプレイヤー実行する際、ビルド時に呼ばれる
    /// エディタ実行では呼ばれない（プレイヤービルドが行われないため）
    /// 
    /// Required: Unity Test Framework 1.1.13以降
    /// 1.1.11ではバグのため動作しない
    /// see: https://forum.unity.com/threads/testplayerbuildmodifier-not-working.844447/
    /// </summary>
    public class TestPlayerBuildModifier : ITestPlayerBuildModifier
    {
        private static IEnumerable<string> ScenesInTests()
        {
            var packagePath = new string[] {"Packages/com.nowsprinting.sceneinpackage"};
            return AssetDatabase.FindAssets("t:SceneAsset", packagePath)
                .Select(AssetDatabase.GUIDToAssetPath);
        }

        public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions)
        {
            foreach (var scenePath in ScenesInTests())
            {
                playerOptions.scenes = playerOptions.scenes.Append(scenePath).ToArray();
                Debug.Log($"Add scene in build: {scenePath}");
            }

            return playerOptions;
        }
    }
}
