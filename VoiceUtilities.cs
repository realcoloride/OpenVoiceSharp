namespace OpenVoiceSharp
{
    public static class VoiceUtilities
    {
        public static int GetSampleSize(int sampleRate, int timeLengthMs) => (int)(sampleRate * 16f / 8f * (timeLengthMs / 1000f));
    }
}
