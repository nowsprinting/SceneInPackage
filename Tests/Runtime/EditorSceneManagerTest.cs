#if UNITY_EDITOR
using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SceneInPackage
{
    /// <summary>
    /// Package内のSceneを<c>SceneManager</c>でロードするテストのサンプル
    /// </summary>
    [TestFixture]
    public class EditorSceneManagerTest
    {
        /// <summary>
        /// Package内の（Scene In Buildに含まれていない）Sceneをロードするテスト
        /// 
        /// UnityEditor.SceneManagement.EditorSceneManagerを使用しているため、エディタでのみ実行できます
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator LoadSceneAsync_sceneNotInBuild_success()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(
                "Packages/com.nowsprinting.sceneinpackage/Tests/Scenes/SceneInPackage.unity", // Projectウィンドウ上のパス
                new LoadSceneParameters(LoadSceneMode.Single));
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(
                "Packages/com.nowsprinting.sceneinpackage/Tests/Scenes/SceneInPackage2.unity", // Projectウィンドウ上のパス
                new LoadSceneParameters(LoadSceneMode.Additive));

            var cube = GameObject.Find("Cube");
            Assert.That(cube, Is.Not.Null);

            var capsule = GameObject.Find("Capsule");
            Assert.That(capsule, Is.Not.Null);
        }
    }
}
#endif
