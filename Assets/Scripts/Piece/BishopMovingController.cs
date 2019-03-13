using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovingController : MovingContoller
{
    public override List<Field> getBites()
    {
        List<Field> result = new List<Field>();
        Field field = board.Map(piece.position.x + 1, piece.position.y + 1);
        while (canMove(field))
        {
            result.Add(field);
            if (occupied(field))
                break;
            field = board.Map(field.x + 1, field.y + 1);
        }

        field = board.Map(piece.position.x + 1, piece.position.y - 1);
        while (canMove(field))
        {
            result.Add(field);
            if (occupied(field))
                break;
            field = board.Map(field.x + 1, field.y - 1);
        }

        field = board.Map(piece.position.x - 1, piece.position.y + 1);
        while (canMove(field))
        {
            result.Add(field);
            if (occupied(field))
                break;
            field = board.Map(field.x - 1, field.y + 1);
        }

        field = board.Map(piece.position.x - 1, piece.position.y - 1);
        while (canMove(field))
        {
            result.Add(field);
            if (occupied(field))
                break;
            field = board.Map(field.x - 1, field.y - 1);
        }

        return result;
    }

    protected override List<Field> getTransfers()
    {
        List<Field> result = new List<Field>();

        result = getBites();

        return result;
    }
}
