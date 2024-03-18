namespace LandsOfAzerith.scripts;

public static class Toolbox
{
    
    /// <summary>
    /// Takes a positive integer in string format and returns it as an integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>-1 if value is not an integer</returns>
    public static int ToInt(string value)
    {
        int result = 0;
        foreach (char c in value)
        {
            if (c is < '0' or > '9')
            {
                return -1;
            }
            result = result * 10 + (c - '0');
        }
        return result;
    }
}