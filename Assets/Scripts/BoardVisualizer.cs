using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardVisualizer : MonoBehaviour
{
	[SerializeField] GameObject oncePickedVisualizerTemplate;
	[SerializeField] GameObject doublePickedVisualizerTemplate;
	[SerializeField] GameObject FieldPickedVisualizerTemplate;

	[SerializeField] GameObject canMoveFieldTemplate;
	[SerializeField] GameObject canBiteFieldTemplate;
	
	private Piece PickedPiece;
	private GameObject PickedFieldVisualizer;
	private GameObject visualizer;
	private List<GameObject> visualizedFields = new List<GameObject>();
	
	public void SetPickedPiece(Piece piece)
	{
		if (visualizer)
			Destroy(visualizer);
		
		visualizer = Instantiate(oncePickedVisualizerTemplate);
		visualizer.transform.position = piece.position.transform.position;
		visualizer.transform.parent = piece.position.transform;
		PickedPiece = piece;

		if (!Game.GameOption.IsQuantum)
		{
			unVisualFIelds();
			visualFields();
		}
	}

	public void SetDoublePickedPiece(Piece piece)
	{
		if (visualizer)
			Destroy(visualizer);
		
		visualizer = Instantiate(doublePickedVisualizerTemplate);
		visualizer.transform.position = piece.position.transform.position;
		visualizer.transform.parent = piece.position.transform;
		PickedPiece = piece;
	}

	public void SetPickedField(GameObject field)
	{
		PickedFieldVisualizer = Instantiate(FieldPickedVisualizerTemplate);
		PickedFieldVisualizer.transform.position = new Vector3(field.transform.position.x, PickedFieldVisualizer.transform.position.y,  field.transform.position.z);
	}

	public void UnPickPiece()
	{
		if (visualizer)
			Destroy(visualizer);
		if (PickedFieldVisualizer)
			Destroy(PickedFieldVisualizer);
		unVisualFIelds();
		PickedPiece = null;
	}
	
	void visualFields()
	{
		var moves = PickedPiece.movingController.getMoves(Game.Board.superPosition[0]);
		foreach (var move in moves)
		{
			if (move.piece == null)
			{
				GameObject field = Instantiate(canMoveFieldTemplate);
				field.transform.position = move.transform.position + new Vector3(0, 0.105f);
				visualizedFields.Add(field);
			}
			else
				if (Game.Diplomate.get_state(move.piece.color, PickedPiece.color) == DiplomateState.Enemy)
				{
					GameObject field = Instantiate(canBiteFieldTemplate);
					field.transform.position = move.transform.position + new Vector3(0, 0.105f);
					visualizedFields.Add(field);
				}
		}
	}

	void unVisualFIelds()
	{
		foreach (var field in visualizedFields)
		{
			Destroy(field);
		}

		visualizedFields.Clear();
	}
}
