/* 
*   BlazePose
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    public sealed partial class BlazePosePredictor {

        /// <summary>
        /// Detected human pose.
        /// </summary>
        public readonly struct Pose {

            #region --Client API--
            /// <summary>
            /// Pose detection score.
            /// </summary>
            public readonly float score;

            /// <summary>
            /// Normalized image keypoints.
            /// </summary>
            public readonly Keypoints keypoints;

            /// <summary>
            /// Estimated world-space 3D keypoints.
            /// </summary>
            public readonly Keypoints3D keypoints3D;
            #endregion


            #region --Operations--

            internal Pose (float score, Keypoints keypoints, Keypoints3D keypoints3D) {
                this.score = score;
                this.keypoints = keypoints;
                this.keypoints3D = keypoints3D;
            }
            #endregion
        }
    }
}