using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]
public abstract class Piece : MonoBehaviour
{
	[SerializeField] private MaterialSetter materialSetter;
	[SerializeField] private Transform transform;



	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public bool hasMoved { get; private set; }
	public List<Vector2Int> avaliableMoves;
	public Vector3 pos  { get; set; }
	public Vector2Int pos2 { get; set; }
	public Vector2Int pos3  { get; set; }
	
	
	private IObjectTweener tweener;

	public bool anim { get;  set; }
	public bool ded { get;  set; }

	public float turnSpeed = .01f;
	Quaternion rot;
	Vector3 direction;


	Transform transform2;


	public abstract List<Vector2Int> SelectAvaliableSquares();

	void Start()
    {
		pos = transform.position;
		pos3 = new Vector2Int((int)pos.x, (int)pos.z);

		//transform2 = transform;
	}

	void Update()
	{
		pos = transform.position;

		
	
		pos2 = new Vector2Int((int)pos.x , (int)pos.z);



		if (pos3 != pos2)
		{
			anim = true;
		}
		else
		{
			anim = false;
		}


		//pos3 = new Vector2Int((int)targetPosition.x, (int)targetPosition.z); 
		
		//transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed);


	}

	private void Awake()
	{
		avaliableMoves = new List<Vector2Int>();
		tweener = GetComponent<IObjectTweener>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
	}

	public void SetMaterial(Material selectedMaterial)
	{
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

	public bool IsFromSameTeam(Piece piece)
	{
		return team == piece.team;
	}

	public bool CanMoveTo(Vector2Int coords)
	{
		return avaliableMoves.Contains(coords);
	}

	public virtual void MovePiece(Vector2Int coords)
	{
		Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
		occupiedSquare = coords;



		transform.LookAt(targetPosition);
		

		//var step = 3 * Time.deltaTime;
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);

		//transform2.position = targetPosition;
		//direction = (transform2.position - transform.position).normalized;
		//rot = Quaternion.LookRotation(direction);

		pos3 = new Vector2Int((int)targetPosition.x, (int)targetPosition.z);
		hasMoved = true;
		tweener.MoveTo(transform, targetPosition);
		
	}


	protected void TryToAddMove(Vector2Int coords)
	{
		avaliableMoves.Add(coords);
	}

	public void SetData(Vector2Int coords, TeamColor team, Board board)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		transform.position = board.CalculatePositionFromCoords(coords);
	}

	public bool IsAttackingPieceOfType<T>() where T : Piece
	{
		foreach (var square in avaliableMoves)
		{
			if (board.GetPieceOnSquare(square) is T)
				return true;
		}
		return false;
	}

}