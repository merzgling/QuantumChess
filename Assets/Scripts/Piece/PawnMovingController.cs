using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Left, Down, Right }

public class PawnMovingController : MovingContoller
{
    public Direction direction;
    [SerializeField]
    protected bool canDoubleMove;

    public override List<Field> getBites()
    {
        List<Field> result = new List<Field>();
        Field fieldBite1 = null;
        Field fieldBite2 = null;

        if (direction == Direction.Up)
        {
            fieldBite1 = board.Map(piece.position.x - 1, piece.position.y + 1);
            fieldBite2 = board.Map(piece.position.x + 1, piece.position.y + 1);
        }
        if (direction == Direction.Left)
        {
            fieldBite1 = board.Map(piece.position.x - 1, piece.position.y - 1);
            fieldBite2 = board.Map(piece.position.x - 1, piece.position.y + 1);
        }
        if (direction == Direction.Down)
        {
            fieldBite1 = board.Map(piece.position.x - 1, piece.position.y - 1);
            fieldBite2 = board.Map(piece.position.x + 1, piece.position.y - 1);

        }
        if (direction == Direction.Right)
        {
            fieldBite1 = board.Map(piece.position.x + 1, piece.position.y - 1);
            fieldBite2 = board.Map(piece.position.x + 1, piece.position.y + 1);

        }

        result.Add(fieldBite1);
        result.Add(fieldBite2);

        return result;
    }

    protected override List<Field> getTransfers()
    {
        List<Field> result = new List<Field>();
        Field fieldMove = null;
        Field fieldDoubleMove = null;
        if (direction == Direction.Up)
        {
            fieldMove = board.Map(piece.position.x, piece.position.y + 1);
            fieldDoubleMove = board.Map(piece.position.x, piece.position.y + 2);
        }
        if (direction == Direction.Left)
        {
            fieldMove = board.Map(piece.position.x - 1, piece.position.y);
            fieldDoubleMove = board.Map(piece.position.x - 2, piece.position.y);
        }
        if (direction == Direction.Down)
        {
            fieldMove = board.Map(piece.position.x, piece.position.y - 1);
            fieldDoubleMove = board.Map(piece.position.x, piece.position.y - 2);

        }
        if (direction == Direction.Right)
        {
            fieldMove = board.Map(piece.position.x + 1, piece.position.y);
            fieldDoubleMove = board.Map(piece.position.x + 2, piece.position.y);

        }

        
        if (!occupied(fieldMove) && fieldMove)
        {
            result.Add(fieldMove);
        }

        if (canDoubleMove && !occupied(fieldDoubleMove) && !occupied(fieldMove) && fieldMove && fieldDoubleMove)
        {
            result.Add(fieldDoubleMove);
        }
        List<Field> biteFields = getBites();

        if (canBite(biteFields[0]))
        {
            result.Add(biteFields[0]);
        }

        if (canBite(biteFields[1]))
        {
            result.Add(biteFields[1]);
        }

        return result;
    }

    public override void OnMove()
    {
        base.OnMove();
        canDoubleMove = false;
    }
}
