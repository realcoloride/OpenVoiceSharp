namespace OpenVoiceSharp
{
    public static class VoiceUtilities
    {
        public static int GetSampleSize(int sampleRate, int timeLengthMs, int channels) 
            => (int)(sampleRate * 16f / 8f * (timeLengthMs / 1000f)) * channels;

        /// <summary>
        /// Converts 16 bit PCM data into float 32.
        /// Note that the float array must be half the size of the byte array.
        /// </summary>
        /// <param name="input">The 16 bit PCM data according to your needs.</param>
        /// <param name="output">The output data in which the result will be returned.</param>
        /// <returns>The 16 bit byte array.</returns>
        public static void Convert16BitToFloat(byte[] input, float[] output)
        {
            int outputIndex = 0;
            short sample;

            for (int n = 0; n < output.Length; n++)
            {
                sample = BitConverter.ToInt16(input, n * 2);
                output[outputIndex++] = sample / 32768f;
            }
        }

        /// <summary>
        /// Converts float 32 PCM data into 16 bit.
        /// Note that the byte array must be double the size of the float array.
        /// </summary>
        /// <param name="input">The float 32 PCM data according to your needs.</param>
        /// <param name="output">The output data in which the result will be returned.</param>
        /// <returns>The float32 PCM array.</returns>
        public static void ConvertFloatTo16Bit(float[] input, byte[] output)
        {
            int sampleIndex = 0, pcmIndex = 0;

            while (sampleIndex < input.Length)
            {
                short outsample = (short)(input[sampleIndex] * short.MaxValue);
                output[pcmIndex] = (byte)(outsample & 0xff);
                output[pcmIndex + 1] = (byte)((outsample >> 8) & 0xff);

                sampleIndex++;
                pcmIndex += 2;
            }
        }

        public static short ApplySoftClipping(short sample, double thresholdFactor)
        {
            double threshold = 32767.0 / thresholdFactor;
            double x = sample / 32767.0;

            return x >= 0 ? 
                (short)(threshold * (1 - Math.Exp(-x / threshold))) : 
                (short)(-threshold * (1 - Math.Exp(x / threshold)));
        }
    }
}
