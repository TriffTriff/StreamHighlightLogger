// See https://aka.ms/new-console-template for more information

string input = "";
bool stayInLoop = true;
//string dateTime = 
StreamHighlightLogger.SHL shl = new StreamHighlightLogger.SHL();
while (stayInLoop)
{
    shl.PrintLineWithColor("Enter stream title:", "ProgramMessage");
    shl.StreamTitle = Console.ReadLine();
    shl.StreamTitle = shl.RemoveBadCharacters(shl.StreamTitle);
    shl.PrintLineWithColor("Is this for a stream today? (y/n):", "ProgramMessage");
    input = Console.ReadLine();
    if (input.Equals("exit") || input.Equals("quit"))
    {
        shl.PrintLineWithColor("Exiting...", "ProgramMessage");
        break;
    }
    switch (input)
    {
        case "y":
        case "Y":
        case "yes":
        case "Yes":
            shl.CheckElapsedTime();
            break;
        case "n":
        case "N":
        case "no":
        case "No":
            //when date entered, todays date used in file name instead:
            shl.GetStreamDate();
            break;
        default:
            shl.PrintLineWithColor("Wrong Input...", "ProgramMessage");
            continue;
    }
    shl.filePath = AppDomain.CurrentDomain.BaseDirectory +@"StreamHighlights\"+ shl.StreamTitle.ToString() + "_" + shl.streamTime.Date.ToShortDateString() + ".txt";
    using (StreamWriter sw = File.AppendText(shl.filePath))
    shl.PrintLineWithColor("Enter title of stream highlight. Time will be saved along side it in the file:", "ProgramMessage");
    //start loop for keeping track of highlights and their timestamps:
    while (true)
    {
        string s = Console.ReadLine();
        if (s.Equals("exit") || s.Equals("quit"))
        {
            shl.PrintLineWithColor("File saved to path: "+ AppDomain.CurrentDomain.BaseDirectory + @"StreamHighlights\" + shl.StreamTitle.ToString() + "_" + shl.streamTime.Date.ToShortDateString() + ".txt", "ProgramMessage");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            System.Environment.Exit(0);
        }
        DateTime dt = shl.GetStreamTimer();
        shl.PrintLineWithColor(dt.Hour+":"+dt.Minute+":"+dt.Second+" - "+s, "Highlights");
        //next, impliment file saving
    }
}

