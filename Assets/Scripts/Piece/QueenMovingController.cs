using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenMovingController : MovingContoller
{

    public override List<Field> getBites(SuperPosition sp)
    {
        List<Field> result = new List<Field>();
        Field field = board.Map(piece.position.x + 1, piece.position.y);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x + 1, field.y);
        }

        field = board.Map(piece.position.x - 1, piece.position.y);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x - 1, field.y);
        }

        field = board.Map(piece.position.x, piece.position.y + 1);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x, field.y + 1);
        }

        field = board.Map(piece.position.x, piece.position.y - 1);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x, field.y - 1);
        }

        field = board.Map(piece.position.x + 1, piece.position.y + 1);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x + 1, field.y + 1);
        }

        field = board.Map(piece.position.x + 1, piece.position.y - 1);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x + 1, field.y - 1);
        }

        field = board.Map(piece.position.x - 1, piece.position.y + 1);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x - 1, field.y + 1);
        }

        field = board.Map(piece.position.x - 1, piece.position.y - 1);
        while (canMove(sp, field))
        {
            result.Add(field);
            if (occupied(sp, field))
                break;
            field = board.Map(field.x - 1, field.y - 1);
        }

        return result;
    }

    protected override List<Field> getTransfers(SuperPosition sp)
    {
        List<Field> result = new List<Field>();

        result = getBites(sp);

        return result;
    }
}
