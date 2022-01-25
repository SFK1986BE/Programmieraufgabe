/*********************************************************************
 *                        Datei      : MazeRunner.cs                 *
 *                        Author     : Sebastian Kuhnert             *
 *                        Erstellt am: 21.01.2022                    *
 *                        Geändert am: 25.01.2022                    *
 *********************************************************************/

class CMazeRunner
{
    private readonly CMaze Maze;
    private bool escaped;

    public CMazeRunner(CMaze aMaze)
    {
        this.Maze = aMaze;
    } /*CMazeRunner*/

    private SMazeIndex GoNorth(SMazeIndex Pos)
    {
        SMazeIndex NewPos = Pos;
        NewPos.r--;
        return NewPos;
    }

    private SMazeIndex GoSouth(SMazeIndex Pos)
    {
        SMazeIndex NewPos = Pos;
        NewPos.r++;
        return NewPos;
    }

    private SMazeIndex GoWest(SMazeIndex Pos)
    {
        SMazeIndex NewPos = Pos;
        NewPos.c--;
        return NewPos;
    }

    private SMazeIndex GoEast(SMazeIndex Pos)
    {
        SMazeIndex NewPos = Pos;
        NewPos.c++;
        return NewPos;
    }

    private SMazeIndex GoUp(SMazeIndex Pos)
    {
        SMazeIndex NewPos = Pos;
        NewPos.l--;
        return NewPos;
    }

    private SMazeIndex GoDown(SMazeIndex Pos)
    {
        SMazeIndex NewPos = Pos;
        NewPos.l++;
        return NewPos;
    }

    private int MazeRun(SMazeIndex Pos, int Steps = 0)
    {
        String PosibleDirect = Maze.PossibleDirections(Pos, Steps);
        int[] DirectSteps = { -1, -1, -1, -1, -1, -1 };
        int NewSteps = -1;

        if (Maze.GetBlock(Pos).blockType == 'E')
        {
            escaped = true;
            return Steps;
        }

        Maze.SetBlockMilestone(Pos, Steps);

        Steps++;
        if (PosibleDirect.Contains('N')) DirectSteps[0] = MazeRun(GoNorth(Pos), Steps);
        if (PosibleDirect.Contains('S')) DirectSteps[1] = MazeRun(GoSouth(Pos), Steps);
        if (PosibleDirect.Contains('W')) DirectSteps[2] = MazeRun(GoWest (Pos), Steps);
        if (PosibleDirect.Contains('E')) DirectSteps[3] = MazeRun(GoEast (Pos), Steps);
        if (PosibleDirect.Contains('U')) DirectSteps[4] = MazeRun(GoUp   (Pos), Steps);
        if (PosibleDirect.Contains('D')) DirectSteps[5] = MazeRun(GoDown (Pos), Steps);

        foreach (int i in DirectSteps)
        {
            if (i > -1)
            {
                if (NewSteps == -1 || NewSteps > i) NewSteps = i;
            }
        }

        return NewSteps;
    } /*MazeRun*/

    public bool EscapeMaze(out int Steps)
    {
        escaped = false;
        Steps = MazeRun(Maze.Start);
        return escaped;
    } /*EscapeMaze*/
} /*class CMazeRunner*/