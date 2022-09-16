/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using NatML.Types;

    public sealed partial class BlazePosePredictor {

        /// <summary>
        /// Normalized pose keypoints.
        /// The `xy` coordinates of each keypoint correspond to the  normalized position of the 
        /// keypoint in the region of interest. The `z` coordinate corresponds to depth.
        /// The `w` coordinate is the keypoint's normalized visibility score.
        /// </summary>
        public readonly struct Keypoints : IReadOnlyList<Vector4> {

            #region --Client API--
            /// <summary>
            /// Number of points in the keypoints.
            /// </summary>
            public readonly int Count => 33;

            /// <summary>
            /// Nose position.
            /// </summary>
            public readonly Vector4 nose => this[0];

            /// <summary>
            /// Inner left eye position.
            /// </summary>
            public readonly Vector4 leftEyeInner => this[1];
            
            /// <summary>
            /// Left eye position.
            /// </summary>
            public readonly Vector4 leftEye => this[2];
            
            /// <summary>
            /// Outer left eye position.
            /// </summary>
            public readonly Vector4 leftEyeOuter => this[3];
            
            /// <summary>
            /// Inner right eye position.
            /// </summary>
            public readonly Vector4 rightEyeInner => this[4];
            
            /// <summary>
            /// Right eye position.
            /// </summary>
            public readonly Vector4 rightEye => this[5];
            
            /// <summary>
            /// Outer right eye position.
            /// </summary>
            public readonly Vector4 rightEyeOuter => this[6];
            
            /// <summary>
            /// Left ear position.
            /// </summary>
            public readonly Vector4 leftEar => this[7];

            /// <summary>
            /// Right ear position.
            /// </summary>
            public readonly Vector4 rightEar => this[8];

            /// <summary>
            /// Mouth left edge position.
            /// </summary>
            public readonly Vector4 mouthLeft => this[9];

            /// <summary>
            /// Mouth right edge position.
            /// </summary>
            public readonly Vector4 mouthRight => this[10];

            /// <summary>
            /// Left shoulder position.
            /// </summary>
            public readonly Vector4 leftShoulder => this[11];

            /// <summary>
            /// Right shoulder position.
            /// </summary>
            public readonly Vector4 rightShoulder => this[12];

            /// <summary>
            /// Left elbow position.
            /// </summary>
            public readonly Vector4 leftElbow => this[13];

            /// <summary>
            /// Right elbow position.
            /// </summary>
            public readonly Vector4 rightElbow => this[14];

            /// <summary>
            /// Left wrist position.
            /// </summary>
            public readonly Vector4 leftWrist => this[15];

            /// <summary>
            /// Right wrist position.
            /// </summary>
            public readonly Vector4 rightWrist => this[16];

            /// <summary>
            /// Left pinky finger position.
            /// </summary>
            public readonly Vector4 leftPinky => this[17];

            /// <summary>
            /// Right pinky finger position.
            /// </summary>
            public readonly Vector4 rightPinky => this[18];

            /// <summary>
            /// Left index finger position.
            /// </summary>
            public readonly Vector4 leftIndex => this[19];

            /// <summary>
            /// Right index finger position.
            /// </summary>
            public readonly Vector4 rightIndex => this[20];

            /// <summary>
            /// Left thumb finger position.
            /// </summary>
            public readonly Vector4 leftThumb => this[21];

            /// <summary>
            /// Right thumb finger position.
            /// </summary>
            public readonly Vector4 rightThumb => this[22];

            /// <summary>
            /// Left hip position.
            /// </summary>
            public readonly Vector4 leftHip => this[23];
            
            /// <summary>
            /// Right hip position.
            /// </summary>
            public readonly Vector4 rightHip => this[24];

            /// <summary>
            /// Left knee position.
            /// </summary>
            public readonly Vector4 leftKnee => this[25];

            /// <summary>
            /// Right knee position.
            /// </summary>
            public readonly Vector4 rightKnee => this[26];

            /// <summary>
            /// Left ankle position.
            /// </summary>
            public readonly Vector4 leftAnkle => this[27];

            /// <summary>
            /// Right ankle position.
            /// </summary>
            public readonly Vector4 rightAnkle => this[28];

            /// <summary>
            /// Left heel position.
            /// </summary>
            public readonly Vector4 leftHeel => this[29];

            /// <summary>
            /// Right heel position.
            /// </summary>
            public readonly Vector4 rightHeel => this[30];

            /// <summary>
            /// Left index toe position.
            /// </summary>
            public readonly Vector4 leftFootIndex => this[31];

            /// <summary>
            /// Right index toe position.
            /// </summary>
            public readonly Vector4 rightFootIndex => this[32];

            /// <summary>
            /// Get the keypoints for a given index.
            /// </summary>
            public readonly Vector4 this [int idx] {
                get {
                    var rawKeypoint = new Vector3(data[5 * idx], data[5 * idx + 1], 1f);
                    var keypoint = Vector3.Scale(rawKeypoint, scale);
                    keypoint.y = 1f - keypoint.y;
                    var depth = data[5 * idx + 2];
                    var visibility = 1f / (1f + Mathf.Exp(-data[5 * idx + 3]));
                    var transformedKeypoint = transformation.MultiplyPoint3x4(keypoint);
                    var transformedZero = transformation.MultiplyPoint3x4(Vector3.zero);
                    var transformedRight = transformation.MultiplyPoint3x4(Vector3.right);
                    var zScale = (transformedRight - transformedZero).magnitude;
                    var result = new Vector4(transformedKeypoint.x, transformedKeypoint.y, zScale * depth, visibility);
                    return result;
                }
            }
            #endregion


            #region --Operations--
            internal readonly float[] data;
            private readonly Vector3 scale;
            private readonly Matrix4x4 transformation;

            internal Keypoints (float[] data, MLImageType imageType, Matrix4x4 transformation) {
                this.data = data;
                this.scale = new Vector3(1f / imageType.width, 1f / imageType.height, 1f);
                this.transformation = transformation;
            }

            readonly IEnumerator<Vector4> IEnumerable<Vector4>.GetEnumerator () {
                for (var i = 0; i < Count; ++i)
                    yield return this[i];
            }

            readonly IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<Vector4>).GetEnumerator();
            #endregion
        }
    }
}