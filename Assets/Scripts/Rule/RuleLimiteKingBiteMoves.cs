using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleLimiteKingBiteMoves : Rule
{
    public override List<Field> onGettingMoves(Piece piece, List<Field> inFields)
    {
        if (piece.figureName == "King")
        {
            List<Field> result = new List<Field>();
            foreach (Field f in inFields)
                if (!piece.movingController.isBitten(board.superPosition[0], f))
                    result.Add(f);
            return result;
        }
        else
            return inFields;
    }
}
