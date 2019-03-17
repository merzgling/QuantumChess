using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    protected int currentTurn;

    public Vector2 x_component = new Vector2(1, 0);
    public Vector2 y_component = new Vector2(0, 1);

    public List<Field> field;
    public List<Piece> piece;
    public List<Rule> rule;
    private List<SuperPosition> superPosition = new List<SuperPosition>();

    protected Field[][] map;
    protected int width = 0;
    protected int height = 0;
    public Field Map(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return map[x][y];
        else return null;
    }

    void initialize()
    {
        Transform fieldsParent = GameObject.Find("Fields").transform;
        for (int i = 0; i < fieldsParent.childCount; i++)
        {
            if (fieldsParent.GetChild(i).GetComponent<Field>())
            {
                field.Add(fieldsParent.GetChild(i).GetComponent<Field>());
            }
        }

        foreach (Field f in field)
        {
            width = Mathf.Max(width, f.x + 1);
            height = Mathf.Max(height, f.y + 1);
        }

        map = new Field[width][];
        for (int i = 0; i < width; i++)
            map[i] = new Field[height];

        foreach (Field f in field)
        {
            map[f.x][f.y] = f;
        }

        Transform t = GameObject.Find("Rules").transform;
        for (int i = 0; i < t.childCount; i++)
        {
            rule.Add(t.GetChild(i).gameObject.GetComponent<Rule>());
        }

        t = GameObject.Find("Pieces").transform;
        for (int i = 0; i < t.childCount; i++)
        {
            piece.Add(t.GetChild(i).gameObject.GetComponent<Piece>());
        }
    }

    void Awake()
    {
        initialize();
    }

    public bool moveRequest(Piece figure, Field fieldTo)
    {
        List<Field> FieldsFigureCanMove = figure.movingController.getMoves();
        if (FieldsFigureCanMove.Contains(fieldTo))
        {
            moveFigure(figure, fieldTo);
            return true;
        }
        return false;
    }

    public void moveFigure(Piece piece, Field fieldTo)
    {
        if (piece == null || fieldTo == null)
            return;

        Field fieldFrom = piece.position;
        foreach (Rule r in rule)
            r.beforeMoving(piece, fieldTo);
        piece.position.piece = null;
        piece.position = fieldTo;
        if (fieldTo.piece)
            eatPiece(fieldTo.piece);
        fieldTo.piece = piece;
        piece.transform.position = fieldTo.transform.position;
        piece.movingController.OnMove();
        foreach (Rule r in rule)
            r.afterMoving(fieldFrom, piece, fieldTo);
    }

    protected void eatPiece(Piece p)
    {
        piece.Remove(p);
        Destroy(p.gameObject);
    }

    private void DoAction(Field f1, Field f2)
    {
        foreach(var s in superPosition)
        {
            
        }
    }
}
