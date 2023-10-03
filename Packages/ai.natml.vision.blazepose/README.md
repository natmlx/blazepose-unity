# BlazePose
[MediaPipe BlazePose](https://google.github.io/mediapipe/solutions/pose.html) for full-body pose detection in Unity Engine with [NatML](https://github.com/natmlx/natml-unity).

## Installing BlazePose
Add the following items to your Unity project's `Packages/manifest.json`:
```json
{
  "scopedRegistries": [
    {
      "name": "NatML",
      "url": "https://registry.npmjs.com",
      "scopes": ["ai.fxn", "ai.natml"]
    }
  ],
  "dependencies": {
    "ai.natml.vision.blazepose": "1.0.2"
  }
}
```

## Predicting Poses in an Image
First, create the BlazePose pipeline:
```csharp
// Fetch the model data
var detectorModelData = await MLModelData.FromHub("@natml/blazepose-detector");
var predictorModelData = await MLModelData.FromHub("@natml/blazepose-landmark");
// Create the BlazePose pipeline
var pipeline = new BlazePosePipeline(detectorModelData, predictorModelData);
```

Then detect poses in the image:
```csharp
// Create image feature
Texture2D image = ...;
// Detect poses in the image
BlazePosePredictor.Pose[] poses = pipeline.Predict(image);
```

___

## Requirements
- Unity 2022.3+

## Quick Tips
- Join the [NatML community on Discord](https://hub.natml.ai/community).
- Discover more ML models on [NatML Hub](https://hub.natml.ai).
- See the [NatML documentation](https://docs.natml.ai/unity).
- Contact us at [hi@natml.ai](mailto:hi@natml.ai).

Thank you very much!