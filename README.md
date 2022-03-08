# BMP-Steganography
Static **BMPCoder** class has two methods for encrypting and decrypting *.bmp* images using steganography
```C#
byte[] Encode(byte[] image, byte[] message);
byte[] Decode(byte[] image);
```

## Usage example from Program.cs
```C#
using System;
using System.IO;
using System.Text;

namespace BMPSteganography
{
    public class Program
    {
        static void Main()
        {
            string inputBMPFilePath = Directory.GetCurrentDirectory() + @"\1.bmp";
            string outputBMPFilePath = Directory.GetCurrentDirectory() + @"\2.bmp";
            string textMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit." +
                " Sed tempus sit amet lectus ut dapibus. Aliquam blandit laoreet metus, non.";

            byte[] message, inputImage, outputImage;
           
            using (FileStream fr = new FileStream(inputBMPFilePath, FileMode.Open))
            {
                inputImage = new byte[fr.Length];
                fr.Read(inputImage);
            }

            message = Encoding.UTF8.GetBytes(textMessage);
            outputImage = BMPCoder.Encode(inputImage, message);                               

            using (FileStream fr = new FileStream(outputBMPFilePath, FileMode.Create))
            {
                fr.Write(outputImage);
            }            

            message = BMPCoder.Decode(outputImage);
            textMessage = Encoding.UTF8.GetString(message);            
            Console.WriteLine(textMessage);
        }
    }
}
