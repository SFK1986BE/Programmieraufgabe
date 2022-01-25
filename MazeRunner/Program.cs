
class CMain
{
    private static String GenerateDemoMazes()
    {
        String InputStr = "";
        InputStr += "     3 4 5\n" +
                    "S....\n" +
                    ".###.\n" +
                    ".##..\n" +
                    "###.#\n" +
                    "     \n" +
                    "#####\n" +
                    "#####\n" +
                    "##.##\n" +
                    "##...\n" +
                    "     \n" +
                    "#####\n" +
                    "#####\n" +
                    "#.###\n" +
                    "####E";

        InputStr += "\n\n1 3 3\n" +
                    "S##\n" +
                    "#E#\n" +
                    "###";

        InputStr += "\n\n1 7 8\n"+
                    "S..#####" +
                    "##.#####" +
                    "##......" +
                    "##.#.##." +
                    "##.#.##." +
                    "##.#.##E" +
                    "##...###";
        InputStr += "\n\n0 0 0";
        return InputStr;
    } /*GenerateDemoMazes*/


    private static bool ReadCmdParameter(String[] args, ref String InputStr)
    {
        InputStr = "";
        if (args.Length != 0)
        {
            if (args[0].Equals("demo"))
            {
                InputStr = GenerateDemoMazes();
                return true;
            }
            if (args[0].Equals("maze"))
            {
                for (int i = 1; i < args.Length; i++)
                {
                    InputStr += args[i] + ' ';
                }
                return true;
            }
        }
        return false;
    } /*ReadCmdParameter*/


    private static String ReadMazeFromKeyboard()
    {
        String InputStr = "";

        Console.WriteLine("Bitte geben Sie die zu lösenden Labyrinthe ein.");
        Console.WriteLine("Beginnen Sie jedes Labyrinth mit der Angabe seiner Größe.");
        Console.WriteLine("Die Größe wird wie folgt angegeben: L R C");
        Console.WriteLine("  L für die Anzahl der Ebenen.");
        Console.WriteLine("  R ist die Länge des Labyrinths.");
        Console.WriteLine("  C ist die Breite des Labyrinths.");
        Console.WriteLine("");
        Console.WriteLine("Für den Startpunkt geben Sie ein \"S\" und für den Endpunkt eine \"E\"ein.");
        Console.WriteLine("Für eine Wand geben Sie ein \"#\" und für eine begehbare Fläche ein \".\" ein.");
        Console.WriteLine("Der Abenteurer kann nach Norden, Osten, Süden, Westen, Hoch und Runter gehen.");
        Console.WriteLine("");
        Console.WriteLine("Beenden Sie die Eingabe, indem sie die Größe \"0 0 0\" eingeben.");
        Console.WriteLine("");

        String? Input = "";
        do
        {
            try
            {
                if ((Input = Console.ReadLine()) != null)
                {
                    InputStr += Input + ' ';
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (Input != "0 0 0");


        return InputStr;
    } /*ReadMazeFromKeyboard*/


    private static void RunMazes(String InputStr)
    {
        List<CMaze> Mazes = CInputParser.ConvertStringToMazes(InputStr);

        foreach (CMaze aMaze in Mazes)
        {
            CMazeRunner MazeRunner = new(aMaze);

            if (MazeRunner.EscapeMaze(out int Steps))
            {
                Console.WriteLine("Entkommen in " + Steps.ToString() + " Minute(n)!");
            }
            else
            {
                Console.WriteLine("Gefangen :-(");
            }
        }
    } /*RunMazes*/


    public static void Main(String[] args)
    {
        String InputStr = "";
        if (!ReadCmdParameter(args, ref InputStr))
        {
            InputStr = ReadMazeFromKeyboard();
        }

        RunMazes(InputStr);
    } /*Main*/
} /*CMain*/