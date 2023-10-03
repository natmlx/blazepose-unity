/* 
*   BlazePose
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using NatML.Features;
    using NatML.Internal;
    using NatML.Types;

    /// <summary>
    /// BlazePose pipeline for joint person detection and pose prediction.
    /// This pipeline combines the `BlazePoseDetector` and `BlazePosePredictor` to detect one or more
    /// poses in a given image.
    /// </summary>
    public sealed partial class BlazePosePipeline : IMLPredictor<BlazePosePredictor.Pose[]> {

        #region --Client API--
        /// <summary>
        /// Detect poses in an image.
        /// </summary>
        /// <param name="inputs">Input image. This MUST be an `MLImageFeature`.</param>
        /// <returns>Detected poses.</returns>
        public BlazePosePredictor.Pose[] Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"BlazePose pipeline expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0] as MLImageFeature;
            if (input == null)
                throw new ArgumentException(@"BlazePose pipeline expects an image feature", nameof(inputs));
            // Detect poses
            var detectedPoses = detector.Predict(input);
            // Predict landmarks
            var capacity = Mathf.Min(detectedPoses.Length, maxDetections);
            var result = new List<BlazePosePredictor.Pose>(capacity);
            for (var i = 0; i < capacity; ++i) {
                // Extract ROI
                var detection = detectedPoses[i];
                var detectionRect = detection.regionOfInterest;
                var roi = new MLImageFeature(detectionRect.width, detectionRect.height);
                input.CopyTo(roi, detectionRect, -detection.rotation, Color.black);
                // Predict landmarks
                var landmarks = predictor.Predict(roi);
                // Create pose
                var inputType = predictor.model.inputs[0] as MLImageType;
                var keypoints = new BlazePosePredictor.Keypoints(landmarks.keypoints.data, inputType, detection.regionOfInterestToImageMatrix);
                var pose = new BlazePosePredictor.Pose(landmarks.score, keypoints, landmarks.keypoints3D);
                result.Add(pose);
            }
            // Return
            return result.ToArray();
        }

        /// <summary>
        /// Dispose the pipeline and release resources.
        /// </summary>
        public void Dispose () {
            detector.Dispose();
            predictor.Dispose();
        }

        /// <summary>
        /// Create the BlazePose pipeline.
        /// </summary>
        /// <param name="maxDetections">Maximum number of detections to return.</param>
        /// <param name="detector">BlazePose detector model data.</param>
        /// <param name="predictor">BlazePose landmark predictor model data.</param>
        /// <returns>BlazePose pipeline.</returns>
        public static async Task<BlazePosePipeline> Create (
            int maxDetections = Int32.MaxValue,
            BlazePoseDetector detector = null,
            BlazePosePredictor predictor = null
        ) {
            detector ??= await BlazePoseDetector.Create();
            predictor ??= await BlazePosePredictor.Create();
            var pipeline = new BlazePosePipeline(detector, predictor, maxDetections);
            return pipeline;
        }
        #endregion


        #region --Operations--
        private readonly BlazePoseDetector detector;
        private readonly BlazePosePredictor predictor;
        private readonly int maxDetections;

        private BlazePosePipeline (BlazePoseDetector detector, BlazePosePredictor predictor, int maxDetections) {
            this.detector = detector;
            this.predictor = predictor;
            this.maxDetections = maxDetections;
        }
        #endregion
    }
}