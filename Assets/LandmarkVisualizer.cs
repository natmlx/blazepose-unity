/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Lightweight 2D pose visualizer.
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class LandmarkVisualizer : MonoBehaviour {

        #region --Inspector--
        /// <summary>
        /// </summary>
        public Image keypoint;
        #endregion


        #region --Client API--
        /// <summary>
        /// </summary>
        public Texture2D image {
            get => rawImage.texture as Texture2D;
            set {
                rawImage.texture = value;
                aspectFitter.aspectRatio = (float)value.width / value.height;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pose"></param>
        /// <param name="color"></param>
        public void Render (IEnumerable<Vector4> pose, Color color) {
            foreach (var point in pose)
                AddKeypoint(point, color);
        }

        /// <summary>
        /// </summary>
        public void Clear () {
            foreach (var obj in current)
                GameObject.Destroy(obj);
            current.Clear();
        }
        #endregion


        #region --Operations--
        private RawImage rawImage;
        private AspectRatioFitter aspectFitter;
        private readonly List<GameObject> current = new List<GameObject>();

        private void Awake () {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
        }

        public void AddKeypoint (Vector2 point, Color color) {
            // Instantiate
            var prefab = Instantiate(keypoint, transform);
            prefab.gameObject.SetActive(true);
            prefab.color = color;
            // Position
            var prefabTransform = prefab.transform as RectTransform;
            var imageTransform = rawImage.transform as RectTransform;
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