namespace OpenVoiceSharp
{
    public enum RecommendedChunkAmount
    {
        Unity = 18,
    }

    /// <summary>
    /// Creates a circular audio buffer that can be read and written.
    /// Takes in chunks/frames of audio and can be read.
    /// It is recommended to wait for the buffer to be full to read the entirety of it.
    /// Useful for Unity or other engines that do not support streamed pcm reading by default.
    /// </summary>
    /// <typeparam name="T">Byte/short/float depending on your needs.</typeparam>
    public class CircularAudioBuffer<T>
    {
        /// <summary>
        /// The raw length of the buffer, in samples.
        /// </summary>
        public int BufferLength { get; private set; }
        /// <summary>
        /// The raw length of a chunk, in samples.
        /// </summary>
        public int ChunkSize { get; private set; }

        public int BufferAvailable => ChunksAvailable * ChunkSize;
        public int ChunksAvailable = 0;

        private readonly T[] Buffer;
        public bool BufferFull => BufferAvailable == BufferLength;

        public bool CanReadChunk => ChunksAvailable > 0;

        // no need to do a for loop to rewrite the buffer.
        // just dont give it.
        /// <summary>
        /// Reads the first chunk available at the front of the buffer.
        /// </summary>
        /// <returns>Chunk</returns>
        public T[] ReadChunk()
        {
            if (!CanReadChunk)
                throw new Exception("No chunks are available.");

            // slice the chunk
            T[] chunk = Buffer[ChunksAvailable..ChunkSize];

            // grab the rest and put it at the front
            chunk.CopyTo(Buffer[ChunkSize..BufferLength], 0);

            ChunksAvailable--;

            return chunk;
        }

        /// <summary>
        /// Reads the first chunk available at the front of the buffer and copies it to another specified buffer.
        /// </summary>
        /// <param name="target">The target buffer to which it'll be copied to</param>
        /// <param name="offset">The begin offset to which it'll begin copying to</param>
        public void ReadChunkTo(T[] target, int offset = 0) => ReadChunk().CopyTo(target, offset);

        /// <summary>
        /// Reads all the buffer that is available.
        /// It is highly recommended you wait for it to be full to read.
        /// </summary>
        /// <returns>Buffer</returns>
        public T[] ReadAllBuffer()
        {
            int available = BufferAvailable;
            ChunksAvailable = 0;

            // read everything thats available
            return Buffer[0..available];
        }
        /// <summary>
        /// Reads all the buffer that is available and copies it to another buffer.
        /// It is highly recommended you wait for it to be full to read.
        /// </summary>
        /// <param name="target">The target buffer to which it'll be copied to</param>
        /// <param name="offset">The begin offset to which it'll begin copying to</param>
        public void ReadAllBufferTo(T[] target, int offset = 0) => ReadAllBuffer().CopyTo(target, offset);

        /// <summary>
        /// Pushes a chunk into the buffer. Will be ignored if the buffer is full. (no overflow)
        /// </summary>
        /// <param name="chunk">Chunk/frame</param>
        public void PushChunk(T[] chunk)
        {
            // push chunk back if not full
            if (BufferFull) return;

            // if size doesnt match
            if (chunk.Length != ChunkSize) throw new Exception($"Invalid chunk size. Submitted {chunk.Length} - should be {ChunkSize}");

            // copy to
            chunk.CopyTo(Buffer, BufferAvailable);
            ChunksAvailable++;
        }

        /// <summary>
        /// Creates a circular audio buffer.
        /// </summary>
        /// <param name="chunkSize">Chunk raw size of ONE frame. Use VoiceUtilities if you need to figure out for your sample.</param>
        /// <param name="amountOfChunks">Amount of chunks the circular audio buffer can take in. Higher values are usually more stable and lower values usually cause more audio cracking, but will do more latency (20 * x ms).</param>
        public CircularAudioBuffer(int chunkSize, int amountOfChunks = 18)
        {
            ChunkSize = chunkSize;
            BufferLength = chunkSize * amountOfChunks;
            Buffer = new T[BufferLength];
        }
        /// <summary>
        /// Creates a circular audio buffer.
        /// </summary>
        /// <param name="chunkSize">Chunk raw size of ONE frame. Use VoiceUtilities if you need to figure out for your sample.</param>
        /// <param name="engine">Recommended engine for the amount of chunks the circular audio buffer can take in.</param>
        public CircularAudioBuffer(int chunkSize, RecommendedChunkAmount engine)
            : this(chunkSize, (int)engine) { }
    }
}
