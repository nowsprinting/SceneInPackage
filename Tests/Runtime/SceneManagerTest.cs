using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SceneInPackage
{
    /// <summary>
    /// Package内のSceneを<c>SceneManager</c>でロードするテストのサンプル
    /// エディタ実行とプレイヤー実行で、Sceneのビルド方法が異なります
    /// </summary>
    [TestFixture]
    public class SceneManagerTest : IPrebuildSetup
    {
#if UNITY_EDITOR
        private const string AssetBundlePath = "Temp/AssetBundleForTests";
        private const string AssetBundleName = "sceneinpackage";
        private AssetBundle _loadedAssetBundle;

        private static IEnumerable<string> ScenesInTests()
        {
            var packagePath = new string[] {"Packages/com.nowsprinting.sceneinpackage"};
            return AssetDatabase.FindAssets("t:SceneAsset", packagePath)
                .Select(AssetDatabase.GUIDToAssetPath);
        }

        /// <summary>
        /// Play Modeテストをエディタ実行する際、必要なSceneをAssetBundle化する
        /// エディタ実行のときのみ使用
        /// プレイヤー実行では、<c>ITestPlayerBuildModifier</c>を使用して一時的にScene in buildに追加するためAssetBundleは使用しない
        /// </summary>
        public void Setup()
        {
            if (!Directory.Exists(AssetBundlePath))
            {
                Directory.CreateDirectory(AssetBundlePath);
            }

            var builds = new AssetBundleBuild[1];
            builds[0].assetBundleName = AssetBundleName;
            builds[0].assetNames = ScenesInTests().ToArray();

            const BuildAssetBundleOptions options = BuildAssetBundleOptions.ForceRebuildAssetBundle;
            var target = EditorUserBuildSettings.activeBuildTarget;

            BuildPipeline.BuildAssetBundles(AssetBundlePath, builds, options, target);
        }

        /// <summary>
        /// Play Modeテストをエディタ実行する際に必要なSceneをAssetBundleからロードする
        /// エディタ実行のときのみ使用
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            var assetBundlePath = Path.Combine(AssetBundlePath, AssetBundleName);
            _loadedAssetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (_loadedAssetBundle == null)
            {
                Assert.Fail("Failed to load AssetBundle");
            }
        }

        /// <summary>
        /// 使用したAssetBundleをアンロードする
        /// エディタ実行のときのみ使用
        /// </summary>
        [OneTimeTearDown]
        public void Cleanup()
        {
            _loadedAssetBundle.Unload(true);
        }
#else
        public void Setup()
        {
            // プレイヤービルドではなにもしない
        }
#endif

        /// <summary>
        /// Package内の（Scene In Buildに含まれていない）Sceneをロードするテスト
        /// エディタ実行では、<c>BuildAssetBundleForTests</c>でビルドしたAssetBundleを<c>LoadAssetBundleForTests</c>でロードしている
        /// プレイヤー実行では、<c>ITestPlayerBuildModifier</c>を使用して一時的にScene in buildに追加している
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator LoadSceneAsync_sceneNotInBuild_success()
        {
            yield return SceneManager.LoadSceneAsync("SceneInPackage");
            yield return SceneManager.LoadSceneAsync("SceneInPackage2", LoadSceneMode.Additive);

            var cube = GameObject.Find("Cube");
            Assert.That(cube, Is.Not.Null);

            var capsule = GameObject.Find("Capsule");
            Assert.That(capsule, Is.Not.Null);
        }
    }
}
