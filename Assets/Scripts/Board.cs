using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Board : MonoBehaviour
{
    protected int currentTurn;

    public Vector2 x_component = new Vector2(1, 0);
    public Vector2 y_component = new Vector2(0, 1);
    
    public GameOption GameOption;

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
                fieldsParent.GetChild(i).GetComponent<Field>().x = (int)(fieldsParent.GetChild(i).localPosition.x + 0.5);
                fieldsParent.GetChild(i).GetComponent<Field>().y = (int)(fieldsParent.GetChild(i).localPosition.z + 0.5);
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

        GameOption = GameObject.Find("Game option").GetComponent<GameOption>();

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

    void AddNewPieceOnBoard(Piece piece, Field field)
    {
        piece.position = field;
        field.piece = piece;
        piece.transform.position = field.transform.position;
        this.piece.Add(piece);
    }

    void RemovePieceFromBoard(Piece piece)
    {
        if (piece.position)
            piece.position = null;
        this.piece.Remove(piece);
        Destroy(piece.gameObject);
    }

    void SetProbabilityes()
    {
        Dictionary<Piece, int> probability = new Dictionary<Piece, int>();
        foreach (var p in piece)
        {
            probability.Add(p, 0);
        }

        foreach (var sp in superPosition)
        {
            foreach (var st in sp.state)
            {
                probability[st.Value]++;
            }
        }

        foreach (var prob in probability)
        {
            if (prob.Value == 0)
                RemovePieceFromBoard(prob.Key);
            else
            {
                float value = (float)prob.Value / superPosition.Count;
                prob.Key.probability = value;
                prob.Key.probabilityBar.SetProbability(value);
            }
        }
    }

    void ActionAfterMove()
    {
        Dictionary<Field, Piece> occupiedFields = new Dictionary<Field, Piece>();
        foreach (var f in field)
        {
            occupiedFields.Add(f, f.piece);
        }

        var conflictSuperPositions1 = new List<SuperPosition>();
        var conflictSuperPositions2 = new List<SuperPosition>();
        Field conflictField = null;
        
        foreach (var sp in superPosition)
        {
            foreach (var st in sp.state)
            {
                if (st.Value != occupiedFields[st.Key])
                {
                    conflictField = st.Key;
                    break;
                }
            }

            if (conflictField != null)
                break;
        }

        if (conflictField != null)
        {
            foreach (var sp in superPosition)
            {
                if (sp.GetPiece(conflictField) != null)
                    if (sp.GetPiece(conflictField) != occupiedFields[conflictField])
                        conflictSuperPositions2.Add(sp);
                    else
                        conflictSuperPositions1.Add(sp);
                    
            }
            
            float t = (float)conflictSuperPositions1.Count / (conflictSuperPositions1.Count + conflictSuperPositions2.Count);
            if (Random.value >= t)
            {
                foreach (var sp in conflictSuperPositions1)
                    superPosition.Remove(sp);
                RemovePieceFromBoard(conflictSuperPositions1[0].GetPiece(conflictField));
            }
            else
            {
                foreach (var sp in conflictSuperPositions2)
                    superPosition.Remove(sp);
                RemovePieceFromBoard(conflictSuperPositions2[0].GetPiece(conflictField));
            }

        }

        SetProbabilityes();
    }
   
    public bool CommonMoveRequest(Piece piece, Field field)
    {
        List<SuperPosition> toMove = new List<SuperPosition>();
        Field piecePosition = piece.position;
        foreach(var sp in superPosition)
        {
            if (sp.GetPiece(piecePosition) != null)
                if (field.piece)
                {
                    if (sp.GetPiece(piecePosition).movingController.IsBittingCorrect(sp, field))
                        toMove.Add(sp);
                }
                else
                {
                    if (sp.GetPiece(piecePosition).movingController.IsMovingCorrect(sp, field))
                        toMove.Add(sp);
                }

        }
        
        if (toMove.Count == superPosition.Count)
        {
            foreach (var sp in toMove)
            {
                sp.DoMovement(piecePosition, field);
            }
            moveFigure(piece, field);
        }
        else
        {
            if (toMove.Count > 0)
            {
                Piece newPiece = Instantiate(piece);
                foreach(var sp in toMove)
                {
                    sp.emptyField(piecePosition);
                    sp.StandPiece(newPiece, field);
                }

                AddNewPieceOnBoard(newPiece, field);
            }
            else
                return false;
        }

        ActionAfterMove();
        return true;
    }

    public bool QuantumMoveRequest(Piece piece, Field field1, Field field2)
    {
        List<SuperPosition> newSuperPositions = new List<SuperPosition>();
        foreach (var sp in superPosition)
        {
            newSuperPositions.Add(sp.Clone());
        }
        
        foreach (var sp in newSuperPositions)
        {
            superPosition.Add(sp);
        }
        
        // first move
        List<SuperPosition> toMove = new List<SuperPosition>();
        Field piecePosition = piece.position;
        
        foreach(var sp in newSuperPositions)
        {
            if (sp.GetPiece(piecePosition) != null)
            {
                if (sp.state[piecePosition].movingController.IsMovingCorrect(sp, field1) &&
                    !sp.state[piecePosition].movingController.occupied(sp, field1))
                    toMove.Add(sp);
            }
        }
        
        if (toMove.Count > 0)
        {
            Piece newPiece = Instantiate(piece);
            foreach(var sp in toMove)
            {
                sp.emptyField(piecePosition);
                sp.StandPiece(newPiece, field1);
            }

            AddNewPieceOnBoard(newPiece, field1);
        }
        else
            return false;

        ActionAfterMove();
        return true;
    }
}
