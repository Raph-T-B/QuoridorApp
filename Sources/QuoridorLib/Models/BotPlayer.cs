using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorLib.Models;

public class BotPlayer
{
    private readonly Board _board;
    private readonly Pawn _botPawn;
    private readonly Pawn _playerPawn;
    private readonly bool _isFirstPlayer;
    private readonly Random _random;

    public BotPlayer(Board board, Pawn botPawn, Pawn playerPawn, bool isFirstPlayer)
    {
        _board = board;
        _botPawn = botPawn;
        _playerPawn = playerPawn;
        _isFirstPlayer = isFirstPlayer;
        _random = new Random();
    }

    public Position GetBestMove()
    {
        var possibleMoves = _board.GetPossibleMoves(_botPawn);
        if (!possibleMoves.Any()) return _botPawn.GetPosition();

        // Calculer les scores pour chaque mouvement possible
        var moveScores = new Dictionary<Position, double>();
        foreach (var move in possibleMoves)
        {
            moveScores[move] = EvaluateMove(move);
        }

        // Sélectionner le meilleur mouvement
        return moveScores.OrderByDescending(x => x.Value).First().Key;
    }

    private double EvaluateMove(Position move)
    {
        double score = 0;

        // 1. Distance au but (le plus important)
        int targetY = _isFirstPlayer ? 8 : 0;
        int distanceToGoal = Math.Abs(move.GetPositionX() - targetY);
        score += (9 - distanceToGoal) * 10; // Plus on est proche du but, meilleur est le score

        // 2. Distance à l'adversaire
        int distanceToPlayer = Math.Abs(move.GetPositionX() - _playerPawn.GetPositionX()) +
                             Math.Abs(move.GetPositionY() - _playerPawn.GetPositionY());
        if (distanceToPlayer == 1)
        {
            // Éviter d'être trop près de l'adversaire sauf si on peut gagner
            score -= distanceToGoal == 1 ? 0 : 5;
        }

        // 3. Position centrale (préférer le centre du plateau)
        int distanceToCenter = Math.Abs(move.GetPositionY() - 4);
        score += (4 - distanceToCenter);

        // 4. Ajouter un peu d'aléatoire pour éviter la prévisibilité
        score += _random.NextDouble();

        return score;
    }

    public (Wall, Wall)? GetBestWallPlacement()
    {
        // Obtenir les mouvements possibles du joueur
        var playerMoves = _board.GetPossibleMoves(_playerPawn);
        if (!playerMoves.Any()) return null;

        // Trouver le mouvement qui rapproche le plus le joueur de son but
        var bestPlayerMove = playerMoves.OrderBy(m => 
            Math.Abs(m.GetPositionX() - (_isFirstPlayer ? 0 : 8))).First();

        // Essayer de bloquer ce mouvement avec un mur
        int x = bestPlayerMove.GetPositionX();
        int y = bestPlayerMove.GetPositionY();

        // Essayer différentes positions de murs
        var wallPositions = new List<(Wall, Wall)>
        {
            // Mur horizontal
            (new Wall(x, y, x + 1, y), new Wall(x + 1, y, x + 2, y)),
            // Mur vertical
            (new Wall(x, y, x, y + 1), new Wall(x, y + 1, x, y + 2))
        };

        foreach (var (wall1, wall2) in wallPositions)
        {
            if (_board.AddCoupleWall(wall1, wall2, wall1.GetFirstPosition().GetPositionX() == wall1.GetSecondPosition().GetPositionX() ? "vertical" : "horizontal"))
            {
                return (wall1, wall2);
            }
        }

        return null;
    }

    public Pawn BotPawn => _botPawn;
} 