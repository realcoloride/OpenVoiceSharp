using OpusDotNet;
using RNNoise.NET;
using WebRtcVadSharp;

namespace OpenVoiceSharp
{
    public abstract class VoiceChatInterface
    {
        public const int FrameLength = 20; // 20 ms, for max compatibility
        public const int SampleRate = 48000; // base opus and RNNoise frequency and webrtc
        public const int DefaultBitrate = 16000; // 16kbps, decent enough for voice chatting

        // properties
        public int Bitrate { get; protected set; } = DefaultBitrate;
        public bool Stereo { get; protected set; } = false;
        public bool EnableNoiseSuppression { get; protected set; } = true;
        public bool FavorAudioStreaming { get; protected set; } = false;
        public bool ApplySoftClipping { get; protected set; } = false;

        public int GetChannelsAmount() => Stereo ? 2 : 1;

        // instances
        private readonly OpusEncoder OpusEncoder;
        private readonly OpusDecoder OpusDecoder;
        private readonly Denoiser Denoiser = new();
        private readonly WebRtcVad VoiceActivityDetector = new()
        {
            FrameLength = WebRtcVadSharp.FrameLength.Is20ms,
            SampleRate = WebRtcVadSharp.SampleRate.Is48kHz
        };

        
        /// <summary>
        /// Returns if voice activity was detected using the WebRTC VAD.
        /// </summary>
        /// <param name="pcmData">The raw pcm frame (in 16 bit PCM)</param>
        /// <returns>If voice activity was detected in the frame.</returns>
        public bool IsSpeaking(byte[] pcmData) => VoiceActivityDetector.HasSpeech(pcmData);


        private readonly float[] FloatSamples;

        public void ApplyNoiseSuppression(byte[] pcmData)
        {
            // convert to float32
            VoiceUtilities.Convert16BitToFloat(pcmData, FloatSamples);

            // apply noise suppression
            Denoiser.Denoise(FloatSamples);

            // convert back to 16 bit pcm
            VoiceUtilities.ConvertFloatTo16Bit(FloatSamples, pcmData);
        }

        /// <summary>
        /// Encodes and processes microphone data. Also handles noise suppression if needed.
        /// </summary>
        /// <param name="pcmData">The 16 bit PCM data according to your needs.</param>
        /// <returns>Encoded Opus data.</returns>
        public (byte[] encodedOpusData, int encodedLength) FeedMicrophoneData(byte[] pcmData, int length)
        {
            if (EnableNoiseSuppression)
                ApplyNoiseSuppression(pcmData);

            return (OpusEncoder.Encode(pcmData, length, out int encodedLength), encodedLength);
        }

        public byte[] ProcessVoiceData(byte[] decodedOpusData, int decodedLength)
        {
            // apply soft clipping
            if (ApplySoftClipping)
            {
                for (int i = 0; i < decodedLength; i += 2)
                {
                    short sample = BitConverter.ToInt16(decodedOpusData, i);
                    short clippedSample = VoiceUtilities.ApplySoftClip(sample);
                    byte[] clippedBytes = BitConverter.GetBytes(clippedSample);
                    Array.Copy(clippedBytes, 0, decodedOpusData, i, 2);
                }
            }

            return decodedOpusData;
        }

        public (byte[] decodedOpusData, int decodedLength) WhenDataReceived(byte[] encodedData, int length)
            => (ProcessVoiceData(
                    OpusDecoder.Decode(encodedData, length, out int decodedLength), decodedLength), 
                decodedLength);

        public VoiceChatInterface(
            int bitrate = DefaultBitrate, 
            bool stereo = false, 
            bool favorAudioStreaming = false, 
            bool applySoftClipping = true,
            OperatingMode? vadOperatingMode = null
        ) {
            Bitrate = bitrate;
            Stereo = stereo;
            FavorAudioStreaming = favorAudioStreaming;
            ApplySoftClipping = applySoftClipping;
            int channels = GetChannelsAmount();

            // fill float samples for noise suppression
            FloatSamples = new float[VoiceUtilities.GetSampleSize(SampleRate, FrameLength, channels) / 2];

            // create opus encoder/decoder
            OpusEncoder = new(
                FavorAudioStreaming ? Application.Audio : Application.VoIP,
                SampleRate,
                channels
            ) {
                Bitrate = Bitrate,
                VBR = false,
                ForceChannels = Stereo ? ForceChannels.Stereo : ForceChannels.Mono
            };

            OpusDecoder = new(FrameLength, SampleRate, channels);

            if (vadOperatingMode != null)
                VoiceActivityDetector.OperatingMode = (OperatingMode)vadOperatingMode;
        }
    }
}
