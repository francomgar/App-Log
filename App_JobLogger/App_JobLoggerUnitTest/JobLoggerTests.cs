using System;
using App_JobLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace App_JobLoggerUnitTest
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid configuration")]
        public void LogMessageTest_InvalidConfiguration()
        {
            //Arrange
            const string strMessage = "UNIT TESTING";
            const int intTipoLog = 3; 

            //Act
            JobLogger jLog = new JobLogger(false, false, false, false, false, true);
            JobLogger.LogMessage(strMessage, intTipoLog);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error or Warning or Message must be specified")]
        public void LogMessageTest_NoTypeSpecified()
        {
            //Arrange
            const string strMessage = "UNIT TESTING";
            const int intTipoLog = 3;

            //Act
            JobLogger jLog = new JobLogger(false, true, false, false, false, false);
            JobLogger.LogMessage(strMessage, intTipoLog);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error or Warning or Message must be specified")]
        public void LogMessageTest_TypeNotExists()
        {
            //Arrange
            const string strMessage = "UNIT TESTING";
            const int intTipoLog = 4;

            //Act
            JobLogger jLog = new JobLogger(false, true, false, false, true, true);
            JobLogger.LogMessage(strMessage, intTipoLog);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No message set")]
        public void LogMessageTest_MessageEmpty()
        {
            //Arrange
            const string strMessage = "";
            const int intTipoLog = 1;

            //Act
            JobLogger jLog = new JobLogger(false, true, false, false, true, true);
            JobLogger.LogMessage(strMessage, intTipoLog);
        }

        [TestMethod]
        public void LogMessageTest_Console()
        {
            using (StringWriter sw = new StringWriter())
            {
                //Arrange
                const string strMessage = "TESTING";
                const int intTipoLog = 1;

                Console.SetOut(sw);
                JobLogger jLog = new JobLogger(false, true, false, true, true, true);
                JobLogger.LogMessage(strMessage, intTipoLog);

                Assert.AreEqual<string>("21/03/2016 - TESTING", Convert.ToString(sw).Trim());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void LogMessageTest_Database()
        {
            //Arrange
            const string strMessage = "TESTING";
            const int intTipoLog = 1;

            JobLogger jLog = new JobLogger(false, false, true, true, true, true);
            JobLogger.LogMessage(strMessage, intTipoLog);
        }

    }
}
