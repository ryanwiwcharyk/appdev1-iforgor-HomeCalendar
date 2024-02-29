using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{

    /// <summary>
    ///Manages the files used in the Calendar project.
    /// </summary>
    public class CalendarFiles
    {
        private static String DefaultSavePath = @"Calendar\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        // verify that the name of the file, or set the default file, and 
        // is it readable?
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================
        /// <summary>
        /// Verifies whether the given file path is a valid file path for reading. 
        /// Use before attempting to read from a file. Defaults to AppData if the file path is invalid.
        /// </summary>
        /// <param name="FilePath">The file path to be verified.</param>
        /// <param name="DefaultFileName">Default file name if the given path is invalid.</param>
        /// <returns>The given <paramref name="FilePath"/> if it is valid.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the given <paramref name="FilePath"/> is not valid.</exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string filePath = CalendarFiles.VerifyReadFromFileName("./file.txt", "defaultName");
        /// ]]>
        /// </code>
        /// </example>
        public static String VerifyReadFromFileName(String? FilePath, String DefaultFileName)
        {

            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;

        }

        // ====================================================================
        // verify that the name of the file, or set the default file, and 
        // is it writable
        // ====================================================================
        /// <summary>
        /// Verifies whether the given file path is a valid file path for writing. 
        /// Uses default directory and file if given path or directory is not valid.
        /// Use before attempting to write to a file.
        /// </summary>
        /// <param name="FilePath">The file path to be verified.</param>
        /// <param name="DefaultFileName">The file path to be verified.</param>
        /// <returns>The given <paramref name="FilePath"/> if it is valid.</returns>
        /// <exception cref="Exception">Thrown when the given <paramref name="FilePath"/> is not valid.</exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string writeToFile = CalendarFiles.VerifyWriteToFileName("./file.txt", "file.txt");
        /// ]]>
        /// </code>
        /// </example>
        public static String VerifyWriteToFileName(String? FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Calendar directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String? folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }

            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;

        }



    }
}
