/* 
*   BlazePose
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using NatML.Vision;
    using Visualizers;
    using VideoKit;

    public sealed class BlazePoseSample : MonoBehaviour {

        [Header(@"VideoKit")]
        public VideoKitCameraManager cameraManager;

        [Header(@"Visualizers")]
        public LandmarkVisualizer visualizer;
        public Landmark3DVisualizer visualizer3D;

        private BlazePosePipeline pipeline;

        private async void Start () {
            // Create the BlazePose pipeline
            pipeline = await BlazePosePipeline.Create(maxDetections: 1);
            // Listen for camera frames
            cameraManager.OnCameraFrame.AddListener(OnCameraFrame);
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
            cameraManager.OnCameraFrame.RemoveListener(OnCameraFrame);
            // Dispose the pipeline
            pipeline?.Dispose();
        }
    }
}