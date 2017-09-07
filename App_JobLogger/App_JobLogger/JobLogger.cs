using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace App_JobLogger
{
    public class JobLogger
    {
        #region "Variables"
        private static bool _blLogToFile;
        private static bool _blLogToConsole;
        private static bool _blLogMessage;
        private static bool _blLogWarning;
        private static bool _blLogError;
        private static bool _blLogToDatabase;
        private bool _initialized;
        #endregion
        
        #region "Constantes"
        private static int _tipoLog = 0;
        private enum TipoLog : int { Message = 1, Error = 2, Warning = 3 };
        #endregion

        public JobLogger(bool blLogToFile, bool blLogToConsole, bool blLogToDatabase, bool blLogMessage, bool blLogWarning, bool blLogError)
        {
            _blLogError = blLogError;
            _blLogMessage = blLogMessage;
            _blLogWarning = blLogWarning;
            _blLogToDatabase = blLogToDatabase;
            _blLogToFile = blLogToFile;
            _blLogToConsole = blLogToConsole;
        }

        public static void LogMessage(string strMessage, int intTipoLog)
        {
            _tipoLog = intTipoLog;
            string strFechaActual = DateTime.Now.ToShortDateString();
            if (Convert.ToString(strMessage).Trim().Length == 0)
            {
                throw new Exception("No message");
            }
            if (!_blLogToConsole && !_blLogToFile && !_blLogToDatabase)
            {
                throw new Exception("Invalid configuration");
            }
            if (!_blLogError && !_blLogMessage && !_blLogWarning)
            {
                throw new Exception("Error or Warning or Message Configuration must be set");
            }
            if (!Enum.IsDefined(typeof(TipoLog), _tipoLog))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            if (_blLogToDatabase)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    try{
                        connection.Open();
                        SqlCommand command = new SqlCommand("Insert into Log Values('" + strMessage + "', " + Convert.ToString(_tipoLog) + ")");
                        command.ExecuteNonQuery();
                    }catch (Exception){
                        throw new Exception("Error in SQL insert");
                    }
                }
            }
            
            if (_blLogToFile)
            {
                string strTextoArchivo = string.Empty;
                if (!File.Exists(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + strFechaActual + ".txt"))
                {
                    strTextoArchivo = File.ReadAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + strFechaActual + ".txt");
                }
                strTextoArchivo += DateTime.Now.ToShortDateString() + " - "+ strMessage;
                File.WriteAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + strFechaActual + ".txt", strTextoArchivo);
            }

            if (_blLogToConsole)
            {
                Console.ForegroundColor = GetColorConsole(_tipoLog);
                Console.WriteLine(DateTime.Now.ToShortDateString() + strMessage);
            }
        }

        private static ConsoleColor GetColorConsole(int _tipoLog)
        {
            if (_tipoLog == (int)TipoLog.Message) return ConsoleColor.White;
            if (_tipoLog == (int)TipoLog.Error) return ConsoleColor.Red;
            if (_tipoLog == (int)TipoLog.Warning) return ConsoleColor.Yellow;
            return ConsoleColor.Black;
        }
    }
}