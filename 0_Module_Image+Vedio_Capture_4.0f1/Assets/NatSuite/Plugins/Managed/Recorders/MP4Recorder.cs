/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders {

    using System;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// MP4 video recorder.
    /// </summary>
    public sealed class MP4Recorder : IMediaRecorder {

        #region --Client API--
        /// <summary>
        /// Video size.
        /// </summary>
        public (int width, int height) frameSize => recorder.frameSize;

        /// <summary>
        /// Create an MP4 recorder.
        /// </summary>
        /// <param name="width">Video width.</param>
        /// <param name="height">Video height.</param>
        /// <param name="frameRate">Video frame rate.</param>
        /// <param name="sampleRate">Audio sample rate. Pass 0 for no audio.</param>
        /// <param name="channelCount">Audio channel count. Pass 0 for no audio.</param>
        /// <param name="bitrate">Video bitrate in bits per second.</param>
        /// <param name="keyframeInterval">Keyframe interval in seconds.</param>
        public MP4Recorder (int width, int height, float frameRate, int sampleRate = 0, int channelCount = 0, int bitrate = (int)(960 * 540 * 11.4f), int keyframeInterval = 3) => this.recorder = new NativeRecorder((callback, context) => Bridge.CreateMP4Recorder(width, height, frameRate, bitrate, keyframeInterval, sampleRate, channelCount, Utility.GetPath(@".mp4"), callback, context));

        /*public MP4Recorder(int width, int height, float frameRate, int sampleRate = 0, int channelCount = 0, int bitrate = 5909760, int keyframeInterval = 3, Action<string> onReplay = null) : this(width, height, frameRate, sampleRate, channelCount, bitrate, keyframeInterval)
        {
            this.onReplay = onReplay;
        }*/

        public MP4Recorder(int videoWidth, int videoHeight, int v1, int v2, int v3, Action<string> onReplay)
        {
            this.videoWidth = videoWidth;
            this.videoHeight = videoHeight;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.onReplay = onReplay;
        }

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have an RGBA8888 pixel layout.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer containing video frame to commit.</param>
        /// <param name="timestamp">Frame timestamp in nanoseconds.</param>
        public void CommitFrame<T> (T[] pixelBuffer, long timestamp) where T : struct => recorder.CommitFrame(pixelBuffer, timestamp);

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have an RGBA8888 pixel layout.
        /// </summary>
        /// <param name="nativeBuffer">Pixel buffer in native memory to commit.</param>
        /// <param name="timestamp">Frame timestamp in nanoseconds.</param>
        public void CommitFrame (IntPtr nativeBuffer, long timestamp) => recorder.CommitFrame(nativeBuffer, timestamp);

        /// <summary>
        /// Commit an audio sample buffer for encoding.
        /// </summary>
        /// <param name="sampleBuffer">Linear PCM audio sample buffer, interleaved by channel.</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds.</param>
        public void CommitSamples (float[] sampleBuffer, long timestamp) => recorder.CommitSamples(sampleBuffer, timestamp);

        /// <summary>
        /// Finish writing and return the path to the recorded media file.
        /// </summary>
        public Task<string> FinishWriting () => recorder.FinishWriting();
        #endregion

        private readonly IMediaRecorder recorder;
        private Action<string> onReplay;
        private int videoWidth;
        private int videoHeight;
        private int v1;
        private int v2;
        private int v3;
    }
}