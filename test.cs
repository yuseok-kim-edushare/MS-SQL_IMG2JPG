#if !RELEASE_WITHOUT_TESTS
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;
using Image2Jpg;

namespace Image2Jpg.Test
{
    [TestFixture]
    public class ImageConverterTests
    {
        private string testFolderPath;
        private ImageConverter converter;

        [SetUp]
        public void Setup()
        {
            // Initialize the converter
            converter = new ImageConverter();
            
            // Test folder path will be directly in the output directory
            testFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test");
            
            if (!Directory.Exists(testFolderPath))
            {
                Assert.Fail($"Test folder not found at {testFolderPath}. Make sure the project is properly configured to copy test assets.");
            }
            
            Console.WriteLine($"Test folder path: {testFolderPath}");
        }

        [Test]
        public void TestPngToJpgConversion()
        {
            // Arrange
            string pngPath = Path.Combine(testFolderPath, "test.png");
            string outputPath = $"{pngPath}.jpg";
            
            // Clean up any existing output file from previous test runs
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            
            // Act
            byte[] imageData = File.ReadAllBytes(pngPath);
            bool result = converter.SaveAsJpg(imageData, outputPath);
            
            // Assert
            Assert.That(result, Is.True, "PNG to JPG conversion failed");
            Assert.That(File.Exists(outputPath), Is.True, "Output JPG file was not created");
            
            // Additional validation that file is a valid JPG
            using (var image = Image.FromFile(outputPath))
            {
                Assert.That(image.RawFormat.Guid, Is.EqualTo(ImageFormat.Jpeg.Guid), "Output file is not a valid JPEG");
            }
        }
        
        [Test]
        public void TestBmpToJpgConversion()
        {
            // Arrange
            string bmpPath = Path.Combine(testFolderPath, "test.bmp");
            string outputPath = $"{bmpPath}.jpg";
            
            // Clean up any existing output file from previous test runs
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            
            // Act
            byte[] imageData = File.ReadAllBytes(bmpPath);
            bool result = converter.SaveAsJpg(imageData, outputPath);
            
            // Assert
            Assert.That(result, Is.True, "BMP to JPG conversion failed");
            Assert.That(File.Exists(outputPath), Is.True, "Output JPG file was not created");
            
            // Additional validation that file is a valid JPG
            using (var image = Image.FromFile(outputPath))
            {
                Assert.That(image.RawFormat.Guid, Is.EqualTo(ImageFormat.Jpeg.Guid), "Output file is not a valid JPEG");
            }
        }
        
        [Test]
        public void TestGifToJpgConversion()
        {
            // Arrange
            string gifPath = Path.Combine(testFolderPath, "test.gif");
            string outputPath = $"{gifPath}.jpg";
            
            // Clean up any existing output file from previous test runs
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            
            // Act
            byte[] imageData = File.ReadAllBytes(gifPath);
            bool result = converter.SaveAsJpg(imageData, outputPath);
            
            // Assert
            Assert.That(result, Is.True, "GIF to JPG conversion failed");
            Assert.That(File.Exists(outputPath), Is.True, "Output JPG file was not created");
            
            // Additional validation that file is a valid JPG
            using (var image = Image.FromFile(outputPath))
            {
                Assert.That(image.RawFormat.Guid, Is.EqualTo(ImageFormat.Jpeg.Guid), "Output file is not a valid JPEG");
            }
        }
        
        [Test]
        public void TestConvertToJpgMethod()
        {
            // Arrange
            string bmpPath = Path.Combine(testFolderPath, "test.bmp");
            byte[] imageData = File.ReadAllBytes(bmpPath);
            
            // Act
            byte[] jpgData = converter.ConvertToJpg(imageData);
            
            // Assert
            Assert.That(jpgData, Is.Not.Null, "JPG data should not be null");
            Assert.That(jpgData.Length, Is.GreaterThan(0), "JPG data should have content");
            
            // Verify it's a valid JPG by writing to a temp file and checking format
            string tempFile = Path.Combine(Path.GetTempPath(), "temp_test.jpg");
            File.WriteAllBytes(tempFile, jpgData);
            
            using (var image = Image.FromFile(tempFile))
            {
                Assert.That(image.RawFormat.Guid, Is.EqualTo(ImageFormat.Jpeg.Guid), "Output data is not a valid JPEG");
            }
            
            // Clean up
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
        
        [Test]
        public void TestGetVersion()
        {
            // Act
            string version = converter.GetVersion();
            
            // Assert
            Assert.That(version, Is.Not.Null, "Version should not be null");
            Assert.That(version, Is.Not.Empty, "Version should not be empty");
            
            // Verify version format (should be in format like "1.0.0")
            Assert.That(version, Does.Match(@"^\d+\.\d+\.\d+$"), "Version should be in format X.Y.Z");
        }
        
        [TearDown]
        public void Cleanup()
        {
            // Optional: Clean up converted files after tests
            // Uncomment if you want to delete the generated JPG files
            /*
            string[] testFiles = { "test.png", "test.bmp", "test.gif" };
            foreach (string file in testFiles)
            {
                string outputPath = Path.Combine(testFolderPath, $"{file}.jpg");
                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }
            }
            */
        }
    }
    
    // Keep a simple Main method for command-line testing
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Run NUnit tests to test image conversion functionality.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
#endif
