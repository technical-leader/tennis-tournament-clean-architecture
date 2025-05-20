using System;
using System.Collections.Generic;
using System.Linq;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using Xunit;

namespace TennisTournament.Tests.Unit.Features
{
  public class TournamentPlayerValidationTests
  {
    // Helper para crear jugadores masculinos de prueba
    private MalePlayer CreateMalePlayer(string name = "Default Male Player", int skill = 50, int strength = 50, int speed = 50)
    {
      return new MalePlayer(name, skill, strength, speed) { Name = name };
    }

    // Helper para crear jugadoras femeninas de prueba
    private FemalePlayer CreateFemalePlayer(string name = "Default Female Player", int skill = 50, int reaction = 50)
    {
      return new FemalePlayer(name, skill, reaction) { Name = name };
    }

    [Fact]
    public void Constructor_WithMaleTournamentAndAllMalePlayers_ShouldSucceed()
    {
      // Arrange
      var malePlayers = new List<Player>
            {
                CreateMalePlayer("Rafael Nadal"),
                CreateMalePlayer("Roger Federer")
            };

      // Act & Assert
      // No debería lanzar excepción si la validación de "potencia de 2" se cumple o no aplica aquí.
      // La validación de género es el foco.
      var tournament = new Tournament(TournamentType.Male, malePlayers);
      Assert.Equal(TournamentType.Male, tournament.Type);
      Assert.Equal(2, tournament.Players.Count);
    }

    [Fact]
    public void Constructor_WithMaleTournamentAndFemalePlayer_ShouldThrowArgumentException()
    {
      // Arrange
      var mixedPlayers = new List<Player>
            {
                CreateMalePlayer("Novak Djokovic"),
                CreateFemalePlayer("Serena Williams") // Jugadora incorrecta
            };

      // Act & Assert
      var exception = Assert.Throws<ArgumentException>(() => new Tournament(TournamentType.Male, mixedPlayers));
      Assert.Contains($"Todos los jugadores deben ser del tipo {TournamentType.Male}", exception.Message);
    }

    [Fact]
    public void Constructor_WithFemaleTournamentAndAllFemalePlayers_ShouldSucceed()
    {
      // Arrange
      var femalePlayers = new List<Player>
            {
                CreateFemalePlayer("Iga Swiatek"),
                CreateFemalePlayer("Aryna Sabalenka")
            };

      // Act & Assert
      var tournament = new Tournament(TournamentType.Female, femalePlayers);
      Assert.Equal(TournamentType.Female, tournament.Type);
      Assert.Equal(2, tournament.Players.Count);
    }

    [Fact]
    public void Constructor_WithFemaleTournamentAndMalePlayer_ShouldThrowArgumentException()
    {
      // Arrange
      var mixedPlayers = new List<Player>
            {
                CreateFemalePlayer("Coco Gauff"),
                CreateMalePlayer("Carlos Alcaraz") // Jugador incorrecto
            };

      // Act & Assert
      var exception = Assert.Throws<ArgumentException>(() => new Tournament(TournamentType.Female, mixedPlayers));
      Assert.Contains($"Todos los jugadores deben ser del tipo {TournamentType.Female}", exception.Message);
    }

    [Fact]
    public void AddPlayer_ToMaleTournamentWithMalePlayer_ShouldSucceed()
    {
      // Arrange
      var tournament = new Tournament { Type = TournamentType.Male };
      var malePlayer = CreateMalePlayer("Jannik Sinner");

      // Act
      tournament.AddPlayer(malePlayer);

      // Assert
      Assert.Single(tournament.Players);
      Assert.Contains(malePlayer, tournament.Players);
    }

    [Fact]
    public void AddPlayer_ToMaleTournamentWithFemalePlayer_ShouldThrowArgumentException()
    {
      // Arrange
      var tournament = new Tournament { Type = TournamentType.Male };
      var femalePlayer = CreateFemalePlayer("Elena Rybakina");

      // Act & Assert
      // Si aplicaste el cambio sugerido, la excepción será ArgumentException.
      // Si no, será InvalidOperationException.
      var exception = Assert.Throws<ArgumentException>(() => tournament.AddPlayer(femalePlayer));
      Assert.Contains($"El jugador debe ser del tipo {TournamentType.Male}", exception.Message);
      Assert.Equal("player", (exception as ArgumentException)?.ParamName); // Verifica el ParamName si es ArgumentException
    }

  }
}