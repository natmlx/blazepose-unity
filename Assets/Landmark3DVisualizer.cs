/* 
*   BlazePose
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples.Visualizers {

    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using NatML.Vision;

    /// <summary>
    /// Lightweight 3D body pose visualizer.
    /// </summary>
    public sealed class Landmark3DVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// Render a body pose.
        /// </summary>
        /// <param name="pose"></param>
        public void Render (BlazePosePredictor.Keypoints3D pose) {
            // Delete current
            foreach (var point in currentSkeleton)
                GameObject.Destroy(point.gameObject);
            currentSkeleton.Clear();
            // Instantiate keypoints
            foreach (var position in new [] {
                pose.nose,
                pose.leftShoulder, pose.leftElbow, pose.leftWrist,
                pose.rightShoulder, pose.rightElbow, pose.rightWrist,
                pose.leftHip, pose.leftKnee, pose.leftAnkle,
                pose.rightHip, pose.rightKnee, pose.rightAnkle
            }) {
                var point = Instantiate(keypointPrefab, position, Quaternion.identity, transform);
                point.gameObject.SetActive(true);
                currentSkeleton.Add(point);
            }
            // Instantiate bones
            foreach (var positions in new [] {
                new [] { pose.leftShoulder, pose.rightShoulder },
                new [] { pose.leftShoulder, pose.leftElbow, pose.leftWrist },
                new [] { pose.rightShoulder, pose.rightElbow, pose.rightWrist },
                new [] { pose.leftShoulder, pose.leftHip },
                new [] { pose.rightShoulder, pose.rightHip },
                new [] { pose.leftHip, pose.rightHip },
                new [] { pose.leftHip, pose.leftKnee, pose.leftAnkle },
                new [] { pose.rightHip, pose.rightKnee, pose.rightAnkle }
            }) {
                var bone = Instantiate(bonePrefab, transform.position, Quaternion.identity, transform);
                bone.gameObject.SetActive(true);
                bone.positionCount = positions.Length;
                bone.SetPositions(positions);
                currentSkeleton.Add(bone.transform);
            };
        }
        #endregion


        #region --Operations--
        [SerializeField] Transform keypointPrefab;
        [SerializeField] LineRenderer bonePrefab;
        readonly List<Transform> currentSkeleton = new List<Transform>();
        #endregion
    }
}