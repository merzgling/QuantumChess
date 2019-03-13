using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleLimitePiecesMoves : Rule
{
    public override List<Field> onGettingMoves(Piece piece, List<Field> inFields)
    {
        List<Field> result = new List<Field>();
        Piece king = null;
        foreach (Piece p in board.piece)
        {
            if (p.figureName == "King" && p.color == piece.color)
                king = p;
        }

        foreach (Field f in inFields)
        {
            bool moveAvailable = true;
            Field piecePositionPre = piece.position;
            Piece pieceStayOnField = f.piece;

            //////////////////

            piece.position.piece = null;
            f.piece = piece;
            piece.position = f;

            foreach (Piece p in board.piece)
            {
                if (Game.Diplomate.get_state(p.color, piece.color) == DiplomateState.Enemy && p != pieceStayOnField)
                {
                    if (king)
                    if (p.movingController.getBites().Contains(king.position))
                    {
                        moveAvailable = false;
                        break;
                    }
                }
            }

            //////////////////

            piece.position = piecePositionPre;
            piecePositionPre.piece = piece;
            f.piece = pieceStayOnField;
            if (moveAvailable)
                result.Add(f);
        }

        return result;
    }
}