namespace OpenVoiceSharp
{
    internal static class Example
    {
        static VoiceChatInterface voiceChatInterface = new();

        // basic recorder
        public static void Main()
        {
            BasicMicrophoneRecorder recorder = new();

            recorder.DataAvailable += WhenDataAvailable;
            recorder.StartRecording();
        }

        // encoding & sending
        private static void WhenDataAvailable(byte[] pcmData, int length)
        {
            (byte[] encodedData, int encodedLength) = voiceChatInterface.SubmitAudioData(pcmData, length);
            Send(encodedData, encodedLength);
        }
        private static void Send(byte[] encodedData, int encodedLength)
        {
            // define your logic here... (assuming you have a proper networking system in place already)
        }

        // decoding & playing
        private static void WhenVoicePacketReceived(byte[] encodedData, int encodedLength)
        {
            // here we assume that encodedData contains the bytes of the opus encoded data
            (byte[] decodedData, int decodedLength) = voiceChatInterface.WhenDataReceived(encodedData, encodedLength);
            SubmitBuffer(decodedData, decodedLength);
        }

        private static void SubmitBuffer(byte[] decodedData, int decodedLength)
        {
            // define your logic here... (convert to float 32 or more if needed using VoiceUtilities)
        }
    }
}
