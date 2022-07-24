/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples.Visualizers {

    using UnityEngine;

    public class HoverCamera : MonoBehaviour {

        public Vector3 target;
        public float radius = 2f;
        public float height = 1f;
        public float speed = 0.5f;

        void Update () {
            transform.position = target + new Vector3(
                radius * Mathf.Cos(speed * Time.time),
                height,
                radius * Mathf.Sin(speed * Time.time)
            );
            transform.LookAt(target);
        }
    }
}