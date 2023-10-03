/* 
*   BlazePose
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed partial class BlazePosePredictor {

        /// <summary>
        /// World-space pose keypoints.
        /// The keypoints are always centered around the mid-hip position.
        /// </summary>
        public readonly struct Keypoints3D : IReadOnlyList<Vector3> {

            #region --Client API--
            /// <summary>
            /// Number of points in the keypoints.
            /// </summary>
            public readonly int Count => 33;

            /// <summary>
            /// Nose position.
            /// </summary>
            public readonly Vector3 nose => this[0];

            /// <summary>
            /// Inner left eye position.
            /// </summary>
            public readonly Vector3 leftEyeInner => this[1];
            
            /// <summary>
            /// Left eye position.
            /// </summary>
            public readonly Vector3 leftEye => this[2];
            
            /// <summary>
            /// Outer left eye position.
            /// </summary>
            public readonly Vector3 leftEyeOuter => this[3];
            
            /// <summary>
            /// Inner right eye position.
            /// </summary>
            public readonly Vector3 rightEyeInner => this[4];
            
            /// <summary>
            /// Right eye position.
            /// </summary>
            public readonly Vector3 rightEye => this[5];
            
            /// <summary>
            /// Outer right eye position.
            /// </summary>
            public readonly Vector3 rightEyeOuter => this[6];
            
            /// <summary>
            /// Left ear position.
            /// </summary>
            public readonly Vector3 leftEar => this[7];

            /// <summary>
            /// Right ear position.
            /// </summary>
            public readonly Vector3 rightEar => this[8];

            /// <summary>
            /// Mouth left edge position.
            /// </summary>
            public readonly Vector3 mouthLeft => this[9];

            /// <summary>
            /// Mouth right edge position.
            /// </summary>
            public readonly Vector3 mouthRight => this[10];

            /// <summary>
            /// Left shoulder position.
            /// </summary>
            public readonly Vector3 leftShoulder => this[11];

            /// <summary>
            /// Right shoulder position.
            /// </summary>
            public readonly Vector3 rightShoulder => this[12];

            /// <summary>
            /// Left elbow position.
            /// </summary>
            public readonly Vector3 leftElbow => this[13];

            /// <summary>
            /// Right elbow position.
            /// </summary>
            public readonly Vector3 rightElbow => this[14];

            /// <summary>
            /// Left wrist position.
            /// </summary>
            public readonly Vector3 leftWrist => this[15];

            /// <summary>
            /// Right wrist position.
            /// </summary>
            public readonly Vector3 rightWrist => this[16];

            /// <summary>
            /// Left pinky finger position.
            /// </summary>
            public readonly Vector3 leftPinky => this[17];

            /// <summary>
            /// Right pinky finger position.
            /// </summary>
            public readonly Vector3 rightPinky => this[18];

            /// <summary>
            /// Left index finger position.
            /// </summary>
            public readonly Vector3 leftIndex => this[19];

            /// <summary>
            /// Right index finger position.
            /// </summary>
            public readonly Vector3 rightIndex => this[20];

            /// <summary>
            /// Left thumb finger position.
            /// </summary>
            public readonly Vector3 leftThumb => this[21];

            /// <summary>
            /// Right thumb finger position.
            /// </summary>
            public readonly Vector3 rightThumb => this[22];

            /// <summary>
            /// Left hip position.
            /// </summary>
            public readonly Vector3 leftHip => this[23];
            
            /// <summary>
            /// Right hip position.
            /// </summary>
            public readonly Vector3 rightHip => this[24];

            /// <summary>
            /// Left knee position.
            /// </summary>
            public readonly Vector3 leftKnee => this[25];

            /// <summary>
            /// Right knee position.
            /// </summary>
            public readonly Vector3 rightKnee => this[26];

            /// <summary>
            /// Left ankle position.
            /// </summary>
            public readonly Vector3 leftAnkle => this[27];

            /// <summary>
            /// Right ankle position.
            /// </summary>
            public readonly Vector3 rightAnkle => this[28];

            /// <summary>
            /// Left heel position.
            /// </summary>
            public readonly Vector3 leftHeel => this[29];

            /// <summary>
            /// Right heel position.
            /// </summary>
            public readonly Vector3 rightHeel => this[30];

            /// <summary>
            /// Left index toe position.
            /// </summary>
            public readonly Vector3 leftFootIndex => this[31];

            /// <summary>
            /// Right index toe position.
            /// </summary>
            public readonly Vector3 rightFootIndex => this[32];

            /// <summary>
            /// Get the keypoints for a given index.
            /// </summary>
            public readonly Vector3 this [int idx] => new Vector3(data[3 * idx], -data[3 * idx + 1], data[3 * idx + 2]);
            #endregion


            #region --Operations--
            private readonly float[] data;

            internal Keypoints3D (float[] data) => this.data = data;

            readonly IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator () {
                for (var i = 0; i < Count; ++i)
                    yield return this[i];
            }

            readonly IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<Vector3>).GetEnumerator();
            #endregion
        }
    }
}