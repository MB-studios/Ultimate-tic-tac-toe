using UnityEngine;

public abstract class WinnableBoard : MonoBehaviour
{
    public int[] squares;
    protected int CheckWin(int squareNumber)
    {
        int firstInRow = squareNumber - squareNumber % 3;
        int firstInCol = squareNumber % 3;

        if (squares[firstInRow] == squares[firstInRow + 1] && squares[firstInRow] == squares[firstInRow + 2])
        {
            return squares[squareNumber];
        }

        if (squares[firstInCol] == squares[firstInCol + 3] && squares[firstInCol] == squares[firstInCol + 6])
        {
            return squares[squareNumber];
        }

        if (squareNumber == 0 || squareNumber == 4 || squareNumber == 8)
        {
            if (squares[0] == squares[4] && squares[0] == squares[8])
            {
                return squares[squareNumber];
            }
        }

        if (squareNumber == 2 || squareNumber == 4 || squareNumber == 6)
        {
            if (squares[2] == squares[4] && squares[2] == squares[6])
            {
                return squares[squareNumber];
            }
        }

        return 0;
    }

}
