using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Image2Jpg
{
    /// <summary>
    /// Provides functionality to convert MS-SQL Image column data to JPG format.
    /// This library is designed for use with PowerBuilder applications but can be used with any COM or .NET application.
    /// </summary>
    [ComVisible(true)]
    [Guid("1F853E9A-4276-4EDB-9C3B-20F7A3ADD432")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class ImageConverter
    {
        /// <summary>
        /// Converts a byte array from MS-SQL Image column to JPG format
        /// </summary>
        /// <param name="imageData">Byte array from MS-SQL Image column</param>
        /// <returns>Byte array containing JPG image data</returns>
        public byte[] ConvertToJpg(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentException("Image data cannot be null or empty", nameof(imageData));
            }

            try
            {
                using (MemoryStream inputStream = new MemoryStream(imageData))
                using (Image originalImage = Image.FromStream(inputStream))
                using (MemoryStream outputStream = new MemoryStream())
                {
                    // Save as JPEG with high quality (100)
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                    
                    ImageCodecInfo jpegCodec = GetJpegCodecInfo();
                    originalImage.Save(outputStream, jpegCodec, encoderParams);
                    
                    return outputStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting image data to JPG: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves MS-SQL Image column data as a JPG file
        /// </summary>
        /// <param name="imageData">Byte array from MS-SQL Image column</param>
        /// <param name="filePath">Path where the JPG file should be saved</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SaveAsJpg(byte[] imageData, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            try
            {
                byte[] jpgData = ConvertToJpg(imageData);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (FileStream fs = File.Open(filePath, FileMode.Create))
                {
                    fs.Write(jpgData, 0, jpgData.Length);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the JPEG codec information
        /// </summary>
        /// <returns>ImageCodecInfo for JPEG</returns>
        private ImageCodecInfo GetJpegCodecInfo()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    return codec;
                }
            }
            
            throw new Exception("JPEG codec not found");
        }

        /// <summary>
        /// Version information for the library
        /// </summary>
        /// <returns>Version string</returns>
        public string GetVersion()
        {
            return "1.0.0";
        }

        /// <summary>
        /// Converts an image file to JPG format
        /// </summary>
        /// <param name="inputFilePath">Path to the input image file</param>
        /// <param name="outputFilePath">Path where the JPG file should be saved</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool ConvertFileToJpg(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrWhiteSpace(inputFilePath))
            {
                throw new ArgumentException("Input file path cannot be null or empty", nameof(inputFilePath));
            }
            if (string.IsNullOrWhiteSpace(outputFilePath))
            {
                throw new ArgumentException("Output file path cannot be null or empty", nameof(outputFilePath));
            }
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException("Input file not found", inputFilePath);
            }

            try
            {
                byte[] imageData = File.ReadAllBytes(inputFilePath);
                return SaveAsJpg(imageData, outputFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting file to JPG: {ex.Message}", ex);
            }
        }
    }
}
