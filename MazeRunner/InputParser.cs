/*********************************************************************
 *                        Datei      : InputParser.cs                *
 *                        Author     : Sebastian Kuhnert             *
 *                        Erstellt am: 25.01.2022                    *
 *                        Geändert am: 25.01.2022                    *
 *********************************************************************/

class CInputParser
{
    private enum EParseSate
    {
        ReadSize,
        ReadMaze,
        End
    }


    static private String ClearInputString(String InputStr)
    {
        String ValidChars = "SE.# 1234567890";
        String CleanStr = "";
        char lastChar = '\0';

        foreach (char ch in InputStr)
        {
            // Unzulässige Zeichen zu Leerzeichen konvertieren.
            if (!ValidChars.Contains(ch))
            {
                if (lastChar != ' ') CleanStr += ' ';
                lastChar = ' ';
            }

            // Nur zulässige Zeichen kommen in den Ausgabe-String.
            if (ValidChars.Contains(ch))
            {
                if (ch == ' ' && lastChar == ' ') continue;
                CleanStr += ch;
                lastChar = ch;
            }
        }

        return CleanStr.Trim();
    } /*RemoveBlanks*/


    static private EParseSate DoReadSize(char ch, ref byte l, ref byte r, ref byte c, ref String Buffer)
    {
        if (ch == ' ')
        {
            if (l == 255)
            {
                l = Byte.Parse(Buffer);
            }
            else if (r == 255)
            {
                r = Byte.Parse(Buffer);
            }
            else if (c == 255)
            {
                c = Byte.Parse(Buffer);
                Buffer = String.Empty;
                if (l == 0 && r == 0 && c == 0) return EParseSate.End;
                return EParseSate.ReadMaze;
            }
            Buffer = String.Empty;
        }
        else Buffer += ch;
        return EParseSate.ReadSize;
    } /*ReadSize*/


    static private EParseSate DoReadMaze(char ch, ref String MazeData, ref String Buffer)
    {
        if (ch >= '0' && ch <= '9')
        {
            MazeData = Buffer;
            Buffer = String.Empty;
            Buffer += ch;
            return EParseSate.ReadSize;
        }
        else
        {
            Buffer += ch;
            MazeData = String.Empty;
        }
        return EParseSate.ReadMaze;
    } /*doReadMaze*/


    static public List<CMaze> ConvertStringToMazes(String InputStr)
    {
        List<CMaze> Mazes = new();
        CMaze aMaze;
        InputStr = ClearInputString(InputStr);
        EParseSate State = EParseSate.ReadSize;
        byte l = 255, r = 255, c = 255;
        String Buffer = "", MazeData = "";

        foreach (char ch in InputStr)
        {
            switch (State)
            {
                case EParseSate.ReadSize: State = DoReadSize(ch, ref l, ref r, ref c, ref Buffer); break;
                case EParseSate.ReadMaze:
                    State = DoReadMaze(ch, ref MazeData, ref Buffer);
                    if (MazeData != "")
                    {
                        aMaze = new(l, r, c);
                        aMaze.StringToMaze(MazeData);
                        Mazes.Add(aMaze);
                        l = 255;
                        r = 255;
                        c = 255;
                    }
                    break;
                case EParseSate.End: break;
            }
            if (State == EParseSate.End) break;
        }

        return Mazes;
    } /*ConvertStringToMazes*/
} /*CInputParser*/