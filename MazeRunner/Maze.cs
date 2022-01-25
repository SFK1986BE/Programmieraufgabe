/*********************************************************************
 *                        Datei      : Maze.cs                       *
 *                        Author     : Sebastian Kuhnert             *
 *                        Erstellt am: 21.01.2022                    *
 *                        Geändert am: 25.01.2022                    *
 *********************************************************************/

struct SMazeBlock
{
    public char blockType;
    public int milestone;
}



struct SMazeIndex
{
    public byte l, r, c;
}



class CMaze
{
    private readonly SMazeBlock[,,] maze;
    private readonly byte size_L;
    private readonly byte size_R;
    private readonly byte size_C;
    private readonly char[] BlockCharSet = { 'S', '.', '#', 'E' };
    public SMazeIndex Start;
    public SMazeIndex End;



    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //                       Private Methoden
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    /*
        Prüft, ob ein eingegebenes Zeichen den Startpunkt (Zeichen S)
        entspricht. Wenn dem so ist, wird die Koordinate des Startpunktes
        gespeichert. (Hilfsmethode für Methode "StringToMaze")
    */
    private void DetectStart(byte l, byte r, byte c, char blockType)
    {
        if (blockType != 'S') return;
        Start.l = l;
        Start.r = r;
        Start.c = c;
    } /*DetectStart*/


    /*
        Prüft, ob ein eingegebenes Zeichen den Endpunkt (Zeichen E)
        entspricht. Wenn dem so ist, wird die Koordinate des Endpunktes
        gespeichert. (Hilfsmethode für Methode "StringToMaze")
    */
    private void DetectEnd(byte l, byte r, byte c, char blockType)
    {
        if (blockType != 'E') return;
        End.l = l;
        End.r = r;
        End.c = c;
    } /*DetectEnd*/


    /*
        Berechnet anhand der Mazegröße denn nächsten Index (die nächste Koordinate)
        für das Einlesen einer Maze aus einem String. (Hilfsmethode für Methode 
        "StringToMaze") 
    */
    private void ComputeNextMazeIndex(ref byte l, ref byte r, ref byte c)
    {
        c++;
        if (c >= size_C)
        {
            c = 0;
            r++;
            if (r >= size_R)
            {
                r = 0;
                l++;
                if (l > size_L) throw new IndexOutOfRangeException();
            }
        }
    } /*ComputeNextMazeIndex*/



    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //                       Öffentliche Methoden
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public CMaze(byte L, byte R, byte C)
    {
        if (L > 30) throw new IndexOutOfRangeException();
        if (R > 30) throw new IndexOutOfRangeException();
        if (C > 30) throw new IndexOutOfRangeException();

        maze = new SMazeBlock[L, R, C];
        size_L = L;
        size_R = R;
        size_C = C;

        ClearMaze();
    } /*CMaze*/


    /*
        Löscht die Mazedaten und belegt alle Felder der Maze
        mit dem Wandsymbol. Start- und Endpunkt werden auf
        die Null-Koordinate der Maze gelegt.
    */
    public void ClearMaze()
    {
        for (byte l = 0; l < size_L; l++)
        {
            for (byte r = 0; r < size_R; r++)
            {
                for (byte c = 0; c < size_C; c++)
                {
                    maze[l, r, c].blockType = '#';
                    maze[l, r, c].milestone = -1;
                }
            }
        }

        Start.l = 0;
        Start.r = 0;
        Start.c = 0;

        End.l = 0;
        End.r = 0;
        End.c = 0;
    } /*ClearMaze*/


