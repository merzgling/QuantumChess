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
    public List<SuperPosition> superPosition = new List<SuperPosition>();

    protected Field[][] map;
    protected int width = 0;
    protected int height = 0;
    public Field Map(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return map[x][y];
        return null;
    }

    void initializeFields()
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
    }

    void initialize()
    {
        initializeFields();

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

        foreach (var p in piece)
        {
            float a = p.transform.localPosition.x, b = p.transform.localPosition.z;
            float a1 = Game.Board.x_component.x, a2 = Game.Board.y_component.x;
            float b1 = Game.Board.x_component.y, b2 = Game.Board.y_component.y;

            float new_x = (a * b2 - a2 * b) / (a1 * b2 - a2 * b1), new_y = (a1 * b - a * b1) / (a1 * b2 - a2 * b1);

            p.position = Game.Board.Map((int)(new_x + 0.5f), (int)(new_y + 0.5f));
            p.position.piece = p;
            p.movingController = p.gameObject.GetComponent<MovingContoller>();
        }
        
        SuperPosition startSp = new SuperPosition();
        foreach (var p in piece)
        {
            startSp.state.Add(p.position, p);
        }
        superPosition.Add(startSp);
    }

    void Awake()
    {
        initialize();
    }

    public bool moveRequest(Piece figure, Field fieldTo)
    {
        List<Field> FieldsFigureCanMove = figure.movingController.getMoves(superPosition[0]);
        if (FieldsFigureCanMove.Contains(fieldTo))
        {
            DoAction(figure.position, fieldTo);
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
        List<SuperPosition> toMove = new List<SuperPosition>();
        foreach(var sp in superPosition)
        {
            if (sp.state[f1] != null)
                if (sp.state[f1].movingController.IsMovingCorrect(sp, f2))
                    toMove.Add(sp);
        }

        if (toMove.Count == superPosition.Count)
        {
            foreach (var sp in toMove)
            {
                sp.DoMovement(f1, f2);
            }
            moveFigure(f1.piece, f2);
        }
        else
        {
            if (toMove.Count > 0)
            {
                Piece newPiece = Instantiate(f1.piece);
                foreach(var sp in toMove)
                {
                    sp.emptyField(f1);
                    sp.StandPiece(newPiece, f2);
                }

                newPiece.position = f2;
                f2.piece = newPiece;
            }
        }
        
    }
}
