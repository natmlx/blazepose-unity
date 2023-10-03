/* 
*   BlazePose
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using VideoKit.UI;

    /// <summary>
    /// Lightweight 2D pose visualizer.
    /// </summary>
    [RequireComponent(typeof(VideoKitCameraView))]
    public sealed class LandmarkVisualizer : MonoBehaviour {

        #region --Inspector--
        /// <summary>
        /// Keypoint prefab.
        /// </summary>
        public Image keypoint;
        #endregion


        #region --Client API--
        /// <summary>
        /// Render a 2D pose.
        /// </summary>
        /// <param name="pose">Pose keypoints in normalized coordinates.</param>
        /// <param name="color">Pose keypoint color.</param>
        public void Render (params IEnumerable<Vector4>[] poses) {
            // Clear
            foreach (var point in current)
                GameObject.Destroy(point);
            current.Clear();
            // Render
            foreach (var pose in poses)
                foreach (var point in pose)
                    AddKeypoint(point, Color.green);
        }
        #endregion


        #region --Operations--
        private readonly List<GameObject> current = new List<GameObject>();

        private void AddKeypoint (Vector2 point, Color color) {
            // Instantiate
            var prefab = Instantiate(keypoint, transform);
            prefab.gameObject.SetActive(true);
            prefab.color = color;
            // Position
            var prefabTransform = prefab.transform as RectTransform;
            var imageTransform = transform as RectTransform;
            prefabTransform.anchorMin = 0.5f * Vector2.one;
            prefabTransform.anchorMax = 0.5f * Vector2.one;
            prefabTransform.pivot = 0.5f * Vector2.one;
            prefabTransform.anchoredPosition = Rect.NormalizedToPoint(imageTransform.rect, point);
            // Add
            current.Add(prefab.gameObject);
        }
        #endregion
    }
}