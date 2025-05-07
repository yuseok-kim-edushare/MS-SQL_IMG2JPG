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
        /// Write Log into C:\temp\Image2Jpg.log
        /// </summary>
        /// <param name="message">Message to write to the log</param>
        private void WriteLog(string message)
        {
            string logFilePath = "C:\\temp\\Image2Jpg.log";
            if (!Directory.Exists("C:\\temp"))
            {
                Directory.CreateDirectory("C:\\temp");
            }
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath);
            }
            File.AppendAllText(logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n");
        }

        /// <summary>
        /// Converts a byte array from MS-SQL Image column to JPG format
        /// </summary>
        /// <param name="imageData">Byte array from MS-SQL Image column</param>
        /// <returns>Byte array containing JPG image data</returns>
        public byte[] ConvertToJpg(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                WriteLog("Image data cannot be null or empty");
                return null;
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
                WriteLog($"Error converting image data to JPG: {ex.Message}");
                return null;
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
                WriteLog("File path cannot be null or empty");
                return false;
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
            catch (Exception ex)
            {
                WriteLog($"Error saving image data as JPG: {ex.Message}");
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
            WriteLog("JPEG codec not found");
            return null;
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
                WriteLog("Input file path cannot be null or empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(outputFilePath))
            {
                WriteLog("Output file path cannot be null or empty");
                return false;
            }
            if (!File.Exists(inputFilePath))
            {
                WriteLog($"Input file not found: {inputFilePath}");
                return false;
            }

            try
            {
                byte[] imageData = File.ReadAllBytes(inputFilePath);
                return SaveAsJpg(imageData, outputFilePath);
            }
            catch (Exception ex)
            {
                WriteLog($"Error converting file to JPG: {ex.Message}");
                return false;
            }
        }
    }
}
