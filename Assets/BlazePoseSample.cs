/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using System.Threading.Tasks;
    using UnityEngine;
    using NatML.Devices;
    using NatML.Devices.Outputs;
    using NatML.Features;
    using NatML.Vision;
    using Visualizers;

    public sealed class BlazePoseSample : MonoBehaviour {

        [Header(@"Visualizers")]
        public LandmarkVisualizer visualizer;
        public Landmark3DVisualizer visualizer3D;

        CameraDevice cameraDevice;
        TextureOutput previewTextureOutput;
        BlazePosePipeline posePipeline;

        async void Start () {
            // Request camera permissions
            var permissionStatus = await MediaDeviceQuery.RequestPermissions<CameraDevice>();
            if (permissionStatus != PermissionStatus.Authorized) {
                Debug.Log(@"User did not grant camera permissions");
                return;
            }
            // Get a camera device
            var query = new MediaDeviceQuery(MediaDeviceCriteria.CameraDevice);
            cameraDevice = query.current as CameraDevice;
            // Start the camera preview
            previewTextureOutput = new TextureOutput();
            cameraDevice.StartRunning(previewTextureOutput);
            // Display the preview
            var previewTexture = await previewTextureOutput;
            visualizer.image = previewTexture;
            // Create the BlazePose pipeline
            var detectorModelData = await MLModelData.FromHub("@natml/blazepose-detector");
            var predictorModelData = await MLModelData.FromHub("@natml/blazepose-landmark");
            posePipeline = new BlazePosePipeline(detectorModelData, predictorModelData, maxDetections: 1);
        }

        void Update () {
            // Check that pipeline has been created
            if (posePipeline == null)
                return;
            // Predict
            var poses = posePipeline.Predict(previewTextureOutput.texture);
            // Visualize
            if (poses.Length == 0)
                return;
            var pose = poses[0];
            visualizer.Clear();
            visualizer.Render(pose.keypoints, Color.green);
            visualizer3D.Render(pose.keypoints3D);
        }
    }
}