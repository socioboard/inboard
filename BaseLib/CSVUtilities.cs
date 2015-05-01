using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace BaseLib
{
    public class CSVUtilities
    {

        static List<string[]> dataToBeWritten = new List<string[]>();
        
        public static List<string[]> parseCSV(string path)
         {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                         row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
            }

            return parsedData;
        }

        public static void writeCSV(string CSVPath, List<string[]> dataToBeWritten)
        {
            foreach (var strArray in dataToBeWritten)
            {
                string eachProfileData = string.Join(",", strArray);
                //GlobusLib.GlobusFileHelper.WriteStringToTextfile("name" + "," + "age" + "," + "sex" + "," + "location" + "," + "Height" + "," + "Weight" + "," + "Measurements" + "," + "ShoeSize" + "," + "HairColor" + "," + "HairLength" + "," + "EyeColor" + "," + "Ethnicity" + "," + "SkinColor" + "," + "Experience" + "," + "Shoot nudes" + "," + "Genres" + "," + "Bust" + "," + "Waist" + "," + "Cup" + "," + "Hips" + "," + "Dress" + "," + "Compensation" + "," + "Image", CSVPath);
                FileUtilities.AppendStringToTextFile(eachProfileData, CSVPath);
            }


        }

        public static void ExportDataCSVFile(string CSV_Header, string CSV_Content, string CSV_FilePath)
        {
            try
            {
                if (!File.Exists(CSV_FilePath))
                {
                    CSVUtilities.WriteCSVLineByLine(CSV_FilePath, CSV_Header);
                }
               
                CSVUtilities.WriteCSVLineByLine(CSV_FilePath, CSV_Content);
            }
            catch (Exception)
            {

            }
        }

        public static void WriteCSVLineByLine(string CSVDestinationFilePath, string commaSeparatedData)
        {
            FileUtilities.AppendStringToTextFile(CSVDestinationFilePath, commaSeparatedData);
        }
    }
}
