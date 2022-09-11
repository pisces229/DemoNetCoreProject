using DemoNetCoreProject.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.UnitTest.Domain.Utilities
{
    [TestClass]
    public class UnitTest_FileUtility
    {
        #region UnsafeFile
        [TestMethod]
        public void Run_UnsafeFile_Fail()
        {
            try
            {
                FileUtility.UnsafeFile("/a.txt");
                Assert.Fail();
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.Message);
            }
        }
        [TestMethod]
        public void Run_UnsafeFile_Success()
        {
            FileUtility.UnsafeFile("a.txt");
        }
        #endregion

        #region UnsafeDirectory
        [TestMethod]
        public void Run_UnsafeDirectory_Fail()
        {
            try
            {
                FileUtility.UnsafeDirectory("../");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        [TestMethod]
        public void Run_UnsafeDirectory_Success()
        {
            FileUtility.UnsafeDirectory("abc");
        }
        #endregion

        #region GetFile
        [TestMethod]
        public void Run_GetFile_Fail()
        {
            try
            {
                FileUtility.GetFile(new DirectoryInfo("d:/workapsce/test/"), "../");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        [TestMethod]
        public void Run_GetFile_Success()
        {
            FileUtility.GetFile(new DirectoryInfo("d:/workapsce/test/"), "abc");
        }
        #endregion

        #region GetDirectory
        [TestMethod]
        public void Run_GetDirectory_Fail()
        {
            try
            {
                FileUtility.GetDirectory(new DirectoryInfo("d:/workapsce/test/"), "../");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        [TestMethod]
        public void Run_GetDirectory_Success()
        {
            FileUtility.GetDirectory(new DirectoryInfo("d:/workapsce/test/"), "abc");
        }
        #endregion
    }
}
