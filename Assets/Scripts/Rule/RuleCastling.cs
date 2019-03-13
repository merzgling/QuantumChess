using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleCastling : Rule
{
    public override void afterMoving(Field from, Piece piece, Field to)
    {
        base.afterMoving(from, piece, to);
        if (piece.figureName == "King")
        {
            if (Geometry.dist(to, from) > 1 && Geometry.stayOnSameLine(from, to))
            {
                Piece rook = null;
                Field rookTo = null;
                Field f = board.Map(to.x, to.y + 1);
                Piece p = null;
                if (f)
                    p = f.piece;
                if (p)
                    if (p.figureName == "Rook" && Geometry.stayOnSameLine(p.position, from))
                    {
                        rook = p;
                        rookTo = board.Map(to.x, to.y - 1);
                    }

                f = board.Map(to.x, to.y - 1);
                p = null;
                if (f)
                    p = f.piece;
                if (p)
                    if (p.figureName == "Rook" && Geometry.stayOnSameLine(p.position, from))
                    {
                        rook = p;
                        rookTo = board.Map(to.x, to.y + 1);
                    }

                f = board.Map(to.x + 1, to.y);
                p = null;
                if (f)
                    p = f.piece;
                if (p)
                    if (p.figureName == "Rook" && Geometry.stayOnSameLine(p.position, from))
                    {
                        rook = p;
                        rookTo = board.Map(to.x - 1, to.y);
                    }

                f = board.Map(to.x - 1, to.y);
                p = null;
                if (f)
                    p = f.piece;
                if (p)
                    if (p.figureName == "Rook" && Geometry.stayOnSameLine(p.position, from))
                    {
                        rook = p;
                        rookTo = board.Map(to.x + 1, to.y);
                    }

                board.moveFigure(rook, rookTo);
            }
        }
    }

    public override List<Field> onGettingMoves(Piece piece, List<Field> inFields)
    {
        List<Field> result = inFields;

        if (piece.figureName == "King")
        {
            if (piece.GetComponent<KingMovingController>().haveMoved == false)
            {
                // Right ////////////////////////////////
                Field f = piece.position;
                f = board.Map(f.x + 1, f.y);
                bool find = false;
                while (f)
                {
                    if (f.piece)
                    {
                        if (f.piece.figureName == "Rook")
                            if (f.piece.GetComponent<RookMovingController>().haveMoved == false)
                                find = true;
                        break;
                    }
                    f = board.Map(f.x + 1, f.y);
                }
                if (find)
                {
                    bool can = true;
                    Field ff = piece.position;
                    while (ff != f)
                    {
                        if (piece.movingController.isBitten(ff))
                            can = false;
                        ff = board.Map(ff.x + 1, ff.y);
                    }

                    if (Geometry.dist(f, piece.position) > 2 && can)
                        result.Add(board.Map(f.x - 1, f.y));
                }

                // Up ////////////////////////////////
                f = piece.position;
                f = board.Map(f.x, f.y + 1);
                find = false;
                while (f)
                {
                    if (f.piece)
                    {
                        if (f.piece.figureName == "Rook")
                            if (f.piece.GetComponent<RookMovingController>().haveMoved == false)
                                find = true;
                        break;
                    }
                    f = board.Map(f.x, f.y + 1);
                }
                if (find)
                {
                    bool can = true;
                    Field ff = piece.position;
                    while (ff != f)
                    {
                        if (piece.movingController.isBitten(ff))
                            can = false;
                        ff = board.Map(ff.x, ff.y + 1);
                    }

                    if (Geometry.dist(f, piece.position) > 2 && can)
                        result.Add(board.Map(f.x, f.y - 1));
                }

                // Left ////////////////////////////////
                f = piece.position;
                f = board.Map(f.x - 1, f.y);
                find = false;
                while (f)
                {
                    if (f.piece)
                    {
                        if (f.piece.figureName == "Rook")
                            if (f.piece.GetComponent<RookMovingController>().haveMoved == false)
                                find = true;
                        break;
                    }
                    f = board.Map(f.x - 1, f.y);
                }
                if (find)
                {
                    bool can = true;
                    Field ff = piece.position;
                    while (ff != f)
                    {
                        if (piece.movingController.isBitten(ff))
                            can = false;
                        ff = board.Map(ff.x - 1, ff.y);
                    }

                    if (Geometry.dist(f, piece.position) > 2 && can)
                        result.Add(board.Map(f.x + 1, f.y));
                }

                // Down ////////////////////////////////
                f = piece.position;
                f = board.Map(f.x, f.y - 1);
                find = false;
                while (f)
                {
                    if (f.piece)
                    {
                        if (f.piece.figureName == "Rook")
                            if (f.piece.GetComponent<RookMovingController>().haveMoved == false)
                                find = true;
                        break;
                    }
                    f = board.Map(f.x, f.y - 1);
                }
                if (find)
                {
                    bool can = true;
                    Field ff = piece.position;
                    while (ff != f)
                    {
                        if (piece.movingController.isBitten(ff))
                            can = false;
                        ff = board.Map(ff.x, ff.y - 1);
                    }

                    if (Geometry.dist(f, piece.position) > 2 && can)
                        result.Add(board.Map(f.x, f.y + 1));
                }
            }
        }

        return result;
    }
}
