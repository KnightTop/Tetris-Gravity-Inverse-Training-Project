using Godot;
using System;
using System.Data;
using System.Globalization;
using System.Linq;

public partial class TileMap : Godot.TileMap
{
	private Vector2I[][] I = new Vector2I[4][]
	{
		new Vector2I[] { new(0, 1), new(1, 1), new(2, 1), new(3, 1) },  // I_0
		new Vector2I[] { new(2, 0), new(2, 1), new(2, 2), new(2, 3) },  // I_90
		new Vector2I[] { new(0, 2), new(1, 2), new(2, 2), new(3, 2) },  // I_180
		new Vector2I[] { new(1, 0), new(1, 1), new(1, 2), new(1, 3) }   // I_270
	};

	// T-shape
	private Vector2I[][] T = new Vector2I[4][]
   {
		new Vector2I[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1) },  // T_0
		new Vector2I[] { new(1, 0), new(1, 1), new(2, 1), new(1, 2) },  // T_90
		new Vector2I[] { new(0, 1), new(1, 1), new(2, 1), new(1, 2) },  // T_180
		new Vector2I[] { new(1, 0), new(0, 1), new(1, 1), new(1, 2) }   // T_270
   };

	// O-shape
	private Vector2I[][] O = new Vector2I[4][]
   {
		new Vector2I[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) },  // O_0
		new Vector2I[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) },  // O_90
		new Vector2I[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) },  // O_180
		new Vector2I[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) }   // O_270
   };

	// Z-shape
	private readonly Vector2I[][] Z = new Vector2I[4][]
	{
		new Vector2I[] { new(0, 0), new(1, 0), new(1, 1), new(2, 1) },  // Z_0
		new Vector2I[] { new(2, 0), new(1, 1), new(2, 1), new(1, 2) },  // Z_90
		new Vector2I[] { new(0, 1), new(1, 1), new(1, 2), new(2, 2) },  // Z_180
		new Vector2I[] { new(1, 0), new(0, 1), new(1, 1), new(0, 2) }   // Z_270
	};
	// S-shape
	private readonly Vector2I[][] S = new Vector2I[4][]
   {
		new Vector2I[] { new(1, 0), new(2, 0), new(0, 1), new(1, 1) },  // S_0
		new Vector2I[] { new(1, 0), new(1, 1), new(2, 1), new(2, 2) },  // S_90
		new Vector2I[] { new(1, 1), new(2, 1), new(0, 2), new(1, 2) },  // S_180
		new Vector2I[] { new(0, 0), new(0, 1), new(1, 1), new(1, 2) }   // S_270
   };
	// L-shape
	private readonly Vector2I[][] L = new Vector2I[4][]
	{
		new Vector2I[] { new(2, 0), new(0, 1), new(1, 1), new(2, 1) },  // L_0
		new Vector2I[] { new(1, 0), new(1, 1), new(1, 2), new(2, 2) },  // L_90
		new Vector2I[] { new(0, 1), new(1, 1), new(2, 1), new(0, 2) },  // L_180
		new Vector2I[] { new(0, 0), new(1, 0), new(1, 1), new(1, 2) }   // L_270
	};
	// J-shape
	private readonly Vector2I[][] J = new Vector2I[4][]
	{
		new Vector2I[] { new(0, 0), new(0, 1), new(1, 1), new(2, 1) },  // J_0
		new Vector2I[] { new(1, 0), new(2, 0), new(1, 1), new(1, 2) },  // J_90
		new Vector2I[] { new(0, 1), new(1, 1), new(2, 1), new(2, 2) },  // J_180
		new Vector2I[] { new(1, 0), new(1, 1), new(0, 2), new(1, 2) }   // J_270
	};
	private readonly Vector2I[][] C = new Vector2I[4][]
	{
		new Vector2I[] { new(0, 0), new(0, 1), new(1, 1), new(2, 1), new(2,0) },  // C_0
		new Vector2I[] { new(1, 0), new(2, 0), new(1, 1), new(1, 2), new(2,2) },  // C_90
		new Vector2I[] { new(0, 1), new(1, 1), new(2, 1), new(2, 2), new(0,2) },  // C_180
		new Vector2I[] { new(1, 0), new(1, 1), new(0, 2), new(1, 2), new(0,0) }   // C_270
	};
	private readonly Vector2I[][] D = new Vector2I[4][]
	{
		new Vector2I[] { new(0, 0), new(1, 0), new(2, 0), new(0, 1), new(2,1),new(0, 2), new(1, 2), new(2, 2) },  // D_0
		new Vector2I[] { new(0, 0), new(1, 0), new(2, 0), new(0, 1), new(2,1),new(0, 2), new(1, 2), new(2, 2) },  // D_90
		new Vector2I[] { new(0, 0), new(1, 0), new(2, 0), new(0, 1), new(2,1),new(0, 2), new(1, 2), new(2, 2) },  // D_180
		new Vector2I[] { new(0, 0), new(1, 0), new(2, 0), new(0, 1), new(2, 1), new(0, 2), new(1, 2), new(2, 2) } // D_270
	};
	private readonly Vector2I[][] X = new Vector2I[4][]
{
		new Vector2I[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1,2) },  // X_0
		new Vector2I[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1,2)},  // X_90
		new Vector2I[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1,2)},  // X_180
		new Vector2I[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1,2) } // X_270
};

	// All shapes
	private Vector2I[][][] AShapesFull = [];
	private Vector2I[][][] AShapes = [];

	private const int COLLUMNS = 10;
	private const int ROWS = 20;

	private Vector2I StartPosition = new Vector2I(5, 1);
	private Vector2I CurrentPosition;
	private Vector2I[] Directions = [Vector2I.Left, Vector2I.Right, Vector2I.Down];
	private float[] Steps;
	private const int STEPSREQUIRED = 50;
	private float Speed;
	private const float ACCELERATION = 0.25f;


	public Vector2I[][] PieceType;
	public Vector2I[][] NextPieceType;
	private int RotationIndex;
	private Vector2I[] ActivePiece;

	private int Score = 0;
	private const int REWARD = 100;
	private bool IsGameRunning = true;

	private int TileId = 0;
	private Vector2I PieceAtlas;
	private Vector2I NextPieceAtlas;

	private int BoardLayer = 0;
	private int ActiveLayer = 1;


	public override void _Ready()
	{
		AShapesFull = AShapesFull.Prepend(I).ToArray();
		AShapesFull = AShapesFull.Append(T).ToArray();
		AShapesFull = AShapesFull.Append(O).ToArray();
		AShapesFull = AShapesFull.Append(S).ToArray();
		AShapesFull = AShapesFull.Append(Z).ToArray();
		AShapesFull = AShapesFull.Append(J).ToArray();
		AShapesFull = AShapesFull.Append(L).ToArray();
		AShapesFull = AShapesFull.Append(C).ToArray();
		AShapesFull = AShapesFull.Append(D).ToArray();
		AShapesFull = AShapesFull.Append(X).ToArray();
		AShapes = AShapesFull.Select(inner => inner.ToArray()).ToArray();
		GetNode<CanvasLayer>("HUD").GetNode<Button>("StartButton").Pressed += NewGame;
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsGameRunning)
		{
			if (Input.IsActionPressed("ui_down"))
			{
				Steps[2] += 10;
			}
			else if (Input.IsActionPressed("ui_left"))
			{
				Steps[0] += 10;
			}
			else if (Input.IsActionPressed("ui_right"))
			{
				Steps[1] += 10;
			}
			else if (Input.IsActionJustPressed("ui_up"))
			{
				RotatePiece();
			}

			Steps[2] += Speed;
			for (int i = 0; i < Steps.Length; i++)
			{
				if (Steps[i] > STEPSREQUIRED)
				{
					MovePiece(Directions[i]);
					Steps[i] = 0;
				}
			}
		}



	}
	public void NewGame()
	{
		IsGameRunning = true;
		Score = 0;
		Speed = 1.0f;
		Steps = [0, 0, 0];
		GetNode<CanvasLayer>("HUD").GetNode<Label>("GameOverLabel").Hide();
		ClearNextPiecePanel();
		ClearBoard();
		if (PieceType != null)
		{
			ClearPiece(PieceType[RotationIndex], CurrentPosition);
		}
		PieceType = PickPiece();
		PieceAtlas = new Vector2I(PickColor(PieceType), 0);
		NextPieceType = PickPiece();
		NextPieceAtlas = new Vector2I(PickColor(NextPieceType), 0);
		CreatePiece();
	}
	private Vector2I[][] PickPiece()
	{
		if (AShapes.Length > 0)
		{
			Random.Shared.Shuffle(AShapes);
			var shape = AShapes.First();
			AShapes = AShapes.Skip(1).ToArray();
			return shape;
		}
		else
		{
			AShapes = AShapesFull.Select(inner => inner.ToArray()).ToArray();
			Random.Shared.Shuffle(AShapes);
			var shape = AShapes.First();
			AShapes = AShapes.Skip(0).ToArray();
			return shape;
		}
	}
	private int PickColor(Vector2I[][] PieceType)
	{
		int ColorNumber = 0;
		for (int i = 0; i < AShapesFull.Length; i++)
		{
			if (PieceType.SequenceEqual(AShapesFull[i])) { ColorNumber = i; };

		}
		return ColorNumber;
	}
	private void CreatePiece()
	{
		Steps = [0, 0, 0];
		CurrentPosition = StartPosition;
		ActivePiece = PieceType[RotationIndex];
		DrawPiece(ActivePiece, CurrentPosition, PieceAtlas);
		DrawPiece(NextPieceType[0], new Vector2I(15, 6), NextPieceAtlas);
	}
	private void MovePiece(Vector2I direction)
	{
		if (CanMove(direction))
		{
			ClearPiece(ActivePiece, CurrentPosition);
			CurrentPosition += direction;
			DrawPiece(ActivePiece, CurrentPosition, PieceAtlas);
		}
		else
		{
			if (direction == Vector2I.Down)
			{
				LandPiece();
				CheckRows();
				PieceType = NextPieceType;
				PieceAtlas = NextPieceAtlas;
				NextPieceType = PickPiece();
				NextPieceAtlas = new Vector2I(PickColor(NextPieceType), 0);
				ClearNextPiecePanel();
				CreatePiece();
				CheckGameOver();
			}
		}

	}
	private void RotatePiece()
	{
		if (CanRotate())
		{
			ClearPiece(ActivePiece, CurrentPosition);
			RotationIndex = (RotationIndex + 1) % 4;
			ActivePiece = PieceType[RotationIndex];
			DrawPiece(ActivePiece, CurrentPosition, PieceAtlas);
		}

	}
	private void LandPiece()
	{
		for (int f = 0; f < ActivePiece.Length; f++)
		{
			EraseCell(ActiveLayer, CurrentPosition + ActivePiece[f]);
			SetCell(BoardLayer, CurrentPosition + ActivePiece[f], TileId, PieceAtlas);
		}
	}
	private bool CanMove(Vector2I dir)
	{
		bool CanMove = true;
		for (int i = 0; i < ActivePiece.Length; i++)
		{
			if (!IsCellFree(ActivePiece[i] + CurrentPosition + dir))
			{
				CanMove = false;
			}
		}
		return CanMove;
	}
	private bool CanRotate()
	{
		var CanRotate = true;
		var TemporaryRotationIndex = (RotationIndex + 1) % 4;
		for (int i = 0; i < PieceType[TemporaryRotationIndex].Length; i++)
		{
			if (!IsCellFree(PieceType[TemporaryRotationIndex][i] + CurrentPosition))
			{
				CanRotate = false;
			}
		}
		return CanRotate;
	}
	private bool IsCellFree(Vector2I Pos)
	{
		var ID = GetCellSourceId(BoardLayer, Pos);
		if (ID == -1)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private void DrawPiece(Vector2I[] Piece, Vector2I Pos, Vector2I Atlas)
	{
		for (int i = 0; i < Piece.Length; i++)
		{
			SetCell(ActiveLayer, Pos + Piece[i], TileId, Atlas);
		}
	}
	private void ClearPiece(Vector2I[] Piece, Vector2I Pos)
	{
		for (int i = 0; i < Piece.Length; i++)
		{
			EraseCell(ActiveLayer, Pos + Piece[i]);
		}
	}
	private void ClearNextPiecePanel()
	{
		for (int x = 14; x < 19; x++)
		{
			for (int y = 5; y < 9; y++)
			{
				EraseCell(ActiveLayer, new Vector2I(x, y));
			}
		}
	}
	private void CheckRows()
	{
		var Row = ROWS;
		while (Row > 0)
		{
			var count = 0;
			for (int f = 0; f < COLLUMNS; f++)
			{
				if (!IsCellFree(new Vector2I(f + 1, Row)))
				{
					count++;
				}
			}
			if (count == COLLUMNS)
			{
				ShiftRows(Row);
				Score += REWARD;
				GetNode<CanvasLayer>("HUD").GetNode<Label>("ScoreLabel").Text = "Score: " + Score;
				Speed += ACCELERATION;
			}
			else
			{
				Row--;
			}
		}
	}
	private void ShiftRows(int Row)
	{
		var Atlas = new Vector2I();
		for (int y = Row; y > 1; y--)
		{
			for (int x = 0; x < COLLUMNS; x++)
			{
				Atlas = GetCellAtlasCoords(BoardLayer, new Vector2I(x + 1, y - 1));
				if (Atlas == new Vector2I(-1, -1))
				{
					EraseCell(BoardLayer, new Vector2I(x + 1, y));
				}
				else
				{
					SetCell(BoardLayer, new Vector2I(x + 1, y), TileId, Atlas);
				}
			}
		}

	}
	private void ClearBoard()
	{
		for (int y = 0; y < ROWS; y++)
		{
			for (int x = 0; x < COLLUMNS; x++)
			{
				EraseCell(BoardLayer, new Vector2I(x + 1, y + 1));
			}
		}
	}
	private void CheckGameOver()
	{
		for (int f = 0; f < ActivePiece.Length; f++)
		{
			if (!IsCellFree(CurrentPosition + ActivePiece[f]))
			{
				LandPiece();
				GetNode<CanvasLayer>("HUD").GetNode<Label>("GameOverLabel").Show();
				IsGameRunning = false;
			}
		}
	}
}
