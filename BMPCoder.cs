using System;
using System.Collections.Generic;

namespace BMPSteganography
{
    public static class BMPCoder
    {
        public const int DataOffset = 54;
        private const byte ByteEndMessage = 0xFF;
        public static byte[] Encode(byte[] image, byte[] message)
        {
            byte[] newImage = new byte[image.Length];
            int countBytesForImage = DataOffset + (message.Length + 1) * 4;

            if (newImage.Length < countBytesForImage)
                throw new ArgumentException("The data does not match the .bmp format");

            for (int i = 0; i < image.Length; i++)
                newImage[i] = image[i];

            for (int i = DataOffset; i < countBytesForImage - 4; i++)
            {
                newImage[i] = (byte)(image[i] & 0xFC);

                newImage[i] |= ((i - DataOffset) % 4) switch
                {
                    0 => (byte)(message[(i - DataOffset) / 4] >> 6 & 0x3),
                    1 => (byte)(message[(i - DataOffset) / 4] >> 4 & 0x3),
                    2 => (byte)(message[(i - DataOffset) / 4] >> 2 & 0x3),
                    3 => (byte)(message[(i - DataOffset) / 4] & 0x3),
                };
            }

            for (int i = countBytesForImage - 4; i < countBytesForImage; i++)
                newImage[i] |= 0x3;

            return newImage;
        }

        public static byte[] Decode(byte[] image)
        {
            if (image.Length <= DataOffset)
                throw new ArgumentException("The data does not match the .bmp format");

            List<byte> message = new List<byte>();

            byte _char = 0;
            for (int i = DataOffset; i < image.Length; i++)
            {

                _char |= ((i - DataOffset) % 4) switch
                {
                    0 => (byte)((image[i] & 3) << 6),
                    1 => (byte)((image[i] & 3) << 4),
                    2 => (byte)((image[i] & 3) << 2),
                    3 => (byte)(image[i] & 3),
                };

                if (_char == ByteEndMessage)
                    return message.ToArray();

                if ((i - DataOffset) % 4 == 3)
                {
                    message.Add(_char);
                    _char = 0;
                }
            }
            return message.ToArray();
        }
    }
}
