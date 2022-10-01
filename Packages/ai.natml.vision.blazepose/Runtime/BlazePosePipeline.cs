/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System;
    using System.Collections.Generic;
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
        /// Create the BlazePose pipeline.
        /// </summary>
        /// <param name="detector">BlazePose detector model data.</param>
        /// <param name="predictor">BlazePose landmark predictor model data.</param>
        /// <param name="maxDetections">Maximum number of detections to return.</param>
        public BlazePosePipeline (MLModelData detector, MLModelData predictor, int maxDetections = Int32.MaxValue) {
            this.detectorData = detector;
            this.predictorData = predictor;
            this.detectorModel = detector.Deserialize() as MLEdgeModel;
            this.predictorModel = predictor.Deserialize() as MLEdgeModel;
            this.detector = new BlazePoseDetector(detectorModel);
            this.predictor = new BlazePosePredictor(predictorModel);
            this.maxDetections = maxDetections;
        }

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
            (input.mean, input.std) = detectorData.normalization;
            input.aspectMode = detectorData.aspectMode;
            var detectedPoses = detector.Predict(input);
            // Predict landmarks
            var capacity = Mathf.Min(detectedPoses.Length, maxDetections);
            var result = new List<BlazePosePredictor.Pose>(capacity);
            for (var i = 0; i < capacity; ++i) {
                // Extract ROI
                var detection = detectedPoses[i];
                var roi = input.RegionOfInterest(detection.regionOfInterest, -detection.rotation, Color.black);
                // Predict landmarks
                (roi.mean, roi.std) = predictorData.normalization;
                roi.aspectMode = predictorData.aspectMode;
                var landmarks = predictor.Predict(roi);
                // Create pose
                var inputType = predictorModel.inputs[0] as MLImageType;
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
            detectorModel.Dispose();
            predictorModel.Dispose();
        }
        #endregion


        #region --Operations--
        private readonly MLModelData detectorData;
        private readonly MLModelData predictorData;
        private readonly MLEdgeModel detectorModel;
        private readonly MLEdgeModel predictorModel;
        private readonly BlazePoseDetector detector;
        private readonly BlazePosePredictor predictor;
        private readonly int maxDetections;
        #endregion
    }
}