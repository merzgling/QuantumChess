using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovingController : MovingContoller
{
    public override List<Field> getBites()
    {
        List<Field> result = new List<Field>();
        Field field = board.Map(piece.position.x + 1, piece.position.y + 2);
        if (field)
            result.Add(field);
        field = board.Map(piece.position.x + 2, piece.position.y + 1);
        if (field)
            result.Add(field);
        field = board.Map(piece.position.x + 1, piece.position.y - 2);
        if (field)
            result.Add(field);
        field = board.Map(piece.position.x + 2, piece.position.y - 1);
        if (field)
            result.Add(field);
        field = board.Map(piece.position.x - 1, piece.position.y + 2);
        if (field)
            result.Add(field);
        field = board.Map(piece.position.x - 2, piece.position.y + 1);
        if (field)
            result.Add(field);
        field = board.Map(piece.position.x - 1, piece.position.y - 2);
        if (field)
            result.Add(field);
        field = board.Map(piece.position.x - 2, piece.position.y - 1);
        if (field)
            result.Add(field);

        return result;
    }

    protected override List<Field> getTransfers()
    {
        List<Field> result = new List<Field>();

        List<Field> bites = getBites();
        foreach(Field f in bites)
        {
            if (canMove(f))
                result.Add(f);
        }

        return result;
    }
}
