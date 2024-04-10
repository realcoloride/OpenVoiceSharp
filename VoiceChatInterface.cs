using OpusDotNet;

namespace OpenVoiceSharp
{
    public abstract class VoiceChatInterface
    {
        public static bool EnableNoiseSuppression => true;
        public const int FrameLength = 20;
        public const int SampleRate = 48000; // Base opus and RN frequency

        public int Bitrate { get; protected set; } = 16000;
        public bool Stereo { get; protected set; } = false;
        public int GetChannelsAmount() => Stereo ? 2 : 1;
        public bool FavorAudioStreaming { get; protected set; } = false;


        private OpusEncoder OpusEncoder;
        private OpusDecoder OpusDecoder;

        // webrtc vad
        public bool IsSpeaking(byte[] pcmData)
        {

        }


        private float[] FloatSamples;
        /// <summary>
        /// Encodes and processes microphone data.
        /// </summary>
        /// <param name="pcmData">The 16 bit PCM data according to your needs.</param>
        /// <returns>Encoded Opus data.</returns>
        public (byte[], int) FeedMicrophoneData(byte[] pcmData)
        {

        }

        public (byte[], int) WhenDataReceived(byte[] pcmData, int length)
        {

            return (OpusDecoder.Decode(pcmData, length, out int decodedLength), decodedLength);
        }

        public VoiceChatInterface(int bitrate, bool stereo, bool favorAudioStreaming)
        {
            Bitrate = bitrate;
            Stereo = stereo;
            FavorAudioStreaming = favorAudioStreaming;

            int channels = GetChannelsAmount();

            OpusEncoder = new(
                FavorAudioStreaming ? Application.Audio : Application.VoIP,
                SampleRate,
                channels
            )
            {
                Bitrate = Bitrate,
                VBR = false,
                ForceChannels = Stereo ? ForceChannels.Stereo : ForceChannels.Mono
            };

            OpusDecoder = new(FrameLength, SampleRate, channels);
        }
    }
}
