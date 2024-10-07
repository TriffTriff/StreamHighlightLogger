using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StreamHighlightLogger
{
    public class SHL
    {
        public ColorChanger colorChanger = new ColorChanger();
        public DateTime dateTimeOfStream { get; set; }
        public DateTime streamTime { get; set; } = DateTime.Now;
        public string StreamTitle = "";
        public string filePath = "";
        public string streamerName = "";
        public void GetSetStreamStartTime(string hourMinSec)
        {
            double hours = Convert.ToDouble(hourMinSec.Split(':')[0]);
            double minutes = Convert.ToDouble(hourMinSec.Split(':')[1]);
            double seconds = Convert.ToDouble(hourMinSec.Split(':')[2]);

            //* by -1.0 to subtract hours:min:sec from dateTimeOfStream:
            this.dateTimeOfStream = System.DateTime.Now.AddHours(-1.0 * hours).AddMinutes(-1.0 * minutes).AddSeconds(-1.0 * seconds);
            //add hours:min:sec to streamTime:
            this.streamTime = System.DateTime.Now.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }
        public void GetSetStreamStartTime(string hourMinSec, DateTime date)
        {
            double hours = Convert.ToDouble(hourMinSec.Split(':')[0]);
            double minutes = Convert.ToDouble(hourMinSec.Split(':')[1]);
            double seconds = Convert.ToDouble(hourMinSec.Split(':')[2]);

            //* by -1.0 to subtract hours:min:sec from dateTimeOfStream:
            this.dateTimeOfStream = date.AddHours(-1.0 * hours).AddMinutes(-1.0 * minutes).AddSeconds(-1.0 * seconds);
            //add hours:min:sec to streamTime:
            this.streamTime = date.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }
        public void GetStreamDate()
        {
            while (true)
            {
                Console.WriteLine("What date was the stream? (YYYY-MM-DD):");
                string date = Console.ReadLine();
                //good enough regex:
                string regexPatern = @"^[0-9][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9]$";
                Regex regex = new Regex(regexPatern);
                if (!regex.IsMatch(date))
                {
                    this.PrintLineWithColor("Date format wrong...", "ProgramMessage");
                    continue;
                }
                string time = "";
                    while (true)
                    {
                        this.PrintLineWithColor("How much time in the stream has elapsed? (24H - HH:MM:SS):", "ProgramMessage");
                        time = Console.ReadLine();
                        string regexPatern2 = @"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
                        Regex regex2 = new Regex(regexPatern2);
                        if (!regex2.IsMatch(time))
                        {
                            this.PrintLineWithColor("Time format wrong...", "ProgramMessage");
                            continue;
                        }
                        //Get and set for use in file name later:
                        this.GetSetStreamStartTime(time);
                        //Set timer that we can pull timestamp from to label rows on 
                        break;
                    }
                this.GetSetStreamStartTime(time,Convert.ToDateTime(date));
                break;
            }
        }


        public DateTime GetStreamTimer()
        {
            DateTime dt = DateTime.Now.Add(new TimeSpan(-1 * Convert.ToInt32(dateTimeOfStream.Hour.ToString()), -1 * Convert.ToInt32(dateTimeOfStream.Minute.ToString()), -1 * Convert.ToInt32(dateTimeOfStream.Second.ToString())));
            return dt;
        }

        public void CheckElapsedTime()
        {
            while (true)
            {
                this.PrintLineWithColor("How much time in the stream has elapsed? (24H - HH:MM:SS):", "ProgramMessage");
                string s = Console.ReadLine();
                string regexPatern = @"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
                Regex regex = new Regex(regexPatern);
                if (!regex.IsMatch(s))
                {
                    this.PrintLineWithColor("Time format wrong...", "ProgramMessage");
                    continue;
                }
                //Get and set for use in file name later:
                this.GetSetStreamStartTime(s);
                //Set timer that we can pull timestamp from to label rows on 
                break;
            }
        }
        public void PrintLineWithColor(string text, string type)
        {
            switch (type)
            {
                case "ProgramMessage":
                    this.colorChanger.SetTextColor("DarkGray");
                    Console.WriteLine(text);
                    break;
                case "Highlights":
                    this.colorChanger.SetTextColor("Green");
                    Console.WriteLine(text);
                    WriteToFile(text);
                    break;
            }
            //reset color:
            this.colorChanger.SetTextColor("White");
        }

        public void WriteToFile(string highlightRow)
        {
            using (StreamWriter outputFile = new StreamWriter(this.filePath,true))
            {
                outputFile.WriteLine(highlightRow);
            }
        }
        public string RemoveBadCharacters(string strWithBadCharacters)
        {
            char[] charsToRemove = new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };
            char[] strWithBadCharactersCharArr = strWithBadCharacters.ToCharArray();
            for(int i= 0; i< strWithBadCharactersCharArr.Length; i++)
            {
                for (int j = 0; j < charsToRemove.Length; j++)
                {
                    if (strWithBadCharactersCharArr[i] == charsToRemove[j])
                        strWithBadCharactersCharArr[i] = '_';
                }
            }
            strWithBadCharacters = new string(strWithBadCharactersCharArr);
            return strWithBadCharacters;
        }
    }
}
