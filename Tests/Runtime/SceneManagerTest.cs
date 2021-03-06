using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SceneInPackage
{
    /// <summary>
    /// Package内のSceneを<c>SceneManager</c>でロードするテストのサンプル
    /// </summary>
    [TestFixture]
    public class SceneManagerTest
    {
        /// <summary>
        /// Package内の（Scene In Buildに含まれていない）Sceneをロードするテスト
        /// 
        /// <c>ITestPlayerBuildModifier</c>を使用して一時的にScene in buildに追加しているため、プレイヤー実行でのみ成功します
        /// </summary>
        /// <returns></returns>
        [UnityTest]
#if UNITY_EDITOR
        [Ignore("Run on player only")]
#endif
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
