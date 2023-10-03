/* 
*   BlazePose
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using NatML.Features;
    using NatML.Internal;
    using NatML.Types;

    /// <summary>
    /// BlazePose predictor.
    /// Given an aligned region-of-interest with an upright human subject, this predictor predicts 
    /// the image-space and world-space pose of the subject, comprising of joint positions.
    /// </summary>
    public sealed partial class BlazePosePredictor : IMLPredictor<BlazePosePredictor.Pose> {

        #region --Client API--
        /// <summary>
        /// BlazePose landmarks model.
        /// </summary>
        public readonly MLEdgeModel model;

        /// <summary>
        /// Predictor tag.
        /// </summary>
        public const string Tag = "@natml/blazepose-landmark";        

        /// <summary>
        /// Detect poses in an image.
        /// </summary>
        /// <param name="inputs">Input image.</param>
        /// <returns>Detected poses.</returns>
        public Pose Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"BlazePose predictor expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0];
            if (!MLImageType.FromType(input.type))
                throw new ArgumentException(@"BlazePose predictor expects an an array or image feature", nameof(inputs));
            // Predict
            var inputType = model.inputs[0] as MLImageType;
            using var inputFeature = (input as IMLEdgeFeature).Create(inputType);
            using var outputFeatures = model.Predict(inputFeature);
            // Create pose
            var score = new MLArrayFeature<float>(outputFeatures[2])[0];
            var keypointData = new MLArrayFeature<float>(outputFeatures[0]).ToArray();
            var keypoint3DData = new MLArrayFeature<float>(outputFeatures[4]).ToArray();
            var keypoints = new Keypoints(keypointData, inputType, Matrix4x4.identity);
            var keypoints3d = new Keypoints3D(keypoint3DData);
            var pose = new Pose(score, keypoints, keypoints3d);
            return pose;
        }

        /// <summary>
        /// Dispose the predictor and release resources.
        /// </summary>
        public void Dispose () => model.Dispose();

        /// <summary>
        /// Create the BlazePose landmark predictor.
        /// </summary>
        /// <param name="configuration">Edge model configuration.</param>
        /// <param name="accessKey">NatML access key.</param>
        /// <returns>Created predictor.</returns>
        public static async Task<BlazePosePredictor> Create (
            MLEdgeModel.Configuration configuration = null,
            string accessKey = null
        ) {
            var model = await MLEdgeModel.Create(Tag, configuration, accessKey);
            var predictor = new BlazePosePredictor(model);
            return predictor;
        }
        #endregion


        #region --Operations--

        private BlazePosePredictor (MLModel model) => this.model = model as MLEdgeModel;
        #endregion
    }
}