    /*
        Wandselt einen String in die Maze-Datenstruktur um.
        Der String muss die zur Mazegröße passende Länge haben
        Es sind folgende Zeichen zulässig: S, #, . und E. Wobei
        S für den Startpunt, E für den Endpunkt, # für eine Wand
        und . für eine begehbare Fläche stehen. Alle anderen 
        Zeichen werden ignoriert.
    */
    public void StringToMaze(String MazeStr)
    {
        byte l = 0, r = 0, c = 0;
        foreach (char BlockType in MazeStr)
        {
            if (BlockCharSet.Contains(BlockType))
            {
                maze[l, r, c].blockType = BlockType;
                maze[l, r, c].milestone = -1;        // -1: Block wurde nicht betreten.

                DetectStart(l, r, c, BlockType);
                DetectEnd  (l, r, c, BlockType);

                ComputeNextMazeIndex(ref l, ref r, ref c);
            }
        }
    } /*StringToMaze*/


    public SMazeBlock GetBlock(byte l, byte r, byte c)
    {
        return maze[l, r, c];
    } /*GetBlock*/

    public SMazeBlock GetBlock(SMazeIndex Ind)
    {
        return GetBlock(Ind.l, Ind.r, Ind.c);
    } /*GetBlock*/

    public void SetBlockMilestone(byte l, byte r, byte c, int newMilestone)
    {
        maze[l, r, c].milestone = newMilestone;
    } /*SetBlockMilestone*/

    public void SetBlockMilestone(SMazeIndex Ind, int newMilestone)
    {
        SetBlockMilestone(Ind.l, Ind.r, Ind.c, newMilestone);
    } /*SetBlockMilestone*/


    /*
        Gibt true zurück, wenn ein gegebener Block (gegeben durch seiner 
        Koordinate) begehbar ist. Andernfalls wird false zurückgegeben.
        Zudem wird geprüft, ob der Block schon mal betreten wurde. Wenn
        der Block schon betreten wurde, wird geprüft, ob ein erneutes
        betreten (Finden einer Wegalternative bei z.B. Kreisen) zu einem
        besseren Ergebnis führen kann. Wenn nicht, wird ebenfalls false
        zurückgegeben.
    */
    public bool IsBlockWalkable(byte l, byte r, byte c, int Steps)
    {
        if (l >= size_L) return false;
        if (r >= size_R) return false;
        if (c >= size_C) return false;

        int Mile = GetBlock(l, r, c).milestone;
        if (Mile <= Steps && Mile >= 0) return false;

        char BlockType = GetBlock(l, r, c).blockType;
        return BlockType == '.' || BlockType == 'S' || BlockType == 'E';
    } /*IsBlockWalkable*/


    /*
        Gibt die Möglichen Richtungen zurück, die von einer gegebenen Koordinate
        aus möglich sind. Die Richtungen werden als String zurückgegeben, wobei
        jedes Zeichen für eine Richtung steht. W steht für Westen, E für Osten,
        N für Norden, S für Süden, U für Oben und D für Unten.
    */
    public String PossibleDirections(byte l, byte r, byte c, int Steps)
    {
        String Direction = "";
        if (IsBlockWalkable(l, r, (byte)(c - 1), Steps)) Direction += 'W';
        if (IsBlockWalkable(l, r, (byte)(c + 1), Steps)) Direction += 'E';
        if (IsBlockWalkable(l, (byte)(r - 1), c, Steps)) Direction += 'N';
        if (IsBlockWalkable(l, (byte)(r + 1), c, Steps)) Direction += 'S';
        if (IsBlockWalkable((byte)(l - 1), r, c, Steps)) Direction += 'U';
        if (IsBlockWalkable((byte)(l + 1), r, c, Steps)) Direction += 'D';

        return Direction;
    } /*PossibleDirections*/

    public String PossibleDirections(SMazeIndex Pos, int Steps)
    {
        return PossibleDirections(Pos.l, Pos.r, Pos.c, Steps);
    } /*PossibleDirections*/


    public void PrintMaze()
    {
        for (int l = 0; l < size_L; l++)
        {
            for (int r = 0; r < size_R; r++)
            {
                for (int c = 0; c < size_C; c++)
                {
                    Console.Write(maze[l, r, c].blockType);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    } /*PrintMaze*/
} /*class CMaze*/