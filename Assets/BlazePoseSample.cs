/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using NatML.Devices;
    using NatML.Devices.Outputs;
    using NatML.VideoKit;
    using NatML.Vision;
    using Visualizers;

    public sealed class BlazePoseSample : MonoBehaviour {

        [Header(@"VideoKit")]
        public VideoKitCameraManager cameraManager;

        [Header(@"Visualizers")]
        public LandmarkVisualizer visualizer;
        public Landmark3DVisualizer visualizer3D;

        private BlazePosePipeline pipeline;

        private async void Start () {
            // Create the BlazePose pipeline
            var detectorModelData = await MLModelData.FromHub("@natml/blazepose-detector");
            var predictorModelData = await MLModelData.FromHub("@natml/blazepose-landmark");
            pipeline = new BlazePosePipeline(detectorModelData, predictorModelData, maxDetections: 1);
            // Listen for camera frames
            cameraManager.OnFrame.AddListener(OnCameraFrame);
        }

        private void OnCameraFrame (CameraFrame frame) {
            // Predict
            var poses = pipeline.Predict(frame);
            // Check
            if (poses.Length == 0)
                return;
            // Visualize
            var pose = poses[0];
            visualizer.Render(pose.keypoints);
            visualizer3D.Render(pose.keypoints3D);
        }

        private void OnDisable () {
            // Stop listening for camera frames
            cameraManager.OnFrame.RemoveListener(OnCameraFrame);
            // Dispose the pipeline
            pipeline?.Dispose();
        }
    }
}